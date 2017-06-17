using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Beeant.Application.Services.Sys;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Utility;
using Component.Extension;
using Configuration;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Authority
{
    public class AuthorityEngineApplicationService : IAuthorityEngineApplicationService, IEventApplicationService
    {
        /// <summary>
        /// 查询实例
        /// </summary>
        public IRepository Repository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICacheRepository CacheRepository { get; set; }


        #region 加载菜单
 


        /// <summary>
        /// 得到菜单
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="subSystemUrl"></param>
        /// <returns></returns>
        public virtual IList<MenuEntity> GetMenus(long accountId, string subSystemUrl = "")
        {
            var args = GetEngin();
            var roleIds = args.GetRolesHandle(accountId);
            if (roleIds == null )
                return null;
            return GetMenus(roleIds, subSystemUrl);
        }

 
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private const string MenuCacheKey = "AuthorityEnginMenu";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRoleMenuCacheKey(long id,string url)
        {
            return Winner.Creator.Get<Winner.Base.ISecurity>().EncryptMd5(string.Format("{0}{1}{2}", MenuCacheKey, id, url)) ;
        }

        /// <summary>
        /// 根据角色加载菜单
        /// </summary>
        /// <param name="subSystemUrl"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual IList<MenuEntity> GetMenus(long[] roleIds, string subSystemUrl)
        {
            var result = new Dictionary<long, MenuEntity>(); 
            foreach (var roleId in roleIds)
            {
                var key = GetRoleMenuCacheKey(roleId, subSystemUrl);
                var infos = CacheRepository.Get<IList<MenuEntity>>(key);
                if (infos == null)
                {
                   
                    var query = new QueryInfo();
                    query.Query<MenuEntity>().Where(it => (
                            !it.Abilities.Any()
                            || it.Abilities.Count(m => m.IsVerify == false && m.RoleAbilities.Count(r => r.Role.Id==roleId) == 0) > 0
                            || it.Abilities.Count(m => m.RoleAbilities.Count(r => r.Role.Id == roleId) > 0) > 0
                          ) && it.IsShow).OrderBy(it => it.Sequence)
                        .Select(it => new object[] { it, it.Subsystem.Url, it.Parent });
                    if (!string.IsNullOrWhiteSpace(subSystemUrl))
                    {
                        var urlName = ConfigurationManager.GetSetting<string>(subSystemUrl);
                        query.AppendWhere<MenuEntity>(
                            it => (it.Subsystem.Url == subSystemUrl || it.Subsystem.Url == urlName));
                    }
                    infos = Repository.GetEntities<MenuEntity>(query);
                    CacheRepository.Set(key, infos, DateTime.MaxValue);
                }
                if (infos != null)
                {
                    foreach (var info in infos)
                    {
                        if(!result.ContainsKey(info.Id))
                            result.Add(info.Id,info);
                    }
                }
            }
            return result.Values.ToList();
        }


        #endregion

        #region 权限验证


        /// <summary>
        /// 得到缓存值
        /// </summary>

        private const string ResourceCacheKey = "AuthorityEnginResource";
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        public virtual VerificationEntity GeVerificationEntity(long accountId, string resourceUrl)
        {
            var args = GetEngin();
            var roleIds = args.GetRolesHandle(accountId);
            if (roleIds == null)
                return null;
            return GeVerificationEntity(roleIds, resourceUrl);
        }


        /// <summary>
        /// 验证资源
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        public virtual VerificationEntity GeVerificationEntity(long[] roleIds, string resourceUrl)
        {
            var verification = new VerificationEntity {IsPass = true, Controls = new Dictionary<string, bool>()};
            var infos = GetResources(roleIds, resourceUrl);
            if (infos == null || infos.Count == 0)
            {
                verification.IsPass = false;
                return verification;
            }
            if (verification.IsPass)
            {
                foreach (var info in infos)
                {
                    AddResourceControl(verification, info);
                }
            }
            return verification;
        }
   

        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="verification"></param>
        /// <param name="info"></param>
        protected virtual void AddResourceControl(VerificationEntity verification, ResourceEntity info)
        {
            if (string.IsNullOrEmpty(info.Controls)) return;
            var ctrls = info.Controls.Split(',');
            foreach (var ctrl in ctrls)
            {
                if (info.IsExcude)
                {
                    if (verification.Controls.ContainsKey(ctrl))
                        verification.Controls.Remove(ctrl);
                    verification.Controls.Add(ctrl, false);
                }
                else
                {
                    if (verification.Controls.ContainsKey(ctrl))
                        verification.Controls.Remove(ctrl);
                    verification.Controls.Add(ctrl, true);
                }
            }


        }
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRoleResourceCacheKey(long id, string url)
        {
            return Winner.Creator.Get<Winner.Base.ISecurity>().EncryptMd5(string.Format("{0}{1}{2}", ResourceCacheKey, id, url));
        }

        /// <summary>
        /// 得到资源
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual IList<ResourceEntity> GetResources(long[] roleIds,string url)
        {
            if (roleIds == null || roleIds.Length == 0)
                return null;
            var result = new List<ResourceEntity>();
            foreach (long roleId in roleIds)
            {
                var urlKey = GetResourceUrlKey(url);
                var key = GetRoleResourceCacheKey(roleId, urlKey);
                var infos= CacheRepository.Get<IList<ResourceEntity>>(key);
                if (infos == null)
                {
                    var query = new QueryInfo();
                    query
                        .Query<ResourceEntity>().Where(it => it.Ability.RoleAbilities.Count(s=>s.Role.Id==roleId)>0 && it.Url.StartsWith(urlKey))
                        .OrderByDescending(it => it.IsExcude)
                        .Select(it => new object[]
                        {
                            it.Url, it.IsValidateParamter, it.Name,
                            it.IsRegexValidate, it.Controls
                        });
                    infos = Repository.GetEntities<ResourceEntity>(query);
                }
                if (infos != null)
                {
                    CacheRepository.Set(key, infos, DateTime.MaxValue);
                    result.AddRange(infos);
                }
            }
            return result.Where(it => IsPageResource(it, url)).ToList();

        }
        /// <summary>
        /// 验证值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        protected virtual bool IsPageResource(ResourceEntity info, string resourceUrl)
        {
            if (info.IsRegexValidate)
                return Regex.IsMatch(resourceUrl, info.Url.Trim(), RegexOptions.IgnoreCase);
            if (info.IsValidateParamter)
                return resourceUrl.ToLower().Equals(info.Url == null ? "" : info.Url.ToLower());
            string url = resourceUrl.Contains("?") ? resourceUrl.Substring(0, resourceUrl.IndexOf("?")) : resourceUrl;
            var rev = url.ToLower().Equals(info.Url == null ? "" : info.Url.ToLower());
            if (string.IsNullOrWhiteSpace(info.Controls) && rev)
                return true;
            return false;
        }

        /// <summary>
        /// 得到资源值
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual string GetResourceUrlKey(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;
           return url.Contains("?") ? url.Substring(0, url.IndexOf("?")).ToLower() : url.ToLower();
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 缓存锁
        /// </summary>

        private static object CacheLocker = new object();

        /// <summary>
        /// 得到缓存值
        /// </summary>

        private const string CacheKey = "AuthorityEnginArgs";



        /// <summary>
        /// 得到用户信息
        /// </summary>
        /// <returns></returns>
        public virtual AuthorityEnginEntity GetEngin()
        {
            var args = CacheRepository.Get<AuthorityEnginEntity>(CacheKey);
            if (args == null)
            {
                lock (CacheLocker)
                {
                    args = CacheRepository.Get<AuthorityEnginEntity>(CacheKey);
                    if (args == null)
                    {
                        args = SetArgsCache();
                    }
                }
            }
            return args;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <returns></returns>
        protected virtual AuthorityEnginEntity SetArgsCache()
        {
            var args = new AuthorityEnginEntity();
            args.GetRolesHandle = GetRoles;
            args.GetOnwerHandle = GetOnwer;
            CacheRepository.Set(CacheKey, args, DateTime.MaxValue);
            return args;
        }

        #region 角色账户
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string RoleAccountCacheKey = "AuthorityEnginRoleAccount";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRoleAccountCacheKey(long id)
        {
            return string.Format("{0}{1}", RoleAccountCacheKey, id);
        }
    
        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected virtual long[] GetRoles(long accountId)
        {
            var key = GetRoleAccountCacheKey(accountId);
            var value = CacheRepository.Get<long[]>(key);
            if (value == null)
            {
                var query=new QueryInfo();
                query.Query<RoleAccountEntity>().Where(it => it.Account.Id == accountId).Select(it => it.Role.Id);
                var infos = Repository.GetEntities<RoleAccountEntity>(query);
                value = infos == null
                    ? new long[0]
                    : infos.Where(it => it.Role != null).Select(it => it.Role.Id).ToArray();
                CacheRepository.Set(key, value, 3600*15);
            }
            return value;
        }
        #endregion

        #region 所属人账户
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string OnwerAccountCacheKey = "AuthorityEnginOnwerAccount";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetOnwerAccountCacheKey(long id,string name)
        {
            return string.Format("{0}{1}{2}", OnwerAccountCacheKey, id,name);
        }
   

        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual OwnerEntity GetOnwer(long accountId,string name)
        {
            var key = GetOnwerAccountCacheKey(accountId, name);
            var value = CacheRepository.Get<OwnerEntity>(key);
            if (value == null)
            {
                var query = new QueryInfo();
                query.Query<OwnerAccountEntity>().Where(it => it.Account.Id == accountId && it.Owner.Name==name).Select(it => new object[] { it.Id, it.Account.Id, it.Owner.Id, it.Owner.Name, it.Owner.ReadCodes, it.Owner.SubmitCode });
                var infos = Repository.GetEntities<OwnerAccountEntity>(query);
                value = infos?.Where(it => it.Owner != null).Select(it => it.Owner).FirstOrDefault();
                CacheRepository.Set(key, value, 3600 * 15);
            }
            return value;
        }
        #endregion

        #endregion

        #region 执行事件

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool Execute(string url, string name)
        {
            switch (name)
            {
                case "ClearAuthorityCache":
                    ClearAuthorityCache(url);
                    break;
                case "ClearAuthorityAccountCache":
                    ClearAuthorityAccountCache(url);
                    break;
                case "ClearAuthorityRoleCache":
                    ClearAuthorityRoleCache(url);
                    break;
                case "ClearAuthorityResourceCache":
                    ClearAuthorityResourceCache(url);
                    break;
            }
            return true;
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearAuthorityCache(string url)
        {
            CacheRepository.Remove(CacheKey);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearAuthorityRoleCache(string url)
        {
            var roleid = HttpUtility.ParseQueryString(url).Get("roleid");
            var subSystemUrl = HttpUtility.ParseQueryString(url).Get("url");
            if (!string.IsNullOrWhiteSpace(subSystemUrl))
            {
                var urls = subSystemUrl.Split(',');
                foreach (var s in urls)
                {
                    var key = GetRoleMenuCacheKey(roleid.Convert<long>(), s);
                    CacheRepository.Remove(key);
                }
            }
            else
            {
                var key = GetRoleMenuCacheKey(roleid.Convert<long>(), null);
                CacheRepository.Remove(key);
            }
            var query = new QueryInfo();
            query.Query<RoleEntity>().Where(it => it.Id == roleid.Convert<long>())
                .Select(it => it.RoleAbilities.Select(s => s.Ability.Resources.Select(n => n.Url)));
            var info = Repository.GetEntities<RoleEntity>(query)?.FirstOrDefault();
            if (info == null || info.RoleAbilities == null)
                return;
            foreach (var roleAbility in info.RoleAbilities)
            {
                if (roleAbility.Ability == null || roleAbility.Ability.Resources == null)
                    continue;
                foreach (var resource in roleAbility.Ability.Resources)
                {
                    var urlKey = GetResourceUrlKey(resource.Url);
                    var key = GetRoleResourceCacheKey(info.Id, urlKey);
                    CacheRepository.Remove(key);
                }
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearAuthorityAccountCache(string url)
        {
           var id= HttpUtility.ParseQueryString(url).Get("accountid").Convert<long>();
            if (id == 0)
                return;
            var query = new QueryInfo();
            query.Query<OwnerEntity>().Select(it => it.Name);
            var owners= Repository.GetEntities<OwnerEntity>(query);
            var key = GetRoleAccountCacheKey(id);
            CacheRepository.Remove(key);
            if (owners == null || owners.Count == 0)
                return;
            foreach (var info in owners)
            {
                key = GetOnwerAccountCacheKey(id, info.Name);
                CacheRepository.Remove(key);
            }
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearAuthorityResourceCache(string url)
        {
            var urlKey =GetResourceUrlKey(HttpUtility.ParseQueryString(url).Get("url")) ;
            if (string.IsNullOrWhiteSpace(urlKey))
                return;
            var query = new QueryInfo();
            query.Query<RoleEntity>().Where(it=>it.RoleAbilities.Count(s=>s.Ability.Resources.Count(n=>n.Url.StartsWith(urlKey))>0)>0).Select(it => it.Id);
            var roles = Repository.GetEntities<OwnerEntity>(query);
            if(roles==null)
                return;
            foreach (var role in roles)
            {
                var key = GetRoleResourceCacheKey(role.Id, urlKey);
                CacheRepository.Remove(key);
            }
     
        }
        #endregion
    }
}

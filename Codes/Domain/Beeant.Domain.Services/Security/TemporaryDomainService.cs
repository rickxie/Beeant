using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Security;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Security
{
    public class TemporaryDomainService : RealizeDomainService<TemporaryEntity> 
    {
     

        #region 重写验证


        #region 验证添加

        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(TemporaryEntity info)
        {
            var rev = ValidateExist(info);
            return rev;
        }



        /// <summary>
        /// 验证是否重复发送
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(TemporaryEntity info)
        {
           var json = Configuration.ConfigurationManager.GetSetting<string>("SecurityTemporary").DeserializeJson<dynamic>();
           int cacheTime = json == null || json.CacheTime == null ? 1800 : json.CacheTime;
            var query = new QueryInfo();
            query.SetPageIndex(0)
                       .SetPageSize(1)
                       .Query<CodeEntity>()
                       .Where(
                           it =>
                           it.Name==info.Name && it.Tag == info.Tag &&
                           it.InsertTime >= DateTime.Now.AddSeconds(-cacheTime));
            var infos = Repository.GetEntities<CodeEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("Exist");
                return false;
            }
            return true;
        }

        #endregion

        #endregion



        #region 接口实现
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="temporary"></param>
        /// <returns></returns>
        public virtual bool Check(TemporaryEntity temporary)
        {
            return CheckTemporary(temporary);
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="temporary"></param>
        /// <returns></returns>
        public virtual IList<IUnitofwork> Release(TemporaryEntity temporary)
        {
            var info = GetTemporary(temporary);
            if (info != null && info.IsUsed)
            {
                info.IsUsed = false;
                info.SaveType = SaveType.Modify;
                info.SetProperty(it => it.IsUsed);
                return Handle(info);
            }
            return null;
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="temporary"></param>
        public virtual IList<IUnitofwork> Set(TemporaryEntity temporary)
        {
            if (string.IsNullOrEmpty(temporary.Tag))
                return null;
            var info = GetTemporary(temporary);
            if (info == null)
            {
                temporary.IsUsed = true;
                temporary.SaveType = SaveType.Add;
            }
            else
            {
                if (info.IsUsed)
                    return null;
                temporary.Id = info.Id;
                temporary.IsUsed = true;
                temporary.SaveType = SaveType.Modify;
                temporary.SetProperty(it => it.IsUsed);
            }
            return Handle(temporary);

        }
        /// <summary>
        /// 检查错误次数
        /// </summary>
        /// <param name="temporary"></param>
        /// <returns></returns>
        protected virtual bool CheckTemporary(TemporaryEntity temporary)
        {
            var info = GetTemporary(temporary);
            if (info != null && info.IsUsed)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 得到登录锁
        /// </summary>
        /// <param name="temporary"></param>
        /// <returns></returns>
        protected virtual TemporaryEntity GetTemporary(TemporaryEntity temporary)
        {
            if (string.IsNullOrEmpty(temporary.Tag))
                return null;
            var query = new QueryInfo();
            query.Query<TemporaryEntity>().Where(it => it.Name == temporary.Name && it.Tag == temporary.Tag && it.EffectiveTime > DateTime.Now).Select(it => new object[] { it.Id, it.IsUsed });
            var infos = Repository.GetEntities<TemporaryEntity>(query);
            return infos?.FirstOrDefault();
        }
        #endregion
    }
}

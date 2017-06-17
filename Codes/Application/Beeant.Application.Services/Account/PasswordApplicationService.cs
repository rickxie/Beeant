
using System;
using System.Linq;
using Component.Extension;
using Beeant.Application.Dtos.Account;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Security;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Account
{
    public class PasswordApplicationService: IPasswordApplicationService
    {
        /// <summary>
        /// 查询实例
        /// </summary>
        public IRepository Repository { get; set; }
        /// <summary>
        /// 登入锁
        /// </summary>
        public ILockerDomainService LockerDomainService { get; set; }
        /// <summary>
        /// 登入锁
        /// </summary>
        public ICodeDomainService CodeValidateDomainService { get; set; }
        /// <summary>
        /// 登入锁
        /// </summary>
        public IDomainService CodeDomainService { get; set; }
        /// <summary>
        /// 登入锁
        /// </summary>
        public IDomainService TemporaryDomainService { get; set; }
        /// <summary>
        /// 登入锁
        /// </summary>
        public IDomainService AccountDomainService { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        protected virtual string Tag { get; set; } = "AccountResetPassword";

        public static readonly string Key = Guid.NewGuid().ToString();

        /// <summary>
        /// 检查用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool CheckPassword(string name, string password, out AccountEntity entity)
        {
            var account = new AccountEntity { Password = password };
            account.SetEncryptPassword();
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => (it.Name == name || it.IsActiveEmail && it.Email==name || it.IsActiveMobile && it.Mobile==name) && it.Password == account.Password)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<AccountEntity>(query);
            entity = infos?.FirstOrDefault();
            return entity!=null;
        }

        /// <summary>
        /// 检查密码是否正确
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool CheckPassword(long accountId, string password)
        {
            var account = new AccountEntity { Password = password };
            account.SetEncryptPassword();
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => it.Id == accountId && it.Password == account.Password)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<AccountEntity>(query);
            return infos != null && infos.Count > 0;
        }
        /// <summary>
        /// 检查账户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual AccountEntity CheckAccount(string name)
        {
            var account = GetAccount(name);
            if (account != null)
            {
                var temporary = new TemporaryEntity
                {
                    Name = name,
                    Tag = Tag,
                    SaveType = SaveType.Add
                };
                var unitofworks=TemporaryDomainService.Handle(temporary);
                Winner.Creator.Get<IContext>().Commit(unitofworks);
            }
            return account;
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual PasswordDto SendCode(string name, CodeEntity code)
        {
            var account = GetAccount(name);
            var result = new PasswordDto
            {
                IsTimeout = CheckTemporary(name)
            };
            if (result.IsTimeout)
                return result;
            if (code.Type == CodeType.Email && !account.IsActiveEmail)
            {
                return result;
            }
            if (code.Type == CodeType.Mobile && !account.IsActiveMobile)
            {
                return result;
            }
            code.Tag = Tag;
            code.Name = name;
            code.ToAddress = code.Type == CodeType.Email ? account.Email : account.Mobile;
            code.SaveType=SaveType.Add;
            var unitofworks = CodeDomainService.Handle(code);
            result.Result = Winner.Creator.Get<IContext>().Commit(unitofworks);
            result.Errors = result.Errors;
            result.SendStep = result.SendStep;
            return result;
        }
        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public virtual PasswordDto CheckCode(string name, string code, CodeType codeType)
        {
            var result = new PasswordDto
            {
                IsTimeout = CheckTemporary(name)
            };
            if (result.IsTimeout)
                return result;
            result.Result= CodeValidateDomainService.ValidateCode(Tag, name, codeType, code);
            return result;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual PasswordDto Reset(string name, string password)
        {
            var result = new PasswordDto
            {
                IsTimeout = CheckTemporary(name)
            };
            if (result.IsTimeout)
                return result;
            var account = GetAccount(name);
            if (account == null)
            {
                result.Result = false;
                return result;
            }
            SetSaveAccount(account, password);
            var unitofworks = AccountDomainService.Handle(account);
            if (unitofworks == null)
            {
                result.Result = false;
                result.Errors = account.Errors;
                return result;
            }
            var locker = new LockerEntity
            {
                Name = account.Name,
                Tag = IdentityEntity.LockTag
            };
            unitofworks.AddList(LockerDomainService.Release(locker));
            result.Result = Winner.Creator.Get<IContext>().Commit(unitofworks);
            result.Errors = locker.Errors;
            return result;
        }

        /// <summary>
        /// 设置仓储
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        protected virtual void SetSaveAccount(AccountEntity account,string password)
        {
            account.Password = password;
            account.SaveType = SaveType.Modify;
            account.SetProperty(it => it.Password);
        }
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetKey(string name)
        {
            return string.Format("{0}{1}{2}",Tag,Key,name);
        }
        /// <summary>
        /// 检查用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual bool CheckTemporary(string name)
        {
            var query=new QueryInfo();
            query.Query<TemporaryEntity>().Where(it => it.Tag == Tag && it.Name == name && it.EffectiveTime>=DateTime.Now).Select(it => it.Id);
            var infos = Repository.GetEntities<TemporaryEntity>(query);
            return infos == null || infos.Count == 0;
        }

        /// <summary>
        /// 得到账户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual AccountEntity GetAccount(string name)
        {
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => (it.Name == name || (it.IsActiveMobile && it.Mobile==name) || (it.IsActiveEmail && it.Email==name)) && it.IsUsed)
                .Select(it => new object[] { it.Id, it.Name, it.Email, it.Mobile,it.IsActiveEmail,it.Mobile });
            var infos = Repository.GetEntities<AccountEntity>(query);
            return infos?.FirstOrDefault();
        }
    }
}

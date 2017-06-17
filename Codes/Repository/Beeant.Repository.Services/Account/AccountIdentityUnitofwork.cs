using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Services.Account;
using Winner;
using Winner.Log;
using Winner.Persistence;
using Winner.Wcf;

namespace Beeant.Repository.Services.Account
{


    public class AccountIdentityUnitofwork : IUnitofwork
    {
        public IWcfService WcfService { get; set; }
        protected AccountIdentityEntity AccountIdentity { get; set; }
        public AccountIdentityUnitofwork(AccountIdentityEntity accountIdentity, IWcfService wcfService)
        {
            if(accountIdentity.Account==null)
                return;
            AccountIdentity = accountIdentity;
            WcfService = wcfService;

        }

        #region 接口的实现
        public IList<object> Entities
        {
            get { return null; }
        }

        public bool IsExcute { get; set; }
        public bool IsDispose { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Execute()
        {
            AccountIdentity.Account = new AccountEntity { Id = AccountIdentity.Account.Id };
            var rev = WcfService.Invoke<IAccountIdentityContract>(SaveAccountIdentity, GetEndPoint, AccountIdentity);
            if (rev == null)
               throw new Exception("ResiterAccountError");
            var accountIdentity = rev.ToString().DeserializeJson<AccountIdentityEntity>();
            if (accountIdentity == null)
                throw new Exception("ResiterAccountError");
            if (accountIdentity.Id == 0 || accountIdentity.Errors != null && accountIdentity.Errors.Count>0)
            {
                var errorMsg = string.Join(",", accountIdentity.Errors.Select(r => r.Message));
                throw new Exception(errorMsg);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        public virtual void Commit()
        {
   
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public virtual void Rollback()
        {
            try
            {
                if (AccountIdentity == null || AccountIdentity.Id == 0) return;
                AccountIdentity.SaveType=SaveType.Remove;
                Creator.Get<IContext>().Set(AccountIdentity, AccountIdentity, AccountIdentity.SaveSequence);
                AccountIdentity.SaveType = AccountIdentity.SaveType;
                var unitofworks= Creator.Get<IContext>().Save();
                Creator.Get<IContext>().Commit(unitofworks);
            }
            catch (Exception ex)
            {

                Creator.Get<ILog>().AddException(ex);
            }
          
        }
        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="service"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual object SaveAccountIdentity(IAccountIdentityContract service, params object[] paramters)
        {
            var info = service.Save(paramters[0].SerializeJson());
            return info;
        }
        /// <summary>
        /// 得到节点
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual IList<EndPointInfo> GetEndPoint(IList<EndPointInfo> endPoints, params object[] paramters)
        {
            if (endPoints == null)
                return null;
            var eps = endPoints.Where(it => it.IsException == false).ToList();
            if (eps.Count == 0)
                return eps;
            var index =(int)(AccountIdentity.Number.GenerateLongId() % eps.Count);
            return new [] {eps[index]};
        }

        #endregion
    }
}

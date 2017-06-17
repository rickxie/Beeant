using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Agent
{
    public class AgentDomainService : RealizeDomainService<AgentEntity>
    {
        public virtual IDomainService AccountDomainService { get; set; }
        private IDictionary<string, IUnitofworkHandle> _itemHandles;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, IUnitofworkHandle> ItemHandles
        {
            get
            {
                return _itemHandles ?? (_itemHandles = new Dictionary<string, IUnitofworkHandle>
                {
                    {"Account", new UnitofworkHandle<AccountEntity> {DomainService = AccountDomainService,IsAppend=false}}
                });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(AgentEntity info)
        {
            var rev = ValidateAccount(info) && ValidateAccountExist(info);
            return rev;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(AgentEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null ||
                info.Account.Id == 0)
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            var account = Repository.Get<AccountEntity>(info.Account.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                return false;
            }
            if (!account.IsUsed)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccountExist(AgentEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            var query = new QueryInfo();
            query.Query<AgentEntity>().Where(it => it.Account.Id == info.Account.Id).Select(it => it.Id);
            var infos = Repository.GetEntities<AgentEntity>(query);
            if (infos != null && infos.Count > 0)
            {
                info.AddError("AccountHasSite");
                return false;
            }
            return true;
        }
        #endregion

    }
}

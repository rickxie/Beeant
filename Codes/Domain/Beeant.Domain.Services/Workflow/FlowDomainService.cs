using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;

namespace Beeant.Domain.Services.Workflow
{
    public class FlowDomainService : RealizeDomainService<FlowEntity>
    {
        /// <summary>
        /// 属性明细
        /// </summary>
        public IDomainService PropertyDomainService { get; set; }
        /// <summary>
        /// 节点成本
        /// </summary>
        public IDomainService NodeDomainService { get; set; }
       
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
                        {"WorkProperties", new UnitofworkHandle<PropertyEntity>{DomainService= PropertyDomainService}},
                        {"Nodes",new UnitofworkHandle<NodeEntity>{DomainService= NodeDomainService} }
                       
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(FlowEntity info)
        {
            var rev = ValidateAccount(info);
            return rev;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(FlowEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && (info.Account.SaveType == SaveType.Add || info.Account.Id==0))
                return true;
            if (info.Account != null && info.Account.Id != 0)
            {
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
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
    }
}

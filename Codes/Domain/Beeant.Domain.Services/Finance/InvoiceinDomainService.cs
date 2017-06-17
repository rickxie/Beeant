using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Finance
{
    public class InvoiceinDomainService : RealizeDomainService<InvoiceinEntity>
    {
   


        /// <summary>
        /// 服务实例
        /// </summary>
        public virtual IDomainService InvoiceinItemDomainService { get; set; }
        #region 业务加载
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
                        {"InvoiceinItems", new UnitofworkHandle<InvoiceinItemEntity>{DomainService= InvoiceinItemDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<InvoiceinEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<InvoiceinEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<InvoiceinEntity>>
                    {
                        {"Account", LoadAccount},
                        {"DataEntity", LoadDataEntity},
                        {"InvoiceinItems", LoadInvoiceinItems}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
     


      
        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadDataEntity(InvoiceinEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<InvoiceinEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadAccount(InvoiceinEntity info)
        {
            LoadDataEntity(info);
            info.Account = info.DataEntity == null ? Repository.Get<AccountEntity>(info.Account.Id) : Repository.Get<AccountEntity>(info.DataEntity.Account.Id);
        }
        /// <summary>
        /// 加载发票核销
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadInvoiceinItems(InvoiceinEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            var query = new QueryInfo();
            query.Query<InvoiceinItemEntity>().Where(it => it.Invoicein.Id == info.Id);
            info.InvoiceinItems = Repository.GetEntities<InvoiceinItemEntity>(query);
        }
        #endregion

        #region 重写验证
       
        #region 验证添加

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(InvoiceinEntity info)
        {
            var rev = ValidateAccount(info, null);
            return rev;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(InvoiceinEntity info, InvoiceinEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
            {
                if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id)
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
            info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
            return false;
        }
      
        #endregion

        #region 修改验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(InvoiceinEntity info)
        {
            var dataEntity = Repository.Get<InvoiceinEntity>(info.Id);
            var rev = ValidateModifyStatus(info);
            if (rev) rev = ValidateModifyProperty(info, dataEntity);
            if (rev) rev = ValidateAccount(info, dataEntity);
            return rev;
        }
        /// <summary>
        /// 验证状态编辑
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateModifyStatus(InvoiceinEntity info)
        {
            if (!info.HasSaveProperty(it => it.Status)) return true;
            if (info.HasSaveProperty(it => it.Account.Id))
            {
                info.AddError("NotAllowModifyAccountAndStatusAtSameTime");
                return false;
            }
            if (info.HasSaveProperty(it => it.Amount))
            {
                info.AddError("NotAllowModifyAmountAndStatusAtSameTime");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateModifyProperty(InvoiceinEntity info, InvoiceinEntity dataEntity)
        {
            if (dataEntity.Status != InvoiceinStatusType.Finish) return true;
            if (info.HasSaveProperty(it => it.Account.Id) && dataEntity.Account.Id.Equals(info.Account.Id))
            {
                info.AddError("NotAllowModifyAccountAtFinish");
                return false;
            }
            if (info.HasSaveProperty(it => it.Amount) && dataEntity.Amount != info.Amount)
            {
                info.AddError("NotAllowModifyAmountAtFinish");
                return false;
            }
            return true;
        }

        #endregion
 
       
        #endregion
        
        
 
    }
}

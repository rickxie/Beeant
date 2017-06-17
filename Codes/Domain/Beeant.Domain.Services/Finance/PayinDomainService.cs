using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Finance
{
    public class PayinDomainService : RealizeDomainService<PayinEntity>
    {

       
        /// <summary>
        /// 服务实例
        /// </summary>
        public virtual IDomainService PayDomainService { get; set; }

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
                       {"Pays", new UnitofworkHandle<OrderPayEntity>{DomainService= PayDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<PayinEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PayinEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PayinEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"PayinItems", LoadPayinItems},
                        {"Account", LoadAccount}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
 
        #region 重写加载
    

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadDataEntity(PayinEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<PayinEntity>(info.Id);

        }
        /// <summary>
        /// 加载账户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadAccount(PayinEntity info)
        {
            LoadDataEntity(info);
            info.Account = info.DataEntity == null ? Repository.Get<AccountEntity>(info.Account.Id) : Repository.Get<AccountEntity>(info.DataEntity.Account.Id);
        }
        /// <summary>
        /// 加载订单
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadPayinItems(PayinEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            var query = new QueryInfo();
            query.Query<PayinItemEntity>().Where(it => it.Payin.Id == info.Id);
            info.PayinItems = Repository.GetEntities<PayinItemEntity>(query);
        }
        #endregion

        #region 重写验证
  
        #region 验证添加

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PayinEntity info)
        {
            var rev = ValidateAccount(info, null) ;
            return rev;
        }


        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(PayinEntity info, PayinEntity dataEntity)
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
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual PayinEntity GetEntity(PayinEntity info)
        {
            var rev = Repository.Get<PayinEntity>(info.Id);
            if (rev != null) rev.Account = Repository.Get<AccountEntity>(rev.Account.Id);
            return rev;
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(PayinEntity info)
        {
            var dataEntity = GetEntity(info);
            var rev = ValidateModifyStatus(info);
            if (rev) rev = ValidateModifyProperty(info, dataEntity);
            if (rev) rev = ValidateAccount(info, dataEntity) ;
            return rev;
        }
        /// <summary>
        /// 验证状态编辑
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateModifyStatus(PayinEntity info)
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
        protected virtual bool ValidateModifyProperty(PayinEntity info, PayinEntity dataEntity)
        {
            if (dataEntity.Status != PayinStatusType.Finish) return true;
            if (info.HasSaveProperty(it => it.Account.Id) && !dataEntity.Account.Id.Equals(info.Account.Id))
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

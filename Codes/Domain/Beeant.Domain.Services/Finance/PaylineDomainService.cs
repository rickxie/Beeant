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
    public class PaylineDomainService : RealizeDomainService<PaylineEntity>
    {
        /// <summary>
        /// 服务实例
        /// </summary>
        public virtual IDomainService PaylineItemDomainService { get; set; }
        /// <summary>
        /// 流水
        /// </summary>
        public virtual IDomainService AccountItemDomainService { get; set; }
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
                        {"PaylineItems", new UnitofworkHandle<PaylineItemEntity>{DomainService= PaylineItemDomainService}},
                        {"AccountItem", new UnitofworkHandle<AccountItemEntity>{DomainService= AccountItemDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IDictionary<string, ItemLoader<PaylineEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PaylineEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PaylineEntity>>
                    {
                     {"DataEntity", LoadDataEntity},
                        {"PaylineItems", LoadPaylineItems}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadDataEntity(PaylineEntity info)
        {
            if (info.SaveType == SaveType.Add || info.DataEntity!=null)
                return;
            info.DataEntity = Repository.Get<PaylineEntity>(info.Id);
        }

        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadPaylineItems(PaylineEntity info)
        {
            if (info.SaveType == SaveType.Add)
            {
              if(info.PaylineItems==null)
                    return;
                var orderIds = info.PaylineItems.Where(it => it.Order != null).Select(it => it.Order.Id).ToArray();
                if (orderIds.Length == 0)
                    return;
                var orders = GetOrders(orderIds);
                if(orders==null)
                    return;
                foreach (var paylineItem in info.PaylineItems)
                {
                    if(paylineItem.Order==null)
                        continue;
                    paylineItem.Order = orders.FirstOrDefault(it => it.Id == paylineItem.Order.Id);
                }
            }
            else
            {
                info.PaylineItems = GetPaylineItems(info.Id);
            }
           
        }
        /// <summary>
        /// 得到订单
        /// </summary>
        /// <param name="paylineId"></param>
        /// <returns></returns>
        protected virtual IList<PaylineItemEntity> GetPaylineItems(long paylineId)
        {
            var query = new QueryInfo();
            query.Query<PaylineItemEntity>().Where(it =>it.Payline.Id== paylineId)
                .Select(it => new object[] { it.Id, it.Amount, it.Order.Id,it.Order.PayAmount,it.Order.TotalPayAmount,it.Order.Status });
            return Repository.GetEntities<PaylineItemEntity>(query);
        }
        /// <summary>
        /// 得到订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        protected virtual IList<OrderEntity> GetOrders(long[] orderIds)
        {
            if(orderIds==null)
                return null;
            var query=new QueryInfo();
            query.Query<OrderEntity>().Where(it => orderIds.Contains(it.Id))
                .Select(it => new object[] {it.Id, it.PayAmount, it.TotalPayAmount, it.Status});
            return Repository.GetEntities<OrderEntity>(query);
        }
        #region 重写验证


        #region 验证添加

        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PaylineEntity info)
        {
            var rev = ValidateAmount(info) && ValidateAccount(info,null) && ValidateOrders(info);
            return rev;
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAmount(PaylineEntity info)
        {
            var amount = info.Amount != 0 || info.PaylineItems == null
                ? info.Amount
                : info.PaylineItems.Sum(it => it.Amount);
            if (amount == 0)
            {
                info.AddErrorByName(typeof(PaylineEntity).FullName, "NoAllowAmount");
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
        protected virtual bool ValidateAccount(PaylineEntity info, PaylineEntity dataEntity)
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


        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrders(PaylineEntity info)
        {
            if (info.PaylineItems == null)
                return true;
            var orderIds = info.PaylineItems.Where(it => it.Order != null).Select(it => it.Order.Id).ToArray();
            var query=new QueryInfo();
            query.Query<OrderEntity>()
                .Where(it => it.Account.Id == info.Account.Id && orderIds.Contains(it.Id))
                .Select(it => new object[] {it.Id, it.PayTypes});
            var orders = Repository.GetEntities<OrderEntity>(query);
            if (orders == null || orders.Count != orderIds.Length)
            {
                info.AddError("AccountNoHasOrder");
                return false;
            }
            if (orders.Count(it => it.PayTypeArray != null && !it.PayTypeArray.Contains(info.TypeName)) > 0)
            {
                info.AddError("OrderNotAllowPayType");
                return false;
            }
            return true;
        }
        #endregion

        #region 验证修改
        /// <summary>
        /// 验证添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(PaylineEntity info)
        {
            var dataEntity = Repository.Get<PaylineEntity>(info.Id);
            var rev = ValidateStatus(info, dataEntity);
            return rev;
        }
        /// <summary>
        /// 验证是否操操作过
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateStatus(PaylineEntity info, PaylineEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Status))
                return true;
            if (dataEntity.Status== PaylineStatusType.Success || dataEntity.Status == PaylineStatusType.Failure)
            {
                info.AddError("StatusAlreadyTrue");
                return false;
            }
            return true;
        }

        #endregion


        #endregion


    }
}

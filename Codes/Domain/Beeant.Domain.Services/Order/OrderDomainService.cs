using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Domain.Entities.Member;

namespace Beeant.Domain.Services.Order
{
    public class OrderDomainService : RealizeDomainService<OrderEntity>
    {
        /// <summary>
        /// 订单明细
        /// </summary>
        public IDomainService OrderItemDomainService { get; set; }
        /// <summary>
        /// 收款明细
        /// </summary>
        public IDomainService OrderPayDomainService { get; set; }
        /// <summary>
        /// 维护记录
        /// </summary>
        public IDomainService OrderNoteDomainService { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public IDomainService OrderAttachmentDomainService { get; set; }
        /// <summary>
        /// 快递信息
        /// </summary>
        public IDomainService OrderExpressDomainService { get; set; }
        /// <summary>
        /// 投诉信息
        /// </summary>
        public IDomainService OrderComplaintDomainService { get; set; }

        /// <summary>
        /// 发票
        /// </summary>
        public IDomainService OrderInvoiceDomainService { get; set; }

        /// <summary>
        /// 订单产品明细
        /// </summary>
        public IDomainService OrderProductDomainService { get; set; }
        /// <summary>
        /// 订单保险明细
        /// </summary>
        public IDomainService OrderInsuranceDomainService { get; set; }
        /// <summary>
        /// 订单保险明细
        /// </summary>
        public IDomainService OrderNumberDomainService { get; set; }

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
                        {"OrderProducts", new UnitofworkHandle<OrderProductEntity>{DomainService= OrderProductDomainService}},
                        {"OrderInsurances", new UnitofworkHandle<OrderInsuranceEntity>{DomainService= OrderInsuranceDomainService}},
                        {"OrderItems", new UnitofworkHandle<OrderItemEntity>{DomainService= OrderItemDomainService}},
                        {"OrderPays", new UnitofworkHandle<OrderPayEntity>{DomainService= OrderPayDomainService}},
                        {"OrderNotes", new UnitofworkHandle<OrderNoteEntity>{DomainService= OrderNoteDomainService}},
                        {"OrderAttachments",new UnitofworkHandle<OrderAttachmentEntity>{DomainService= OrderAttachmentDomainService} },
                        {"OrderComplaints", new UnitofworkHandle<CouponEntity>{DomainService= OrderComplaintDomainService} },
                        {"OrderExpresses", new UnitofworkHandle<OrderExpressEntity>{DomainService= OrderExpressDomainService}},
                        {"OrderInvoices", new UnitofworkHandle<OrderInvoiceEntity>{DomainService= OrderInvoiceDomainService}},
                        {"OrderNumbers", new UnitofworkHandle<OrderNumberEntity>{DomainService= OrderNumberDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<OrderEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderEntity>>
                    {
                        {"DataEntity", LoadDataEntity},
                        {"OrderItems", LoadOrderItems},
                        {"OrderProducts", LoadOrderProducts},
                        {"OrderPays", LoadOrderPays},
                        {"Account", LoadAccount}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        #region 填充业务
        /// <summary>
        /// 加载原始数据
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadDataEntity(OrderEntity info)
        {
            if(info.SaveType==SaveType.Add)return;
            info.DataEntity = Repository.Get<OrderEntity>(info.Id);
        }
        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadOrderItems(OrderEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            var orderItems = info.OrderItems == null
                               ? null
                               : info.OrderItems.Where(it => it.SaveType == SaveType.Add).ToList();
            var query = new QueryInfo();
            query.Query<OrderItemEntity>().Where(it => it.Order.Id == info.Id);
            info.OrderItems = Repository.GetEntities<OrderItemEntity>(query);
            if (info.OrderItems != null)
            {
                info.OrderItems = info.OrderItems.ToList();
                info.OrderItems.AddList(orderItems);
            }
        }


        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadOrderProducts(OrderEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            var orderProducts = info.OrderProducts == null
                               ? null
                               : info.OrderProducts.Where(it => it.SaveType == SaveType.Add).ToList();
            var query = new QueryInfo();
            query.Query<OrderProductEntity>().Where(it => it.Order.Id == info.Id).Select(it => new object[] { it, it.Product.Count, it.Product.Goods.UnusedStatus });
            info.OrderProducts = Repository.GetEntities<OrderProductEntity>(query);
            if (info.OrderProducts != null)
            {
                info.OrderProducts = info.OrderProducts.ToList();
                info.OrderProducts.AddList(orderProducts);
            }
        }

        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadOrderPays(OrderEntity info)
        {
            if (info.SaveType == SaveType.Add) return;
            var receiveds = info.OrderPays == null
                                ? null
                                : info.OrderPays.Where(it => it.SaveType == SaveType.Add).ToList();
            var query = new QueryInfo();
            query.Query<OrderPayEntity>().Where(it => it.Order.Id == info.Id);
            info.OrderPays = Repository.GetEntities<OrderPayEntity>(query).ToList();
            if (info.OrderPays != null)
            {
                info.OrderPays = info.OrderPays.ToList();
                info.OrderPays.AddList(receiveds);
            }
        }

  
    
        /// <summary>
        /// 填充订单明细
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadAccount(OrderEntity info)
        {
            info.Account = Repository.Get<AccountEntity>(info.Account.Id);
        }
  
        #endregion

        #region 重写验证


        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderEntity info)
        {
            return ValidateAccount(info) ; 
        }

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(OrderEntity info)
        {
            if (!info.HasSaveProperty(it => it.Account.Id))
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            if (info.Account != null && info.Account.Id!=0)
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

    
        
 
        #endregion

         
  
        #endregion


    }
}

using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities.Purchase;
using Beeant.Domain.Services;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Purchase
{
    public class PurchaseExpressApplicationService : RealizeApplicationService<PurchaseExpressEntity>
    {
        public IDomainService OrderExpressDomainService { get; set; }
        protected static readonly object Locker = new object();

        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<PurchaseExpressEntity> infos)
        {
            lock (Locker)
            {
                var unitofworks = Handle(infos);
                if (AddOrderExpress(infos, unitofworks))
                {
                    return Commit(unitofworks);
                }
                return false;
            }
        }

        /// <summary>
        /// 保存订单信息
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="unitofworks"></param>
        /// <returns></returns>
        protected virtual bool AddOrderExpress(IList<PurchaseExpressEntity> infos,IList<IUnitofwork> unitofworks)
        {
            var purchaseExpresses = GetPurchaseExpresses(infos);
            if (purchaseExpresses == null)
                return true;
            var orderExpresses = GetOrderExpresses(purchaseExpresses);
            if (orderExpresses == null || orderExpresses.Count==0)
                return true;
            IList<Domain.Entities.Order.OrderExpressEntity> expressEntities = new List<Domain.Entities.Order.OrderExpressEntity>();
            foreach (var purchaseExpress in purchaseExpresses)
            {
                if(purchaseExpress.Purchase==null || purchaseExpress.Purchase.Order==null )
                    continue;
                var orderExpress =
                    orderExpresses.FirstOrDefault(it => it.Order != null && it.Order.Id == purchaseExpress.Purchase.Order.Id);
                if(orderExpress==null)
                    continue;
                var expressEntity = new Domain.Entities.Order.OrderExpressEntity();
                if (string.IsNullOrEmpty(orderExpress.Number))
                {
                    expressEntity.Order = orderExpress.Order;
                    expressEntity.Number = purchaseExpress.Number;
                    expressEntity.Name = purchaseExpress.Name;
                    expressEntity.Id = orderExpress.Id;
                    expressEntity.SaveType = SaveType.Modify;
                    expressEntity.SetProperty(it => it.Number).SetProperty(it => it.Name);
                }
                else
                {
                    expressEntity.Order = orderExpress.Order;
                    expressEntity.Number = purchaseExpress.Number;
                    expressEntity.Name = purchaseExpress.Name;
                    expressEntity.Address = purchaseExpress.Address;
                    expressEntity.Mobile = purchaseExpress.Mobile;
                    expressEntity.Postcode = purchaseExpress.Postcode;
                    expressEntity.Recipient = purchaseExpress.Recipient;
                    expressEntity.Remark = purchaseExpress.Remark;
                    expressEntity.SaveType = SaveType.Add;
                }
                expressEntities.Add(expressEntity);
            }
           var units= OrderExpressDomainService.Handle(expressEntities);
            if (units == null)
                return false;
            unitofworks.AddList(units);
            return true;
        }
        /// <summary>
        /// 得到订单快递信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual IList<Domain.Entities.Order.OrderExpressEntity> GetOrderExpresses(IList<PurchaseExpressEntity> infos)
        {
           
            var orderIds =
                infos.Where(it => it.Purchase != null && it.Purchase.Order != null && it.Purchase.Order.Id > 0).Select(it => it.Purchase.Order.Id).ToArray();
            var query = new QueryInfo();
            query.Query<Domain.Entities.Order.OrderExpressEntity>()
                 .Where(it => orderIds.Contains(it.Order.Id))
                 .Select(it => new object[] { it.Id, it.Name,it.Number,it.Order.Id});
            return Repository.GetEntities<Domain.Entities.Order.OrderExpressEntity>(query);
        }
        /// <summary>
        /// 得到订单快递信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual IList<PurchaseExpressEntity> GetPurchaseExpresses(IList<PurchaseExpressEntity> infos)
        {
            var ids = infos.Where(it => it.SaveType == SaveType.Add && it.Purchase!=null && it.Purchase.Id>0).Select(it=>it.Purchase.Id).ToArray();
            if (ids.Length == 0) return null;
            var query = new QueryInfo();
            query.Query<PurchaseExpressEntity>()
                 .Where(it => ids.Contains(it.Purchase.Id) && it.Purchase.Type==PurchaseType.Sales)
                 .Select(it => new object[] {it, it.Purchase.Type,it.Purchase.Order.Id});
            return Repository.GetEntities<PurchaseExpressEntity>(query);
        }
        public long IdentityUserId
        {
            get { return ConfigurationManager.GetSetting<long>("SystemUserId"); }
        } 
    }
}

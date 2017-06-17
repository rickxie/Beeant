using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Order.OrderProduct
{
    public partial class Update : UpdatePageBase<OrderProductEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }
  
        protected override OrderProductEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                if (info.Order != null)
                    info.Order = Ioc.Resolve<IApplicationService>().GetEntity<OrderEntity>(info.Order.Id);
                if (info.Order==null || info.Order.Status != OrderStatusType.WaitHandle)
                    ((AuthorizePageBase)Page).InvalidateData("不能修改订单明细");
                if (info.Order==null || info.Order.Status != OrderStatusType.WaitDelivery)
                    if (info.Product!=null && info.Product.Id>0 && info.Order != null && info.Order.Type==OrderType.Standard)
                {
                    Edit1.PriceInput.Disabled = false;
                }
            }
                
            return info;
        }
    }
}
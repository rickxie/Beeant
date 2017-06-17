using Component.Extension;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Order.OrderPay
{
    public partial class Add : AddPageBase<OrderPayEntity>
    {


        protected override OrderPayEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Order = new OrderEntity { Id = Request.QueryString["OrderId"].Convert<long>() };
            }
            return info;
        }
        protected override void LoadEntity()
        {
            var order = this.GetEntity<OrderEntity>(Request.QueryString["OrderId"].Convert<long>());
            if (order.Status != OrderStatusType.WaitDelivery)
                base.LoadEntity();
        }
    }
}
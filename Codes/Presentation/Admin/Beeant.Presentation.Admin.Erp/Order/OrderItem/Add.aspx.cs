using Component.Extension;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Erp.Order.OrderItem
{
    public partial class Add : AddPageBase<OrderItemEntity>
    {


        protected override OrderItemEntity FillEntity()
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
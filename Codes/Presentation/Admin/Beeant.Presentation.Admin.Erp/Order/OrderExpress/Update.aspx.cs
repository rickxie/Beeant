using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Erp.Order.OrderExpress
{
    public partial class Update : UpdatePageBase<OrderExpressEntity>
    {
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
 
        protected override OrderExpressEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                if (info.Order != null)
                    info.Order = Ioc.Resolve<IApplicationService>().GetEntity<OrderEntity>(info.Order.Id);
                if (info.Order == null || info.Order.Status != OrderStatusType.WaitDelivery)
                    ((AuthorizePageBase)Page).InvalidateData("不能修改订单快递信息");
            }
            return info;
        }
    }
}
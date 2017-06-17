using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Order.OrderInvoice
{
    public partial class Update : UpdatePageBase<OrderInvoiceEntity>
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
 
        protected override OrderInvoiceEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                if (info.Order != null)
                    info.Order = Ioc.Resolve<IApplicationService>().GetEntity<OrderEntity>(info.Order.Id);
                if (info.Order == null || info.Order.Status != OrderStatusType.WaitHandle)
                    ((AuthorizePageBase)Page).InvalidateData("不能修改订单发票信息");
            }
            return info;
        }
    }
}
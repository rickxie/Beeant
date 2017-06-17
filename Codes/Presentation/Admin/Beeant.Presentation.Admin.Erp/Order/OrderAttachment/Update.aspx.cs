using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Order.OrderAttachment
{
    public partial class Update : UpdatePageBase<OrderAttachmentEntity>
    {
        public override bool IsUpdatePanel
        {
            get
            {
                return false;
            }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        /// <summary>
        /// 给ID赋值
        /// </summary>
        /// <returns></returns>
        protected override OrderAttachmentEntity FillEntity()
        {
            var info = base.FillEntity();
            info.Order = Ioc.Resolve<IApplicationService, OrderEntity>().GetEntity<OrderEntity>(Request.QueryString["orderid"].Convert<long>());
            return info;
        }

        protected override OrderAttachmentEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null)
            {
                if (info.Order != null)
                    info.Order = Ioc.Resolve<IApplicationService>().GetEntity<OrderEntity>(info.Order.Id);
                if (info.Order == null || info.Order.Status != OrderStatusType.WaitHandle)
                    ((AuthorizePageBase)Page).InvalidateData("不能修改订单附件");
            }
            return info;
        }
    }
}
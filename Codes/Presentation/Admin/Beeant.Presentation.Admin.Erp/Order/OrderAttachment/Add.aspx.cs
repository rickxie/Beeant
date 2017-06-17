using Component.Extension;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Order.OrderAttachment
{
    public partial class Add : AddPageBase<OrderAttachmentEntity>
    {
        /// <summary>
        /// 给ID赋值
        /// </summary>
        /// <returns></returns>
        protected override OrderAttachmentEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info.SaveType == SaveType.Add)
                info.Order = new OrderEntity { Id = Request["OrderId"].Convert<long>() };
            return info;
        }
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

        protected override void LoadEntity()
        {
            var order = this.GetEntity<OrderEntity>(Request.QueryString["OrderId"].Convert<long>());
            if (order.Status != OrderStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("不能新增订单附件");

            base.LoadEntity();
        }


    }
}
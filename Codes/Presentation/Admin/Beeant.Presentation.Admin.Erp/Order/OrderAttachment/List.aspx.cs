using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Order.OrderAttachment
{
    public partial class List : ListPageBase<OrderAttachmentEntity>
    {
        /// <summary>
        /// 根据订单Id来查询游客记录
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<OrderAttachmentEntity>().Where(it => it.Order.Id == Request.QueryString["OrderId"].Convert<long>());
            base.SetQueryWhere(query);

        }
        protected override void LoadData()
        {
            var order = this.GetEntity<OrderEntity>(Request.QueryString["OrderId"].Convert<long>());
            if (order.Status == OrderStatusType.WaitHandle)
                base.LoadData();
        }
    }
}
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.StockItem
{
    public partial class List : ListPageBase<StockItemEntity>
    {
        public long StockId
        {
            get { return Request.QueryString["StockId"].Convert<long>(); }
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<StockItemEntity>().Where(it => it.Stock.Id == StockId);
            base.SetQueryWhere(query);
        }

        protected override void LoadData()
        {
            var order = this.GetEntity<StockEntity>(StockId);
            if (order.Status != StockStatusType.WaitHandle)
                ((AuthorizePageBase) Page).InvalidateData("您没有权限在该状态下操作出入库明细");
            base.LoadData();
        }

    }
}
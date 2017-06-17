using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Wms.Wms.Inventory
{
    public partial class List : ListPageBase<InventoryEntity>
    {
        protected override void SetQueryWhere(QueryInfo query)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["StorehouseId"]))
                query.Query<InventoryEntity>().Where(it => it.Storehouse.Id == Request.QueryString["StorehouseId"].Convert<long>());
            base.SetQueryWhere(query);
        }
    }
}
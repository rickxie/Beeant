using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Ajax.Wms
{
    public partial class Inventory : AjaxPageBase<InventoryEntity>
    {
      
        protected override string GetListItem(InventoryEntity info)
        {
            return string.Format("Count: '{0}'"
                                 , info.Count);
        }

        protected override void SetQuerySelect(QueryInfo query)
        {
            query.Query<InventoryEntity>().Select(it => it.Count);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<InventoryEntity>().Where(it => it.Storehouse.Id == (Request.QueryString["StorehouseId"].Convert<long>()) && it.Product.Id == (Request.QueryString["ProductId"].Convert<long>()));
        }

        protected override System.Collections.Generic.IList<InventoryEntity> GetEntities()
        {
            var query = new QueryInfo { FromExp = typeof(InventoryEntity).FullName };
            SetQuery(query);
            return Ioc.Resolve<IApplicationService, InventoryEntity>().GetEntities<InventoryEntity>(query);
        }

    }
}
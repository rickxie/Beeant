using System.Collections.Generic;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Report.Ajax.ZTree
{
    public partial class OnlyWarehouseWmsAsyncTree : AsyncTree
    {
        protected override IList<BaseEntity> GetEntities()
        {
            List<BaseEntity> result = new List<BaseEntity>();
            var query = new QueryInfo();
            query.Query<StorehouseEntity>();
            query.WhereExp = string.Format("IsWarehouse==@IsWarehouse && IsUsed==@IsUsed && {0}==@ParentKey", DataParentField);
            query.SetParameter("IsWarehouse", 1);
            query.SetParameter("IsUsed", 1);
            query.SetParameter("ParentKey", IdKey.Length > 0 ? IdKey : DefaultParentKey);
            query.SelectExp = ObjectFields;
            return Ioc.Resolve<IApplicationService>().GetEntities<BaseEntity>(query);
        }
    }
}
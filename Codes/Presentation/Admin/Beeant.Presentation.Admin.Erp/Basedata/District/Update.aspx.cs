using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Basedata;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.District
{
    public partial class Update : UpdatePageBase<DistrictEntity>
    {
        protected override DistrictEntity GetEntity()
        {
            if (RequestId==0)
                return null;
            var query = new QueryInfo();
            query.Query<DistrictEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Parent.Name, it });
            return Ioc.Resolve<IApplicationService, DistrictEntity>().GetEntities<DistrictEntity>(query).FirstOrDefault();

        }
    }
}
using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Basedata.District
{
    public partial class Add : AddPageBase<DistrictEntity>
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
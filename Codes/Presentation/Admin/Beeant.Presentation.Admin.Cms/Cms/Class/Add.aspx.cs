using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Cms.Cms.Class
{
    public partial class Add : AddPageBase<ClassEntity>
    {

        protected override ClassEntity GetEntity()
        {
            if (RequestId==0)
                return null;
            var query = new QueryInfo();
            query.Query<ClassEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Parent.Name, it });
            return Ioc.Resolve<IApplicationService, ClassEntity>().GetEntities<ClassEntity>(query).FirstOrDefault();

        }
    }
}
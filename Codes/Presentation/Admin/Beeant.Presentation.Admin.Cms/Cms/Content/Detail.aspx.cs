using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Cms;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Cms.Cms.Content
{
    public partial class Detail : DetailPageBase<ContentEntity>
    {
      
        protected override ContentEntity GetEntity()
        {
            if (RequestId==0)
                return null;
            var query = new QueryInfo();
            query.Query<ContentEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Class.Name, it });
            var info= Ioc.Resolve<IApplicationService, ContentEntity>().GetEntities<ContentEntity>(query).FirstOrDefault();
            return info;
        }
    }
}
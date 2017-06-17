using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.Property
{
    public partial class Update : UpdatePageBase<PropertyEntity>
    {
        protected override PropertyEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<PropertyEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Category.Name, it });
            return Ioc.Resolve<IApplicationService, PropertyEntity>().GetEntities<PropertyEntity>(query).FirstOrDefault();

        }
      
    }
}
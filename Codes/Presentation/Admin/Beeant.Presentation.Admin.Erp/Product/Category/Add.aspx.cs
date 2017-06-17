using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Product.Category
{
    public partial class Add : AddPageBase<CategoryEntity>
    {

        protected override CategoryEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<CategoryEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Parent.Name, it });
            return Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntities<CategoryEntity>(query).FirstOrDefault();
          
        }
    }
}
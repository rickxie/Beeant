using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Product.Product
{
    public partial class Detail : DetailPageBase<ProductEntity>
    {
 

        protected override ProductEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<ProductEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it,it.Goods.Name, it.Goods.Category.Name, it.IsCustom });
            var infos= Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query);
            var info = infos == null ? null : infos.FirstOrDefault();
            return info;
        }
       

       
    }
}
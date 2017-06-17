using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.Product
{
    public partial class Update : UpdatePageBase<ProductEntity>
    {
        protected override void Page_Load(object sender, System.EventArgs e)
        {

            base.Page_Load(sender, e);
        }

        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }

        protected override ProductEntity GetEntity()
        {
            if (RequestId == 0)
                return null;
            var query = new QueryInfo();
            query.Query<ProductEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it, it.Goods.Category.Name});
            var info= Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query).FirstOrDefault();
            return info;
            
        }

    }
}
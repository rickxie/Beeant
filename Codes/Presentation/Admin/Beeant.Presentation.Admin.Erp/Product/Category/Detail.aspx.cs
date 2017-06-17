using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.Category
{
    public partial class Detail : DetailPageBase<CategoryEntity>
    {
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                ddlPropertyType.LoadData();
            }
        }
        protected override CategoryEntity GetEntity()
        {
            if (RequestId==0)
                return null;
            var query = new QueryInfo();
            query.Query<CategoryEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Parent.Name, it });
            return Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntities<CategoryEntity>(query).FirstOrDefault();

        }

    }
}
using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.PropertyRule
{
    public partial class Update : UpdatePageBase<PropertyRuleEntity>
    {
        public override bool IsFillAllEntity
        {
            get { return false; }
            set
            {
                base.IsFillAllEntity = value;
            }
        }
        protected override PropertyRuleEntity GetEntity()
        {
            if (RequestId==0)
                return null;
            var query = new QueryInfo();
            query.Query<PropertyRuleEntity>().Where(it => it.Id == RequestId)
                .Select(it => new object[] { it.Property.Name, it });
            return Ioc.Resolve<IApplicationService, PropertyRuleEntity>().GetEntities<PropertyRuleEntity>(query).FirstOrDefault();

        }
        protected override PropertyRuleEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.SetProperty(it => it.Type);
            }
            return info;
        }
  
    }
}
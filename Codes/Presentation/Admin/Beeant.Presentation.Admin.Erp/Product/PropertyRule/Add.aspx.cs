using Component.Extension;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.PropertyRule
{
    public partial class Add : AddPageBase<PropertyRuleEntity>
    {

        protected override PropertyRuleEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Property = new PropertyEntity { Id = Request["propertyid"].Convert<long>() };
            }
            return info;
        }
   
    }
}
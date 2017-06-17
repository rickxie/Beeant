using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Ajax.Product
{
    public partial class Product : AjaxPageBase<ProductEntity>
    {
        protected override string GetListItem(ProductEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Id);
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where("IsSales");
            if (!string.IsNullOrEmpty(Request.QueryString["Name"]))
            {
                query.Where(string.Format("{0} && Name.StartsWith(@Name)", query.WhereExp));
                query.SetParameter("Name", Server.UrlDecode(Request.QueryString["Name"]));              
            }
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id,Name";
        }
    }
}
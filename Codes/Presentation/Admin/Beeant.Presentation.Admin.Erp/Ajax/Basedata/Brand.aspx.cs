using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Ajax.Basedata
{
    public partial class Brand : AjaxPageBase<BrandEntity>
    {
        protected override string GetListItem(BrandEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Name);
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where("IsUsed==@IsUsed").SetParameter("IsUsed", true);
            if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                query.Where(string.Format("{0} && (Name.StartsWith(@Name))", query.WhereExp));
                query.SetParameter("Name", Server.UrlDecode(Request.QueryString["name"]));

            }
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Name";
        }
    }
}
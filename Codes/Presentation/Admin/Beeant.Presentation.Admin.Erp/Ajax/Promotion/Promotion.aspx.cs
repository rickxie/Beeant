using System.Linq;
using Beeant.Domain.Entities.Promotion;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Ajax.Promotion
{
    public partial class Promotion : AjaxPageBase<PromotionEntity>
    {
        protected override string GetListItem(PromotionEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Id);
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<PromotionEntity>().Where(it => it.IsUsed);
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
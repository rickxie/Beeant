using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cloud.Ajax.Account
{
    public partial class Account : AjaxPageBase<AccountEntity>
    {

        protected override string GetListItem(AccountEntity info)
        {
           return string.Format("Text:'{0}|{1}',Value:'{2}'",info.Name,info.RealName, info.Id);
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where("IsUsed");
            if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                query.Where(string.Format("{0} && (Name.StartsWith(@Name) || RealName.StartsWith(@Name))", query.WhereExp));
                query.SetParameter("Name", Server.UrlDecode(Request.QueryString["name"]));
            }
      
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id,Name,RealName";
        }
    }
}
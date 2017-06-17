using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Finance.Ajax.User
{
    public partial class User : AjaxPageBase<UserEntity>
    {
        protected override string GetListItem(UserEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Id);
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id, Name, RealName";
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where("IsUsed==@IsUsed").SetParameter("IsUsed", true);
            if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                query.Where(string.Format("{0} && (Name.StartsWith(@Name) || RealName.StartsWith(@Name))", query.WhereExp));
                query.SetParameter("Name", Server.UrlDecode(Request.QueryString["name"]));

            }
        }
    }
}
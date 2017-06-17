using Component.Extension;
using Beeant.Domain.Entities.Supplier;

using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Ajax.Supplier
{
    public partial class SupplierAccount : AjaxPageBase<SupplierEntity>
    {
        protected override string GetListItem(SupplierEntity info)
        {
            return string.Format("Text:'{0}', Value:'{1}'", info.Name, info.Account.Id);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where(string.Format("Account.Id!=@AccountId")).SetParameter("AccountId", 0);
            if (Request.QueryString["userId"] != null && Request.QueryString["userId"].Convert<long>() > 0)
            {
                query.Where(string.Format("{0} && UserId==@UserId ", query.WhereExp))
                   .SetParameter("UserId", Request.QueryString["userId"].Convert<long>());
            }
            if (Request.QueryString["Status"] == "Effective")
            {
                query.Where(string.Format("{0} && Status==@Status ", query.WhereExp))
                        .SetParameter("Status", SupplierStatusType.Effective);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Name"]))
            {
                var id = Request.QueryString["Name"].Convert<long>();
                query.Where(string.Format("{0} && (Name.Contains(@Name) || Id==@Id)", query.WhereExp));
                query.SetParameter("Name", Server.UrlDecode(Request.QueryString["Name"]))
                     .SetParameter("Id", id);

            }
        }

        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id, Name, Account.Id, Account.Name";
        }
    }
}
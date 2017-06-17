using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Ajax.Supplier
{
    public partial class Supplier : AjaxPageBase<SupplierEntity>
    {
        protected override string GetListItem(SupplierEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Id);
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where("Status==@Status").SetParameter("Status", SupplierStatusType.Effective);
            if (Request.QueryString["ServiceId"] != null && Request.QueryString["ServiceId"].Convert<long>() > 0)
                query.Query<SupplierEntity>().Where(it => it.Service.Id == Request.QueryString["ServiceId"].Convert<long>());
            if (!string.IsNullOrEmpty(Request.QueryString["Name"]))
            {
                query.Where(string.Format("{0} && Name.Contains(@Name)", query.WhereExp));
                query.SetParameter("Name", Server.UrlDecode(Request.QueryString["Name"]));

            }

        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id,Name";
        }
    }
}
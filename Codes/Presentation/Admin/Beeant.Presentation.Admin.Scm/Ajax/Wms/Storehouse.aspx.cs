using Beeant.Domain.Entities.Wms;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Ajax.Wms
{
    public partial class Storehouse : AjaxPageBase<StorehouseEntity>
    {
        protected override string GetListItem(StorehouseEntity info)
        {
            return string.Format("Text:'{0}',Value:'{1}'", info.Name, info.Id);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["IsUsed"]))
            {
                query.Where("IsUsed==@IsUsed").SetParameter("IsUsed", true);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["IsWarehouse"]))
            {
                if (string.IsNullOrEmpty(query.WhereExp))
                    query.Where("IsWarehouse==@IsWarehouse").SetParameter("IsWarehouse", true);
                else
                    query.Where(string.Format("{0} && IsWarehouse==@IsWarehouse",query.WhereExp)).SetParameter("IsWarehouse", true);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                if (string.IsNullOrEmpty(query.WhereExp))
                    query.Where("Name.StartsWith(@Name)");
                else
                    query.Where(string.Format("{0} && Name.StartsWith(@Name)", query.WhereExp));
                query.SetParameter("Name", Server.UrlDecode(Request.QueryString["name"]));

            }
        }
    }
}
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Supplier;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Ajax.Finance
{
    public partial class Bank : AjaxPageBase<BankEntity>
    {
        protected override System.Collections.Generic.IList<BankEntity> GetEntities()
        {
            if (string.IsNullOrEmpty(Request.QueryString["AccountId"])) return null;
            return base.GetEntities();
        }
        protected override string GetListItem(BankEntity info)
        {
            return string.Format("Text:'{0}|{1}|{2}|{3}',Value:'{0}'", info.Number, info.Name,info.Holder,info.Remark);
        }
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.SelectExp = "Id,Number,Name,Holder,Remark";
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Where("Account.Id==@AccountId").SetParameter("AccountId", Request.QueryString["AccountId"]);
            if (!string.IsNullOrEmpty(Request.QueryString["Number"]))
            {
                query.Where(string.Format("{0} && Number.Contains(@Number)", query.WhereExp));
                query.SetParameter("Number", Server.UrlDecode(Request.QueryString["Number"]));

            }
        }
    }
}
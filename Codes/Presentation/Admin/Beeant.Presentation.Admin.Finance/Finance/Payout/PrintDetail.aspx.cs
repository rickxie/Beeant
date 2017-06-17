using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Payout
{
    public partial class PrintEntity : DetailPageBase<PayoutEntity>
    {
        protected override PayoutEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            var query = new QueryInfo();
            query.Query<PurchasePayEntity>().Where(it => it.Payout.Id == Request.QueryString["id"].Convert<long>());
            info.Pays = Ioc.Resolve<IApplicationService, PurchasePayEntity>().GetEntities<PurchasePayEntity>(query);
  
            return info;
           
        }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            base.Page_Load(sender, e);
            var info = GetEntity();
            var PurchaseIds = info.Pays.Select(it => it.Purchase.Id);
            string strPurchaseIds = string.Join(",", PurchaseIds.ToList());
            lblPurchasheId.Text = strPurchaseIds;

        }
    }
}
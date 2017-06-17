using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Payin
{
    public partial class PrintEntity : DetailPageBase<PayinEntity>
    {
            protected override PayinEntity GetEntity()
            {
                var info = base.GetEntity();
                if (info != null && info.Account != null)
                {
                    info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
                }
                var query = new QueryInfo();
                query.Query<PayinItemEntity>().Where(it => it.Payin.Id == Request.QueryString["id"].Convert<long>());
                info.PayinItems = Ioc.Resolve<IApplicationService, PayinItemEntity>().GetEntities<PayinItemEntity>(query);

                return info;
            }
            protected override void Page_Load(object sender, System.EventArgs e)
            {
                base.Page_Load(sender, e);
                var info = GetEntity();
                var ordersId = info.Pays.Select(it => it.Order.Id);
                string strPurchaseIds = string.Join(",", ordersId.ToList());
                lblOrdersId.Text = strPurchaseIds;

            }
    }
}
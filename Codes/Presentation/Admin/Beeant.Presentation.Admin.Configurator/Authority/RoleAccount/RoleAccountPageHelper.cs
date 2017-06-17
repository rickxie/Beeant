using System.Web.UI;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Dependent;

namespace Beeant.Presentation.Admin.Configurator.Authority.RoleAccount
{
    public static class RoleAccountPageHelper
    {

        public static string GetAccountName(this Page page)
        {
            if (!string.IsNullOrEmpty(page.Request["Accountid"]))
            {
                var info = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(page.Request["Accountid"].Convert<long>());
                if (info != null) return string.Format("{0}-", info.Name);
            }
            return "";
        }
    }
}
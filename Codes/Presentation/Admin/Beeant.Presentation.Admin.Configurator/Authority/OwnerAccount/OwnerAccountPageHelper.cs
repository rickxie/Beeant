using System.Web.UI;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Management;
using Dependent;

namespace Beeant.Presentation.Admin.Configurator.Authority.OwnerAccount
{
    public static class OwnerAccountPageHelper
    {

        public static string GetAccountName(this Page page)
        {
            if (!string.IsNullOrEmpty(page.Request["Accountid"]))
            {
                var info = Ioc.Resolve<IApplicationService, UserEntity>().GetEntity<UserEntity>(page.Request["Accountid"].Convert<long>());
                if (info != null) return string.Format("{0}-", info.Name);
            }
            return "";
        }
    }
}
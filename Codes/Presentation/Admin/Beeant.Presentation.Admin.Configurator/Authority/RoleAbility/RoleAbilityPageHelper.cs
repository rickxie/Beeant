using System.Web.UI;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Authority;
using Dependent;

namespace Beeant.Presentation.Admin.Configurator.Authority.RoleAbility
{
    public static class RoleAbilityPageHelper
    {

        public static string GetRoleName(this Page page)
        {
            if (!string.IsNullOrEmpty(page.Request["roleid"]))
            {
                var info = Ioc.Resolve<IApplicationService, RoleEntity>().GetEntity<RoleEntity>(page.Request["roleid"].Convert<long>());
                if (info != null) return string.Format("{0}-", info.Name);
            }
            return "";
        }
   
    }
}
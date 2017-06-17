using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Authority;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Authority.RoleAbility
{
    public partial class Delete : ListPageBase<RoleAbilityEntity>
    {

        protected void btnIsForbid_Click(object sender, System.EventArgs e)
        {
            SaveEntities(SaveType.Modify, "修改成功", "修改失败", ddlIsForbid);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<RoleAbilityEntity>().Where(
                it => it.Role.Id == Request["roleid"].Convert<long>());
            base.SetQueryWhere(query);
        }
      
    }
}
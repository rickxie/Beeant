using System.Linq;
using Beeant.Domain.Entities.Authority;
using Component.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Authority.RoleAccount
{
    public partial class Delete : Basic.Services.WebForm.Pages.ListPageBase<RoleAccountEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (string.IsNullOrEmpty(Request["Accountid"])) return;
            query.Query<RoleAccountEntity>().Where(
                it => it.Account.Id == Request["Accountid"].Convert<long>());

        }

        public override void Remove_Click(object sender, System.EventArgs e)
        {
            SaveEntities(SaveType.Remove, "回收成功", "回收失败");
        }
        
    }
}
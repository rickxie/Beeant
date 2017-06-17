using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Workflow.AuditorAccount
{
    public partial class Delete : Basic.Services.WebForm.Pages.ListPageBase<AuditorAccountEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (string.IsNullOrEmpty(Request["Accountid"])) return;
            query.Query<AuditorAccountEntity>().Where(
                it => it.Account.Id == Request["Accountid"].Convert<long>());

        }

        public override void Remove_Click(object sender, System.EventArgs e)
        {
            SaveEntities(SaveType.Remove, "回收成功", "回收失败");
        }
        
    }
}
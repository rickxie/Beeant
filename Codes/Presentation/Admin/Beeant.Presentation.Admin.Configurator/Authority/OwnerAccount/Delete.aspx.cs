using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Authority.OwnerAccount
{
    public partial class Delete : Basic.Services.WebForm.Pages.ListPageBase<OwnerAccountEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            if (string.IsNullOrEmpty(Request["accountid"])) return;
            query.Query<OwnerAccountEntity>().Where(
                it => it.Account.Id == Request["accountid"].Convert<long>());

        }

        public override void Remove_Click(object sender, System.EventArgs e)
        {
            SaveEntities(SaveType.Remove, "回收成功", "回收失败");
        }
        
    }
}
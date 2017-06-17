using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Api;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Configurator.Api.VoucherProtocol
{
    public partial class Delete : ListPageBase<VoucherProtocolEntity>
    {

        protected void btnIsForbid_Click(object sender, System.EventArgs e)
        {
            SaveEntities(SaveType.Modify, "修改成功", "修改失败", ddlIsForbid);
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<VoucherProtocolEntity>().Where(
                it => it.Voucher.Id == Request["VoucherId"].Convert<long>());
            base.SetQueryWhere(query);
        }
      
    }
}
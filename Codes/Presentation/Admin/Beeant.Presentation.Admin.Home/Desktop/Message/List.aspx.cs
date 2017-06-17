using System.Linq;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Workflow;
using Dependent;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Home.Desktop.Message
{
    public partial class List : ListPageBase<MessageEntity>
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        protected override void SetQueryWhere(QueryInfo query)
        {

            if (Identity.Id != 0)
            {
                query.Query<MessageEntity>().Where(it => it.Account.Id == Identity.Id);
            }

            
            base.SetQueryWhere(query);
            
        }

        protected virtual void ShowMessageDetail(long id)
        {
            var info = Ioc.Resolve<IApplicationService, MessageEntity>().GetEntity<MessageEntity>(id);
            info.SetProperty("IsRead");
            info.IsRead = true;
            info.SaveType = SaveType.Modify;
            Ioc.Resolve<IApplicationService, MessageEntity>().Save(info);
            LoadData();
            this.ShowMessage("流程信息", info.Title);
        }

        protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ReadMessage") && e.CommandArgument!=null)
            {
                ShowMessageDetail(e.CommandArgument.Convert<long>());
            }
        }
    }
}
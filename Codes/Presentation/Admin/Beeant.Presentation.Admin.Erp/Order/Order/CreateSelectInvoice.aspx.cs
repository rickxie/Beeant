using System.Linq;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Member;
using Component.Extension;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class CreateSelectInvoice : AddPageBase<InvoiceEntity>
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        public override System.Web.UI.WebControls.Button SaveButton
        {
            get { return btnSave; }
            set
            {
                base.SaveButton = value;
            }
        }
        public override MessageControlBase MessageControl
        {
            get { return Message1; }
            set
            {
                base.MessageControl = value;
            }
        }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.LoadData();
                ddlGeneralType.LoadData();

            }
            base.Page_Load(sender, e);
        }
        protected override InvoiceEntity GetEntity()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["AccountId"]))
            {
                var query = new QueryInfo();
                query.Query<InvoiceEntity>().Where(it => it.Account.Id == Request.QueryString["AccountId"].Convert<long>() && it.Type == InvoiceType.Vat);
                var infos = Ioc.Resolve<IApplicationService, InvoiceEntity>().GetEntities<InvoiceEntity>(query);
                return infos == null ? null : infos.FirstOrDefault();
            }
            return base.GetEntity();
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <returns></returns>
        protected override InvoiceEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.Account = new AccountEntity { Id = Request.QueryString["AccountId"].Convert<long>() };
            }
            return info;
        }

        protected override void SetResult(bool rev, System.Collections.Generic.IList<Winner.Filter.ErrorInfo> errors)
        {
            base.SetResult(rev, errors);
            if (rev)
            {
                this.ExecuteScript(string.Format("parent.window.note.CloseButton.click();parent.document.getElementById('{0}').click();", Request["btn"]));
            }
        }
    }
}

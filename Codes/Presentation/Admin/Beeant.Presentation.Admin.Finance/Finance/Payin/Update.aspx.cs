using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Finance.Finance.Payin
{
    public partial class Update : WorkflowPageBase<PayinEntity>
    {

      
        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript(string.Format("validator.BindControlValidateEvent(document.getElementById('{0}'), 'change');", txtPayTime.ClientID));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            cbBank.AccountInputHidden = cbAccount.InputHidden;
            cbBank.BankHolderText = txtBankHolder;
            cbBank.BankNameText = txtBankName;
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCurrency.LoadData();
                ddlPayType.LoadData();
            }
            base.Page_Load(sender, e);
        }
        protected override PayinEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
    }
}
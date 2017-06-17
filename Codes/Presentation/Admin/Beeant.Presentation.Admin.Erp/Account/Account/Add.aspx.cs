using System.Collections.Generic;
using System.Text;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Account.Account
{
    public partial class Add : AddPageBase<AccountEntity>
    {
        public override System.Web.UI.WebControls.Button SaveButton
        {
            get
            {
                return btnSave;
            }
            set
            {
                base.SaveButton = value;
            }
        }

 
        protected override void SetValidScriptString()
        {
            base.SetValidScriptString();
            var script = new StringBuilder();
            script.Append(ValidScriptString);
            script.Append("validator.AddValidateEntity({");
            script.AppendFormat("Control:document.getElementById('{0}'),Message:'两次输入密码不一致',ValidateEvent:'blur'", txtSurePassword.ClientID);
            script.Append("}).Handles.push({Function:function(){");
            script.AppendFormat("return document.getElementById('{0}').value==document.getElementById('{1}').value;", txtSurePassword.ClientID, txtPassword.ClientID);
            script.Append("},Message:'两次输入密码不一致'});");
            ValidScriptString = script.ToString();

        }
        protected override void Save()
        {
            if (!txtPassword.Value.Equals(txtSurePassword.Value))
            {
                this.ShowMessage("验证错误","两次输入密码不一致");
                return;
            }
            base.Save();
        }
        protected override AccountEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.Payword = "";
                info.IsActiveEmail = false;
                info.IsActiveMobile = false;
                info.IsReality = true;
                info.AccountIdentites = new List<AccountIdentityEntity>();
                info.AccountIdentites.Add(new AccountIdentityEntity
                {
                    Account = info,
                    Name = "Name",
                    Number = info.Name,
                    SaveType = SaveType.Add
                });
            }
            return info;
        }
     
    }
}
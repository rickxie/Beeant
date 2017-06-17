using System;
using System.Text;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Domain.Entities.Account;

namespace Beeant.Presentation.Admin.Configurator.Management.User
{
    public partial class Passsword : Basic.Services.WebForm.Pages.UpdatePageBase<AccountEntity>
    {
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
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
                this.ShowMessage("验证错误", "两次输入密码不一致");
                return;
            }
            base.Save();
        }

        protected override void OnPreLoad(EventArgs e)
        {
            SaveButton = btnSave;
            base.OnPreLoad(e);
        }
    }
}
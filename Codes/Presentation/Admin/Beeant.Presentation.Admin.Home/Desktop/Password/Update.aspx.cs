using System;
using System.Text;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;

namespace Beeant.Presentation.Admin.Home.Desktop.Password
{
    public partial class Update : UpdatePageBase<AccountEntity>
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
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        public override long RequestId
        {
            get
            {
                return Identity.Id;
            }
            set
            {
                base.RequestId = value;
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
            script.Append("validator.AddValidateEntity({");
            script.AppendFormat("Control:document.getElementById('{0}'),Message:'原始密码不能为空',ValidateEvent:'blur'", txtOriginPassword.ClientID);
            script.Append("}).Handles.push({Function:function(){");
            script.AppendFormat("return document.getElementById('{0}').value!='';", txtOriginPassword.ClientID);
            script.Append("},Message:'原始密码不能为空'});");
            ValidScriptString = script.ToString();

        }

        protected override void Save()
        {
            if (!txtPassword.Value.Equals(txtSurePassword.Value))
            {
                this.ShowMessage("验证错误", "两次输入密码不一致");
                return;
            }
            if (string.IsNullOrEmpty(txtOriginPassword.Value))
            {
                this.ShowMessage("验证错误", "原始密码错误");
                return;
            }
            var login = new Domain.Entities.Utility.LoginEntity {Name=Identity.Name,Password= txtOriginPassword.Value };
            var info = Ioc.Resolve<ILoginApplicationService>().Login(login);
            if (info == null || info.Errors!=null &&info.Errors.Count>0)
            {
                this.ShowMessage("验证错误", "原始密码错误");
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
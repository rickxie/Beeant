using System;
using System.Linq;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Utility;
using Dependent;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.Common.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Configurator
{
    public partial class Default : System.Web.UI.Page
    {


        private const string CodeName = "ConfiguratorErrorCodeCount";
        public virtual bool IsShowCode
        {
            get
            {
                var count = CodeHelper.GetErrorCount(CodeName);
                if (count == null || count >= 5)
                    return true;
                return false;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CodeHelper.InitilzeCodeErrorCount(CodeName);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsShowCode && !CodeHelper.ValidateCode(txtCode.Value, "ConfiguratorCode"))
            {
                this.ExecuteScript("alert(\'验证码错误\');");
                return;
            }
            var login = new LoginEntity { Name = txtUserName.Value.Trim(), Password = txtPassword.Value.Trim() };
            var info = Ioc.Resolve<ILoginApplicationService>().Login(login);
            var identity = new IdentityEntity { Name = login.Name };
            var error = info.Errors?.FirstOrDefault();
            if (info.Errors == null || info.Errors.Count == 0)
            {
                Ioc.Resolve<IIdentityApplicationService>().Set(info.Identity);
                identity = info.Identity;
                this.AddLoginLog(identity, error == null ? "" : error.Message);
                CodeHelper.RemoveCodeErrorCount(CodeName);
                this.RedirectDefaultPage();
            }
            else
            {
                CodeHelper.AddCodeErrorCount(CodeName);
                this.AddLoginLog(identity, error == null ? "" : error.Message);
                this.ExecuteScript(string.Format("alert('{0}')", info.Errors[0].Message), false);
            }
 

        }
      
    }
}
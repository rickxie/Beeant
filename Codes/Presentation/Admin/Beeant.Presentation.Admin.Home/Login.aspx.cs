using System;
using System.Linq;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Utility;
using Dependent;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Management;

namespace Beeant.Presentation.Admin.Home
{
    public partial class Login : System.Web.UI.Page
    {
     
        /// <summary>
        /// 登入错误次数
        /// </summary>
        public string Mark
        {
            get
            {
                return string.IsNullOrEmpty(Request.QueryString["Mark"])?null: Request.QueryString["Mark"].ToLower();
            }
        }
        /// <summary>
        /// 登入错误次数
        /// </summary>
        public string UserName
        {
            get
            {
                return string.IsNullOrEmpty(Request.QueryString["UserName"]) ? null : Request.QueryString["UserName"].ToLower();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginUser();
        }
        /// <summary>
        /// 登入
        /// </summary>

        protected virtual void LoginUser()
        {
            if (GetMark() != Mark)
            {
                Response.Redirect("Default.aspx");
                return;
            }
            var login = new LoginEntity { Name =UserName,Type= "IgnorePassword" };
            var info = Ioc.Resolve<ILoginApplicationService>().Login(login);
            var identity = new IdentityEntity { Name = login.Name };
            var error = info.Errors?.FirstOrDefault();
            if (info.Errors == null || info.Errors.Count == 0)
            {
                Ioc.Resolve<IIdentityApplicationService>().Set(info.Identity);
                identity = info.Identity;
                this.AddLoginLog(identity, error == null ? "" : error.Message);
                this.RedirectDefaultPage();
            }
            else
            {
                this.AddLoginLog(identity, error == null ? "" : error.Message);
                Response.Redirect("Default.aspx");
            }
          
     
        }
        /// <summary>
        /// 得到Mark
        /// </summary>
        /// <returns></returns>
        protected virtual string GetMark()
        {
            var c = string.IsNullOrEmpty(UserName) ? 'j':UserName[0];
            var mark = Winner.Creator.Get<Winner.Base.ISecurity>().EncryptMd5(DateTime.Now.Date.ToString());
            return mark.Remove(12, 1).Insert(12,c.ToString());
        }
    }
}
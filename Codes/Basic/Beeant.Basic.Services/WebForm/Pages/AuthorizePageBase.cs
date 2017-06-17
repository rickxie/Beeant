using System;
using System.Web;
using Configuration;
using Beeant.Application.Services.Utility;
using Dependent;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Management;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class AuthorizePageBase : System.Web.UI.Page
    {
        public virtual bool IsImageRecover { get; set; } = true;

        /// <summary>
        /// 是否验证资源
        /// </summary>
        public virtual bool IsVerifyResource
        {
            get
            {
                return ConfigurationManager.GetSetting<bool>("IsVerifyResource") ; }
        }

        private IdentityEntity _identity;
        /// <summary>
        /// 当前登入身份
        /// </summary>
        public virtual IdentityEntity Identity
        {
            get { return _identity ?? (_identity = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>()); }
        }

        protected override void OnInit(EventArgs e)
        {  
            Verify();
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (IsImageRecover)
            {
                this.ImageRecover();
            }
            base.OnPreRender(e);
        }
        
        /// <summary>
        /// 验证
        /// </summary>
        public virtual bool Verify()
        {
            if(Identity == null )  this.RedirectLoginPage();
            else if (!IsPostBack && IsVerifyResource && !this.VerifyResource(Identity.Id))
            {
                throw new HttpException(403, "您无权访问该资源");
            }
            return true;
        }

        /// <summary>
        /// 验证消息
        /// </summary>
        /// <param name="title"></param>
        public virtual void InvalidateData(string title)
        {

            this.Redirect(string.Format("{0}?title={1}",
                                       ConfigurationManager.GetSetting<string>("InvalidatePage")
                                        , Server.UrlEncode(title)));
        }
        /// <summary>
        /// 重写事件
        /// </summary>
        /// <param name="sourceControl"></param>
        /// <param name="eventArgument"></param>
        protected override void RaisePostBackEvent(System.Web.UI.IPostBackEventHandler sourceControl, string eventArgument)
        {
             if(Identity == null )  
                 this.RedirectLoginPage();
            if (IsVerifyResource && Identity!=null)
            {
                var info = this.GetVerification(Identity.Id);
                if (info.IsPass)
                {
                    var name = sourceControl.GetType().GetProperty("ClientID").GetValue(sourceControl,null).ToString().Replace("_", "$");
                    if(info.Controls.ContainsKey(name) && !info.Controls[name])
                        return;
                }
                else
                {
                    return;
                }
            }
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

       
    }
}

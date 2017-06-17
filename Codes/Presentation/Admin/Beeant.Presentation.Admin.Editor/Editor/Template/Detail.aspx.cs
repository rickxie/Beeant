using System;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Editor;
using Configuration;
using Dependent;

namespace Beeant.Presentation.Admin.Editor.Editor.Template
{
    public partial class Detail : Basic.Services.WebForm.Pages.AuthorizePageBase
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Write();
        }

    

        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Write()
        {
            if(string.IsNullOrEmpty(Request.QueryString["id"]))return;
            var info = Ioc.Resolve<IApplicationService, TemplateEntity>().GetEntity<TemplateEntity>(Request.QueryString["id"].Convert<long>());
            if(info!=null)
                Response.Write(string.Format("<html><head></head><body><script type='text/javascript'>document.domain='{0}';</script>{1}</body></html>", ConfigurationManager.GetSetting<string>("Domain"), info.Detail));
        }

    }
}
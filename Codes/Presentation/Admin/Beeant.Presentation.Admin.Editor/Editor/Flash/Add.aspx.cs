using System;
using System.Text;
using Beeant.Domain.Entities.Editor;
using Configuration;

namespace Beeant.Presentation.Admin.Editor.Editor.Flash
{
    public partial class Add : Basic.Services.WebForm.Pages.AuthorizePageBase
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Save();
        }

    

        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Save()
        {
            var info = this.SaveFlashEntity(Server.UrlDecode(Request.QueryString["Path"]),0);
            Response.Write(string.Format("<script type='text/javascript'>document.domain='{0}';{1};</script>", ConfigurationManager.GetSetting<string>("Domain"),
                                                 string.Format(Request["function"], info.FullFileName, GetErrorMessage(info))));
        }

        /// <summary>
        /// 得到错误信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string GetErrorMessage(FlashEntity info)
        {
            if (info == null || info.Errors == null || info.Errors.Count == 0) return "";
            var sb = new StringBuilder();
            foreach (var errorEntity in info.Errors)
            {
                sb.AppendFormat(@"{0}\r\n", errorEntity.Message);
            }
            return sb.ToString();
        }
    }
}
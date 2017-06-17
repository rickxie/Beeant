using System;
using System.Text;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Editor;
using Dependent;
using Beeant.Domain.Entities.Finance;
using Configuration;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Editor.Editor.Template
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
            var info = new TemplateEntity
                {
                    SaveType = SaveType.Add,
                    Account = new AccountEntity { Id = 0 },
                    Folder=new FolderEntity{Id=0},
                    Name = Request.Form["templatename"], Detail = Request.Form["editorvalue"]
                };
            Ioc.Resolve<IApplicationService, TemplateEntity>().Save(info);
            Response.Write(string.Format("<script type='text/javascript'>document.domain='{0}';{1};</script>", ConfigurationManager.GetSetting<string>("Domain"),
                                      string.Format(Request["function"], string.Format("/Editor/Template/Detail.aspx?id={0}", info.Id), GetErrorMessage(info))));
        }

        /// <summary>
        /// 得到错误信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string GetErrorMessage(TemplateEntity info)
        {
            if (info == null || info.HandleResult == true) return "";
            var sb = new StringBuilder();
            foreach (var errorEntity in info.Errors)
            {
                sb.AppendFormat(@"{0}\r\n", errorEntity.Message);
            }
            return sb.ToString();
        }
    }
}
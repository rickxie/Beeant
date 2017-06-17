using System;
using System.Text;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Editor;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Editor.Editor.Image
{
    public partial class Move : Basic.Services.WebForm.Pages.AuthorizePageBase
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
            var info = new ImageEntity {Id= Request["id"].Convert<long>() ,Folder = new FolderEntity { Id = Request["folderid"].Convert<long>() },SaveType=SaveType.Modify };
            info.SetProperty(it => it.Folder.Id);
            Ioc.Resolve<IApplicationService, ImageEntity>().Save<ImageEntity>(info);
            Response.Write(GetErrorMessage(info));
        }

        /// <summary>
        /// 得到错误信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string GetErrorMessage(ImageEntity info)
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
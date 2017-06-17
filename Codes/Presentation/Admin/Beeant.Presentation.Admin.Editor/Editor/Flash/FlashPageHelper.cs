using System.IO;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Editor;
using Dependent;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Editor.Editor.Flash
{
    public static class FlashPageHelper
    {
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="page"></param>
        /// <param name="path"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        static public FlashEntity GetFlashEntity(this AuthorizePageBase page, string path, long folderId)
        {
            if (string.IsNullOrEmpty(path) || page.Request.Files.Count == 0) return null;
            string fileName = string.Format("{0}{1}", path, Path.GetFileName(page.Request.Files[0].FileName));
            var info = new FlashEntity
                {
                    FileName = fileName,
                    Name = Path.GetFileName(page.Request.Files[0].FileName),
                    Folder = new FolderEntity { Id = folderId },
                    SaveType = SaveType.Add,
                    Account = new AccountEntity { Id = 0 },
                    FileByte = new byte[page.Request.Files[0].ContentLength]
                };
            page.Request.Files[0].InputStream.Read(info.FileByte, 0, info.FileByte.Length);
            return info;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="page"></param>
        /// <param name="path"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public static FlashEntity SaveFlashEntity(this AuthorizePageBase page, string path, long folderId)
        {
            var info = GetFlashEntity(page, path, folderId);
            if (info == null) return null;
            Ioc.Resolve<IApplicationService, FlashEntity>().Save(info);
            return info;
        }
    }
}
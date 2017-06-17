using System.IO;
using System.Web;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Editor;
using Dependent;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Editor.Editor.Image
{
    public static class ImagePageHelper
    {
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="page"></param>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        static public ImageEntity GetImageEntity(this AuthorizePageBase page, HttpPostedFile file, string path, long folderId)
        {
            if (string.IsNullOrEmpty(path) || file.ContentLength == 0) return null;
            string fileName = string.Format("{0}{1}", path, Path.GetFileName(file.FileName));
            var info = new ImageEntity
            {
                FileName = fileName,
                Name = Path.GetFileName(file.FileName),
                SaveType = SaveType.Add,
                Folder = new FolderEntity { Id = folderId },
                Account = new AccountEntity { Id = 0 },
                FileByte = new byte[file.ContentLength]
            };
            file.InputStream.Position = 0;
            file.InputStream.Read(info.FileByte, 0, info.FileByte.Length);
            return info;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="page"></param>
        /// <param name="path"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public static ImageEntity SaveImageEntity(this AuthorizePageBase page, string path, long folderId)
        {
            var info = GetImageEntity(page, page.Request.Files[0], path, folderId);
            if (info == null) return null;
            Ioc.Resolve<IApplicationService, ImageEntity>().Save<ImageEntity>(info);
            return info;
        }
    }
}
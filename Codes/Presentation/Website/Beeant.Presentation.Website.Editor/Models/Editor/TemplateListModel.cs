using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Editor;

namespace Beeant.Presentation.Website.Editor.Models.Editor
{
    public class TemplateListModel : PagerModel
    {
        /// <summary>
        /// 文件夹编号
        /// </summary>
        public string FolderId { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 文件夹
        /// </summary>

        public IList<FolderEntity> Folders { get; set; }
        /// <summary>
        /// flash
        /// </summary>

        public IList<TemplateEntity> Templates { get; set; }
    }
}
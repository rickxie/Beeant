using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Editor;

namespace Beeant.Presentation.Website.Editor.Models.Editor
{
    public class FolderListModel : PagerModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件夹
        /// </summary>

        public IList<FolderEntity> Folders { get; set; }
    
    }
}
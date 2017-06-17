using System.Collections.Generic;

namespace Beeant.Presentation.Mvc.Shared.Models.Home
{
    public class SearchModel
    {
        /// <summary>
        /// 是否张开
        /// </summary>
        public bool IsExpand { get; set; }   
        /// <summary>
        /// 搜索
        /// </summary>
        public string SearchKey { get; set; }
        /// <summary>
        /// 热门搜索
        /// </summary>
        public IList<string> HotKeys { get; set; }
 
    }
}
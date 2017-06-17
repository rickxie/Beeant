using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Website.Models.News
{
    public class NewsListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 12; }
        }
       
        /// <summary>
        /// 新闻
        /// </summary>
        public IList<NewsEntity> Newses { get; set; } 
      
    }
}

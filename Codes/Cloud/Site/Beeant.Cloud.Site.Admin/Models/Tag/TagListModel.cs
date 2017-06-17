using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Tag
{
    public class TagListModel:PagerModel
    {
        public override int PageSize { get; set; } = 50;
        /// <summary>
        /// 类目
        /// </summary>
        public IList<TagEntity> Tags { get; set; } 
    }
}
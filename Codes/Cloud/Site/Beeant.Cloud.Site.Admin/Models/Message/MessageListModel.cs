using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Message
{
    public class MessageListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 24; }
        }
        /// <summary>
        /// 类目
        /// </summary>
        public IList<MessageEntity> Messages { get; set; } 
    }
}
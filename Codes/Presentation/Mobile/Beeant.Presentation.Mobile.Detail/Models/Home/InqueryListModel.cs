using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Mobile.Detail.Models.Home
{
    public class CommentListModel : PagerModel
    {
        /// <summary>
        /// 查询
        /// </summary>
        public IList<CommentEntity> Comments { get; set; }

    }
}
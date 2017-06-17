using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Site;

namespace Beeant.Cloud.Site.Admin.Models.Inquery
{
    public class InqueryListModel : PagerModel
    {
        public override int PageSize { get; set; } = 24;

        /// <summary>
        /// 类目
        /// </summary>
        public IList<InqueryEntity> Inqueries { get; set; }
    }
}
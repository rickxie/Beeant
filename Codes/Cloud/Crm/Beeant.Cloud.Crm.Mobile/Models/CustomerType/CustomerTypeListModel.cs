using System.Collections.Generic;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Crm;

namespace Beeant.Cloud.Crm.Mobile.Models.CustomerType
{
    public class CustomerTypeListModel : PagerModel
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
        public IList<CustomerTypeEntity> CustomerTypes { get; set; } 
    }
}
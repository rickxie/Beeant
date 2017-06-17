using System.Collections.Generic;
using System.Web;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Crm;

namespace Beeant.Cloud.Crm.Mobile.Models.Customer
{
    public class CustomerListModel : PagerModel
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 24; }
        }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string TypeId { get; set; }
        /// <summary>
        /// 客户渠道
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// 是否只加载自己
        /// </summary>
        public bool? IsReadSelf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 类目
        /// </summary>
        public IList<CustomerChannelEntity> CustomerChannels { get; set; }
        /// <summary>
        /// 类目
        /// </summary>
        public IList<CustomerTypeEntity> CustomerTypes { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public IList<CustomerEntity> Customers { get; set; }

    
    }
}
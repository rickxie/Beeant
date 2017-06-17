using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Crm
{
    [Serializable]
    public class CustomerChannelEntity : BaseEntity<CustomerChannelEntity>
    {
        /// <summary>
        /// 客户信息
        /// </summary>
        public CrmEntity Crm { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public IList<CustomerEntity> Customers { get; set; } 
    }
}

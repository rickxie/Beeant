using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Crm
{
    [Serializable]
    public class CustomerNoteEntity : BaseEntity<CustomerNoteEntity>
    {
        
        /// <summary>
        /// 客户
        /// </summary>
        public CrmEntity Crm { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public CustomerEntity Customer { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 添加处理
        /// </summary>
        protected override void SetAddBusiness()
        {
            InvokeItemLoader("Customer");
            if (Customer == null)
                return;
            Crm = Customer.Crm;
            base.SetAddBusiness();
        }
    }
}

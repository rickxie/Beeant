using System;

namespace Beeant.Domain.Entities.Crm
{
    [Serializable]
    public class ContractEntity : BaseEntity<ContractEntity>
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
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
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

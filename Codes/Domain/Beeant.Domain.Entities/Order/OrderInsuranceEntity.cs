using System;
using Beeant.Domain.Entities.Product;


namespace Beeant.Domain.Entities.Order
{

    [Serializable]
    public class OrderInsuranceEntity : BaseEntity<OrderInsuranceEntity>
    {
    
        /// <summary>
        /// 
        /// </summary>
        public OrderEntity Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProductEntity Product { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        ///身份证
        /// </summary>
        public string IdCardNumber { get; set; }
        /// <summary>
        /// 既往病史
        /// </summary>
        public string MedicalHistory { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime  ExpireDate { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public string EffectiveDateName
        {
            get { return EffectiveDate == this.GetMinDateTime() ? "" : EffectiveDate.ToString("yyyy-MM-dd"); }
        }

        /// <summary>
        /// 到期日期
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetStatusName(IsUsed); }
        }

        /// <summary>
        /// 到期日期
        /// </summary>
        public string ExpireDateName
        {
            get { return ExpireDate == this.GetMinDateTime() ? "" : ExpireDate.ToString("yyyy-MM-dd"); }
        }
    }
    
}

using System;

namespace Beeant.Domain.Entities.Order
{
    [Serializable]
    public class OrderExpressEntity : BaseEntity<OrderExpressEntity>
    {
        /// <summary>
        /// 总订单标识Id
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 快递名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 快递编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// 接收地址
        /// </summary>
        public string Address { get; set; }
      
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public OrderExpressEntity DataEntity { get; set; }


    



    }
}

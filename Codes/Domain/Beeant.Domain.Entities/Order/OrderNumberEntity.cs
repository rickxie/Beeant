using System;

namespace Beeant.Domain.Entities.Order
{
    [Serializable]
    public class OrderNumberEntity : BaseEntity<OrderNumberEntity>
    {
        /// <summary>
        /// 总订单标识Id
        /// </summary>
        public OrderEntity Order { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 快递编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 数据实体
        /// </summary>
        public OrderNumberEntity DataEntity { get; set; }

    }
}

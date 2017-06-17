using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class DeliveryEntity : BaseEntity<DeliveryEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 限制
        /// </summary>
        public int LimitCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsUsedName
        {
            get
            {
                return this.GetStatusName(IsUsed);
            }
        }
    }
}

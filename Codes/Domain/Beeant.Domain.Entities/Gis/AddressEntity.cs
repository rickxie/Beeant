using System;

namespace Beeant.Domain.Entities.Gis
{
    [Serializable]
    public class AddressEntity : BaseEntity<AddressEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 限制
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsStartWith { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsStartWithName
        {
            get
            {
                return this.GetStatusName(IsStartWith);
            }
        }
    }
}

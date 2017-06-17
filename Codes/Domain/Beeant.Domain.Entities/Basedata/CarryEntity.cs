using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class CarryEntity : BaseEntity<CarryEntity>
    {
        /// <summary>
        /// 运价
        /// </summary>
        public FreightEntity Freight { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 首次数量
        /// </summary>
        public int DefaultCount { get; set; }
        /// <summary>
        /// 首次价格
        /// </summary>
        public decimal DefaultPrice { get; set; }
        /// <summary>
        /// 续重数量
        /// </summary>
        public int ContinueCount { get; set; }
        /// <summary>
        /// 续重价格
        /// </summary>
        public decimal ContinuePrice { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string[] RegionArray
        {
            get
            {
                if (string.IsNullOrEmpty(Region))
                    return null;
                return Region.Split(',');
            }
        }

    }
}

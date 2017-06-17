using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class PostageEntity : BaseEntity<PostageEntity>
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
        /// 利润比例
        /// </summary>
        public decimal Profit { get; set; }
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

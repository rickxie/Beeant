
using System;

namespace Beeant.Domain.Entities.Utility
{
    [Serializable]
    public class IpEntity
    {
        public string Ip { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
    }
}

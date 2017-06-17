using System;

namespace Beeant.Domain.Entities.Log
{
    [Serializable]
    public class WebImageEntity : BaseEntity<WebImageEntity>
    {
       /// <summary>
       /// 网页地址
       /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string Path { get; set; }
    }
}

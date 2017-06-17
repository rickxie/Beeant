using System;

namespace Beeant.Domain.Entities.Log
{
    [Serializable]
    public class EchoEntity : BaseEntity<EchoEntity>
    {
    
        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求内容
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// 相应内容
        /// </summary>
        public string Response { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

 
    }
}

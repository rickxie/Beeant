using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Log
{
    [Serializable]
    public class ApiTraceEntity : BaseEntity<ApiTraceEntity>
    {
    
        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 请求内容
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// 相应内容
        /// </summary>
        public string Response { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public AccountEntity Account { get; set; }
 
    }
}

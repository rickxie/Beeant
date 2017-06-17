using System;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Log
{
    [Serializable]
    public class LoginEntity : BaseEntity<LoginEntity>
    {   
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 错误页面
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string Device { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 登录消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public AccountEntity Account { get; set; }
    }
}

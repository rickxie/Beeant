
using System;

namespace Winner.Mail
{
    [Serializable]
    public class ServerInfo
    {
        /// <summary>
        /// 发送邮箱
        /// </summary>
        public string FromMail { get; set; }
        /// <summary>
        /// Smtp
        /// </summary>
        public string SmtpHost { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}

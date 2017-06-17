using System.Collections.Generic;
using Winner.Filter;

namespace Beeant.Presentation.Website.Register.Models.Home
{
    public class RegisterModel 
    {
        public IList<ErrorInfo> Errors { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string SurePassword { get; set; }
        /// <summary>
        /// 是否阅读协议
        /// </summary>
        public bool IsReadProtocol { get; set; }
        /// <summary>
        /// 跳转的URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
using System.Collections.Generic;
using Winner.Filter;

namespace Beeant.Presentation.Mobile.Password.Models.Home
{
    public class PasswordModel
    {
        /// <summary>
        /// 账户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 身份验证码
        /// </summary>
        public string ValidateValue { get; set; }
        /// <summary>
        /// 验证方式
        /// </summary>
        public int ValidateType { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string AcountEmail { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string AccountMobile { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string SurePassword { get; set; }
        /// <summary>
        /// 发送验证码时间
        /// </summary>
        public int CodeStepTime { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
        /// <summary>
        /// 步骤
        /// </summary>
        public int Step { get; set; }
    }
}
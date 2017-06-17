using System.Collections.Generic;
using Winner.Filter;

namespace Beeant.Presentation.Mobile.Password.Models.Account
{
    public class PasswordModel
    {
        /// <summary>
        /// 老密码
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string SurePassword { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
    }
}

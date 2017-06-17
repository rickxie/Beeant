using System.Collections.Generic;
using Winner.Filter;

namespace Beeant.Application.Dtos.Account
{
    public class PasswordDto
    {
        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsTimeout { get; set; }
        /// <summary>
        /// 发送步长
        /// </summary>
        public int SendStep { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
    }
}

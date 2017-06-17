using System.Collections.Generic;
using Winner.Filter;

namespace Beeant.Presentation.Website.Password.Models.Account
{
    public class NameModel
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; } 
        
    }
}

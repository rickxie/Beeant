using Beeant.Domain.Entities.Utility;

namespace Beeant.Presentation.Website.Login.Models.Home
{
    public class LoginModel : LoginEntity
    {
        public string Url { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShowCode { get; set; }
        /// <summary>
        /// 是保存Cookie
        /// </summary>
        public bool IsSaveCookie { get; set; }

    }
}
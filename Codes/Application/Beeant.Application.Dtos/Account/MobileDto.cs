using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;
using Winner.Filter;

namespace Beeant.Application.Dtos.Account
{
    public class MobileDto
    {
        /// <summary>
        /// 账户
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 操作的类型
        /// </summary>
        public string Action { get; set; }

        #region 输出

        /// <summary>
        /// 验证码
        /// </summary>
        public CodeEntity CodeEntity { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; } 
        /// <summary>
        /// 对应账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual void SetAction()
        {
            switch (Action)
            {
                case "Valid":
                    Action = Result ? "Bind": "Valid"; break;
                case "Bind":
                    Action = Result ? "Finish" : "Bind"; break;
                default:
                    Result = true;
                    Action = Account != null && Account.IsActiveMobile ? "Valid" : "Bind";
                    break;
            }
        }
        /// <summary>
        /// 设置地址
        /// </summary>
        public virtual string GetMobile()
        {
            switch (Action)
            {
                case "Valid":
                    return Account.Mobile;
                case "Bind":
                    return Mobile;
            }
            return "";

        }
        #endregion
    }
}

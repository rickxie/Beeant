
using System;
using System.Collections.Generic;
using Winner;
using Winner.Filter;

namespace Beeant.Domain.Entities.Utility
{
    [Serializable]
    public class LoginEntity
    {
        /// <summary>
        /// 登入用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登入密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否忽略密码
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 登陆锁类型
        /// </summary>
        public string LockerTag { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
        /// <summary>
        /// 返回的结果对象
        /// </summary>
        public IdentityEntity Identity { get; set; }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="paramters"></param>
        public virtual void AddError(string propertyName, params object[] paramters)
        {
            Errors = Errors ?? new List<ErrorInfo>();
            var error = Creator.Get<IValidation>().GetErrorInfo(typeof (LoginEntity).FullName, propertyName);
            error.Message = string.Format(error.Message, paramters);
            Errors.Add(error);
        }
     
    }
}

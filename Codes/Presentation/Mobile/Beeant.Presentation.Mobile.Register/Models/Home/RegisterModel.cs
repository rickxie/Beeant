using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Utility;
using Winner.Filter;

namespace Beeant.Presentation.Mobile.Register.Models.Home
{
    public class RegisterModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string SurePassword { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        public string MobileCode { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否登入
        /// </summary>
        public bool? IsLogin { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
      
    }
}
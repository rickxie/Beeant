using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Utility;
using Winner.Filter;

namespace Beeant.Presentation.Mobile.Login.Models.Home
{
    public class LoginModel 
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
        /// 是否显示
        /// </summary>
        public bool IsShowCode { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<ErrorInfo> Errors { get; set; }
        /// <summary>
        /// 保存用户名
        /// </summary>
        public virtual void SaveUserName(Controller controller,TokenEntity token,IdentityEntity identity)
        {
            if(token==null || identity==null)
                return;
            var cookie = new HttpCookie("username")
            {
                Expires = DateTime.Now.AddMinutes(token.TimeOut),
                Value = controller.Server.UrlEncode(identity.Name)
            };
            controller.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 保存用户名
        /// </summary>
        public virtual void InitUserName(Controller controller)
        {
           var cookie = controller.Response.Cookies["username"];
            if(cookie==null)
                return;
            Name = controller.Server.UrlDecode(cookie.Value);
        }
    }
}
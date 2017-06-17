using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension.Mobile;

namespace Beeant.Distributed.Outside.Reception.Controllers.Thirdparty
{
    public class WechatController:BaseController
    {
        #region 首页
        public ActionResult Index()
        {
            var handles=new Dictionary<string,Action<XmlDocument>>
            {
                {"subscribe",Subscribe }, {"unsubscribe",Unsubscribe }
            };
            var wechat = this.Wechat();
            if (wechat == null)
                return null;
            this.Wechat().Response(handles);
            return null;
        }
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void Subscribe(XmlDocument doc)
        {
            
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void Unsubscribe(XmlDocument doc)
        {

        }
        #endregion

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Oauth(string url, string code)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            url = Server.UrlDecode(url);
            if (url != null && url.Contains("?"))
            {
                var index = url.IndexOf("?");
                var builder=new StringBuilder(url.Substring(0, index+1));
                var qs = url.Substring(index + 1, url.Length - 1 - index).Split('&');
                foreach (var q in qs)
                {
                    var ps = q.Split('=');
                    builder.AppendFormat("{0}={1}&", ps[0], Server.UrlEncode(ps[1]));
                }
                url = builder.Remove(builder.Length-1,1).ToString();
            }
            url = string.Format("{0}{1}code={2}", url,  url.Contains("?") ? "&" : "?", code);
            return new RedirectResult(url);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Domain.Entities.Site;
using Component.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Site.Reception.Controllers.Thirdparty
{
    public class WechatController : BaseController
    {
        #region 首页
        public ActionResult Index(long? siteId)
        {
            var handles = new Dictionary<string, Action<XmlDocument>>
            {
                {"subscribe",Subscribe }, {"unsubscribe",Unsubscribe }
                , {"text",Text }
          };
            var wechat = this.Wechat(siteId);
            if (wechat == null)
                return null;
            wechat.Response(handles);
          
            return null;
        }
 
        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void Subscribe(XmlDocument doc)
        {
            var content = "";
            var fromUserNameNode = doc.SelectSingleNode("xml/FromUserName");
            var toUserNameNode = doc.SelectSingleNode("xml/ToUserName");
            if (fromUserNameNode == null || toUserNameNode == null)
                return;
            var siteId = RouteData.Values["SiteId"] ??
                             HttpContext.Request["SiteId"];
            var id = siteId.Convert<long>();
            if (id != 0)
            {
                var query = new QueryInfo();
                query.Query<MessageEntity>().Where(it => it.Site.Id == id && it.Type == "subscribe")
                    .Select(it => it.Content);
                var entities = this.GetEntities<MessageEntity>(query);
                var entity = entities?.FirstOrDefault();
                if (entity != null && !string.IsNullOrEmpty(entity.Content))
                    content = entity.Content;
            }
            const string mess = "<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{3}]]></Content></xml>";
            Response.Write(string.Format(mess, fromUserNameNode.InnerText, toUserNameNode.InnerText, GetDateTimeInt(), content));
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void Text(XmlDocument doc)
        {
            var content = "";
            var fromUserNameNode = doc.SelectSingleNode("xml/FromUserName");
            var toUserNameNode = doc.SelectSingleNode("xml/ToUserName");
            var contentNode = doc.SelectSingleNode("xml/Content");
            if (fromUserNameNode == null || contentNode == null || toUserNameNode == null)
                return;
            var siteId = RouteData.Values["SiteId"] ??
                         HttpContext.Request["SiteId"];
            var id = siteId.Convert<long>();
            if (id != 0)
            {
                var name = contentNode.InnerText;
                var query = new QueryInfo();
                query.Query<MessageEntity>().Where(it => it.Site.Id == id && it.Type == "text" && it.Name== name)
                    .Select(it => it.Content);
                var entities = this.GetEntities<MessageEntity>(query);
                var entity = entities?.FirstOrDefault();
                if (entity != null && !string.IsNullOrEmpty(entity.Content))
                    content = entity.Content;
            }
            const string mess =
                "<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{3}]]></Content></xml>";
            Response.Write(string.Format(mess, fromUserNameNode.InnerText, toUserNameNode.InnerText, GetDateTimeInt(),
                content));
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void Unsubscribe(XmlDocument doc)
        {

        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <returns></returns>
        protected virtual int GetDateTimeInt()
        {
            DateTime time = DateTime.Now;
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;

        }
        #endregion



    }
}

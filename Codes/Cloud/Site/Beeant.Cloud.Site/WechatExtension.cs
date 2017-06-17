using System.Web.Mvc;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Site;
using Component.Extension;
using Component.Sdk;
using Configuration;
using Dependent;

namespace Beeant.Cloud.Site
{
    public static class WechatExtension
    {
        private static WechatSdk _wechat;
        /// <summary>
        /// 微信
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static WechatSdk Wechat(this Controller controller, long? siteId)
        {
            if (siteId.HasValue)
            {
                var crm = Ioc.Resolve<IApplicationService, SiteEntity>().GetEntity<SiteEntity>(siteId.Value);
                if (crm == null || string.IsNullOrEmpty(crm.WechatAppId))
                    return null;
                var wechat = new WechatSdk(crm.WechatAppId, crm.WechatSecret, crm.WechatToken)
                {
                    Domain = ConfigurationManager.GetSetting<string>("Domain")
                };
                return wechat;
            }
            if (_wechat == null)
            {
                var json = ConfigurationManager.GetSetting<string>("Wechat").DeserializeJson<dynamic>();
                if (json == null)
                    return null;
                _wechat = new WechatSdk((string)json.AppId, (string)json.Secret, (string)json.Token)
                {
                    Domain = ConfigurationManager.GetSetting<string>("Domain")
                };
            }
            return _wechat;
        }


    }
}

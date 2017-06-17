using System.Web.Mvc;
using Component.Extension;
using Component.Sdk;
using Configuration;

namespace Beeant.Basic.Services.Mvc.Extension.Mobile
{
    public static class ThirtyPartyExtension
    {
        private static WechatSdk _wechat;

        public static WechatSdk Wechat(this Controller controller)
        {
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

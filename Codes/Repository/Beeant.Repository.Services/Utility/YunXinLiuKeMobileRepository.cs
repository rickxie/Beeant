using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Component.Extension;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Repository.Services.Utility
{
    public class YunXinLiuKeMobileRepository : MobileRepository
    {
        public override string Url
        {
            get { return Configuration.ConfigurationManager.GetSetting<string>("YunXinLiuKeSms"); }
        }
        /// <summary>
        /// 发生短信
        /// </summary>
        /// <param name="info"></param>
        protected override string SendMessage(MobileEntity info)
        {
            var result = new List<string>();
            var mobiles = info.ToMobiles.ToArray();
            foreach (var mobile in mobiles)
            {
                info.ToMobiles = new [] { mobile };
                result.Add(base.SendMessage(info));
            }
            return string.Join(",", result.ToArray());
        }
        /// <summary>
        /// 发生短信
        /// </summary>
        /// <param name="info"></param>
        protected override string GetSendStatus(MobileEntity info)
        {
            var json = Configuration.ConfigurationManager.GetSetting<string>("YunXinLiuKeSms").DeserializeJson<dynamic>();
            if (json == null)
                return "";
            var status = WebRequestHelper.SendPostRequest(json.Url.ToString(),
                new Dictionary<string, string>
                {
                    {"Msg", System.Web.HttpUtility.UrlEncode(info.Body, Encoding.GetEncoding("UTF-8"))},
                    {"DesNo", string.Join(",", info.ToMobiles)},
                    {"UserCode", json.UserCode.ToString()},
                    {"UserPass",json.UserPass.ToString()},
                    {"Channel", "0"}
                });
            string reg = "<string(.*?)>(?<num>.*?)</string>";
            Regex r = new Regex(reg, RegexOptions.IgnoreCase);
            Match mc = r.Match(status);
            return mc.Groups["num"].ToString();
        }


    }

}

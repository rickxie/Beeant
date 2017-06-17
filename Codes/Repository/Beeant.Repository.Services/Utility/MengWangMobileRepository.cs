using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Component.Extension;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Repository.Services.Utility
{
    public class MengWangMobileRepository : MobileRepository
    {
    
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
        /// 得到状态
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override string GetSendStatus(MobileEntity info)
        {
            var json = Configuration.ConfigurationManager.GetSetting<string>("MengWangSms").DeserializeJson<dynamic>();
            if (json == null)
                return "";
            var status = WebRequestHelper.SendPostRequest(json.Url.ToString(), Encoding.GetEncoding("GBK"),
                new Dictionary<string, string>
                {
                    {"pszMsg", System.Web.HttpUtility.UrlEncode(info.Body)},
                    {"iMobiCount", info.ToMobiles.Length.ToString()},
                    {"pszMobis", string.Join(",", info.ToMobiles)},
                    {"pszSubPort", "*"},
                    {"UserId", json.UserId.ToString()},
                    {"Password", json.Password.ToString()}
                });
            string reg = "<string(.*?)>(?<num>.*?)</string>";
            Regex r = new Regex(reg, RegexOptions.IgnoreCase);
            Match mc = r.Match(status);
            return mc.Groups["num"].ToString();
        }




    }
}

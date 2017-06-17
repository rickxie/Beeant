using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component.Extension;
using Beeant.Domain.Entities.Utility;

namespace Beeant.Repository.Services.Utility
{
    public class YiTongMobileRepository : MobileRepository
    {
        public override string Url
        {
            get { return Configuration.ConfigurationManager.GetSetting<string>("YiTongSms"); }
        }
        /// <summary>
        /// 发生短信
        /// </summary>
        /// <param name="info"></param>
        protected override string SendMessage(MobileEntity info)
        {
            var result=new List<string>();
            var mobiles = info.ToMobiles.ToArray();
            foreach (var mobile in mobiles)
            {
                info.ToMobiles=new [] {mobile};
                result.Add(base.SendMessage(info));
            }
            return string.Join(",",result.ToArray());
        }
        /// <summary>
        /// 得到状态
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override string GetSendStatus(MobileEntity info)
        {
            var status = WebRequestHelper.SendPostRequest(Url, Encoding.GetEncoding("GBK"),
                new Dictionary<string, string>
                {
                    {"sm", EncodeHexString(15, info.Body)},
                    {"dc", "15"},
                    {"da", string.Join(",", info.ToMobiles)}
                });
            var index = status.LastIndexOf("=");
            if (index > -1)
            {
                status = status.Substring(index + 1, status.Length - index - 1);
            }
            return status;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="dataCoding"></param>
        /// <param name="realStr"></param>
        /// <returns></returns>
        //字符编码成HEX
        private static string EncodeHexString(int dataCoding, string realStr)
        {
            string strhex = "";

            byte[] bytSource = null;
            if (dataCoding == 15)
            {
                bytSource = Encoding.GetEncoding("GBK").GetBytes(realStr);

            }
            else if (dataCoding == 8)
            {
                bytSource = Encoding.BigEndianUnicode.GetBytes(realStr);

            }
            else
            {
                bytSource = Encoding.ASCII.GetBytes(realStr);

            }
            for (int i = 0; i < bytSource.Length; i++)
            {
                strhex = strhex + bytSource[i].ToString("X2");

            }
            return strhex;
        }
    }
}

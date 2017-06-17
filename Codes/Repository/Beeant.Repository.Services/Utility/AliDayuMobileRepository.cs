using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component.Extension;
using Beeant.Domain.Entities.Utility;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Beeant.Repository.Services.Utility
{
    public class AliDayuMobileRepository : MobileRepository
    {
        public dynamic Setting
        {
            get { return Configuration.ConfigurationManager.GetSetting<string>("AliDayuSms").DeserializeJson<dynamic>(); }
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
        /// 得到状态
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override string GetSendStatus(MobileEntity info)
        {
            var builder = new StringBuilder("{");
            builder.AppendFormat("code:'{0}'", info.Body.Replace("'", "''"));
            builder.Append("}");
            string url = Setting.Url;
            string appKey = Setting.AppKey;
            string appSecret = Setting.AppSecret;
            string smsFreeSignName = Setting.SmsFreeSignName;
            string smsType = Setting.SmsType;
            string smsTemplateCode = Setting.SmsTemplateCode;
            ITopClient client = new DefaultTopClient(url, appKey, appSecret);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = smsType;
            //单引号转义.
            req.SmsParam = builder.ToString();
            req.RecNum = string.Join(",", info.ToMobiles);
            req.SmsTemplateCode = smsTemplateCode;
            req.SmsFreeSignName = smsFreeSignName;
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            return rsp.SerializeJson();
        }
    }
}

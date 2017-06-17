using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Configuration;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;
using Component.Extension;

namespace Beeant.Repository.Services.Finance
{
    public class WechatPaylineRepository : PaylineRepository
    {
        private dynamic _wechatConfig;
        public dynamic WechatConfig
        {
            get
            {
                if (_wechatConfig == null)
                    _wechatConfig = ConfigurationManager.GetSetting<string>("Wechat").DeserializeJson<dynamic>();
                return _wechatConfig;
            }
        }
        /// <summary>
        /// 商户证书ID
        /// </summary>
        public string AppId
        {
            get { return WechatConfig.AppId; }
        }
   
        /// <summary>
        /// 商户证书ID
        /// </summary>
        public string PartnerKey
        {
            get { return WechatConfig.PartnerKey; }
        }
        /// <summary>
        /// 商户证书ID
        /// </summary>
        public string PaySignKey
        {
            get { return WechatConfig.PaySignKey; }
        }
 
        #region 创建

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Create(PaylineEntity info)
        {
            var resParams = CreateWechatOrder(info);
            if (resParams == null || !resParams.ContainsKey("appid") || !resParams.ContainsKey("prepay_id")
                || string.IsNullOrEmpty(resParams["appid"]) || string.IsNullOrEmpty(resParams["prepay_id"]))
                return false;
            var paraTemp = new SortedDictionary<string, string>
            {
                {"appId", resParams["appid"]},
                {"timeStamp", (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.Convert<long>().ToString()},
                {"nonceStr", Guid.NewGuid().ToString().Replace("-","")},
                {"package", string.Format("prepay_id={0}",resParams["prepay_id"])},
                {"signType", "MD5"}
            };
            paraTemp.Add("paySign", MakeSign(paraTemp, PaySignKey));
            info.Forms = paraTemp;
            info.OutNumber = resParams["prepay_id"];
            info.Status= PaylineStatusType.Waiting;
            info.Request = GetRequest(info, resParams);
            return true;
        }
        /// <summary>
        /// 预下单
        /// </summary>
        /// <param name="info"></param>
        protected virtual SortedDictionary<string, string> CreateWechatOrder(PaylineEntity info)
        {
            try
            {
                string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                var processUrl = string.Format("{0}/WechartPay/Process",
                    ConfigurationManager.GetSetting<string>("DistributedOutsidePayUrl"));
                var amount= info.Amount != 0 || info.PaylineItems == null ? info.Amount : info.PaylineItems.Sum(it => it.Amount);
                var requsetParams = new SortedDictionary<string, string>();
                requsetParams.Add("body", info.Number);
                requsetParams.Add("attach", info.Number);
                requsetParams.Add("out_trade_no", info.Number);
                requsetParams.Add("total_fee", (amount * 100).Convert<int>().ToString());
                requsetParams.Add("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                requsetParams.Add("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
                requsetParams.Add("trade_type", HttpContext.Current.Request["trade_type"]);
                requsetParams.Add("openid", info.Forms==null ||!info.Forms.ContainsKey("openid") ? null: info.Forms["openid"]);
                requsetParams.Add("appid", AppId);
                requsetParams.Add("mch_id", PartnerKey);
                requsetParams.Add("spbill_create_ip", HttpContext.Current.Request.UserHostAddress);
                requsetParams.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));
                requsetParams.Add("notify_url", processUrl);
                requsetParams.Add("sign", MakeSign(requsetParams, PaySignKey).ToUpper());
                string xml = ToRequestXml(requsetParams);
                string response = WebRequestHelper.SendPostRequest(url,Encoding.UTF8, xml);
                return FromResponseXml(response);
            }
            catch (Exception ex)
            {

                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return null;
        }

        /// <summary>
        /// 得到输出
        /// </summary>
        /// <param name="info"></param>
        /// <param name="resParas"></param>
        /// <returns></returns>
        protected virtual string GetRequest(PaylineEntity info, IDictionary<string, string>  resParas)
        {
            if (HttpContext.Current.Request["trade_type"] == "JSAPI")
                return GetJsApiRequest(info);
            return GetNativeRequest(info, resParas);
        }

        /// <summary>
        /// 得到请求
        /// </summary>
        /// <param name="info"></param>
        /// <param name="resParas"></param>
        /// <returns></returns>
        protected virtual string GetNativeRequest(PaylineEntity info, IDictionary<string, string> resParas)
        {
            var builder = new StringBuilder();
            builder.Append("<script>");
            builder.AppendFormat("window.location.href='{0}';", resParas["code_url"]);
            builder.Append("</script>");
            return builder.ToString();
        }
        /// <summary>
        /// 得到请求
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string GetJsApiRequest(PaylineEntity info)
        {
            var builder = new StringBuilder();
            var ticket = Guid.NewGuid().ToString().Replace("-", "");
            Winner.Creator.Get<Winner.Cache.ICache>().Set(ticket, ticket, DateTime.Now.AddMinutes(5));
            builder.Append("<script type=\"text/javascript\"> function onBridgeReady(){");
            builder.AppendFormat("WeixinJSBridge.invoke('getBrandWCPayRequest',{0}", info.Forms.SerializeJson());
            builder.Append(",function(res){");
            builder.Append("if (res.err_msg == \"get_brand_wcpay_request:ok\") { ");
            builder.AppendFormat("window.location.href='{0}/WechatPay/Process?number={1}&ticket={2}';",
                ConfigurationManager.GetSetting<string>("DistributedOutsidePayUrl"),
                info.Number, ticket);
            builder.Append(" }})}");
            builder.Append(" if (typeof WeixinJSBridge == \"undefined\")");
            builder.Append(" {if (document.addEventListener) {");
            builder.Append(" document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);}");
            builder.Append(
                " else if (document.attachEvent) { document.attachEvent('WeixinJSBridgeReady', onBridgeReady);");
            builder.Append(" document.attachEvent('onWeixinJSBridgeReady', onBridgeReady); } }");
            builder.Append("else{ onBridgeReady();}</script> ");
            return builder.ToString();
        }
        #endregion

        #region 处理

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public override PaylineEntity Process()
        {
            var sPara = GetResponse();
            var number = sPara.ContainsKey("number")
                       ? sPara["number"]
                       : sPara.ContainsKey("out_trade_no") ? sPara["out_trade_no"] : "";
            if (string.IsNullOrWhiteSpace(number))
                return null;
            var result = GetProcessResult(sPara, number);
            if (result == null || !result.ContainsKey("trade_state") || result["trade_state"] != "SUCCESS")
                return null;
            var info = GetPayline(sPara["number"]);
            if (info == null || info.Status != PaylineStatusType.Waiting)
                return null;
            info.Status = result["trade_state"] == "SUCCESS" ? PaylineStatusType.Success : PaylineStatusType.Failure;
            info.Response = sPara.SerializeJson();
            info.OutNumber = result.ContainsKey("transaction_id") ? result["transaction_id"] : "";
            info.SetProperty(it => it.Status);
            info.SetProperty(it => it.Response);
            info.SetProperty(it => it.OutNumber);
            info.SaveType = SaveType.Modify;
            return info;
        }
        /// <summary>
        /// 得到结果
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, string> GetResponse()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["ticket"]))
            {
                var request = HttpContext.Current.Request;
                var sArray = new SortedDictionary<string, string>();
                NameValueCollection coll = HttpContext.Current.Request.QueryString;
                foreach (string key in coll.AllKeys)
                {
                    sArray.Add(key, coll[key]);
                }
                return sArray;
            }
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            using (var s = HttpContext.Current.Request.InputStream)
            {
                int count;
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
            }
            return FromResponseXml(builder.ToString());
        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="inputPara"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GetProcessResult(IDictionary<string, string> inputPara,string number)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["ticket"]))
            {
                var rev= Winner.Creator.Get<Winner.Cache.ICache>().Get<string>(HttpContext.Current.Request["ticket"]) ==
                       HttpContext.Current.Request["ticket"];
                return rev ? GetPayResult(number) : null;
            }
            if (inputPara.ContainsKey("return_code") && inputPara["return_code"] != "SUCCESS")
            {
                if (MakeSign(inputPara, PaySignKey) != inputPara["sign"])
                    return null;
                return GetPayResult(!inputPara.ContainsKey("out_trade_no") ? "" : inputPara["out_trade_no"]);

            }
            return null;
        }
     


        /// <summary>
        /// 获取是否是支付宝服务器发来的请求的验证结果
        /// </summary>
        /// <returns>验证结果</returns>
        protected virtual IDictionary<string,string> GetPayResult(string number)
        {
            try
            {
                string url = "https://api.mch.weixin.qq.com/pay/orderquery";
                var requsetParams = new SortedDictionary<string, string>();
                requsetParams.Add("out_trade_no", number);
                requsetParams.Add("transaction_id ", "");
                requsetParams.Add("appid", AppId);
                requsetParams.Add("mch_id", PartnerKey);
                requsetParams.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
                requsetParams.Add("sign", MakeSign(requsetParams, PaySignKey));
                string xml = ToRequestXml(requsetParams);
                string response = WebRequestHelper.SendPostRequest(url, Encoding.UTF8, xml);
                return  FromResponseXml(response);
               

            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return null;
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected virtual string ToRequestXml(SortedDictionary<string, string> paras)
        {
            var xml=new StringBuilder("<xml>");
            foreach (KeyValuePair<string, string> pair in paras)
            {
                xml.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", pair.Key, pair.Value);
            }
            xml.AppendFormat("</xml>");
            return xml.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        protected virtual SortedDictionary<string, string> FromResponseXml(string xml)
        {
            var mValues = new SortedDictionary<string, string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild; //获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                mValues[xe.Name] = xe.InnerText; //获取xml的键值对到WxPayData内部的数据中
            }
            return mValues;
        }
        #endregion
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Check(PaylineEntity info)
        {
            if (string.IsNullOrEmpty(info.OutNumber) || info.Status != PaylineStatusType.Waiting || string.IsNullOrEmpty(info.Number))
                return false;
            var result=  GetPayResult(info.Number);
            if (result == null || !result.ContainsKey("trade_state"))
                return false;
            if (result["trade_state"] == "USERPAYING")
                return false;
            if (result["trade_state"] == "SUCCESS")
                info.Status= PaylineStatusType.Success;
            else
                info.Status = PaylineStatusType.Failure;
            info.OutNumber = result.ContainsKey("transaction_id") ? result["transaction_id"] : "";
            info.SaveType= SaveType.Modify;
            info.SetProperty(it => it.Status);
            info.SetProperty(it => it.OutNumber);
            return true;
        }
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Refund(PaylineEntity info)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            var requsetParams = new SortedDictionary<string, string>();
            requsetParams.Add("total_fee ", Math.Abs(info.Amount * 100).ToString());
            requsetParams.Add("refund_fee ", Math.Abs(info.Amount * 100).ToString());
            requsetParams.Add("out_refund_no ", info.Number);
            requsetParams.Add("transaction_id ", info.OutNumber);
            requsetParams.Add("appid", AppId);
            requsetParams.Add("mch_id", PartnerKey);
            requsetParams.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            requsetParams.Add("sign", MakeSign(requsetParams, PaySignKey));
            string xml = ToRequestXml(requsetParams);
            string response = WebRequestHelper.SendPostRequest(url, Encoding.UTF8, xml);
            var result = FromResponseXml(response);
            if (result == null || !result.ContainsKey("return_code") || result["return_code"] != "SUCCESS")
                return false;
            info.Status = PaylineStatusType.Success;
            info.OutNumber = result.ContainsKey("refund_id") ? result["refund_id"] : "";
            return true;
        }
     
    }
}

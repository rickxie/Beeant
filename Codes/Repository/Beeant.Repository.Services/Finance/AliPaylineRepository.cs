using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities.Finance;
using Winner.Persistence;

namespace Beeant.Repository.Services.Finance
{
    public class AliPaylineRepository : PaylineRepository
    {
        private dynamic _aliPayConfig;
        public dynamic AliPayConfig
        {
            get
            {
                if (_aliPayConfig == null)
                    _aliPayConfig = ConfigurationManager.GetSetting<string>("AliPay").DeserializeJson<dynamic>();
                return _aliPayConfig;
            }
        }
        /// <summary>
        /// 合作伙伴
        /// </summary>
        public string Url
        {
            get { return AliPayConfig.Url; }
        }
        /// <summary>
        /// 合作伙伴
        /// </summary>
        public string AppId
        {
            get { return AliPayConfig.AppId; }
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public string PrivateKey
        {
            get
            {
                return AliPayConfig.PrivateKey;
            }
        }

        /// <summary>
        /// 支付宝邮箱
        /// </summary>
        public string AliPayPublicKey
        {
            get
            {
                 return AliPayConfig.AliPayPublicKey;
            }
        }

        #region 创建


        private IAopClient _aopClient;
        protected virtual IAopClient AopClient
        {
            get
            {
                if (_aopClient == null)
                    _aopClient = new DefaultAopClient(Url, AppId, PrivateKey, "json", "1.0", "RSA2", AliPayPublicKey, "UTF-8", false);
                return _aopClient;
            }
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Create(PaylineEntity info)
        {
            return CreateByWap(info);
        }
        /// <summary>
        /// 手机支付
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool CreateByWap(PaylineEntity info)
        {
            var builder = new StringBuilder("{");
            builder.AppendFormat("\"body\":\"{0}\",", info.TypeName);
            builder.AppendFormat("\"subject\":\"{0}\",", info.TypeName);
            builder.AppendFormat("\"out_trade_no\":\"{0}\",", info.Number);
            builder.AppendFormat("\"total_amount\":\"{0}\",", info.Amount!=0 || info.PaylineItems==null?info.Amount:info.PaylineItems.Sum(it=>it.Amount));
            builder.AppendFormat("\"product_code\":\"{0}\",", "QUICK_WAP_PAY");
            builder.Append("}");
            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            request.BizContent = builder.ToString();
            var processUrl = string.Format("{0}/AliPay/Process",
                ConfigurationManager.GetSetting<string>("DistributedOutsidePayUrl"));
            request.SetReturnUrl(processUrl);
            request.SetNotifyUrl(processUrl);
            AlipayTradeWapPayResponse response = AopClient.pageExecute(request);
            info.Request = response.Body;
            return true;
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
            bool isVerify = VerifyProcess(sPara);
            var number = sPara.ContainsKey("out_trade_no") ? sPara["out_trade_no"] : "";
            var info = GetPayline(number);
            if (info == null || info.Amount != sPara["total_amount"].Convert<decimal>())
                return null;
            info.OutNumber = sPara["trade_no"];
            info.Status = isVerify ? PaylineStatusType.Success : PaylineStatusType.Failure;
            info.Response = sPara.SerializeJson();
            info.SetProperty(it => it.OutNumber);
            info.SetProperty(it => it.Status);
            info.SetProperty(it => it.Response);
            info.SaveType = SaveType.Modify;
            return info;
        }

      
        /// <summary>
        /// 得到请求参数
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, string> GetResponse()
        {
            var request = HttpContext.Current.Request;
            var sArray = new SortedDictionary<string, string>();
            NameValueCollection coll = request.Form.AllKeys.Contains("notify_id")
                                           ? request.Form
                                           : request.QueryString;

            foreach (string key in coll.AllKeys)
            {
                sArray.Add(key, coll[key]);
            }
            return sArray;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="inputPara"></param>
        /// <returns></returns>
        protected override bool VerifyProcess(IDictionary<string, string> inputPara)
        {
            return AlipaySignature.RSACheckV1(inputPara, AliPayPublicKey, "UTF-8", "RSA2", false);
        }
 

        #endregion
        /// <summary>
        /// 得到支付结果
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected virtual string GetPayResult(string number)
        {
            var builder = new StringBuilder("{");
            builder.AppendFormat("\"out_trade_no\":\"{0}\",", number);
            builder.Append("}");
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.BizContent = builder.ToString();
            AlipayTradeQueryResponse response = AopClient.pageExecute(request);
            return response.TradeStatus;
        }
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Check(PaylineEntity info)
        {
            if (string.IsNullOrEmpty(info.OutNumber) || info.Status != PaylineStatusType.Waiting || string.IsNullOrEmpty(info.Number))
                return false;
            var result = GetPayResult(info.Number);
            if (string.IsNullOrEmpty(result) || result == "WAIT_BUYER_PAY")
                return false;
            if (result == "TRADE_SUCCESS" || result == "TRADE_FINISHED")
                info.Status = PaylineStatusType.Success;
            else
                info.Status = PaylineStatusType.Failure;
            info.SaveType = SaveType.Modify;
            info.SetProperty(it => it.Status);
            return true;
        }
    }
}

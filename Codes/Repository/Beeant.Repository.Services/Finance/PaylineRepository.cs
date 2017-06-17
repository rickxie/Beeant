using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Component.Extension;
using Configuration;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Services.Finance;
using Winner;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Repository.Services.Finance
{
    public class PaylineRepository : IPaylineRepository
    {
        /// <summary>
        /// 参加接口
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Create(PaylineEntity info)
        {
            try
            {
                info.Request = GetRequest(info);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return true;
        }
        /// <summary>
        /// 处理接口
        /// </summary>
        /// <returns></returns>
        public virtual PaylineEntity Process()
        {
            var inputparas = GetResponse();
            var info = new PaylineEntity
            {
                Number = HttpContext.Current.Request["Number"],
                OutNumber = HttpContext.Current.Request["OutNumber"],
                Response = inputparas.SerializeJson(),
                Status = VerifyProcess(inputparas)? PaylineStatusType.Success : PaylineStatusType.Failure
            };
            info.SetProperty(it => it.OutNumber);
            info.SetProperty(it => it.Status);
            info.SetProperty(it => it.Response);
            info.SaveType = SaveType.Modify;
            return info;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Check(PaylineEntity info)
        {
            if (info.Status == PaylineStatusType.Waiting)
            {
                info.Status= PaylineStatusType.Failure;
                info.SaveType=SaveType.Modify;
                info.SetProperty(it => it.Status);
            }
            return true;
        }
        /// <summary>
        /// 得到输出结果
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string GetRequest(PaylineEntity info)
        {
            var url = string.Format("{0}/{1}Payline/Create",
                   ConfigurationManager.GetSetting<string>("DistributedOutsidePayUrl"), info.Type);
            var requset = (HttpWebRequest) WebRequest.Create(url);
            var cookies=new CookieCollection();
            foreach (Cookie cookie in HttpContext.Current.Request.Cookies)
            {
                cookies.Add(cookie);
            }
            requset.CookieContainer.Add(new Uri(ConfigurationManager.GetSetting<string>("Domain")), cookies);
            return WebRequestHelper.SendPostRequest(requset,Encoding.UTF8,  new Dictionary<string, string>
                {
                    {"ChannelType",info.ChannelType.ToString() },
                    {"AccountId",info.Account==null?"0":info.Account.Id.ToString() },
                    {"Amount",info.Amount.ToString() },
                    {"OrderIds",info.PaylineItems==null?"":string.Join(",",info.PaylineItems.Where(it=>it.Order!=null).Select(it=>it.Order.Id).ToArray()) }
                });
        }
        /// <summary>
        /// 得到响应内容
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GetResponse()
        {
            return new Dictionary<string, string>();
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="inputPara"></param>
        /// <returns></returns>
        protected virtual bool VerifyProcess(IDictionary<string, string> inputPara)
        {
            return true;
        }
        #region 公共方法

        /// <summary>
        /// 得到MD5加密
        /// </summary>
        /// <param name="paraTemp"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual string MakeSign(IDictionary<string, string> paraTemp,string key)
        {
            var s = CreateSignString(FilterParam(paraTemp));
            var input = string.Format("{0}&key={1}", s, key);
            var md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(input));
            var sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="paraTemp">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static IDictionary<string, string> FilterParam(IDictionary<string, string> paraTemp)
        {
            return
                paraTemp.Where(
                    temp =>
                    temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" &&
                    !string.IsNullOrEmpty(temp.Value)).ToDictionary(temp => temp.Key, temp => temp.Value);
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="param">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateSignString(IDictionary<string, string> param)
        {
            var prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in param)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }
            return prestr.Remove(prestr.Length - 1, 1).ToString();
        }
        #endregion
        /// <summary>
        /// 得到在线支付
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected virtual PaylineEntity GetPayline(string number)
        {
            var query = new QueryInfo();
            query.Query<PaylineEntity>().Where(it => it.Number == number);
            var info= Creator.Get<IContext>().GetInfos<IList<PaylineEntity>>(query)?.FirstOrDefault();
            return info;

        }
        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Refund(PaylineEntity info)
        {
            return true;
        }
    }
}

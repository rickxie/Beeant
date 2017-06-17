using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Beeant.Domain.Entities.Api;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Utility;
using Component.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;
using VerificationEntity = Beeant.Domain.Entities.Api.VerificationEntity;

namespace Beeant.Application.Services.Api
{
    public class ApiEngineApplicationService : MarshalByRefObject,IApiEngineApplicationService
    { /// <summary>
      /// 查询实例
      /// </summary>
        public IRepository Repository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICacheRepository CacheRepository { get; set; }




        #region 接口实现

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual VerificationEntity Verify(ApiArgsEntity args)
        {
            if (string.IsNullOrWhiteSpace(args.Token) || string.IsNullOrWhiteSpace(args.Method) || string.IsNullOrWhiteSpace(args.Ip))
                return null;
            var info = VerifyVoucher(args);

            if (info != null && info.Error == null)
            {
                bool isSign = true;
                if (info.VoucherProtocol != null)
                    isSign = info.VoucherProtocol.IsSign;
                else if (info.Voucher != null)
                    isSign = info.Voucher.IsSign;
                else if (info.Protocol != null)
                    isSign = info.Protocol.IsSign;
                if (isSign)
                {
                    VerifySign(info, args.Token, args.Value, args.Sign);
                }
            }

            return info;
        }

        /// <summary>
        /// 验证凭证
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual VerificationEntity VerifyVoucher(ApiArgsEntity args)
        {
            var enginArgs = GetEngin();
            args.Method = args.Method?.ToLower();
            var info = new VerificationEntity
            {
                Voucher = enginArgs.GetVoucher(args.Token),
                Protocol = enginArgs.GetProtocol(args.Method),
                VoucherProtocol=enginArgs.GetVoucherProtocol(args.Token, args.Method)
            };
            if (info.Voucher == null || info.Voucher.Type == VoucherType.Forbid)
            {
                info.SetError("00002");
                return info;
            }
            if (info.Voucher.Type == VoucherType.Global)
            {
                if (info.Protocol != null)
                {
                    ValidateRequest(info, info.Protocol.SecondCount, info.Protocol.DayCount, args.Token, args.Method);
                }
                return info;
            }
            if (info.Protocol != null)
            {
                if (!info.Protocol.IsStart)
                {
                    info.SetError("00002");
                    return info;
                }
                if (!info.Protocol.IsVerify)
                {
                    ValidateRequest(info, info.Protocol.SecondCount, info.Protocol.DayCount, args.Token, args.Method);
                    return info;
                }
                if (info.VoucherProtocol==null || info.VoucherProtocol.IsForbid)
                {
                    info.SetError("00002");
                    return info;
                }
                ValidateRequest(info, info.VoucherProtocol.SecondCount, info.VoucherProtocol.DayCount, args.Token, args.Method);

            }
            if(info.Voucher.IpsArray!=null && !info.Voucher.IpsArray.ContainsKey(args.Ip))
            {
                info.SetError("00005");
                return info;
            }
            return info;
        }

        /// <summary>
        /// 验证请求限制
        /// </summary>
        /// <param name="info"></param>
        /// <param name="secondCount"></param>
        /// <param name="dayCount"></param>
        /// <param name="token"></param>
        /// <param name="protocolName"></param>
        /// <returns></returns>
        protected virtual void ValidateRequest(VerificationEntity info, int secondCount, long dayCount, string token,
            string protocolName)
        {

            ValidateSecondRequest(info, secondCount, token, protocolName);
            ValidateDayRequest(info, dayCount, token, protocolName);
        }

        /// <summary>
        /// 验证请求限制
        /// </summary>
        /// <param name="info"></param>
        /// <param name="secondCount"></param>
        /// <param name="token"></param>
        /// <param name="protocolName"></param>
        /// <returns></returns>
        protected virtual void ValidateSecondRequest(VerificationEntity info, int secondCount, string token, string protocolName)
        {
            if (secondCount == 0) return;
            var name = string.Format("{0}{1}SecondCount", token, protocolName);
            var value = CacheRepository.Get<Dictionary<string, object>>(name);
            var count = value == null || !value.ContainsKey("Count") ? 0 : value["Count"].Convert<long>();
            count++;
            if (count > secondCount)
            {
                info.SetError("00003", secondCount);
            }
            if (value == null)
            {
                value=new Dictionary<string, object>
                {
                    {"Count",count },
                    {"Time",DateTime.Now.AddSeconds(1) },
                };
            }
            value["Count"] = count;
            CacheRepository.Set(name, value, value["Time"].Convert<DateTime>());
        }

        /// <summary>
        /// 验证请求限制
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dayCount"></param>
        /// <param name="token"></param>
        /// <param name="protocolName"></param>
        /// <returns></returns>
        protected virtual void ValidateDayRequest(VerificationEntity info,  long dayCount, string token, string protocolName)
        {
            if (dayCount == 0) return;
            var name = string.Format("{0}{1}DayCount", token, protocolName);
            var value = CacheRepository.Get<Dictionary<string, object>>(name);
            var count = value == null || !value.ContainsKey("Count") ? 0 : value["Count"].Convert<long>();
            count++;
            if (count > dayCount)
            {
                info.SetError("00004", dayCount);
            }
            if (value == null)
            {
                value = new Dictionary<string, object>
                {
                    {"Count",count },
                    {"Time",DateTime.Now.Date.AddDays(1) },
                };
            }
            value["Count"] = count;
            CacheRepository.Set(name, value, value["Time"].Convert<DateTime>());
        }
        #region 验证

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="info"></param>
        /// <param name="token"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public virtual void VerifySign(VerificationEntity info, string token, string value, string sign)
        {
            if (string.IsNullOrEmpty(sign) || string.IsNullOrEmpty(token) || GetSign(token, value).ToLower() != sign.ToLower())
            {
                info.SetError("00002");
            }
        }

        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param>密钥</param>
        /// <returns>签名结果</returns>
        public static string GetSign(string key, string prestr)
        {
            var sb = new StringBuilder(32);
            prestr = string.Join("", string.Format("{0}{1}", prestr, key).OrderByDescending(it => it));
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion


        #endregion


        #region 初始化
        /// <summary>
        /// 缓存锁
        /// </summary>

        private static object CacheLocker = new object();

        /// <summary>
        /// 得到缓存值
        /// </summary>

        private const string CacheKey = "ApiEngineArgs";



        /// <summary>
        /// 得到用户信息
        /// </summary>
        /// <returns></returns>
        public virtual ApiEnginEntity GetEngin()
        {
            var args = CacheRepository.Get<ApiEnginEntity>(CacheKey);
            if (args == null)
            {
                lock (CacheLocker)
                {
                    args = CacheRepository.Get<ApiEnginEntity>(CacheKey);
                    if (args == null)
                    {
                        args = SetArgsCache();
                    }
                }
            }
            return args;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <returns></returns>
        protected virtual ApiEnginEntity SetArgsCache()
        {
            var args = new ApiEnginEntity();
            args.GetVoucherProtocolHandle = GetVoucherProtocol;
            args.GetVoucherHandle = GetVoucher;
            args.GetProtocolHandle = GetProtocol;
            CacheRepository.Set(CacheKey, args, DateTime.MaxValue);
            return args;
        }

        #region 票据协议账户
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string VoucherProtocolCacheKey = "ApiEngineVoucherProtocol";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetVoucherProtocolCacheKey(string token,string name)
        {
            return string.Format("{0}{1}{2}", VoucherProtocolCacheKey, token, name);
        }
    

        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual VoucherProtocolEntity GetVoucherProtocol(string token,string name)
        {
            var key = GetVoucherProtocolCacheKey(token,name);
            var value = CacheRepository.Get<VoucherProtocolEntity>(key);
            if (value == null)
            {
                var query = new QueryInfo();
                query.Query<VoucherProtocolEntity>().Where(it => it.Voucher.Token==token && it.Protocol.Name==name)
                    .Select(it => new object[] {it.IsForbid,it.IsLog,it.IsSign,it.SecondCount,it.DayCount,it.Args});
                var infos = Repository.GetEntities<VoucherProtocolEntity>(query);
                value = infos?.FirstOrDefault();
                CacheRepository.Set(key, value, 3600 * 15);
            }
            return value;
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearVoucherProtocolCache(string url)
        {
            var ids = HttpUtility.ParseQueryString(url).Get("VoucherProtocolId");
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var idArray = ids.Split(',').Select(it=>it.Convert<long>()).ToArray();
                var infos=new QueryInfo<VoucherProtocolEntity>()
                    .Query().Where(it=>idArray.Contains(it.Id)).Select(it=>new object[] {it.Voucher.Token,it.Protocol.Name})
                    .ToList<VoucherProtocolEntity>();
                if(infos==null)
                    return;
                foreach (var info in infos)
                {
                    if(info.Voucher==null || info.Protocol==null)
                        continue;
                    var key = GetVoucherProtocolCacheKey(info.Voucher.Token, info.Protocol.Name);
                    CacheRepository.Remove(key);
                }
            }
        }
        #endregion

        #region 票据

        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string VoucherCacheKey = "ApiEngineVoucher";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetVoucherCacheKey(string name)
        {
            return string.Format("{0}{1}", VoucherCacheKey, name);
        }
    
        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected virtual VoucherEntity GetVoucher(string token)
        {
            var key = GetVoucherCacheKey(token);
            var value = CacheRepository.Get<VoucherEntity>(key);
            if (value == null)
            {
                var query = new QueryInfo();
                query.Query<VoucherEntity>().Where(it => it.Token==token)
                    .Select(it => new object[] { it.Id,it.Account.Id,it.Ips,it.IsSign, it.IsLog,it.Type,it.Url });
                var infos = Repository.GetEntities<VoucherEntity>(query);
                value = infos?.FirstOrDefault();
                CacheRepository.Set(key, value, 3600 * 15);
            }
            return value;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearVoucherCache(string url)
        {
            var tokens = HttpUtility.ParseQueryString(url).Get("token");
            if (!string.IsNullOrWhiteSpace(tokens))
            {
                var tokenArr = tokens.Split(',');
                foreach (var token in tokenArr)
                {
                    var key = GetVoucherCacheKey(token);
                    CacheRepository.Remove(key);

                }
            }
        }
        #endregion

        #region 协议
        /// <summary>
        /// 得到缓存值
        /// </summary>

        private static string ProtocolCacheKey = "ApiEngineProtocol";
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <returns></returns>
        protected virtual string GetProtocolCacheKey(string name)
        {
            return string.Format("{0}{1}", ProtocolCacheKey, name);
        }
   

        /// <summary>
        /// 得到角色
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual ProtocolEntity GetProtocol(string name)
        {
            var key = GetProtocolCacheKey(name);
            var value = CacheRepository.Get<ProtocolEntity>(key);
            if (value == null)
            {
                var query = new QueryInfo();
                query.Query<ProtocolEntity>().Where(it => it.Name == name)
                    .Select(it => new object[] { it.Id, it.IsLog,it.IsSign, it.SecondCount,it.DayCount,it.IsStart,it.IsVerify });
                var infos = Repository.GetEntities<ProtocolEntity>(query);
                value = infos?.FirstOrDefault();
                CacheRepository.Set(key, value, 3600 * 15);
            }
            return value;
        }


        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearProtocolCache(string url)
        {
            var names = HttpUtility.ParseQueryString(url).Get("name");
            if (!string.IsNullOrWhiteSpace(names))
            {
                var nameArr = names.Split(',');
                foreach (var name in nameArr)
                {
                    var key = GetProtocolCacheKey(name);
                    CacheRepository.Remove(key);
                }
            }
        }
        #endregion

        #endregion

        #region 执行事件

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool Execute(string url, string name)
        {
            switch (name)
            {
                case "ClearApiCache":
                    ClearApiCache();
                    break;
                case "ClearVoucherCache":
                    ClearVoucherCache(url);
                    break;
                case "ClearProtocolCache":
                    ClearProtocolCache(url);
                    break;
            }
            return true;
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        protected virtual void ClearApiCache()
        {
            CacheRepository.Remove(CacheKey);
        }


        #endregion




    }
}

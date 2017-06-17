using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Beeant.Application.Dtos.Order;
using Beeant.Application.Services.Finance;
using Beeant.Application.Services.Order;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Component.Extension;
using Configuration;
using Dependent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winner.Persistence;

namespace Beeant.Test.Services.Finance
{
    [TestClass]
    public class WechatTestService : TestBase
    {
        [TestMethod]
        public virtual void Order()
        {

            var payline = new PaylineEntity
            {
                Account = new AccountEntity {Id = 30310},
                ChannelType= ChannelType.Website,
                Type= PaylineType.Wechat,
                Status= PaylineStatusType.Create,
                OutNumber= "4008932001201706125466991385"
            };
            payline.PaylineItems=new List<PaylineItemEntity>
            {
                {
                    new PaylineItemEntity
                    {
                        Order=new OrderEntity {Id=31317} ,
                        Payline=payline,
                        Amount=(decimal)-0.01,
                        SaveType =SaveType.Add
                    }
                }
            };
            payline.Amount= payline.PaylineItems.Sum(it => it.Amount); 
            Ioc.Resolve<IPaylineApplicationService>("Beeant.Application.Services.Finance.IWechatPaylineApplicationService").Refund(payline);
        }

        [TestMethod]
        public virtual void Staff()
        {
            var builder=new StringBuilder("name=1111");
            var sign = GetSign("95c401f4824440bb9a10066c6c6499cf", builder.ToString());
            WebRequestHelper.SendPostRequest("http://dev.api.welfare.beeant.com/staff/index",
                new Dictionary<string, string>
                {
                    {"name","1111" },
                    {"sign",sign },
                    {"token","95c401f4824440bb9a10066c6c6499cf" }
                });
            return;
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
    }
}

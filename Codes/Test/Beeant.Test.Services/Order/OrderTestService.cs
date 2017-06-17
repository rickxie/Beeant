using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Beeant.Application.Dtos.Order;
using Beeant.Application.Services.Order;
using Beeant.Domain.Entities;
using Component.Extension;
using Configuration;
using Dependent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beeant.Test.Services.Order
{
    [TestClass]
    public class OrderTestService : TestBase
    {
        [TestMethod]
        public virtual void Order()
        {
            var settlement = new SettlementDto
            {
                IsGenerate =true,
                AccountId = 30310,
                ChannelType = ChannelType.Website,
                Products = new List<OrderProductDto>
                {
                    new OrderProductDto
                    {
                        ProductId = 10930,
                        Count = 1
                    },
                    new OrderProductDto
                    {
                        ProductId = 10931,
                        Count = 1
                    },
                    new OrderProductDto
                    {
                        ProductId = 10932,
                        Count = 1
                    }
                } 
              
                
            };
            Ioc.Resolve<IOrderApplicationService>().Create(settlement);
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

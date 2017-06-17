using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Winner.Wcf;

namespace Winner.Cache.Distributed
{

    public class RemainderCache : DistributedCache
    {
        
        #region 计算Key值
        

        /// <summary>
        /// 根据key得到要存放的服务器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override IList<EndPointInfo> GetEndPoints(string key)
        {
            var longId = GenerateLongId(EncryptMd5(key));
            int hash = (int)(longId % Nodes.Count);
            if (!Nodes.ContainsKey(hash))
            {
                hash = GetHashValue(hash);
            }
            return new List<EndPointInfo> {Nodes[hash]};
        }
        /// <summary>
        /// 得到MD5加密
        /// </summary>
        /// <returns></returns>
        protected virtual string EncryptMd5(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytValue = Encoding.UTF8.GetBytes(input);
            byte[] bytHash = md5.ComputeHash(bytValue);
            var sTemp = new StringBuilder();
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp.Append(bytHash[i].ToString("X").PadLeft(2, '0'));
            }
            return sTemp.ToString().ToLower();
        }
        /// <summary>
        /// 得到缓存值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual long GenerateLongId(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;
            byte[] buffer = Encoding.Default.GetBytes(EncryptMd5(input));
            var i = BitConverter.ToInt64(buffer, 0);
            return i;
        }
        /// <summary>
        /// key节点名称,nodecount虚拟节点个数
        /// </summary>
        /// <param name="endPoint"></param>
        protected virtual void AddNode(EndPointInfo endPoint)
        {
            Nodes.Add(Nodes.Count+1,endPoint);
        }
        #endregion
    }
}

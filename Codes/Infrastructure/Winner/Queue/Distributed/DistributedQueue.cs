using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Winner.Wcf;

namespace Winner.Queue.Distributed
{

    public class DistributedQueue :  IQueue
    {
        #region 属性
        private SortedList<int, EndPointInfo> _nodes = new SortedList<int, EndPointInfo>();
        /// <summary>
        /// 虚拟节点名称和值
        /// </summary>
        protected SortedList<int, EndPointInfo> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }
        /// <summary>
        /// 服务实例
        /// </summary>
        public IWcfService WcfService { get; set; }
        #endregion

        #region 接口的实现
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxCount"></param>
        public virtual void Open(string name, int maxCount)
        {
            WcfService.Invoke<IQueueContract>(GetEndPoints(name), OpenHandle, name, maxCount);
        }
        /// <summary>
        /// 存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int Push<T>(string name, T value)
        {
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                return (int)WcfService.Invoke<IQueueContract>(GetEndPoints(name), PushHandle, name, value);
            }
            var val = SerializeJson(value);
            return (int)WcfService.Invoke<IQueueContract>(GetEndPoints(name), PushHandle, name, val);
        }
        /// <summary>
        /// 取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T Pop<T>(string name)
        {
            var value = WcfService.Invoke<IQueueContract>(GetEndPoints(name), PopHandle, name);
            if (value == null)
                return default(T);
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                return (T)value;
            }
            return DeserializeJson<T>(value.ToString());
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="name"></param>
        public virtual void Close(string name)
        {
            WcfService.Invoke<IQueueContract>(GetEndPoints(name), Close, name);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 得到值
        /// </summary>
        /// <param name="queueContract"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object OpenHandle(IQueueContract queueContract, params object[] paramters)
        {
            queueContract.Open(paramters[0] as string, (int) paramters[1]);
            return null;
        }


        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="queueContract"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object PushHandle(IQueueContract queueContract, params object[] paramters)
        {
            return queueContract.Push(paramters[0] as string, paramters[1]);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="queueContract"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object PopHandle(IQueueContract queueContract, params object[] paramters)
        {
            return queueContract.Pop(paramters[0] as string);
        }

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="queueContract"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object Close(IQueueContract queueContract, params object[] paramters)
        {
            queueContract.Close(paramters[0] as string);
            return null;
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="input"></param>
        protected virtual string SerializeJson(object input)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="input"></param>
        protected virtual T DeserializeJson<T>(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                    return default(T);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        protected virtual object DeserializeJson(string input,Type type)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                    return null;
                return Newtonsoft.Json.JsonConvert.DeserializeObject(input,type);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 计算Key值
        /// <summary>
        /// key节点名称,nodecount虚拟节点个数
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="nodecount"></param>
        protected virtual void AddNode(EndPointInfo endPoint, int nodecount)
        {
            nodecount = nodecount / 4;
            for (int i = 0; i < nodecount; i++)
            {
                byte[] temp = ComputeMd5(string.Format("{0}{1}", endPoint.Name, i));
                for (int k = 0; k < 4; k++)
                {
                    int m = Hash(temp, k);
                    Nodes[m] = endPoint;
                }
            }
        }

        /// <summary>
        /// 获取digest的nTime倍数32位的int类型
        /// </summary>
        /// <param name="digest"></param>
        /// <param name="nTime"></param>
        /// <returns></returns>
        protected virtual int Hash(byte[] digest, int nTime)
        {
            int rv = ((digest[3 + nTime * 4] & 0xFF) << 24)
                    | ((digest[2 + nTime * 4] & 0xFF) << 16)
                    | ((digest[1 + nTime * 4] & 0xFF) << 8)
                    | (digest[0 + nTime * 4] & 0xFF);

            return rv & 0xFFFF; /* Truncate to 32-bits */
        }
        /// <summary>
        /// 返回key加密后的16位MD5
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual byte[] ComputeMd5(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] keyBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            md5.Clear();
            return keyBytes;
        }

        /// <summary>
        /// 根据key得到要存放的服务器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual IList<EndPointInfo> GetEndPoints(string key)
        {
            int hash = Hash(ComputeMd5(key), 0);
            if (!Nodes.ContainsKey(hash))
            {
                hash = GetHashValue(hash);
            }
            return new List<EndPointInfo> { Nodes[hash] };
        }
        /// <summary>
        /// Nodes不包含hash时，得到hash值
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        protected virtual int GetHashValue(int hash)
        {
            var tailMap = (from coll in Nodes where coll.Key > hash select new { coll.Key }).ToList();
            if (tailMap.Count == 0)
                hash = Nodes.FirstOrDefault().Key;
            else
            {
                var firstOrDefault = tailMap.FirstOrDefault();
                if (firstOrDefault != null) hash = firstOrDefault.Key;
            }
            return hash;
        }
        #endregion
    
    }
}

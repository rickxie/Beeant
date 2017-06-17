using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Winner.Wcf;

namespace Winner.Cache.Distributed
{

    public class DistributedCache :  ICache
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
        /// 得到缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            var value = WcfService.Invoke<ICacheContract>(GetEndPoints(key), GetValue, key);
            if (value == null)
                return default(T);
            if (typeof (T).IsValueType || typeof (T) == typeof (string))
            {
                return (T) value;
            }
            return DeserializeJson<T>(value.ToString());
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object Get(string key, Type type)
        {
            var value = WcfService.Invoke<ICacheContract>(GetEndPoints(key), GetValue, key);
            if (value == null)
                return null;
            if (type.IsValueType || type == typeof (string))
            {
                return value;
            }
            return DeserializeJson(value.ToString(), type);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value, DateTime time)
        {
            object rev;
            if (value.GetType().IsValueType || value is string)
            {
                rev= WcfService.Invoke<ICacheContract>(GetEndPoints(key), SetValueByTime, key, value, time);
            }
            else
            {
                var val = SerializeJson(value);
                rev = WcfService.Invoke<ICacheContract>(GetEndPoints(key), SetValueByTime, key, val, time);
            }
            if (rev == null)
                return false;
            return (bool)rev;
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value, long timeSpan)
        {
            object rev;
            if (typeof (T).IsValueType || typeof (T) == typeof (string))
            {
                 rev=WcfService.Invoke<ICacheContract>(GetEndPoints(key), SetValueByTimeSpan, key, value, timeSpan);
            }
            else
            {
                var val = SerializeJson(value);
                rev = WcfService.Invoke<ICacheContract>(GetEndPoints(key), SetValueByTimeSpan, key, val, timeSpan);
            }
            if (rev == null)
                return false;
            return (bool) rev;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Set<T>(string key, T value)
        {
            object rev;
            if (value.GetType().IsValueType || value is string)
            {
                rev = WcfService.Invoke<ICacheContract>(GetEndPoints(key), SetValue, key, value);
            }
            else
            {
                var val = SerializeJson(value);
                rev = WcfService.Invoke<ICacheContract>(GetEndPoints(key), SetValue, key, val);
            }
            if (rev == null)
                return false;
            return (bool)rev;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public virtual bool Remove(string key)
        {
            var rev= WcfService.Invoke<ICacheContract>(GetEndPoints(key), RemoveValue, key);
            if (rev == null)
                return false;
            return (bool)rev;
        }


        #endregion

        #region 方法

        /// <summary>
        /// 得到值
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object GetValue(ICacheContract cacheService, params object[] paramters)
        {
            return cacheService.Get(paramters[0] as string);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object SetValue(ICacheContract cacheService, params object[] paramters)
        {
            return cacheService.Set(paramters[0] as string, paramters[1]);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object SetValueByTime(ICacheContract cacheService, params object[] paramters)
        {
            return cacheService.SetByTime(paramters[0] as string, paramters[1], (DateTime) paramters[2]);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object SetValueByTimeSpan(ICacheContract cacheService, params object[] paramters)
        {
            return cacheService.SetByTimeSpan(paramters[0] as string, paramters[1], (long) paramters[2]);
        }

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object RemoveValue(ICacheContract cacheService, params object[] paramters)
        {
            return cacheService.Remove(paramters[0] as string);
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
            catch (Exception ex)
            {
                WcfService.Log.AddException(ex);
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
            catch (Exception ex)
            {
                WcfService.Log.AddException(ex);
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
            catch (Exception ex)
            {
                WcfService.Log.AddException(ex);
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

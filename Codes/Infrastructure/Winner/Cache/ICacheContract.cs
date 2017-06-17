using System;

namespace Winner.Cache
{ 
   
    /// <summary>
    /// 分布式缓存接口
    /// </summary>
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://Winner.Cache", ConfigurationName = "Winner.Cache.ICacheContract")]
    public interface ICacheContract
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Cache.ICacheContract/SetByTime", ReplyAction = "http://Winner.Cache.ICacheContract/SetByTimeResponse")]
        bool SetByTime(string key, object value, DateTime time);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Cache.ICacheContract/SetByTimeSpan", ReplyAction = "http://Winner.Cache.ICacheContract/SetByTimeSpanResponse")]
        bool SetByTimeSpan(string key, object value, long timeSpan);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Cache.ICacheContract/Set", ReplyAction = "http://Winner.Cache.ICacheContract/SetResponse")]
        bool Set(string key, object value);
        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Cache.ICacheContract/Get", ReplyAction = "http://Winner.Cache.ICacheContract/GetResponse")]
        object Get(string key);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Cache.ICacheContract/Remove", ReplyAction = "http://Winner.Cache.ICacheContract/RemoveResponse")]
        bool Remove(string key);
    }
}

using System.Collections.Generic;
using Winner.Log;

namespace Winner.Wcf
{
    public delegate object InvokeDelegate<in T>(T client, params object[] paramters);

    public delegate IList<EndPointInfo> GetEndPointsDelegate(IList<EndPointInfo> endPoints, params object[] paramters);
    public interface IWcfService
    {
        /// <summary>
        /// 日志
        /// </summary>
        ILog Log { get; set; }
        IList<EndPointInfo> EndPoints { get;set; }
        GetEndPointsDelegate GetEndPointsHandle { get; set; }
        /// <summary>
        /// 群集调用wcf
        /// </summary>
        /// <param name="invokeHandler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        object Invoke<T>(InvokeDelegate<T> invokeHandler, params object[] paramters);

        /// <summary>
        /// 群集调用wcf
        /// </summary>
        /// <param name="invokeHandler"></param>
        /// <param name="getEndPointsHandler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        object Invoke<T>(InvokeDelegate<T> invokeHandler, GetEndPointsDelegate getEndPointsHandler, params object[] paramters);
        /// <summary>
        /// 群集调用wcf
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="invokeHandler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        object Invoke<T>(IList<EndPointInfo> endPoints, InvokeDelegate<T> invokeHandler, params object[] paramters);

        /// <summary>
        /// 群集调用wcf
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="invokeHandler"></param>
        /// <param name="getEndPointsHandler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        object Invoke<T>(IList<EndPointInfo> endPoints, InvokeDelegate<T> invokeHandler, GetEndPointsDelegate getEndPointsHandler, params object[] paramters);
        /// <summary>
        /// 群集调用wcf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endPointInfos"></param>
        /// <param name="invokeHandler"></param>
        /// <param name="getEndPointsHandler"></param>
        /// <param name="isSendPoint"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        object Invoke<T>(IList<EndPointInfo> endPointInfos, InvokeDelegate<T> invokeHandler,
                         GetEndPointsDelegate getEndPointsHandler, bool isSendPoint,
                         params object[] paramters);
    }
}

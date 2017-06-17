using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Winner.Log;

namespace Winner.Wcf
{
    public class WcfService : IWcfService
    {

        #region 属性
        /// <summary>
        /// 自定义配置文件路径
        /// </summary>
        public string ClientFile { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        public IList<EndPointInfo> EndPoints { get; set; }
        /// <summary>
        /// 实例
        /// </summary>
        public GetEndPointsDelegate GetEndPointsHandle { get; set; }
        private static ConcurrentDictionary<string,object> _clients = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// 客户端对象
        /// </summary>
        public static ConcurrentDictionary<string, object> Clients
        {
            get { return _clients; }
            set { _clients = value; }
        }

        private ILog _log;

        /// <summary>
        /// 实例
        /// </summary>
        public ILog Log
        {
            get
            {
                if (_log == null)
                    _log = Creator.Get<ILog>();
                return _log;
            }
            set { _log = value; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WcfService()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientFile"></param>
        /// <param name="endPoints"></param>
        public WcfService(string clientFile, IList<EndPointInfo> endPoints)
        {
            ClientFile = clientFile;
            EndPoints = endPoints;
        }

        #endregion

        #region 接口的实现
        /// <summary>
        /// 调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual object Invoke<T>(InvokeDelegate<T> handler, params object[] paramters)
        {
            return Invoke(EndPoints, handler, GetEndPointsHandle, paramters);
        }
        /// <summary>
        /// 调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="invokeHandler"></param>
        /// <param name="getEndPointsHandler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public object Invoke<T>(InvokeDelegate<T> invokeHandler, GetEndPointsDelegate getEndPointsHandler, params object[] paramters)
        {
            return Invoke(EndPoints, invokeHandler, getEndPointsHandler, paramters);
        }

        /// <summary>
        ///调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endPointInfos"></param>
        /// <param name="handler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public virtual object Invoke<T>(IList<EndPointInfo> endPointInfos, InvokeDelegate<T> handler, params object[] paramters)
        {
            return Invoke(endPointInfos, handler, GetEndPointsHandle, paramters);
        }
        /// <summary>
        /// 调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endPointInfos"></param>
        /// <param name="invokeHandler"></param>
        /// <param name="getEndPointsHandler"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public object Invoke<T>(IList<EndPointInfo> endPointInfos, InvokeDelegate<T> invokeHandler, GetEndPointsDelegate getEndPointsHandler,
                                params object[] paramters)
        {
            return Invoke(endPointInfos, invokeHandler, getEndPointsHandler, false, paramters);
        }

        /// <summary>
        /// 调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endPointInfos"></param>
        /// <param name="invokeHandler"></param>
        /// <param name="getEndPointsHandler"></param>
        /// <param name="isSendPoint"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public object Invoke<T>(IList<EndPointInfo> endPointInfos, InvokeDelegate<T> invokeHandler, GetEndPointsDelegate getEndPointsHandler, bool isSendPoint,
                                params object[] paramters)
        {
            var points = getEndPointsHandler != null ? getEndPointsHandler(endPointInfos, paramters) : endPointInfos.Where(it=>it.IsException==false).ToList();
            if (points == null || points.Count==0)
                return null;
            EndPointInfo endPoint = points.GetBestEndPoint();
            var endPoints = endPoint.GetAllEndPoints();
            for (int i = 0; i < endPoints.Count; i++)
            {
                try
                {
                    return ExecuteInovke(endPoints[i], invokeHandler, isSendPoint, paramters);
                }
                catch (Exception ex)
                {
                    Log.AddException(ex);
                    endPoints[i].IsException = true;
                    Action<EndPointInfo> action = CheckConnectionAlive<T>;
                    action.BeginInvoke(endPoint, null, null);
                }
            }
            return null;
        }
        static private readonly object CheckAliveLocker=new object();
        /// <summary>
        /// 检查连接一次
        /// </summary>
        /// <param name="endPoint"></param>
        protected virtual void CheckConnectionAlive<T>(EndPointInfo endPoint)
        {
            lock (CheckAliveLocker)
            {
                if (endPoint.IsStartCheckAlive || !endPoint.IsException)
                    return;
                endPoint.IsStartCheckAlive = true;
            }
            System.Threading.Thread.Sleep(endPoint.CheckAlivePeriod);
            try
            {
                using (var channel = new CustomClientChannel<T>(endPoint.Name, ClientFile))
                {
                    var client = channel.CreateChannel() as ICommunicationObject; 
                    if (client.State != CommunicationState.Opened)
                        client.Open();
                    client.Close();
                    endPoint.IsException = false;
                    endPoint.IsStartCheckAlive = false;
                }
            }
            catch (Exception ex)
            {
                endPoint.IsException = true;
                endPoint.IsStartCheckAlive = false;
                Action<EndPointInfo> action = CheckConnectionAlive<T>;
                action.BeginInvoke(endPoint, null, null);
            }
        }

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="handler"></param>
        /// <param name="isSendPoint"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object ExecuteInovke<T>(EndPointInfo endPoint, InvokeDelegate<T> handler, bool isSendPoint,
                                                  params object[] paramters)
        {
            var key = string.Format("{0}{1}", typeof(T), endPoint.Name);
            CheckClient<T>(endPoint, key);
            if (!TryOpen<T>(key, endPoint))
                return null;
            if (isSendPoint)
            {
                if (paramters == null)
                {
                    paramters = new object[] { endPoint };
                }
                else
                {
                    var temp = new List<object>(paramters) { endPoint };
                    paramters = temp.ToArray();
                }
            }
            object rev= handler((T) Clients[key], paramters);
            endPoint.UseConnect();
            return rev;
        }
        static public object ClientLocker = new object();
        /// <summary>
        /// 检查客户端
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="key"></param>
        protected virtual void CheckClient<T>(EndPointInfo endPoint, string key)
        {
            if (!Clients.ContainsKey(key))
            {
                var channel = new CustomClientChannel<T>(endPoint.Name, ClientFile);
                var client = channel.CreateChannel();
                Clients.TryAdd(key, client);
            }
        }
        static private readonly object Locker = new object();
        /// <summary>
        /// 尝试打开
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected virtual bool TryOpen<T>(string key, EndPointInfo endPoint)
        {
            var client = (ICommunicationObject)Clients[key];
            if (client.State != CommunicationState.Opened)
            {
                try
                {
                    client.Open();
                    return true;
                }
                catch
                {
                    CreateNewClientChannel<T>(key, endPoint);
                }
            }
            return true;
        }
        /// <summary>
        /// 创建新通道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="endPoint"></param>
        protected virtual bool CreateNewClientChannel<T>(string key, EndPointInfo endPoint)
        {
            lock (Locker)
            {
                try
                {
                    ((ICommunicationObject) Clients[key]).Close();
                }
                catch
                {

                }
                finally
                {
                    Clients[key] = null;
                }
                var channel = new CustomClientChannel<T>(endPoint.Name, ClientFile);
                var newClient = channel.CreateChannel() as ICommunicationObject;
                Clients[key] = newClient;
                newClient.Open();
                return true;
            }
        }

        #endregion
    }
}

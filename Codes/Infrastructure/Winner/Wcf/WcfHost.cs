using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Winner.Log;

namespace Winner.Wcf
{
    public class WcfHost: IWcfHost
    {
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
        private static readonly object Locker=new object();
   
        protected IDictionary<Type, AutoResetEvent> AutoResetEvents { get; set; }=new Dictionary<Type, AutoResetEvent>();
        /// <summary>
        /// 开启
        /// </summary>
        /// <returns></returns>
        public virtual bool Start(Type type)
        {
          
            lock (Locker)
            {
                try
                {
                    if (AutoResetEvents.ContainsKey(type))
                    {
                        AutoResetEvents.Remove(type);
                    }
                    AutoResetEvents.Add(type, new AutoResetEvent(false));
                    var thread = new Thread(StartListen) { IsBackground = true };
                    thread.Start(type);
                }
                catch (Exception ex)
                {
                    Log.AddException(ex);
                }
            }
            return true;
        }
        /// <summary>
        /// 停止 
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop(Type type)
        {
            lock (Locker)
            {
                if (AutoResetEvents.ContainsKey(type))
                {
                    AutoResetEvents[type].Set();
                    AutoResetEvents[type] = null;
                    AutoResetEvents.Remove(type);
                }

            }
            return true;
        }
        #region 创建线程服务
        /// <summary>
        /// 开启监听
        /// </summary>
        /// <returns></returns>
        public virtual void StartListen(object obj)
        {
            var type = obj as Type;
            try
            {
            
                ServiceHost serviceHost = new ServiceHost(type);
                if (serviceHost.State != CommunicationState.Opened)
                {
                    serviceHost.Open();
                }
                AutoResetEvents[type].WaitOne();
                serviceHost.Close();
            }
            catch (Exception ex)
            {
                Log.AddException(ex);
                Start(type);
            }
            
        }
   
   
        #endregion
    }
}

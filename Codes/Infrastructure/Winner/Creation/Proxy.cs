using System.Collections.Generic;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;

namespace Winner.Creation
{


    public class Proxy : RealProxy, IProxy
    {
        #region 属性
        /// <summary>
        /// 实例
        /// </summary>
        public object Target { get; set; }
        /// <summary>
        /// 方法执行前的委托
        /// </summary>
        public IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> Befores { get; set; }
        /// <summary>
        /// 方法执行后的委托
        /// </summary>
        public IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> Afters { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 对象，方法执行前，方法执行后
        /// </summary>
        /// <param name="target"></param>
        /// <param name="befores"></param>
        /// <param name="afters"></param>
        public Proxy(object target, IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> befores, IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> afters)
            : base(target.GetType())
        {
            Target = target;
            Befores = befores;
            Afters = afters;
        }
        #endregion

        #region 接口的实现
        /// <summary>
        /// 得到代理
        /// </summary>
        /// <returns></returns>
        public virtual object GetProxy()
        {
            return GetTransparentProxy();
        }

        #endregion

        #region Aop调用
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            var callMessage = (IMethodCallMessage)msg;
            Proceede(new AopMethodInfo{IsBeforeCall=true,Message = callMessage}, Befores);
            object returnValue = callMessage.MethodBase.Invoke(Target, callMessage.Args);
            Proceede(new AopMethodInfo { IsAfterCall = true, Message = callMessage,ReturnValue = returnValue}, Afters);
            return new ReturnMessage(returnValue, new object[0], 0, null, callMessage);
        }

        /// <summary>
        /// 执行注入方法
        /// </summary>
        /// <param name="info"></param>
        /// <param name="handles"></param>
        protected virtual void Proceede(AopMethodInfo info, IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> handles)
        {
            string name = info.Message.Properties["__MethodName"].ToString();
            if (handles == null || !handles.ContainsKey(name))
                return;
            var h = handles[name];
            if (h.Value)
                h.Key.BeginInvoke(info, null, null);
            else
                h.Key(info);
        }

        #endregion



    }
  
}

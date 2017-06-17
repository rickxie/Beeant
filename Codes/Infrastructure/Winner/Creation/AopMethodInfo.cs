using System.Runtime.Remoting.Messaging;

namespace Winner.Creation
{
    public class AopMethodInfo
    {
        public IMethodCallMessage Message { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public object ReturnValue { get; set; }
        /// <summary>
        /// 是否执行前调用
        /// </summary>
        public bool IsBeforeCall { get; set; }
        /// <summary>
        /// 是否执行后调用
        /// </summary>
        public bool IsAfterCall { get; set; }
    }
}

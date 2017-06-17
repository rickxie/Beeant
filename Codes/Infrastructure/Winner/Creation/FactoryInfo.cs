using System;
using System.Collections.Generic;

namespace Winner.Creation
{
    [Serializable]
    public class FactoryInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类
        /// </summary>
        public object Target { get; set; }
        /// <summary>
        /// 类
        /// </summary>
        public Type TargetType
        {
            get { return Target.GetType(); }
        }
        /// <summary>
        /// 是否为单例
        /// </summary>
        public bool IsSingle { get; set; }
        /// <summary>
        /// AOP开始切面
        /// </summary>
        public IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> BeforeCuts { get; set; }
        /// <summary>
        /// AOP结束切面
        /// </summary>
        public IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> AfterCuts { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public IList<FactoryPropertyInfo> Properties { get; set; } 
 
    }
}

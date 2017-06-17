using System;

namespace Winner.Persistence.Route
{
    [Serializable]
    public class RuleInfo
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 是否Hash
        /// </summary>
        public bool IsHash { get; set; }
        /// <summary>
        /// 是否适用存储
        /// </summary>
        public bool IsSave { get; set; }
    }
}

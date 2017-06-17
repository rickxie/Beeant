using System;
using Winner.Wcf;

namespace Winner.Storage
{
    [Serializable]
    public class DataServiceInfo
    {
    
        /// <summary>
        /// 类型
        /// </summary>
        public DataServiceType Type { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        public EndPointInfo EndPoint { get; set; }
    }
}

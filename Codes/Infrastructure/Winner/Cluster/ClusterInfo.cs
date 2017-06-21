using Winner.Wcf;

namespace Winner.Cluster
{
    public class ClusterInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 传输
        /// </summary>
        public IWcfService WcfService { get; set; }
    }
}

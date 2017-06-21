namespace Winner.Cluster
{

    /// <summary>
    /// 分布式缓存接口
    /// </summary>
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://Winner.Cluster", ConfigurationName = "Winner.Cluster.IClusterContract")]
    public interface IClusterContract
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Cluster.IClusterContract/Execute", ReplyAction = "http://Winner.Cluster.IClusterContract/Execute")]
        void Execute(string name,int count,int index,object value);
      
    }
}

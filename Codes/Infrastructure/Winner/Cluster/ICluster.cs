namespace Winner.Cluster
{
    public interface ICluster
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        bool Execute(ClusterInfo info);
    }
}

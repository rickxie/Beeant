using System;
using System.Collections.Generic;

namespace Winner.Cluster
{
    public class ClusterService : IClusterContract
    {
        /// <summary>
        /// 节点
        /// </summary>
        public IDictionary<string, Action<ClusterArgsInfo>> Handles { get; set; } = new Dictionary<string, Action<ClusterArgsInfo>>();
      
        public void Execute(string name, int count, int index, object value)
        {
            if(!Handles.ContainsKey(name))
                return;
            var args = new ClusterArgsInfo
            {
                Name = name,
                Index = index,
                Count=count,
                Value = value
            };
            Handles[name].BeginInvoke(args, null, null);
        }
    }
}

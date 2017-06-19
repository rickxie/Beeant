using System;
using System.Collections.Generic;
using Winner.Wcf;

namespace Winner.Cluster
{
    public class Cluster : ICluster
    {
   

        /// <summary>
        /// 虚拟节点名称和值
        /// </summary>
        protected IDictionary<string, IList<EndPointInfo>> Nodes { get; set; } = new SortedList<string, IList<EndPointInfo>>();

        public virtual bool Execute(ClusterInfo info)
        {
            if (!Nodes.ContainsKey(info.Name) || Nodes[info.Name] == null || Nodes[info.Name].Count == 0)
                return false;
            object value=info.Value;
            if (value != null && (value.GetType().IsValueType || value.GetType() != typeof (string)))
            {
                value = SerializeJson(value);
            }
            for (int i = 0; i < Nodes[info.Name].Count; i++)
            {
                Action<ClusterInfo, int, object> action = BeginExecute;
                action.BeginInvoke(info, i, value, null, null);
            }
            return true;
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual void BeginExecute(ClusterInfo info, int index, object value)
        {

            info.WcfService.Invoke<IClusterContract>(new List<EndPointInfo> { Nodes[info.Name][index] }, Tigger, info.Name, index,value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="clusterService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object Tigger(IClusterContract clusterService, params object[] paramters)
        {
            clusterService.Execute(paramters[0] as string, (int) paramters[1], paramters[3]);
            return null;
        }


        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="input"></param>
        protected virtual string SerializeJson(object input)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(input);
        }
    }
}

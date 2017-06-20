using System;
using System.Collections.Generic;
using Winner.Wcf;

namespace Winner.Cluster
{
    public class Cluster : ICluster
    {
   

     
        public virtual bool Execute(ClusterInfo info)
        {
            if (info == null || info.WcfService==null || info.WcfService.EndPoints==null || info.WcfService.EndPoints.Count==0)
                return false;
            object value=info.Value;
            if (value != null && (value.GetType().IsValueType || value.GetType() != typeof (string)))
            {
                value = SerializeJson(value);
            }
            for (int i = 0; i < info.WcfService.EndPoints.Count; i++)
            {
                Action<ClusterInfo, int,  object> action = BeginExecute;
                action.BeginInvoke(info, i, value, null, null);
            }
            return true;
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="info"></param>
        /// <param name="count"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual void BeginExecute(ClusterInfo info,int index, object value)
        {

            info.WcfService.Invoke<IClusterContract>(new List<EndPointInfo> { info.WcfService.EndPoints[index] }, Tigger, info.Name, info.WcfService.EndPoints.Count, index, value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="clusterService"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        protected virtual object Tigger(IClusterContract clusterService, params object[] paramters)
        {
            clusterService.Execute(paramters[0] as string, (int) paramters[1], (int)paramters[1], paramters[3]);
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

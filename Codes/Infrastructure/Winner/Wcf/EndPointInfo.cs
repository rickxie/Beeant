using System;
using System.Collections.Generic;
using System.Linq;

namespace Winner.Wcf
{
    public class EndPointInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 当前并发数
        /// </summary>
        public virtual int CurrentCount { get; set; }
        /// <summary>
        /// 读取故障转移的数据
        /// </summary>
        public virtual IList<EndPointInfo> Failovers { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 是否为异常
        /// </summary>
        public virtual bool IsException { get; set; }
        /// <summary>
        /// 是否启用了检查
        /// </summary>
        public virtual bool IsStartCheckAlive { get; set; }

        private int _checkAlivePeriod = 5000;
        /// <summary>
        /// 重置为非异常周期
        /// </summary>
        public virtual int CheckAlivePeriod
        {
            get { return _checkAlivePeriod; }
            set { _checkAlivePeriod = value; }
        }

        /// <summary>
        /// 得到读取时候的连接
        /// </summary>
        /// <returns></returns>
        public virtual IList<EndPointInfo> GetAllEndPoints()
        {
            var rev = new List<EndPointInfo>();
            if (Failovers != null && Failovers.Count > 0)
                rev = Failovers.OrderBy(it => it.IsException).ThenBy(it => it.CurrentCount).ToList();
            if (IsException) rev.Add(this);
            else rev.Insert(0, this);
            return rev;
        }
        /// <summary>
        /// 使用连接
        /// </summary>
        public virtual void UseConnect()
        {
            if (CurrentCount >= int.MaxValue)
            {
                CurrentCount = 0;
            }
            CurrentCount++;
        }

    }

    public static class EndPointExtension
    {
        public static EndPointInfo GetBestEndPoint(this IList<EndPointInfo> endPoints)
        {
            return endPoints.OrderBy(it => it.IsException).ThenBy(it => it.CurrentCount).FirstOrDefault();
        }
      
    }
}

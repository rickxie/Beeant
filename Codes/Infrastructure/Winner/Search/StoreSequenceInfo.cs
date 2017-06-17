
using System;

namespace Winner.Search
{
    [Serializable]
    public class StoreSequenceInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 密度
        /// </summary>
        public double Density { get; set; }
        /// <summary>
        /// 百分比
        /// </summary>
        public float Percentage { get; set; }
    }
   
}

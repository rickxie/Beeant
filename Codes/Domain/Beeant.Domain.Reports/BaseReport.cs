using System;
using Beeant.Domain.Entities;

namespace Beeant.Domain.Reports
{


    [Serializable]
    public class BaseReport:BaseEntity
    {
    

      
        /// <summary>
        /// 数量
        /// </summary>
        public virtual int RecordCount { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public virtual string Day { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public virtual string Month { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public virtual string Year { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public virtual string Week { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public virtual string Quarter { get; set; }

    }
    
}

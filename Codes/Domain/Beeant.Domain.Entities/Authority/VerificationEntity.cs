using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Authority
{
    /// <summary>
    /// 验证返回的类
    /// </summary>
    [Serializable]
    public class VerificationEntity
    {
  
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>
        /// 控件
        /// </summary>
        public IDictionary<string, bool> Controls { get; set; } 
    }
}

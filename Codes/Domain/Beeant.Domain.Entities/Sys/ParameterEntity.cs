using System;

namespace Beeant.Domain.Entities.Sys
{
    [Serializable]
    public class ParameterEntity : BaseEntity<ParameterEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 执行的类名
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

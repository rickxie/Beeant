using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class UnitEntity : BaseEntity<UnitEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 货币代码
        /// </summary>
        public int Sequence { get; set; }
    }
}
using System;

namespace Beeant.Domain.Entities.Workflow
{
    [Serializable]
    public class LevelEntity : BaseEntity<LevelEntity>
    {
  
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 颜色值
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}

using System;

namespace Beeant.Domain.Entities.Search
{
    [Serializable]
    public class SimilarEntity : BaseEntity<SimilarEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 搜索的次数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 词
        /// </summary>
        public WordEntity Word { get; set; }
    }
}

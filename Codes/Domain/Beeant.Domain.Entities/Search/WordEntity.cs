using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Search
{
    [Serializable]
    public class WordEntity : BaseEntity<WordEntity>
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 名称长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        public string Pinyin { get; set; }
        /// <summary>
        /// 原词
        /// </summary>
        public string Original { get; set; }
        /// <summary>
        /// 是否禁止
        /// </summary>
        public bool IsForbid { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public long Count { get; set; }
       
        /// <summary>
        /// 关联词
        /// </summary>
        public IList<SimilarEntity> Similars { get; set; }
        /// <summary>
        /// 是否禁止
        /// </summary>
        public string IsForbidName
        {
            get { return this.GetStatusName(IsForbid); }
        }
       
    }
}

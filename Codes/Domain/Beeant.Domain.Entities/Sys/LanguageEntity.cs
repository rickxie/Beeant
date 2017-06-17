using System;

namespace Beeant.Domain.Entities.Sys
{
    [Serializable]
    public class LanguageEntity: BaseEntity<LanguageEntity>   
    {
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class CategoryEntity : BaseEntity<CategoryEntity>
    {
        /// <summary>
        /// 父类
        /// </summary>
        public CategoryEntity Parent { get; set; }
  
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        public string Pinyin { get; set; }
        /// <summary>
        /// 首字母
        /// </summary>
        public string Initial { get; set; }
        /// <summary>
        /// 链接地址        
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否允许发布
        /// </summary>
        public bool IsPublish { get; set; }
        /// <summary>
        /// 是否展示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 图片数量
        /// </summary>
        public int ImageCount { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public IList<PropertyEntity> CategoryProperties { get; set; }
        /// <summary>
        /// 子类
        /// </summary>
        public IList<CategoryEntity> Children { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public string IsPublishName
        {
            get { return GetLanguage(typeof (BaseEntity).FullName, "StatusName", IsPublish); }
        }

        /// <summary>
        /// 是否展示名称
        /// </summary>
        public string IsShowName
        {
            get { return this.GetShowName(IsShow); }
        }
    
      
    }
}

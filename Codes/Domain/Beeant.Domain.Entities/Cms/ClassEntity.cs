using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Cms
{
    [Serializable]
    public class ClassEntity : BaseEntity<ClassEntity>
    {
        /// <summary>
        ///父菜单
        /// </summary>
        public ClassEntity Parent { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 是否允许前台选择
        /// </summary>
        public bool IsPublic{ get; set; }
        /// <summary>
        /// 是否运行发布
        /// </summary>
        public bool IsPublish { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsPublicName
        {
            get { return this.GetStatusName(IsPublic); }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsPublishName
        {
            get { return this.GetStatusName(IsPublish); }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetStatusName(IsUsed); }
        }
        /// <summary>
        ///子类
        /// </summary>
        public IList<ClassEntity> Children { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public IList<ContentEntity> Contents { get; set; } 
    }
}

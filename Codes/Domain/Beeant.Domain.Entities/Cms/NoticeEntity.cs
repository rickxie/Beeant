using System;

namespace Beeant.Domain.Entities.Cms
{


    [Serializable]
    public class NoticeEntity : BaseEntity<NoticeEntity>
    {
        /// <summary>
        /// 账户信息
        /// </summary>
        public CmsEntity Cms { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
      
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 是否展示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 是否展示名称
        /// </summary>
        public string IsShowName
        {
            get { return this.GetShowName(IsShow); }
        }

 
 

    }
    
}

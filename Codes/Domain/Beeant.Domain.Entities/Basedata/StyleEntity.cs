using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class StyleEntity : BaseEntity<StyleEntity>
    {

        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public StyleType Type { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        
        /// <summary>
        /// 优惠券备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否需要密码
        /// </summary>
        public string IsShowName
        {
            get { return this.GetShowName(IsShow); }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName
        {
            get { return Type.GetName(); }
        }
    }
}

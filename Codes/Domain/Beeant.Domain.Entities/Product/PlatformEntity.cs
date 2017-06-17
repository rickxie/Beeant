using System;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class PlatformEntity : BaseEntity<PlatformEntity>
    {
        /// <summary>
        /// 商品
        /// </summary>
        public GoodsEntity Goods { get; set; }
        /// <summary>
        /// 平台类型
        /// </summary>
        public PlatformType Type { get; set; }
        /// <summary>
        /// 外部编号
        /// </summary>
        public string DataId { get; set; }
        /// <summary>
        /// 同步时间
        /// </summary>
        public string SynchTime { get; set; }
        /// <summary>
        /// 平台类型
        /// </summary>
        public string TypeName
        {
            get {return Type.GetName(); }
        }

    }
}

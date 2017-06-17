using System;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class CommodityImageEntity : BaseEntity<CommodityImageEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public CommodityEntity Commodity { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        ///文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FileByte { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string FullFileName
        {
            get { return this.GetFullFileName(FileName); }
        }

     

        #region 业务处理

        /// <summary>
        /// 添加处理
        /// </summary>
        protected override void SetAddBusiness()
        {
            InvokeItemLoader("Commodity");
            if (Commodity == null)
                return;
            Site = Commodity.Site;
            base.SetAddBusiness();
        }

        #endregion
    }
}

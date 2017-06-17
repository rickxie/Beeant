using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class CommodityEntity : BaseEntity<CommodityEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public SiteEntity Site { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public CatalogEntity Catalog { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public CommodityStatusType Status { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 是否显示价格
        /// </summary>
        public bool IsShowPrice { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string VenderName { get; set; }
        /// <summary>
        /// 供应商联系人
        /// </summary>
        public string VenderLinkman { get; set; }
        /// <summary>
        /// 供应商手机号码
        /// </summary>
        public string VenderMobile { get; set; }
        /// <summary>
        /// 供应商地址
        /// </summary>
        public string VenderAddress { get; set; }
        /// <summary>
        /// 密保
        /// </summary>
        public string Password { get; set; }
    
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
        /// <summary>
        /// 图册图片
        /// </summary>
        public string AlbumFileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] AlbumFileByte { get; set; }
        /// <summary>
        /// 是否创建图册
        /// </summary>
        public bool IsCreateAlbum { get; set; }
        /// <summary>
        /// 全部文件路径
        /// </summary>
        public string AlbumFullFileName
        {
            get { return this.GetFullFileName(AlbumFileName); }
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName
        {
            get { return Status.GetName(); }
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string IsShowPriceName
        {
            get { return this.GetShowName(IsShowPrice); }
        }

        /// <summary>
        /// 细节图
        /// </summary>
        public IList<CommodityImageEntity> CommodityImages { get; set; }
        /// <summary>
        /// 细节图
        /// </summary>
        public IList<CommodityTagEntity> CommodityTags { get; set; }
        #region 业务处理

        /// <summary>
        /// 添加处理
        /// </summary>
        protected override void SetAddBusiness()
        {
            InvokeItemLoader("Catalog");
            if (Catalog == null)
                return;
            Site = Catalog.Site;
            base.SetAddBusiness();
        }

        #endregion
    }
}

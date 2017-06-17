using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Site
{
    public class CommodityDomainService : RealizeDomainService<CommodityEntity>
    {

        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"},
               new FileEntity {FilePropertyName = "AlbumFileName",BytePropertyName = "AlbumFileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
        /// <summary>
        /// 图片信息
        /// </summary>
        public IDomainService CommodityImageDomainService { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public IDomainService CommodityTagDomainService { get; set; }
        private IDictionary<string, IUnitofworkHandle> _itemHandles;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, IUnitofworkHandle> ItemHandles
        {
            get
            {
                return _itemHandles ?? (_itemHandles = new Dictionary<string, IUnitofworkHandle>
                    {
                        {"CommodityImages", new UnitofworkHandle<CommodityImageEntity>{DomainService= CommodityImageDomainService}},
                       {"CommodityTags", new UnitofworkHandle<CommodityTagEntity>{DomainService= CommodityTagDomainService}}
                   });
            }
            set
            {
                base.ItemHandles = value;
            }
        }

        private IDictionary<string, ItemLoader<CommodityEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<CommodityEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<CommodityEntity>>
                    {
                        {"Catalog", LoadCatalog}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual void LoadCatalog(CommodityEntity info)
        {
            info.Catalog =  info.Catalog.SaveType == SaveType.Add ? info.Catalog : Repository.Get<CatalogEntity>(info.Catalog.Id);
           
        }
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CommodityEntity info)
        {
            var rev = ValidateCatalog(info, null);
            return rev;
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(CommodityEntity info)
        {
            var dataEntity = Repository.Get<CommodityEntity>(info.Id);
            var rev = ValidateCatalog(info, dataEntity) ;
            return rev;
        }

 

        #endregion
        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCatalog(CommodityEntity info, CommodityEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Catalog.Id))
                return true;
            if (info.Catalog != null && info.Catalog.SaveType == SaveType.Add)
                return true;
            if (info.Catalog != null && info.Catalog.Id!=0)
            {
                if (dataEntity != null && dataEntity.Catalog != null && dataEntity.Catalog.Id == info.Catalog.Id)
                    return true;
                info.Catalog = Repository.Get<CatalogEntity>(info.Catalog.Id);
                if (info.Catalog == null)
                {
                    info.AddErrorByName(typeof(CatalogEntity).FullName, "NoExist");
                    return false;
                }
                if (dataEntity != null && dataEntity.Site != null && info.Catalog != null &&
                    info.Catalog.Site != null && info.Catalog.Site.Id != dataEntity.Site.Id)
                {
                    info.AddErrorByName(typeof(CatalogEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(CatalogEntity).FullName, "NoExist");
            return false;
        }
    
    }
}

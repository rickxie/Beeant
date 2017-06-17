using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Site;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Site
{
    public class CommodityImageDomainService : RealizeDomainService<CommodityImageEntity>
    {

        private IList<FileEntity> _fileProperties = new List<FileEntity>
            {
               new FileEntity {FilePropertyName = "FileName",BytePropertyName = "FileByte"}
            };

        public override IList<FileEntity> FileProperties
        {
            get { return _fileProperties; }
            set { _fileProperties = value; }
        }
        private IDictionary<string, ItemLoader<CommodityImageEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<CommodityImageEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<CommodityImageEntity>>
                    {
                        {"Commodity", LoadCommodity}
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
        protected virtual void LoadCommodity(CommodityImageEntity info)
        {
            info.Commodity =  info.Commodity.SaveType == SaveType.Add ? info.Commodity : Repository.Get<CommodityEntity>(info.Commodity.Id);
           
        }
        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(CommodityImageEntity info)
        {
            var rev = ValidateCatalog(info, null);
            return rev;
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(CommodityImageEntity info)
        {
            var dataEntity = Repository.Get<CommodityImageEntity>(info.Id);
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
        protected virtual bool ValidateCatalog(CommodityImageEntity info, CommodityImageEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Commodity.Id))
                return true;
            if (info.Commodity != null && info.Commodity.SaveType == SaveType.Add)
                return true;
            if (info.Commodity != null && info.Commodity.Id!=0)
            {
                if (dataEntity != null && dataEntity.Commodity != null && dataEntity.Commodity.Id == info.Commodity.Id)
                    return true;
                info.Commodity = Repository.Get<CommodityEntity>(info.Commodity.Id);
                if (info.Commodity == null)
                {
                    info.AddErrorByName(typeof(CommodityEntity).FullName, "NoExist");
                    return false;
                }
                if (dataEntity != null && dataEntity.Site != null && info.Commodity != null &&
                    info.Commodity.Site != null && info.Commodity.Site.Id != dataEntity.Site.Id)
                {
                    info.AddErrorByName(typeof(CommodityEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(CommodityEntity).FullName, "NoExist");
            return false;
        }
    
    }
}

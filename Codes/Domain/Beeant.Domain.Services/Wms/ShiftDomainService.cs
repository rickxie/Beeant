using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Wms;
using Winner.Persistence;

namespace Beeant.Domain.Services.Wms
{
    public class ShiftDomainService : RealizeDomainService<ShiftEntity>
    {
        /// <summary>
        /// 库存商品
        /// </summary>
        public IDomainService ShelfDomainService { get; set; }



        #region 重写事务
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
                        {"Shelf", new UnitofworkHandle<ShelfEntity>{DomainService= ShelfDomainService}}
                    });
            }
            set
            {
                base.ItemHandles = value;
            }
        }
        private IDictionary<string, ItemLoader<ShiftEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<ShiftEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<ShiftEntity>>
                    {
                        {"Shelf", LoadShelf} 
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }



 
        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadShelf(ShiftEntity info)
        {
            if(info.Shelf.SaveType==SaveType.None)
                return;
            info.Shelf = Repository.Get<ShelfEntity>(info.Shelf.Id);
        }
      
        #endregion

        #region 重写验证



        #region 添加验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(ShiftEntity info)
        {
            var rev = ValidateShelf(info, null);
            return rev;
        }


        #endregion

        #region 修改验证
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(ShiftEntity info)
        {
            var dataEntity = Repository.Get<ShiftEntity>(info.Id);
            var rev = ValidateCount(info, dataEntity);
            return rev;
        }
        /// <summary>
        /// 验证数量
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCount(ShiftEntity info, ShiftEntity dataEntity)
        {
            if (dataEntity.Count != 0)
            {
                info.AddError("HasCountNotAllowRemove");
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 验证仓库
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateShelf(ShiftEntity info, ShiftEntity dataEntity)
        {

            if (!info.HasSaveProperty(it => it.Shelf.Id))
                return true;
            if (info.Shelf != null && info.Shelf.SaveType == SaveType.Add)
                return true;
            if (info.Shelf != null && info.Shelf.Id!=0)
            {
                if (dataEntity != null && dataEntity.Shelf != null && dataEntity.Shelf.Id == info.Shelf.Id)
                    return true;
                var shelf = Repository.Get<ShelfEntity>(info.Shelf.Id);
                if (shelf == null)
                {
                    info.AddErrorByName(typeof(ShelfEntity).FullName, "NoExist");
                    return false;
                }
                if (!shelf.IsUsed)
                {
                    info.AddErrorByName(typeof(ShelfEntity).FullName, "UnUsed");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(ShelfEntity).FullName, "NoExist");
            return false;
        }
        #endregion


    }
}

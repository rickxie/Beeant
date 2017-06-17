using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Product;
using Beeant.Domain.Services.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Application.Services.Product
{
    public class GoodsApplicationService : RealizeApplicationService<GoodsEntity>, IGoodsApplicationService
    {
        /// <summary>
        /// 缓存实例
        /// </summary>
        /// 
        public ICacheRepository CacheRepository { get; set; }

     
        /// <summary>
        /// 得到事务接口
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected override IList<IUnitofwork> Handle(IList<GoodsEntity> infos)
        {
            foreach (var info in infos)
            {
                SetProducts(info);
                SetGoodsImages(info);
                SetGoodsProperties(info);
                SetCatalogueGoodses(info);
            }
            var unitofworks = base.Handle(infos);
            foreach (var info in infos)
            {
                SetDefaultImage(info);
            }
            return unitofworks;
        }


        #region 设置图片存储类型

        /// <summary>
        /// 设置图片类型
        /// </summary>
        /// <param name="goods"></param>
        protected virtual void SetDefaultImage(GoodsEntity goods)
        {
            if (goods == null || goods.GoodsImages == null) return;
            if (goods.HasSaveProperty(it => it.GoodsImages) || goods.Properties == null)
            {
                var goodsImage = goods.GoodsImages.FirstOrDefault();
                goods.FileName = goodsImage == null ? "" : goodsImage.FileName;
                var index = goods.FileName.IndexOf("Files/");
                if (index > -1)
                {
                    goods.FileName = goods.FileName.Substring(index, goods.FileName.Length - index);
                }
                if (goods.Properties != null)
                    goods.SetProperty(it => it.FileName);
                if (goods.Products != null)
                {
                    foreach (var product in goods.Products)
                    {
                        var productGoodsImage =
                            goods.GoodsImages.FirstOrDefault(
                                it => it.Product == product || it.Product.Id == product.Id && product.Id != 0);
                        productGoodsImage = productGoodsImage ?? goodsImage;
                        product.FileName = productGoodsImage == null ? "" : productGoodsImage.FileName;
                        var productIndex = product.FileName.IndexOf("Files/");
                        if (productIndex > -1)
                        {
                            product.FileName = product.FileName.Substring(productIndex, product.FileName.Length - productIndex);
                        }
                        if (product.Properties != null)
                            product.SetProperty(it => it.FileName);
                    }
                }
            }

        }
        /// <summary>
        /// 设置图片类型
        /// </summary>
        /// <param name="goods"></param>
        protected virtual void SetGoodsImages(GoodsEntity goods)
        {
            if(goods==null  || goods.GoodsImages == null)return;
            if (goods.HasSaveProperty(it => it.GoodsImages) || goods.Properties == null)
            {
                foreach (var info in goods.GoodsImages)
                {
                    info.Goods = goods;
                    SetGoodsImageSaveType(goods, info);
                }
              
            }

        }

        /// <summary>
        /// 设置图片存储方式 
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="info"></param>
        protected virtual void SetGoodsImageSaveType(GoodsEntity goods, GoodsImageEntity info)
        {
            if (goods.SaveType == SaveType.Add || info.Id<=0)
            {
                info.SaveType = SaveType.Add;
                info.Id = 0;
            }
            else if(info.SaveType!=SaveType.Remove)
            {
                info.SaveType = SaveType.Modify;
            }
            else
            {
                info.SaveType = SaveType.Remove;
                
            }
        }
        #endregion

        #region 设置属性存储类型
        /// <summary>
        /// 得到属性
        /// </summary>
        /// <returns></returns>
        protected virtual IList<GoodsPropertyEntity> GetDataGoodsProperties(GoodsEntity goods)
        {
            if (goods.Id == 0) return null;
            var query = new QueryInfo();
            query.Query<GoodsPropertyEntity>().Where(it => it.Goods.Id == goods.Id).Select(it=>new object[]{it.Id,it.Property.Name,it.Value,it.Property.IsSku});
            return Repository.GetEntities<GoodsPropertyEntity>(query);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="goods"></param>
        protected virtual void SetGoodsProperties(GoodsEntity goods)
        {
            if (goods == null || goods.GoodsProperties == null) return;
            if (goods.HasSaveProperty(it => it.GoodsProperties) || goods.Properties == null)
            {
                if (!goods.HasSaveProperty(it => it.GoodsProperties) || goods.GoodsProperties == null)
                    return;
                var dataEntities = GetDataGoodsProperties(goods);
                foreach (var info in goods.GoodsProperties)
                {
                    if (dataEntities != null && info.Property != null && info.Product != null && info.Product.Id > 0)
                    {
                        var t =
                            dataEntities.FirstOrDefault(
                                it =>
                                it.Property != null && it.Property.Name == info.Property.Name &&
                                it.Value == info.Value);
                        if (t != null)
                        {
                            info.Id = t.Id;
                            info.SaveType = SaveType.Modify;
                        }

                    }
                    else if (goods.SaveType == SaveType.Add || info.Id == 0)
                    {
                        info.SaveType = SaveType.Add;
                    }
                    else if (dataEntities != null && dataEntities.FirstOrDefault(it => it.Id == info.Id) != null)
                    {
                        info.SaveType = SaveType.Modify;
                    }
                 
                    info.Goods = goods;
                }
            }
        }

        #endregion

        #region 设置产品存储类型

        /// <summary>
        /// 设置产品
        /// </summary>
        /// <param name="goods"></param>
        protected virtual void SetProducts(GoodsEntity goods)
        {
            if (goods == null || goods.Products == null) return;
            if (goods.HasSaveProperty(it => it.Products) || goods.Properties == null)
            {
                foreach (var info in goods.Products)
                {
                    if (goods.SaveType == SaveType.Add || info.Id == 0)
                    {
                        info.SaveType = SaveType.Add;
                        info.Id = 0;
                    }
                    else if (info.Id > 0 && info.SaveType == SaveType.Remove)
                    {
                        info.SaveType = SaveType.Remove;
                    }
                    else
                    {
                        info.SetProperty(it => it.Cost).SetProperty(it => it.Count).SetProperty(it => it.Name)
                            .SetProperty(it => it.OrderMinCount).SetProperty(it => it.OrderStepCount).SetProperty(it => it.Price).SetProperty(it => it.Sku)
                            .SetProperty(it => it.IsSales).SetProperty(it => it.DataId);
                        info.SaveType = SaveType.Modify;
                    }
                    info.Goods = goods;
                }
            }
            var products = GetProducts(goods.Id);
            if (products != null && goods.Products!=null)
            {
                foreach (var product in products)
                {
                    var pd = goods.Products.FirstOrDefault(it => it.Id == product.Id);
                    if(pd!=null)
                        continue;
                    product.SaveType=SaveType.Remove;
                    goods.Products.Add(product);
                }
            }
        }
        /// <summary>
        /// 得到产品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        protected virtual IList<ProductEntity> GetProducts(long goodsId)
        {
            if (goodsId == 0)
                return null;
            var query=new QueryInfo();
            query.Query<ProductEntity>().Where(it => it.Goods.Id == goodsId).Select(it=>it.Id);
            return Repository.GetEntities<ProductEntity>(query);
        } 
        #endregion

        #region 设置自定义目录存储类型
    
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="goods"></param>
        protected virtual void SetCatalogueGoodses(GoodsEntity goods)
        {
            if (goods == null || goods.CatalogueGoodses == null) return;
            if (!goods.HasSaveProperty(it => it.CatalogueGoodses) || goods.Properties == null)
            {
                foreach (var info in goods.CatalogueGoodses)
                {
                    if (goods.SaveType == SaveType.Add || info.Id == 0)
                    {
                        info.SaveType = SaveType.Add;
                    }
                    else
                    {
                        info.SaveType = SaveType.None;
                    }
                    info.Goods = goods;
                }
            }
        

        }

        #endregion

        #region 更新缓存
        /// <summary>
        /// 得到缓存
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<GoodsEntity> GetGoodsByCache(long[] ids)
        {
            var infos = new List<GoodsEntity>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    try
                    {
                        var value = "";
                        try
                        {
                            value = CacheRepository.Get<string>(GetCacheKey(id));
                        }
                        catch
                        {
                        }
                        if (string.IsNullOrEmpty(value))
                        {
                            var info = Repository.Get<GoodsEntity>(id);
                            infos.Add(info);
                            if (info != null)
                            {
                                CacheRepository.Set(GetCacheKey(info.Id), info.SerializeJson(), DateTime.MaxValue);
                            }
                        }
                        else
                        {
                            infos.Add(value.DeserializeJson<GoodsEntity>());
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return infos;
        }

      

 
        /// <summary>
        /// 得到缓存Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual string GetCacheKey(long id)
        {
            return string.Format("Goods_{0}", id);
        }

        #endregion


    }
}

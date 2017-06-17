using System.Collections.Generic;
using System.IO;
using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Admin.Models.Commodity
{
    public class CommodityImageModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///文件名
        /// </summary>
        public string FileName { get; set; }

    }
    public class CommodityTagModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

    }
    public class CommodityModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string CatalogId { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public decimal? Cost { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal? Price { get; set; }
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
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        /// 是否显示销售价
        /// </summary>
        public bool? IsShowPrice { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommodityStatusType? Status { get; set; }
        /// <summary>
        /// 数组
        /// </summary>
        public string ImagesValue { get; set; }
        /// <summary>
        /// 数组
        /// </summary>
        public string TagsValue { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public IList<CommodityImageModel> Images { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public IList<CommodityTagModel> Tags { get; set; }
        /// <summary>
        ///图册
        /// </summary>
        public string AlbumFileName { get; set; }
        /// <summary>
        /// 是否创建
        /// </summary>
        public bool? IsCreateAlbum { get; set; }
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="dataImages"></param>
        /// <param name="dataTags"></param>
        /// <returns></returns>
        public virtual CommodityEntity CreateEntity(SaveType saveType,IList<CommodityImageEntity> dataImages, IList<CommodityTagEntity> dataTags)
        {
            if (!string.IsNullOrEmpty(ImagesValue))
                Images = ImagesValue.DeserializeJson<List<CommodityImageModel>>();
            if (!string.IsNullOrEmpty(TagsValue))
                Tags = TagsValue.DeserializeJson<List<CommodityTagModel>>();
            var entity = new CommodityEntity
            {
                Name = Name,
                Catalog = new CatalogEntity {Id =string.IsNullOrEmpty(CatalogId) ? 0 : CatalogId.Convert<long>()},
                Sequence = Sequence == null ? 1 : Sequence.Value,
                Cost = Cost == null ? 0 : Cost.Value,
                Price = Price == null ? 0 : Price.Value,
                VenderName =string.IsNullOrWhiteSpace(VenderName)  ? "": VenderName,
                VenderLinkman = string.IsNullOrWhiteSpace(VenderLinkman) ? "" : VenderLinkman,
                VenderMobile = string.IsNullOrWhiteSpace(VenderMobile) ? "" : VenderMobile,
                VenderAddress = string.IsNullOrWhiteSpace(VenderAddress) ? "" : VenderAddress,
                Description = string.IsNullOrWhiteSpace(Description) ? "" : Description,
                FileName = string.IsNullOrWhiteSpace(FileName) ? "" : FileName,
                Password = string.IsNullOrWhiteSpace(Password) ? "" : Password,
                IsShowPrice = IsShowPrice.HasValue ? IsShowPrice.Value : true,
                IsCreateAlbum = IsCreateAlbum.HasValue ? IsCreateAlbum.Value : true,
                Status =Status.HasValue?Status.Value:CommodityStatusType.Normal,
                SaveType = saveType
            };
            FillImages(entity, saveType, dataImages);
            FillTags(entity, dataTags);
            if (!string.IsNullOrEmpty(FileName))
            {
                entity.FileByte = Ioc.Resolve<IFileApplicationService>().Grab(FileName.Substring(FileName.IndexOf("/Files")));
                entity.FileName = string.Format("Files/Images/SiteCommodity/copy{0}", Path.GetExtension(FileName));
            }
            if (!string.IsNullOrEmpty(AlbumFileName) && AlbumFileName != " " && AlbumFileName != "")
            {
                entity.AlbumFileByte = Ioc.Resolve<IFileApplicationService>().Grab(AlbumFileName.Substring(AlbumFileName.IndexOf("/Files")));
                entity.AlbumFileName = string.Format("Files/Images/SiteAlbum/copy{0}", Path.GetExtension(AlbumFileName));
            }
            if (AlbumFileName == " " || AlbumFileName == "")
            {
                entity.AlbumFileName = "";
            }
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (!string.IsNullOrEmpty(Name))
                    entity.SetProperty(it => it.Name);
                if (CatalogId != null)
                    entity.SetProperty(it => it.Catalog.Id);
                if (Status != null)
                    entity.SetProperty(it => it.Status);
                if (Sequence != null)
                    entity.SetProperty(it => it.Sequence);
                if (Cost != null)
                    entity.SetProperty(it => it.Cost);
                if (Price != null)
                    entity.SetProperty(it => it.Price);
                if (VenderName !=null)
                    entity.SetProperty(it => it.VenderName);
                if (VenderLinkman != null)
                    entity.SetProperty(it => it.VenderLinkman);
                if (VenderMobile != null)
                    entity.SetProperty(it => it.VenderMobile);
                if (VenderAddress != null)
                    entity.SetProperty(it => it.VenderAddress);
                if (Description != null)
                    entity.SetProperty(it => it.Description);
                if (Password != null)
                    entity.SetProperty(it => it.Password);
                if (!string.IsNullOrEmpty(FileName))
                    entity.SetProperty(it => it.FileName);
                if (IsShowPrice.HasValue)
                    entity.SetProperty(it => it.IsShowPrice);
                if (IsCreateAlbum.HasValue)
                    entity.SetProperty(it => it.IsCreateAlbum);
                if (AlbumFileName != null)
                    entity.SetProperty(it => it.AlbumFileName);
            }
            return entity;
        }
        /// <summary>
        /// 填充图片
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveType"></param>
        /// <param name="dataImages"></param>
        protected virtual void FillImages(CommodityEntity entity, SaveType saveType,
            IList<CommodityImageEntity> dataImages)
        {
            if (Images == null) return;
            entity.CommodityImages = new List<CommodityImageEntity>();
            var i = 0;
            if (saveType == SaveType.Add)
            {
                foreach (var image in Images)
                {
                    i++;
                    if (string.IsNullOrEmpty(image.FileName)) continue;
                    entity.CommodityImages.Add(new CommodityImageEntity
                    {
                        Id = 0,
                        Commodity=entity,
                        FileName =
                            string.Format("Files/Images/SiteImage/copy{0}", Path.GetExtension(image.FileName)),
                        FileByte =
                            Ioc.Resolve<IFileApplicationService>()
                                .Grab(image.FileName.Substring(image.FileName.IndexOf("/Files"))),
                        Sequence= i,
                        SaveType = SaveType.Add
                    });
           
                }
                return;
            }
            if (dataImages != null)
            {
                foreach (var dataImage in dataImages)
                {
                    var img = Images.FirstOrDefault(it => it.Id == dataImage.Id.ToString());
                    if (img==null)
                    {
                        entity.CommodityImages.Add(new CommodityImageEntity
                        {
                            Id = dataImage.Id,
                            SaveType = SaveType.Remove
                        });
                    }
                }
                foreach (var image in Images)
                {
                    i++;
                    var id = image.Id.Convert<long>();
                    if(id==0 && string.IsNullOrEmpty(image.FileName))
                        continue;
                    var commdityImage = new CommodityImageEntity
                    {
                        Id = id,
                        Commodity = entity,
                        Sequence = i,
                        SaveType = dataImages.FirstOrDefault(it => it.Id == id) == null ? SaveType.Add : SaveType.Modify
                    };
                    entity.CommodityImages.Add(commdityImage);
                    if (!string.IsNullOrEmpty(image.FileName))
                    {
                        commdityImage.FileName = string.Format("Files/Images/SiteImage/copy{0}",
                            Path.GetExtension(image.FileName));
                        commdityImage.FileByte =
                            Ioc.Resolve<IFileApplicationService>()
                                .Grab(image.FileName.Substring(image.FileName.IndexOf("/Files")));
                    }
                    else if(commdityImage.SaveType==SaveType.Modify)
                    {
                        commdityImage.SetProperty(it => it.Sequence);
                    }
                }
                return;
            }
            foreach (var image in Images)
            {
                i++;
                if (string.IsNullOrEmpty(image.FileName)) continue;
                entity.CommodityImages.Add(new CommodityImageEntity
                {
                    Id = 0,
                    Commodity = entity,
                    FileName =
                        string.Format("Files/Images/SiteImage/copy{0}", Path.GetExtension(image.FileName)),
                    FileByte =
                        Ioc.Resolve<IFileApplicationService>()
                            .Grab(image.FileName.Substring(image.FileName.IndexOf("/Files"))),
                    Sequence=i,
                    SaveType = SaveType.Add
                });
   
            }
        }

        /// <summary>
        /// 填充图片
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dataTags"></param>
        protected virtual void FillTags(CommodityEntity entity, IList<CommodityTagEntity> dataTags)
        {
            entity.CommodityTags = new List<CommodityTagEntity>();
            if (Tags != null)
            {
                foreach (var tag in Tags)
                {
                    var data = dataTags?.FirstOrDefault(it => it.Tag != null && it.Tag.Id == tag.Id.Convert<long>());
                    if (data == null)
                    {
                        entity.CommodityTags.Add(new CommodityTagEntity
                        {
                            Id = 0,
                            Tag = new TagEntity {Id = tag.Id.Convert<long>()},
                            Commodity = entity,
                            SaveType = SaveType.Add
                        });

                    }
                }
            }
            if (dataTags != null)
            {
                foreach (var dataTag in dataTags)
                {
                    if (dataTag.Tag == null)
                        continue;
                    var data = Tags?.FirstOrDefault(it => dataTag.Tag.Id == it.Id.Convert<long>());
                    if (data == null)
                    {
                        entity.CommodityTags.Add(new CommodityTagEntity
                        {
                            Id = dataTag.Id,
                            SaveType = SaveType.Remove
                        });

                    }
                }
            }
        }
    }
}
using System.IO;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Admin.Models.Catalog
{
    public class CatalogModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拍下
        /// </summary>
        public int? Sequence { get; set; }
        /// <summary>
        ///文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual CatalogEntity CreateEntity(SaveType saveType)
        {
            var entity = new CatalogEntity
            {
                Name = Name,
                Sequence = Sequence==null?1: Sequence.Value,
                SaveType = saveType
            };
            if (!string.IsNullOrEmpty(FileName) && FileName!=" " && FileName != "")
            {
                entity.FileByte = Ioc.Resolve<IFileApplicationService>().Grab(FileName.Substring(FileName.IndexOf("/Files")));
                entity.FileName = string.Format("Files/Images/SiteCatalog/copy{0}", Path.GetExtension(FileName));
            }
            if (FileName == " " || FileName == "")
            {
                entity.FileName = "";
            }
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (Name != null)
                    entity.SetProperty(it => it.Name);
                if (FileName != null)
                    entity.SetProperty(it => it.FileName);
                if (Sequence != null)
                    entity.SetProperty(it => it.Sequence);
            }
            return entity;
        }
    }
}
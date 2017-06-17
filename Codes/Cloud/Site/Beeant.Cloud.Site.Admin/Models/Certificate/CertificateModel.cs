using System.IO;
using Component.Extension;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Cloud.Site.Admin.Models.Certificate
{

    public class CertificateModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
       
        /// <summary>
        /// 供应商联系人
        /// </summary>
        public bool? IsShow { get; set; }
      
        /// <summary>
        ///文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sequence { get; set; }
       

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        public virtual CertificateEntity CreateEntity(SaveType saveType)
        {
         
            var entity = new CertificateEntity
            {
                IsShow = IsShow.HasValue? IsShow.Value:true,
                Sequence = Sequence == null ? 1 : Sequence.Value,
                FileName = string.IsNullOrWhiteSpace(FileName) ? "" : FileName,
                SaveType = saveType
            };
            if (!string.IsNullOrEmpty(FileName))
            {
                entity.FileByte = Ioc.Resolve<IFileApplicationService>().Grab(FileName.Substring(FileName.IndexOf("/Files")));
                entity.FileName = string.Format("Files/Images/SiteCertificate/copy{0}", Path.GetExtension(FileName));
            }
            if (saveType == SaveType.Modify)
            {
                entity.Id = Id.Convert<long>();
                if (IsShow != null)
                    entity.SetProperty(it => it.IsShow);
                if (FileName != null)
                    entity.SetProperty(it => it.FileName);
                if (Sequence != null)
                    entity.SetProperty(it => it.Sequence);
            }
            return entity;
        }
      
    }
}
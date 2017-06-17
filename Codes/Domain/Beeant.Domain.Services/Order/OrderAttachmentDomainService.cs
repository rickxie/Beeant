using System.Collections.Generic;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;

namespace Beeant.Domain.Services.Order
{
    public class OrderAttachmentDomainService : RealizeDomainService<OrderAttachmentEntity>
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
        #region 加载
        private IDictionary<string, ItemLoader<OrderAttachmentEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<OrderAttachmentEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<OrderAttachmentEntity>>
                    {
                       
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }

        
        #endregion

        #region 重写验证

 
        /// <summary>
        /// 删除添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(OrderAttachmentEntity info)
        {
            return ValidateOrder(info);
        }
   
        /// <summary>
        /// 验证订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateOrder(OrderAttachmentEntity info)
        {
            if (!info.HasSaveProperty(it => it.Order.Id))
                return true;
            if (info.Order != null && info.Order.SaveType == SaveType.Add)
                return true;
            if (info.Order != null && info.Order.Id!=0)
            {
                if (Repository.Get<OrderEntity>(info.Order.Id) != null)
                    return true;
            }
            info.AddErrorByName(typeof(OrderEntity).FullName, "NoExist");
            return false;
        }
 
        #endregion


    }
}

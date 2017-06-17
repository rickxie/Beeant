using System.Collections.Generic;
using System.Linq;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Merchant;
using Beeant.Domain.Entities.Basedata;
using Beeant.Domain.Entities.Management;
using Beeant.Domain.Entities.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Merchant
{
    public class PartnerDomainService : RealizeDomainService<PartnerEntity>
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
        private IDictionary<string, ItemLoader<PartnerEntity>> _itemLoaders;
        /// <summary>
        /// 处理
        /// </summary>
        protected override IDictionary<string, ItemLoader<PartnerEntity>> ItemLoaders
        {
            get
            {
                return _itemLoaders ?? (_itemLoaders = new Dictionary<string, ItemLoader<PartnerEntity>>
                    {
                        {"ServiceUser", LoadServiceUser}
                    });
            }
            set
            {
                base.ItemLoaders = value;
            }
        }
        #region 加载
        /// <summary>
        /// 加载用户
        /// </summary>
        /// <param name="info"></param>
        protected virtual void LoadServiceUser(PartnerEntity info)
        {
            info.Service = Repository.Get<UserEntity>(info.Service.Id);
        }
        #endregion

        #region 重写验证

      

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(PartnerEntity info)
        {
            var rev = ValidateAccount(info, null) && ValidateService(info, null) 
                && ValidateWebsiteStyle(info,null) && ValidateMobileStyle(info,null);
            return rev;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="info"></param>
        protected override bool ValidateModify(PartnerEntity info)
        {
            var dataEntity = Repository.Get<PartnerEntity>(info.Id);
            return ValidateAccount(info, dataEntity) && ValidateService(info, dataEntity)
                && ValidateWebsiteStyle(info, dataEntity) && ValidateMobileStyle(info, dataEntity);
        }
      
        #endregion

        #region 删除验证

        /// <summary>
        /// 删除验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateRemove(PartnerEntity info)
        {
            var dataEntity = Repository.Get<PartnerEntity>(info.Id);
            if (dataEntity.Account != null && dataEntity.Account.Id!=0)
            {
                info.AddError("OpenAccountNotAllowRemove");
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateAccount(PartnerEntity info ,PartnerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Account.Id) || info.Account == null || info.Account.Id==0)
                return true;
            if (dataEntity != null && dataEntity.Account != null && dataEntity.Account.Id == info.Account.Id) 
                return true;
            if (info.Account != null && info.Account.SaveType == SaveType.Add)
                return true;
            var account = Repository.Get<AccountEntity>(info.Account.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "NoExist");
                return false;
            }
            if (!account.IsUsed)
            {
                info.AddErrorByName(typeof(AccountEntity).FullName, "UnUsed");
                return false;
            }
            var query = new QueryInfo();
            query.Query<PartnerEntity>().Where(it => it.Account.Id == info.Account.Id);
            var infos = Repository.GetEntities<PartnerEntity>(query);
            if (infos != null && infos.Count(it => it.Id != info.Id) > 0)
            {
                info.AddError("AccountHasPartner");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证店铺风格
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateWebsiteStyle(PartnerEntity info, PartnerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.WebsiteStyle.Id) || info.WebsiteStyle == null || info.WebsiteStyle.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.WebsiteStyle != null && dataEntity.WebsiteStyle.Id == info.WebsiteStyle.Id)
                return true;
            if (info.WebsiteStyle != null && info.WebsiteStyle.SaveType == SaveType.Add)
                return true;
            var style = Repository.Get<StyleEntity>(info.WebsiteStyle.Id);
            if (style == null)
            {
                info.AddErrorByName(typeof(StyleEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证店铺风格
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateMobileStyle(PartnerEntity info, PartnerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.MobileStyle.Id) || info.MobileStyle == null || info.MobileStyle.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.MobileStyle != null && dataEntity.MobileStyle.Id == info.MobileStyle.Id)
                return true;
            if (info.MobileStyle != null && info.MobileStyle.SaveType == SaveType.Add)
                return true;
            var style = Repository.Get<StyleEntity>(info.MobileStyle.Id);
            if (style == null)
            {
                info.AddErrorByName(typeof(StyleEntity).FullName, "NoExist");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证客服
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateService(PartnerEntity info, PartnerEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Service.Id) || info.Service == null || info.Service.Id == 0)
                return true;
            if (dataEntity != null && dataEntity.Service != null && dataEntity.Service.Id == info.Service.Id)
                return true;
            var user = Repository.Get<UserEntity>(info.Service.Id);
            if (user == null)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "NoExist");
                return false;
            }
            if (!user.IsUsed)
            {
                info.AddErrorByName(typeof(UserEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }
        #endregion

    }
}

using Beeant.Domain.Entities.Cms;
using Winner.Persistence;

namespace Beeant.Domain.Services.Cms
{
    public class NoticeDomainService : RealizeDomainService<NoticeEntity>
    {

  

        #region 重写验证

        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(NoticeEntity info)
        {
            var rev = ValidateCms(info) ;
            return rev;
        }
  

        /// <summary>
        /// 验证账户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateCms(NoticeEntity info)
        {
            if (!info.HasSaveProperty(it => it.Cms.Id) || info.Cms == null ||
                info.Cms.Id == 0)
                return true;
            if (info.Cms != null && info.Cms.SaveType == SaveType.Add)
                return true;
            var account = Repository.Get<CmsEntity>(info.Cms.Id);
            if (account == null)
            {
                info.AddErrorByName(typeof(CmsEntity).FullName, "NoExist");
                return false;
            }
            if (!account.IsUsed)
            {
                info.AddErrorByName(typeof(CmsEntity).FullName, "UnUsed");
                return false;
            }
            return true;
        }

        #endregion
    }
}

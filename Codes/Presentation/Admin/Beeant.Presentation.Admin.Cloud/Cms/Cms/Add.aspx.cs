using System.Collections.Generic;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Cms;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cloud.Cms.Cms
{
    public partial class Add : AddPageBase<CmsEntity>
    {
   
        protected override CmsEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
  
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="info"></param>
        protected override void Save(CmsEntity info)
        {
            if (info == null) return;
            var rev = Ioc.Resolve<IApplicationService, CmsEntity>().Save(info);
            if (rev)
            {
                var accountNumber = new AccountNumberEntity
                {
                    Account = info.Account,
                    Tag = "CmsId",
                    NumberEntity = info,
                    Name = "CMS系统编号",
                    SaveType = SaveType.Add
                };
                Ioc.Resolve<IApplicationService, AccountNumberEntity>().Save(accountNumber);
            }
            SetResult(rev, info.Errors);
        }
    }
}
using System.Collections.Generic;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Site;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cloud.Site.Site
{
    public partial class Add : AddPageBase<SiteEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        protected override SiteEntity GetEntity()
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
        protected override void Save(SiteEntity info)
        {
            if (info == null) return;
            var rev = Ioc.Resolve<IApplicationService, SiteEntity>().Save(info);
            if (rev)
            {
                var accountNumber = new AccountNumberEntity
                {
                    Account = info.Account,
                    Tag = "SiteId",
                    NumberEntity = info,
                    Name = "站点系统编号",
                    SaveType = SaveType.Add
                };
                Ioc.Resolve<IApplicationService, AccountNumberEntity>().Save(accountNumber);
            }
            SetResult(rev, info.Errors);
        }
    }
}
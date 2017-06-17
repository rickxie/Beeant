using System.Collections.Generic;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Hr;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cloud.Hr.Hr
{
    public partial class Add : AddPageBase<HrEntity>
    {
   
        protected override HrEntity GetEntity()
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
        protected override void Save(HrEntity info)
        {
            if (info == null) return;
            var rev = Ioc.Resolve<IApplicationService, HrEntity>().Save(info);
            if (rev)
            {
                var accountNumber = new AccountNumberEntity
                {
                    Account = info.Account,
                    Tag = "HrId",
                    NumberEntity = info,
                    Name = "人事系统编号",
                    SaveType = SaveType.Add
                };
                Ioc.Resolve<IApplicationService, AccountNumberEntity>().Save(accountNumber);
            }
            SetResult(rev, info.Errors);
        }
    }
}
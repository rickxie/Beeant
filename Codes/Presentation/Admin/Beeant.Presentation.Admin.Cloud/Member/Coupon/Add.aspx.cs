using System;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Member;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Cloud.Member.Coupon
{
    public partial class Add : AddPageBase<CouponEntity>
    {
      
        protected override CouponEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }

        protected override CouponEntity FillEntity()
        {

            var info= base.FillEntity();
            if (info != null)
            {
                info.CollectTime = DateTime.Now;
                info.UsedTime = DateTime.Now;
            }
            return info;
        }
    }
}
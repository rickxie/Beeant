using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Member;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Cloud.Member.Coupon
{
    public partial class Update : UpdatePageBase<CouponEntity>
    {
        public override bool IsFillAllEntity
        {
            get { return false; }
            set
            {
                base.IsFillAllEntity = value;
            }
        }
      
        protected override CouponEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
          
            return info;
        }
    
    }
}
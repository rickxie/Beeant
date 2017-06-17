using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Member;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Cloud.Member.Member
{
    public partial class Add : AddPageBase<MemberEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
        protected override MemberEntity GetEntity()
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
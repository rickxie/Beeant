using System.Web.UI.WebControls;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Finance.Finance.Integral
{
    public partial class Add : WorkflowPageBase<IntegralEntity>
    {
        public int Index = 0;
        public override Button SaveButton
        {
            get { return btnSave; }
            set { base.SaveButton = value; }
        }
      
     
        protected override IntegralEntity GetEntity()
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
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Freight
{
    public partial class Update : UpdatePageBase<FreightEntity>
    {
        public override bool IsFillAllEntity
        {
            get { return false; }
            set { base.IsFillAllEntity = value; }
        }

        public override bool IsUpdatePanel
        {
            get { return false; }
            set { base.IsUpdatePanel = value; }
        }


        protected override FreightEntity GetEntity()
        {
            var info = base.GetEntity();
            if (info != null && info.Account != null)
            {
                info.Account = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(info.Account.Id);
            }
            return info;
        }
        /// <summary>
        /// 填充
        /// </summary>
        /// <returns></returns>
        protected override FreightEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                info.Carries = Edit1.GetSaveCarries();
            }
            return info;
        }
  

    }
}
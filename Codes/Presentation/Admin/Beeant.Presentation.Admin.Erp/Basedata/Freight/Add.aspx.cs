using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Freight
{
    public partial class Add : AddPageBase<FreightEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set { base.IsUpdatePanel = value; }
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
                info.Account = new AccountEntity {Id = 0};
                info.Carries = Edit1.GetSaveCarries();
            }
            return info;
        }
  
 
    }
}
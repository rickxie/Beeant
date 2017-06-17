using System;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Account.Account
{
    public partial class Update : UpdatePageBase<AccountEntity>
    {
        public override bool IsFillAllEntity
        {
            get
            {
                return false;
            }
            set
            {
                base.IsFillAllEntity = value;
            }
        }
   
        protected override void OnPreLoad(EventArgs e)
        {
            SaveButton = btnSave;
            base.OnPreLoad(e);
        }
    
    }
}
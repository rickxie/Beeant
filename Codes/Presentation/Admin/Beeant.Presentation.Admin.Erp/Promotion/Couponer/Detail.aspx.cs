﻿using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Promotion;

namespace Beeant.Presentation.Admin.Erp.Promotion.Couponer
{
    public partial class Detail : DetailPageBase<CouponerEntity>
    {

        protected override CouponerEntity GetEntity()
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
using System;
using System.Collections.Generic;
using System.Linq;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Member;
using Beeant.Domain.Entities.Account;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Promotion;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Promotion.Couponer
{
    public partial class List : ListPageBase<CouponerEntity>
    {
        protected override IList<CouponerEntity> GetEntities()
        {
            var infos = base.GetEntities();
            if (infos != null)
            {
                SetAccounts(infos);
            }
            return infos;
        }

        protected virtual void SetAccounts(IList<CouponerEntity> infos)
        {
            var accountIds =
                   infos.Where(it => it.Account != null && it.Account.Id!=0)
                        .Select(it => it.Account.Id)
                        .ToArray();
            if (accountIds.Length > 0)
            {
                var query = new QueryInfo();
                query.Query<AccountEntity>().Where(it => accountIds.Contains(it.Id));
                var accounts = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntities<AccountEntity>(query);
                if (accounts != null)
                {
                    foreach (var info in infos)
                    {
                        if (info.Account == null) continue;
                        info.Account = accounts.FirstOrDefault(it => it.Id == info.Account.Id);
                    }
                }
            }
        }

        public void btnCreate_Click(object sender, EventArgs e)
        {
            var rev = Create();
            this.ShowMessage("操作提醒", rev ? "生成成功" : "生成失败");
            if(rev)
                LoadData();
        }

        /// <summary>
        /// 生成优惠券
        /// </summary>
        protected virtual bool Create()
        {
            var infos = GetSaveEntities<CouponerEntity>(SaveType.None);
            if (infos == null || infos.Count == 0)
                return false;
            foreach (var info in infos)
            {
                Ioc.Resolve<ICouponApplicationService>().CreateCoupon(info.Id);
            }
            return true;
        }
    }
}
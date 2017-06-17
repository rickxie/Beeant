using System.Linq;
using Beeant.Domain.Entities.Authority;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Authority.Owner
{
    public partial class List : MaintenPageBase<OwnerEntity>
    {


        protected override void SetQuery(QueryInfo query)
        {
            query.Query<OwnerEntity>().Where(it => it.Account.Id == 0);
            base.SetQuery(query);
        }
        /// <summary>
        /// 填充实体
        /// </summary>
        /// <returns></returns>

        protected override OwnerEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
                info.Account = new AccountEntity {Id = 0};
            return info;
        }
    }
}
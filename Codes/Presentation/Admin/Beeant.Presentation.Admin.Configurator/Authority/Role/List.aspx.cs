using System.Linq;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Authority.Role
{
    public partial class List : Basic.Services.WebForm.Pages.MaintenPageBase<RoleEntity>
    {
        protected override void SetQuery(QueryInfo query)
        {
            query.Query<RoleEntity>().Where(it => it.Account.Id == 0);
            base.SetQuery(query);
        }

        /// <summary>
        /// 填充实体
        /// </summary>
        /// <returns></returns>

        protected override RoleEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
                info.Account = new AccountEntity { Id = 0 };
            return info;
        }
    }
}
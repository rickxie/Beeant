using System.Linq;
using Beeant.Domain.Entities.Account;
using Component.Extension;
using Beeant.Domain.Entities.Authority;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Authority.OwnerAccount
{
    public partial class Add : Basic.Services.WebForm.Pages.ListPageBase<OwnerEntity>
    {

        protected override void SetQueryWhere(QueryInfo query)
        {
            if(string.IsNullOrEmpty(Request["AccountId"])) return;
            query.Query<OwnerEntity>().Where(
                it => it.OwnerAccounts.Count(i => i.Account.Id == Request["AccountId"].Convert<long>()) == 0);

        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            var infos = GetSaveEntities<OwnerAccountEntity>(SaveType.Add);
            SaveEntities(infos, "授权成功", "授权失败");
           
        }

        /// <summary>
        /// 重写
        /// </summary>
        /// <typeparam name="TEntityType"></typeparam>
        /// <param name="id"></param>
        /// <param name="saveType"></param>
        /// <returns></returns>
        protected override TEntityType CreateSaveEntity<TEntityType>(long id, SaveType saveType)
        {
            var info = new OwnerAccountEntity
            {
                SaveType = saveType,
                Owner = new OwnerEntity {Id = id},
                Account = new AccountEntity {Id = Request.QueryString["AccountId"].Convert<long>()}
            };
            return info as TEntityType;
        }

    }
}
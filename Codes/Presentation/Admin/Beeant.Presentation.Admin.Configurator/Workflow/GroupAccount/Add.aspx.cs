using System.Linq;
using Beeant.Domain.Entities.Account;
using Component.Extension;
using Beeant.Domain.Entities.Workflow;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Configurator.Workflow.GroupAccount
{
    public partial class Add : Basic.Services.WebForm.Pages.ListPageBase<GroupEntity>
    {
        public long AccountId
        {
            get { return Request["AccountId"].Convert<long>(); }
        }

        protected override void SetQueryWhere(QueryInfo query)
        {
            if(AccountId == 0)
                return;
            query.Query<GroupEntity>().Where(
                it => it.GroupAccounts.Count(i => i.Account.Id == AccountId) == 0);

        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            var infos = GetSaveEntities<GroupAccountEntity>(SaveType.Add);
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
            var info = new GroupAccountEntity
            {
                    SaveType=saveType,
                    Group = new GroupEntity { Id = id }, Account = new AccountEntity { Id = AccountId }
                };
            return info as TEntityType;
        }      

    }
}
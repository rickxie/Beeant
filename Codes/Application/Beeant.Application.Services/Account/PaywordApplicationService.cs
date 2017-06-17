using System.Linq;
using Beeant.Domain.Entities.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Account
{
    public class PaywordApplicationService : PasswordApplicationService
    {
        protected override string Tag
        {
            get { return "AccountResetPayword"; }
            set { base.Tag = value; }
        }

        /// <summary>
        /// 检查用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool CheckPassword(string name, string password, out AccountEntity entity)
        {
            var account = new AccountEntity { Password = password };
            account.SetEncryptPayword();
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => (it.Name == name || it.IsActiveEmail && it.Email == name || it.IsActiveMobile && it.Mobile == name) && it.Payword == account.Payword)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<AccountEntity>(query);
            entity = infos?.FirstOrDefault();
            return entity != null;
        }

        /// <summary>
        /// 检查密码是否正确
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool CheckPassword(long accountId, string password)
        {
            var account = new AccountEntity { Payword = password };
            account.SetEncryptPayword();
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => it.Id == accountId && it.Payword == account.Payword)
                .Select(it => it.Id);
            var infos = Repository.GetEntities<AccountEntity>(query);
            return infos != null && infos.Count > 0;
        }
        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        protected override void SetSaveAccount(AccountEntity account, string password)
        {
            account.Payword = password;
            account.SaveType = SaveType.Modify;
            account.SetProperty(it => it.Payword);
        }
    }
}

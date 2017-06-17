using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Application.Services;
using Dependent;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Presentation.Website.Password.Models.Account;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Password.Controllers.Account
{
    [AuthorizeFilter]
    public class NameController : BaseController
    {

        #region 首页

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            return View("~/Views/Account/Name/Index.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Modify(NameModel model)
        {
            var data = this.GetEntity<AccountEntity>(Identity.Id);
            if (data == null)
                return null;
            if (data.Name != model.Name)
            {
                var account = new AccountEntity
                {
                    Id = Identity.Id,
                    Name = model.Name,
                    SaveType = SaveType.Modify
                };
                account.SetProperty(it => it.Name);
                model.Errors = account.Errors;
                account.AccountIdentites = new List<AccountIdentityEntity>();
                var accountIdentiy = GetAccountIdentity(data.Name);
                accountIdentiy.SaveType = SaveType.Remove;
                account.AccountIdentites.Add(new AccountIdentityEntity
                {
                    Account = account,
                    Name = "Name",
                    Number = model.Name,
                    SaveType = SaveType.Add
                });
            }
         
            return View("~/Views/Account/Name/Index.cshtml",model);
        }
 
        /// <summary>
        /// 检查用户名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool CheckAccountName(string name)
        {
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => it.Name == name).Select(it => it.Id);
            var infos = Ioc.Resolve<IApplicationService, AccountEntity>().GetEntities<AccountEntity>(query);
            if (infos != null && infos.Count == 0)
                return true;
            return false;
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual AccountIdentityEntity GetAccountIdentity(string name)
        {
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>().Where(it => it.Number == name)
                .Select(it => new object[] { it.Id });
            var infos = this.GetEntities<AccountIdentityEntity>(query);
            return infos?.FirstOrDefault();
        }
        #endregion

    }
}

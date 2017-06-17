using System.Collections.Generic;
using System.Linq;
using Beeant.Application.Dtos.Account;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Security;
using Beeant.Domain.Services.Utility;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Account
{
    public class MobileApplicationService : IMobileApplicationService
    {
        /// <summary>
        /// 查询实例
        /// </summary>
        public IRepository Repository { get; set; }

        /// <summary>
        /// 登入锁
        /// </summary>
        public ICodeDomainService CodeValidateDomainService { get; set; }
        /// <summary>
        /// 登入锁
        /// </summary>
        public IDomainService CodeDomainService { get; set; }

        /// <summary>
        /// 登入锁
        /// </summary>
        public IDomainService AccountDomainService { get; set; }
        /// <summary>
        /// 手机短信
        /// </summary>
        public IMobileRepository MobileRepository { get; set; }

        private const string Tag = "AccountMobile";

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual MobileDto Load(MobileDto dto)
        {
            dto.Account = GetAccount(dto.AccountId);
            dto.SetAction();
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual MobileDto SendCode(MobileDto dto)
        {
            dto.Account = GetAccount(dto.AccountId);
            if (dto.Account == null )
                return dto;
            dto.CodeEntity = new CodeEntity
            {
                Tag = string.Format("{0}{1}", Tag, dto.Action),
                Type = CodeType.Mobile,
                Name = dto.AccountId.ToString(),
                ToAddress = dto.GetMobile(),
                SaveType = SaveType.Add
            };
            var unitofworks = CodeDomainService.Handle(dto.CodeEntity);
            dto.Result = Winner.Creator.Get<IContext>().Commit(unitofworks);
            dto.Errors = dto.Errors;
            if (dto.Result)
            {
                MobileRepository.Send(new MobileEntity
                {
                    Body=dto.CodeEntity.Body,
                    ToMobiles=new []
                    {
                        dto.CodeEntity.ToAddress
                    }
                });
            }
            return dto;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual MobileDto Action(MobileDto dto)
        {
            dto.Account = GetAccount(dto.AccountId);
            if (dto.Account == null)
                return dto;
            dto.Result = CodeValidateDomainService.ValidateCode(string.Format("{0}{1}", Tag, dto.Action), dto.AccountId.ToString(),CodeType.Mobile, dto.Code);
            if (dto.Result && dto.Action == "Bind")
            {
                dto.Account.AccountIdentites = new List<AccountIdentityEntity>();
                if (dto.Mobile != dto.Account.Mobile)
                {
                    var accountIdentiy = GetAccountIdentity(dto.Account.Mobile);
                    accountIdentiy.SaveType = SaveType.Remove;
                    dto.Account.AccountIdentites.Add(accountIdentiy);
                }
                dto.Account.Mobile = dto.Mobile;
                dto.Account.IsActiveMobile = true;
                dto.Account.SetProperty(it => it.Mobile)
                    .SetProperty(it=>it.IsActiveMobile);
                dto.Account.SaveType=SaveType.Modify;
                dto.Account.AccountIdentites.Add(new AccountIdentityEntity
                {
                    Account = dto.Account,
                    Name = "Mobile",
                    Number = dto.Mobile,
                    SaveType = SaveType.Add
                });
                var unitofworks = AccountDomainService.Handle(dto.Account);
                dto.Result = Winner.Creator.Get<IContext>().Commit(unitofworks);
                dto.Errors = dto.Errors;
            }
            dto.SetAction();
            return dto;
        }
   

        /// <summary>
        /// 得到账户
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected virtual AccountEntity GetAccount(long accountId)
        {
            var query = new QueryInfo();
            query.Query<AccountEntity>().Where(it => it.Id==accountId && it.IsUsed)
                .Select(it => new object[] { it.Id,it.Mobile,it.IsActiveMobile});
            var infos = Repository.GetEntities<AccountEntity>(query);
            return infos?.FirstOrDefault();
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual AccountIdentityEntity GetAccountIdentity(string mobile)
        {
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>().Where(it => it.Number == mobile)
                .Select(it => new object[] { it.Id });
            var infos = Repository.GetEntities<AccountIdentityEntity>(query);
            return infos?.FirstOrDefault();
        }
    }
}

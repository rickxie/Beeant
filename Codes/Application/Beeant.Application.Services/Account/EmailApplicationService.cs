using System.Collections.Generic;
using System.Linq;
using Beeant.Application.Dtos.Account;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Security;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services;
using Beeant.Domain.Services.Security;
using Beeant.Domain.Services.Utility;
using Winner.Mail;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Application.Services.Account
{
    public class EmailApplicationService: IEmailApplicationService
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
        /// 邮件
        /// </summary>
        public IEmailRepository EmailRepository { get; set; }

        private const string Tag = "AccountEmail";

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual EmailDto Load(EmailDto dto)
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
        public virtual EmailDto SendCode(EmailDto dto)
        {
            dto.Account = GetAccount(dto.AccountId);
            if (dto.Account == null)
                return dto;
            dto.CodeEntity = new CodeEntity
            {
                Tag = string.Format("{0}{1}",Tag,dto.Action),
                Type = CodeType.Email,
                Name = dto.AccountId.ToString(),
                ToAddress = dto.GetEmail(),
                SaveType = SaveType.Add
            };
            var unitofworks = CodeDomainService.Handle(dto.CodeEntity);
            dto.Result = Winner.Creator.Get<IContext>().Commit(unitofworks);
            dto.Errors = dto.Errors;
            if (dto.Result)
            {
                EmailRepository.Send(new EmailEntity
                {
                    Mail = new MailInfo
                    {
                        Body = dto.CodeEntity.Body,
                        IsBodyHtml = true,
                        Subject = dto.CodeEntity.Subject,
                        ToMails =
                            new[] {dto.CodeEntity.ToAddress}
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
        public virtual EmailDto Action(EmailDto dto)
        {
            dto.Account = GetAccount(dto.AccountId);
            if (dto.Account == null)
                return dto;
            dto.Result = CodeValidateDomainService.ValidateCode(string.Format("{0}{1}", Tag, dto.Action), dto.AccountId.ToString(), CodeType.Email, dto.Code);
            if (dto.Action == "Bind")
            {
                dto.Account.AccountIdentites= new List<AccountIdentityEntity>();
                if (dto.Email != dto.Account.Email)
                {
                    var accountIdentiy = GetAccountIdentity(dto.Account.Email);
                    accountIdentiy.SaveType=SaveType.Remove;
                    dto.Account.AccountIdentites.Add(accountIdentiy);
                }
                dto.Account.Email = dto.Email;
                dto.Account.IsActiveEmail = true;
                dto.Account.SetProperty(it => it.Email);
                dto.Account.SetProperty(it => it.IsActiveEmail);
                dto.Account.SaveType=SaveType.Modify;
                dto.Account.AccountIdentites.Add(new AccountIdentityEntity
                {
                    Account= dto.Account,
                    Name = "Email",
                    Number= dto.Email,
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
                .Select(it => new object[] { it.Id,it.Email,it.IsActiveEmail});
            var infos = Repository.GetEntities<AccountEntity>(query);
            return infos?.FirstOrDefault();
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual AccountIdentityEntity GetAccountIdentity(string email)
        {
            var query = new QueryInfo();
            query.Query<AccountIdentityEntity>().Where(it => it.Number== email)
                .Select(it => new object[] { it.Id });
            var infos = Repository.GetEntities<AccountIdentityEntity>(query);
            return infos?.FirstOrDefault();
        }
    }
}

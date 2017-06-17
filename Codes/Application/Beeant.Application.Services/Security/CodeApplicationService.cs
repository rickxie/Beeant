using System.Collections.Generic;
using Beeant.Domain.Entities.Security;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Security;
using Beeant.Domain.Services.Utility;
using Winner.Mail;

namespace Beeant.Application.Services.Security
{
    public class CodeApplicationService : RealizeApplicationService<CodeEntity>, ICodeApplicationService
    {
        /// <summary>
        /// 手机短信
        /// </summary>
        public IMobileRepository MobileRepository { get; set; }
        /// <summary>
        /// 手机短信
        /// </summary>
        public IEmailRepository EmailRepository { get; set; }
        /// <summary>
        /// 手机短信
        /// </summary>
        public ICodeDomainService CodeDomainService { get; set; }
        /// <summary>
        /// 重写Save
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public override bool Save(IList<CodeEntity> infos)
        {

            var rev= base.Save(infos);
            if (rev)
            {
                SendMessage(infos);
            }
            return rev;
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void SendMessage(IList<CodeEntity> infos)
        {
            try
            {
                foreach (var info in infos)
                {
                    if (string.IsNullOrEmpty(info.ToAddress))
                        continue;
                    switch (info.Type)
                    {
                            case CodeType.Email:
                                SendEmailMessage(info);
                                break;
                            case CodeType.Mobile:
                                SendMobileMessage(info);
                                break;
                    }
                }
            }
            catch 
            {
                
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="info"></param>
        protected virtual void SendEmailMessage(CodeEntity info)
        {
            EmailRepository.Send(new EmailEntity
                {
                    Mail=new MailInfo
                        {
                            ToMails = new[] { info.ToAddress },
                            Body = info.Body,
                            IsBodyHtml = true,
                            Subject = info.Subject
                        }
               });
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="info"></param>
        protected virtual void SendMobileMessage(CodeEntity info)
        {
            MobileRepository.Send(new MobileEntity
            {
                ToMobiles = new[] { info.ToAddress },
                Body = info.Body
            });
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool ValidateCode(string tag, string name, CodeType type, string value)
        {
            return CodeDomainService.ValidateCode(tag, name, type, value);
        }


    }
}

using System;
using Beeant.Application.Services.Sys;
using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Component.Extension;


namespace Beeant.Application.Services.Utility
{
    public class EmailApplicationService : QueueJobApplicationService, IEmailApplicationService
    {
        public IEmailRepository EmailRepository { get; set; }
      
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="info"></param>
        public virtual string Send(EmailEntity info)
        {
            return EmailRepository.Send(info);
        }

        protected override string Name
        {
            get { return "SendEmail"; }
        }
        /// <summary>
        /// 处理队列
        /// </summary>
        /// <param name="entity"></param>
        protected override bool Handle(QueueEntity entity)
        {
            var info = entity.Value.DeserializeJson<EmailEntity>();
            EmailRepository.Send(info);
            return true;
        }
    }
}

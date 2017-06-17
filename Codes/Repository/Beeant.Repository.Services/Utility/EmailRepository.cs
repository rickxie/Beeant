using System;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Services.Utility;
using Winner;
using Winner.Log;
using Winner.Mail;
using Winner.Persistence;
using EmailEntity = Beeant.Domain.Entities.Utility.EmailEntity;


namespace Beeant.Repository.Services.Utility
{
    public class EmailRepository : IEmailRepository
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="info"></param>
        public virtual string Send(EmailEntity info)
        {
            try
            {
                var status = Creator.Get<IMail>().Send(info.Mail);
                Action<EmailEntity, bool> func = AddLog;
                func.BeginInvoke(info, status, null, null);
                return status.ToString();
            }
            catch (Exception ex)
            {
                Creator.Get<ILog>().AddException(ex);
            }
            return null;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="status"></param>
        protected virtual void AddLog(EmailEntity info, bool status)
        {
            try
            {
                if (info == null || !info.IsLog || info.Mail == null)
                    return;
                if (info.Mail.SendServer != null)
                {
                    var message = new MessageEntity
                    {
                        Name = info.Name,
                        Description = info.Description,
                        Type = MessageType.Email,
                        FromAddress = info.Mail != null && info.Mail.SendServer != null ? "" : info.Mail.SendServer.FromMail,
                        ToAddress = string.Join(",", info.Mail.ToMails),
                        Content = string.Format("Subject:{0},Detail:{1}", info.Mail.Subject, info.Mail.Body),
                        Number = info.Number,
                        Status = status.ToString(),
                        SaveType = SaveType.Add
                    };
                    Creator.Get<IContext>().Set(message, message);
                    message.SaveType = message.SaveType;
                }
                var unitofworks = Creator.Get<IContext>().Save();
                Creator.Get<IContext>().Commit(unitofworks);
            }
            catch (Exception ex)
            {
                Creator.Get<ILog>().AddException(ex);
            }
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Winner.Mail
{
    public class Mail : IMail
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public IList<ServerInfo> Servers { get; set; }

        #region 接口的实现

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="info"></param>
        public virtual bool Send(MailInfo info)
        {
            if(info==null)return false;
            var server = GetServer(info);
            if(server==null)return false;
            foreach (string toMail in info.ToMails)
            {
                var mm = new MailMessage { From = new MailAddress(server.FromMail) };
                    mm.To.Add(new MailAddress(toMail.Trim()));
                    mm.Subject = info.Subject;
                    mm.IsBodyHtml = info.IsBodyHtml;
                    mm.Body = info.Body;
                    var sc = new SmtpClient
                        {
                            Host = server.SmtpHost,
                            UseDefaultCredentials = true,
                            Credentials = new NetworkCredential(server.UserName, server.Password),
                            DeliveryMethod = SmtpDeliveryMethod.Network
                        };
                    sc.Send(mm);
            }
            return true;
        }

        /// <summary>
        /// 得到服务地址
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual ServerInfo GetServer(MailInfo info)
        {
            var servers = info.Servers ?? Servers;
            if (servers == null || servers.Count == 0)
                return null;
            var random = new Random(DateTime.Now.Second);
            var index = random.Next(0, servers.Count - 1);
            info.SendServer = servers[index];
            return info.SendServer;
        }

        #endregion

     
    }
}

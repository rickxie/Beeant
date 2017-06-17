using System;
using System.Collections.Generic;
using System.Xml;
using Component.Extension;
using Beeant.Domain.Entities.Log;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Winner;
using Winner.Log;
using Winner.Persistence;

namespace Beeant.Repository.Services.Utility
{
    public class MobileRepository : IMobileRepository
    {
        public virtual string Url
        {
            get { return Configuration.ConfigurationManager.GetSetting<string>("YimeiSms"); }
        }

        /// <summary>
        /// 发送手机号码
        /// </summary>
        /// <param name="info"></param>
        public virtual string Send(MobileEntity info)
        {
            if (info == null) return null;
            try
            {
                return SendMessage(info);
            }
            catch (Exception ex)
            {
                Creator.Get<ILog>().AddException(ex);
            }
            return null;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="info"></param>
        protected virtual string SendMessage(MobileEntity info)
        {
            var status = GetSendStatus(info);
            Action< MobileEntity,string> func = AddLog;
            func.BeginInvoke(info,status, null, null);
            return status;
        }

        /// <summary>
        /// 发生短信
        /// </summary>
        /// <param name="info"></param>
        protected virtual string GetSendStatus(MobileEntity info)
        {
            var status = WebRequestHelper.SendPostRequest(Url,
                new Dictionary<string, string>
                               {
                                     {"message", info.Body},
                                     {"phone", string.Join(",", info.ToMobiles)}
                               });
            var doc = new XmlDocument();
            doc.LoadXml(status.Replace("\r", "").Replace("\n", ""));
            var node = doc.SelectSingleNode("response/error");
            return node==null?"": node.InnerXml;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="status"></param>
        protected virtual void AddLog(MobileEntity info,string status)
        {
            try
            {
                if (info == null || !info.IsLog)
                    return;
                var message = new MessageEntity
                {
                    Name = info.Name,
                    Description = info.Description,
                    Type = MessageType.Mobile,
                    FromAddress = Url,
                    ToAddress = string.Join(",", info.ToMobiles),
                    Content = info.Body,
                    Number = info.Number,
                    Status = status,
                    SaveType = SaveType.Add
                };
                Creator.Get<IContext>().Set(message, message);
                message.SaveType = message.SaveType;
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

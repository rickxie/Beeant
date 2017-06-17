using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Component.Extension;
using Winner;
using Winner.Persistence;

namespace Beeant.Repository.Services.Utility
{
    public class QueueMobileRepository : IMobileRepository
    {
      
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="info"></param>
        public virtual string Send(MobileEntity info)
        {
            var queue=new QueueEntity();
            queue.Name = "SendMobile";
            queue.Value = info.SerializeJson();
            queue.SaveType=SaveType.Add;
            Creator.Get<IContext>().Set(queue, queue, queue.SaveSequence);
            var unitofworks= Creator.Get<IContext>().Save();
            return Creator.Get<IContext>().Commit(unitofworks).ToString();
        }

    }
}

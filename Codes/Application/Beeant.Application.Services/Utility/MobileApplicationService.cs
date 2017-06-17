using Beeant.Application.Services.Sys;
using Beeant.Domain.Entities.Sys;
using Beeant.Domain.Entities.Utility;
using Beeant.Domain.Services.Utility;
using Component.Extension;

namespace Beeant.Application.Services.Utility
{
    public class MobileApplicationService : QueueJobApplicationService,IMobileApplicationService
    {
        public IMobileRepository MobileRepository { get; set; }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="info"></param>
        public virtual string Send(MobileEntity info)
        {
            return MobileRepository.Send(info);
        }

        protected override string Name
        {
            get { return "SendMobile"; }
        }
        /// <summary>
        /// 处理队列
        /// </summary>
        /// <param name="entity"></param>
        protected override bool Handle(QueueEntity entity)
        {
            var info = entity.Value.DeserializeJson<MobileEntity>();
            MobileRepository.Send(info);
            return true;
        }
    }
}

using System.Web.Mvc;
using Beeant.Application.Services.Utility;
using Dependent;

namespace Beeant.Distributed.Inside.Api.Controllers.Utility
{

    public class QueueController : Controller
    {
       
        /// <summary>
        /// 保存取值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual int Push(string name, string value)
        {
            Ioc.Resolve<IQueueApplicationService>().Open(name, int.MaxValue);
            return Ioc.Resolve<IQueueApplicationService>().Push(name, value);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public virtual string Pop(string name)
        {
            return Ioc.Resolve<IQueueApplicationService>().Pop<string>(name);
        }
     
    }
}

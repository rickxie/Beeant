using System.Collections.Generic;

namespace Beeant.Application.Services.Sys
{
    public interface IEventEngineApplicationService
    {
        /// <summary>
        /// 执行事件
        /// </summary>
        void Trigger(string name,IDictionary<string,string> args);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="eventApplicationServices"></param>
        /// <returns></returns>
        void Execute(string url,string name,IList<IEventApplicationService> eventApplicationServices);
    }
}

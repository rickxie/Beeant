namespace Beeant.Application.Services.Sys
{
    public interface IEventApplicationService
    {
        
        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        bool Execute(string url,string name);
    }
}

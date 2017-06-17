namespace Beeant.Application.Services
{
    public interface IJobApplicationService
    {
        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="args"></param>
        bool Execute(object[] args);
    }
}

using Beeant.Domain.Services.Utility;

namespace Beeant.Application.Services.Utility
{
    public class QueueApplicationService:IQueueApplicationService
    {
        public IQueueRepository QueueRepository { get; set; }
        /// <summary>
        /// 打开消息队列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        public virtual void Open(string name, int count)
        {
            QueueRepository.Open(name, count);
        }
        /// <summary>
        /// 保存取值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual int Push<T>(string name, T value)
        {
            return QueueRepository.Push(name, value);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public virtual T Pop<T>(string name)
        {
            return QueueRepository.Pop<T>(name);
        }
        /// <summary>
        /// 关闭消息队列
        /// </summary>
        /// <param name="name"></param>
        public virtual void Close(string name)
        {
            QueueRepository.Close(name);
        }
    }
}

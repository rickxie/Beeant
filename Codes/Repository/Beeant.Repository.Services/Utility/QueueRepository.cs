using System;
using Beeant.Domain.Services.Utility;

namespace Beeant.Repository.Services.Utility
{
    public class QueueRepository : IQueueRepository
    {
        /// <summary>
        /// 打开消息队列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        public virtual void Open(string name, int count)
        {
            try
            {
                Winner.Creator.Get<Winner.Queue.IQueue>().Open(name, count);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
         
        }
        /// <summary>
        /// 保存取值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual int Push<T>(string name, T value)
        {
            try
            {
                return Winner.Creator.Get<Winner.Queue.IQueue>().Push(name, value);
            }
            catch (Exception ex)
            {

                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return 0;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public virtual T Pop<T>(string name)
        {
            try
            {
                return Winner.Creator.Get<Winner.Queue.IQueue>().Pop<T>(name);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
            return default(T);
        }
        /// <summary>
        /// 关闭消息队列
        /// </summary>
        /// <param name="name"></param>
        public virtual void Close(string name)
        {
            try
            {
                Winner.Creator.Get<Winner.Queue.IQueue>().Close(name);
            }
            catch (Exception ex)
            {
                Winner.Creator.Get<Winner.Log.ILog>().AddException(ex);
            }
           
        }
    }
}

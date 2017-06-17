using System.Collections.Concurrent;

namespace Winner.Queue
{
    public class LocalQueue : IQueue
    {

        private static ConcurrentDictionary<string, ConcurrentQueue<object>> _message = new ConcurrentDictionary<string, ConcurrentQueue<object>>();
        /// <summary>
        /// 消息
        /// </summary>
        public static ConcurrentDictionary<string, ConcurrentQueue<object>> Message
        {
            get { return _message; }
            set { _message = value; }
        }
        private static ConcurrentDictionary<string, int> _expirations = new ConcurrentDictionary<string, int>();
        /// <summary>
        /// 消息
        /// </summary>
        public static ConcurrentDictionary<string, int> Expirations
        {
            get { return _expirations; }
            set { _expirations = value; }
        }




        /// <summary>
        /// 打开消息队列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        public virtual void Open(string name, int count)
        {
            if (!Message.ContainsKey(name))
            {
                Message.TryAdd(name, new ConcurrentQueue<object>());
                Expirations.TryAdd(name, count);
            }
        }
        /// <summary>
        /// 保存取值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual int Push<T>(string name, T value)
        {
            if (!Message.ContainsKey(name))
                return 0;
            Message[name].Enqueue(value);
            if (Message[name].Count > Expirations[name])
                Pop<T>(name);
            return Message[name].Count;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public virtual T Pop<T>(string name)
        {
            if (!Message.ContainsKey(name))
                return default(T);
            object result;
            Message[name].TryDequeue(out result);
            return (T)result;
        }
        /// <summary>
        /// 关闭消息队列
        /// </summary>
        /// <param name="name"></param>
        public virtual void Close(string name)
        {
            ConcurrentQueue<object> result;
            int count;
            if (Message.ContainsKey(name))
                Message.TryRemove(name, out result);
            if (Expirations.ContainsKey(name))
                Expirations.TryRemove(name, out count);
        }
      
    }
}

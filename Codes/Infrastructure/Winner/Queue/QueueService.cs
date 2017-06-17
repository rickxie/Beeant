namespace Winner.Queue
{
    public class QueueService : IQueueContract
    {
        #region 属性
     

        /// <summary>
        /// 缓存实例
        /// </summary>
        public IQueue Queuer { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public QueueService()
        { }

        /// <summary>
        /// WCF配置文路径件,缓存实例
        /// </summary>
        /// <param name="queuer"></param>
        public QueueService(IQueue queuer)
        {

            Queuer = queuer;
        }
        #endregion 


        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxCount"></param>
        public virtual void Open(string name, int maxCount)
        {
            Queuer.Open(name,maxCount);
        }
        /// <summary>
        /// 存
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int Push(string name, object value)
        {
            return Queuer.Push(name, value);
        }
        /// <summary>
        /// 取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual object Pop(string name)
        {
            return Queuer.Pop<object>(name);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="name"></param>
        public virtual void Close(string name)
        {
            Queuer.Close(name);
        }
    }
}

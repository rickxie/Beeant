namespace Winner.Queue
{
    public interface IQueue
    {
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxCount"></param>
        void Open(string name, int maxCount);
        /// <summary>
        /// 保存取值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        int Push<T>(string name, T value);

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        T Pop<T>(string name);
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="name"></param>
        void Close(string name);

    }
}

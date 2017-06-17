namespace Winner.Persistence.Key
{
    public interface IKey
    {
        /// <summary>
        /// 初始化Key
        /// </summary>
        void Initialize();
        /// <summary>
        /// 得到主键
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetKey(string name);
    }
}

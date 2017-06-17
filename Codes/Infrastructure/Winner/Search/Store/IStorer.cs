using System.Collections.Generic;

namespace Winner.Search.Store
{
    public interface IStorer
    {
        /// <summary>
        /// 路径
        /// </summary>
        string Path { get; set; }
        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Save<T>(string name, T value);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Delete(string name);
        /// <summary>
        /// 读
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Read<T>(string name);
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        void Flush(string name);

        /// <summary>
        /// 根据根目录读取所有存储名称
        /// </summary>
        /// <param name="rootName"></param>
        /// <returns></returns>
        IList<string> GetNames(string rootName);
    }
}

namespace Winner.Search.Word
{

    public interface IWorder
    {
        /// <summary>
        /// 加载词库
        /// </summary>
        /// <param name="storeIndex"></param>
        void Load(StoreIndexInfo storeIndex);
        /// <summary>
        /// 写入词库
        /// </summary>
        /// <param name="storeIndex"></param>
        void Write(StoreIndexInfo storeIndex);
        /// <summary>
        /// 插入词
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        bool Insert(StoreIndexInfo storeIndex, WordInfo word);
        /// <summary>
        /// 插入词
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        bool Update(StoreIndexInfo storeIndex, WordInfo word);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        bool Remove(StoreIndexInfo storeIndex, string word);
        /// <summary>
        /// 得到词
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        WordInfo GetInfo(StoreIndexInfo storeIndex, string word);
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <returns></returns>
        bool Clear(StoreIndexInfo storeIndex);
    }
}

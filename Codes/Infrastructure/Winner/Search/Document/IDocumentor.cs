using System.Collections.Generic;

namespace Winner.Search.Document
{
    public interface IDocumentor
    {
    
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        bool Insert(StoreIndexInfo storeIndex , DocumentInfo document);
        /// <summary>
        /// 根据行号更新记录
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        bool Update(StoreIndexInfo storeIndex, DocumentInfo document);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Remove(StoreIndexInfo storeIndex, long id);

        /// <summary>
        /// 得到行数据
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        DocumentInfo GetInfo(StoreIndexInfo storeIndex, long id);
        /// <summary>
        /// 清楚记录
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <returns></returns>
        bool Clear(StoreIndexInfo storeIndex);
        /// <summary>
        /// 初始化主键
        /// </summary>
        void InitlizeIndex(IList<StoreIndexInfo> storeIndexs);
    }
}

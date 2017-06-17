using System.Collections.Generic;
using Winner.Search.Store;

namespace Winner.Search.Document
{
    public class Documentor : IDocumentor
    {
        #region 静态变量
        /// <summary>
        /// 锁
        /// </summary>
        static private readonly object Locker = new object();
        #endregion

        #region 属性
        protected string RowName = @"{0}\Document\{1}.dum";
        protected string RootName = @"{0}\Document\";
 
        /// <summary>
        /// 存储实例
        /// </summary>
        public IStorer Storer { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public Documentor()
        { 
        }

        /// <summary>
        /// 排序实例，存储器实例,词库实例
        /// </summary>
        /// <param name="storer"></param>
        public Documentor(IStorer storer) 
        {
            Storer = storer;
        }
        #endregion

        #region 接口的实现
 
        /// <summary>
        /// 录入数据
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual bool Insert(StoreIndexInfo storeIndex,DocumentInfo document)
        {
            if (storeIndex==null || document == null) 
                return false;
            IList<DocumentInfo> documents = GetDocuments(storeIndex);
            documents = documents ?? new List<DocumentInfo>();
            document.Id = GetDocumentId(storeIndex);
            documents.Add(document);
            Storer.Save(GetStoreName(storeIndex.Name, GetStoreIndex(storeIndex, document.Id)), documents);
            return true;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual bool Update(StoreIndexInfo storeIndex, DocumentInfo document)
        {
            if (storeIndex==null || document == null) 
                return false;
            string pageName = GetStoreName(storeIndex.Name, GetStoreIndex(storeIndex, document.Id));
            var documents = Storer.Read<IList<DocumentInfo>>(pageName);
            if (documents != null)
            {
                var index = GetDocumentIndex(storeIndex, document.Id);
                documents[index] = document;
            }
            Storer.Save(pageName, documents);
            return true;
        }
        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Remove(StoreIndexInfo storeIndex, long id)
        {
            if (storeIndex==null)
                return false;
            string pageName = GetStoreName(storeIndex.Name, GetStoreIndex(storeIndex,id));
            var documents = Storer.Read<IList<DocumentInfo>>(pageName);
            var index = GetDocumentIndex(storeIndex, id);
            documents[index].Feilds = null;
            Storer.Save(pageName, documents);
            return true;
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <returns></returns>
        public virtual bool Clear(StoreIndexInfo storeIndex)
        {
            var names = Storer.GetNames(string.Format(RootName, storeIndex.Name));
            if (names == null) return true;
            foreach (var indexName in names)
            {
                Storer.Delete(indexName);
            }
            storeIndex.StoreDocument.DataCount = 0;
            return true;
        }
  

        /// <summary>
        /// 初始化主键
        /// </summary>
        public virtual void InitlizeIndex(IList<StoreIndexInfo> storeIndexs)
        {
            foreach (var storeIndex in storeIndexs)
            {
                IList<DocumentInfo> documents = null;
                var i = 0;
                while (true)
                {
                    var drs = Storer.Read<IList<DocumentInfo>>(GetStoreName(storeIndex.Name, i));
                    if(drs==null)break;
                    documents = drs;
                    i++;
                }
                if (documents == null) storeIndex.StoreDocument.DataCount = 0;
                else storeIndex.StoreDocument.DataCount = documents[documents.Count - 1].Id + 1;
            }

        }

        /// <summary>
        /// 得到行数据
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual DocumentInfo GetInfo(StoreIndexInfo storeIndex, long id)
        {
            if (storeIndex==null) return null;
            IList<DocumentInfo> documents = GetInfos(storeIndex.Name, GetStoreIndex(storeIndex,id));
            var index = GetDocumentIndex(storeIndex, id);
            if (documents != null && index < documents.Count)
            {
                return documents[index];
            }
            return null;

        }
        /// <summary>
        /// 得到索引
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual int GetStoreIndex(StoreIndexInfo storeIndex, long id)
        {
            return (int)((id - 1) / storeIndex.StoreDocument.PageSize);
        }
     
        /// <summary>
        /// 得到索引
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual int GetDocumentIndex(StoreIndexInfo storeIndex, long id)
        {
            return (int)((id - 1) % storeIndex.StoreDocument.PageSize);
        }
     
        /// <summary>
        /// 得到数据集
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual IList<DocumentInfo> GetInfos(string name, long index)
        {
            string rowname = GetStoreName(name, index);
            var dataRows = Storer.Read<IList<DocumentInfo>>(rowname);
            return dataRows;
        }
        #endregion
 
   

        /// <summary>
        /// 得到插入时候的数据集
        /// </summary>
        /// <param name="storeIndex"></param>
        /// <returns></returns>
        protected virtual IList<DocumentInfo> GetDocuments(StoreIndexInfo storeIndex)
        {
            var pageCount = storeIndex.StoreDocument.DataCount % storeIndex.StoreDocument.PageSize == 0 ? storeIndex.StoreDocument.PageCount + 1 : storeIndex.StoreDocument.PageCount;
            IList<DocumentInfo> documents = GetInfos(storeIndex.Name, pageCount);
            documents = documents ?? new List<DocumentInfo>();
            return documents;
        }

     
        /// <summary>
        /// 得到行名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public virtual string GetStoreName(string name, long pageindex)
        {
            return string.Format(RowName, name, pageindex);
        }

        #region 得到主键

        /// <summary>
        /// 得到Key
        /// </summary>
        protected virtual long GetDocumentId(StoreIndexInfo storeIndex)
        {
            lock (Locker)
            {
                storeIndex.StoreDocument.DataCount++;
                return storeIndex.StoreDocument.DataCount;
            }
        }

        #endregion

    }
}

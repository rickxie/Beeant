using System;
using System.Collections.Generic;
using System.Linq;

namespace Winner.Search
{
 
    /// <summary>
    /// 词库存储信息
    /// </summary>
    [Serializable]
    public class StoreIndexInfo
    {
        /// <summary>
        /// 词库名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 保留的查询记录
        /// </summary>
        public int TopDocumentCount { get; set; }

        private StoreDocumentInfo _storeDocument = new StoreDocumentInfo();
        /// <summary>
        ///文档存储信息
        /// </summary>
        public StoreDocumentInfo StoreDocument
        {
            get { return _storeDocument; }
            set { _storeDocument = value; }
        }

        private IList<StoreSequenceInfo> _storeSequences = new List<StoreSequenceInfo>();
        /// <summary>
        ///字段排序信息
        /// </summary>
        public IList<StoreSequenceInfo> StoreSequences
        {
            get { return _storeSequences; }
            set { _storeSequences = value; }
        }

        private IList<StoreFieldInfo> _storeFields = new List<StoreFieldInfo>();
        /// <summary>
        ///字段存储信息
        /// </summary>
        public IList<StoreFieldInfo> StoreFields
        {
            get { return _storeFields; }
            set { _storeFields = value; }
        }

        /// <summary>
        /// 得到存储字段
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StoreFieldInfo GetStoreField(int index)
        {
            return StoreFields.FirstOrDefault(it=>it.Index==index);
        }

    }
 
   
}

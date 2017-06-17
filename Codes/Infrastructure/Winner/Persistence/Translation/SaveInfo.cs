using Winner.Persistence.Relation;
using System.Linq;
namespace Winner.Persistence.Translation
{
    public class SaveInfo
    {
        public int Sequence { get; set; }
        /// <summary>
        /// 是否启用批量插入
        /// </summary>
        public bool IsBulkCopy { get; set; }
        /// <summary>
        /// 存储信息
        /// </summary>
        public EntityInfo Information { get; set; }
        /// <summary>
        /// Orm
        /// </summary>
        public OrmObjectInfo Object { get; set; }
        /// <summary>
        /// 实体
        /// </summary>
        public object Entity { get; set; }
        /// <summary>
        /// 是否设置版本
        /// </summary>
        public bool IsSetVersion
        {
            get
            {
                return Information.SaveType == SaveType.Modify &&
                       string.IsNullOrEmpty(Information.WhereExp) &&
                       Object.VersionProperty != null &&
                       Object.Properties.Any(
                           it =>it.IsOptimisticLocker &&
                           (Information.Properties == null 
                           || Information.Properties.Contains(it.PropertyName)));
            }
        }

        private string _setDataBase;
        /// <summary>
        /// 得到写库
        /// </summary>
        public virtual string SetDataBase
        {
            set { _setDataBase = value; }
            get
            {
                if (string.IsNullOrEmpty(_setDataBase))
                    return Object.SetDataBase;
                return _setDataBase;
            }
        }
        /// <summary>
        /// 表索引
        /// </summary>
        public virtual string TableIndex { get; set; }

        private string _setTableName;
        /// <summary>
        /// 得到写库
        /// </summary>
        public virtual string SetTableName
        {
            get
            {
                if (string.IsNullOrEmpty(_setTableName))
                    _setTableName = GetSetTableName(Object);
                return _setTableName;
            }
        }
        /// <summary>
        /// 得到表名
        /// </summary>
        /// <param name="ormObject"></param>
        /// <returns></returns>
        public virtual string GetSetTableName(OrmObjectInfo ormObject)
        {
            if (!string.IsNullOrEmpty(TableIndex) && !string.IsNullOrEmpty(ormObject.RouteName))
                return string.Format("{0}{1}", ormObject.SetTableName, TableIndex);
            return ormObject.SetTableName;
        }
    }



 
    
}
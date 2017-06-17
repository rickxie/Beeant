using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Winner.Persistence.Relation;
using Winner.Persistence.Translation;

namespace Winner.Persistence.Key
{
    /// <summary>
    /// 加载ORM
    /// </summary>
    public class Key:IKey
    {
        #region 属性
        /// <summary>
        /// ORM实例
        /// </summary>
        public IOrm Orm{get;set;}
        /// <summary>
        /// 执行实例
        /// </summary>
        public IExecutor Executor { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        private IList<OrmKeyInfo> _ormKeys=new List<OrmKeyInfo>();
        /// <summary>
        /// 主键
        /// </summary>
        public IList<OrmKeyInfo> OrmKeys
        {
            get { return _ormKeys; }
            set { _ormKeys = value; }
        }
        private IDictionary<string, OrmKeyInfo> _keys = new Dictionary<string, OrmKeyInfo>();
        /// <summary>
        /// Orm主键
        /// </summary>
        public IDictionary<string, OrmKeyInfo> Keys
        {
            get { return _keys; }
            set { _keys = value; }
        }

        /// <summary>
        /// 检查Keys的锁
        /// </summary>
        static protected object CheckKeyLocker = new object();
        /// <summary>
        /// 读取主键的锁
        /// </summary>
        static protected Hashtable KeyLockers = Hashtable.Synchronized(new Hashtable()); 
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public Key()
        { 
        }

        /// <summary>
        /// Orm实例
        /// </summary>
        /// <param name="orm"></param>
        /// <param name="executor"></param>
        public Key(IOrm orm,IExecutor executor)
        {
            Orm = orm;
            Executor = executor;
        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 初始化主键
        /// </summary>
        public virtual void Initialize()
        {
            var orms = Orm.GetOrms();
            if (orms == null) return;
            foreach (var orm in orms.Values)
            {
                if (orm == null || orm.Key == null) continue;
                InitliazeKey(orm);
            }
        }

        /// <summary>
        /// 得到Key
        /// </summary>
        public object GetKey(string name)
        {
            OrmObjectInfo orm = Orm.GetOrm(name);
            if (orm == null || orm.Key == null)return null;
            return GetKeyValue(orm);
        }

        #endregion

        #region 主键操作

        /// <summary>
        /// 初始化Key
        /// </summary>
        /// <param name="orm"></param>
        protected virtual void InitliazeKey(OrmObjectInfo orm)
        {
            if (Keys.ContainsKey(orm.ObjectName)) return;
            lock (CheckKeyLocker)
            {
                if (!KeyLockers.ContainsKey(orm.ObjectName))
                    KeyLockers.Add(orm.ObjectName, new object());
                if (!Keys.ContainsKey(orm.ObjectName))
                    Keys.Add(orm.ObjectName, CloneOrmKey(orm.Key));
                var key = Keys[orm.ObjectName];
                key.Count = GetEntityKey(orm);
            }
        }

        /// <summary>
        /// 克隆Key
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        protected virtual OrmKeyInfo CloneOrmKey(string keyName)
        {
            var key = OrmKeys.FirstOrDefault(it => it.Name.Equals(keyName));
            if (key == null) return null;
            return new OrmKeyInfo
            {
                Count = 0,
                Flag = key.Flag,
                RightLength = key.RightLength
            };
        }
        #endregion

        #region 得到实体键

        /// <summary>
        /// 得到实体键
        /// </summary>
        /// <param name="orm"></param>
        /// <returns></returns>
        protected virtual int GetEntityKey(OrmObjectInfo orm)
        {
            var key = OrmKeys.FirstOrDefault(it => it.Name.Equals(orm.Key));
            if (key == null || string.IsNullOrEmpty(key.Recovery) || orm.PrimaryProperty.IsIdentityKey) return 0;
            var sql = key.Recovery.Replace("{Key}", orm.PrimaryProperty.PropertyName).Replace("{Table}", orm.SetTableName);
            var dt = Executor.ExecuteQuery<DataTable>(orm.SetDataBase, sql, CommandType.Text);
            return dt != null && dt.Rows.Count > 0 ?
                Convert.ToInt32(dt.Rows[0][0].ToString()) : 0;
        }

        /// <summary>
        /// 得到key的值
        /// </summary>
        /// <param name="orm"></param>
        /// <returns></returns>
        protected virtual string GetKeyValue(OrmObjectInfo orm)
        {
            InitliazeKey(orm);
            lock (KeyLockers[orm.ObjectName])
            {
                var key = Keys[orm.ObjectName];
                key.Count++;
                return string.Format("{0}{1}", key.Flag, key.Count.ToString(CultureInfo.InvariantCulture).PadLeft(key.RightLength, '0'));
            }
        }
        #endregion

    }
}

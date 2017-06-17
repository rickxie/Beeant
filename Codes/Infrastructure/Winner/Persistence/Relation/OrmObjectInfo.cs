using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Winner.Persistence.Relation
{


    [Serializable]
    public class OrmObjectInfo
    {
        /// <summary>
        /// 对象名称
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// 对象名称不包含程序集
        /// </summary>
        public string ShortObjectName
        {
            get { return ObjectName.Split(',')[0]; }
        }
        /// <summary>
        /// 别名
        /// </summary>
        public string NickObjectName { get; set; }
        /// <summary>
        /// 操作表名称
        /// </summary>
        public string SetTableName { get; set; }
        /// <summary>
        /// 查询表名称
        /// </summary>
        public string GetTableName { get; set; }

        private IList<OrmPropertyInfo> _properties = new List<OrmPropertyInfo>();
        /// <summary>
        /// 属性
        /// </summary>
        public IList<OrmPropertyInfo> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }
        /// <summary>
        /// 主键
        /// </summary>
        public OrmPropertyInfo PrimaryProperty { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public OrmPropertyInfo VersionProperty { get; set; }
        /// <summary>
        /// 数据库写信息
        /// </summary>
        public string SetDataBase { get; set; }
        /// <summary>
        /// 数据库读信息
        /// </summary>
        public string GetDataBase { get; set; }

        /// <summary>
        /// Key的名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 标记删除
        /// </summary>
        public string RemoveMark { get; set; }
        /// <summary>
        /// 标记还原
        /// </summary>
        public string RestoreMark { get; set; }
        /// <summary>
        /// 操作默认条件
        /// </summary>
        public string SetDefaultWhere { get; set; }
        /// <summary>
        /// 查询默认条件
        /// </summary>
        public string GetDefaultWhere { get; set; }
        /// <summary>
        /// 是否实体缓存
        /// </summary>
        public bool IsCache { get; set; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        public long CacheTime { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }


        /// <summary>
        /// 得到属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual OrmPropertyInfo GetPropertyInfo(string name)
        {
            return Properties.FirstOrDefault(pi => pi.PropertyName.Equals(name) || pi.NickPropertyName!=null && pi.NickPropertyName.Contains(name));
        }
        #region 得到属性连

        /// <summary>
        /// 得到属性连
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="isContainRemote"></param>
        /// <returns></returns>
        public virtual IList<OrmPropertyInfo> GetChainProperties( string propertyName,bool isContainRemote=false)
        {
            var orm = this;
            var name = propertyName;
            var chainProperties = new List<OrmPropertyInfo>();
            while (orm!=null)
            {
                name = AddNearProperty(chainProperties, orm, name);
                if(string.IsNullOrEmpty(name))break;
                var p = chainProperties.LastOrDefault();
                if (p == null || p.Map == null) break;
                if (!isContainRemote && p.Map.CheckRemote())
                    break;
                orm = p.Map.GetMapObject();
            }
            return chainProperties;
        }

        /// <summary>
        /// 得到最近的一个属性
        /// </summary>
        /// <param name="chainProperties"></param>
        /// <param name="orm"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual string AddNearProperty(IList<OrmPropertyInfo> chainProperties, OrmObjectInfo orm, string propertyName)
        {
            var p = orm.GetPropertyInfo(propertyName);
            if (p != null)
            {
                chainProperties.Add(p);
                return null;
            }
            var index = propertyName.IndexOf('.');
            if (index == -1) return null;
            p = orm.GetPropertyInfo(propertyName.Substring(0, index));
            if (p != null) chainProperties.Add(p);
            return propertyName.Substring(index + 1, propertyName.Length - 1 - index);
        }
        
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual OrmObjectInfo Clone()
        {
            var formatter = new BinaryFormatter();
            var stream = new System.IO.MemoryStream();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as OrmObjectInfo;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Winner.Persistence.Relation;
using Winner.Persistence.Translation;

namespace Winner.Persistence.Compiler.Common
{
    public class QueryCompilerInfo
    {
        public Action<OrmObjectInfo,QueryInfo, QueryCompilerInfo> TranslateQuery { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public QueryCompilerInfo Parent { get; set; }
        /// <summary>
        /// 映射属性
        /// </summary>
        public string RemotePropertyName { get; set; }
        /// <summary>
        /// 外链
        /// </summary>
        public string Chainon { get; set; }
        /// <summary>
        /// 查询
        /// </summary>
        public QueryInfo Query { get; set; }
        /// <summary>
        /// 查询
        /// </summary>
        public QueryInfo SubQuery { get; set; }
        /// <summary>
        /// 对象
        /// </summary>
        public OrmObjectInfo Object { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TableInfo Table { get; set; }
        /// <summary>
        /// 解析内容
        /// </summary>
        public string Exp { get; set; }

        /// <summary>
        /// 拼接
        /// </summary>
        public StringBuilder Builder { get; set; }
        /// <summary>
        /// 字段数量
        /// </summary>
        public int FieldCount { get; set; }

     
        public QueryCompilerInfo(OrmObjectInfo obj, string exp,TableInfo table, StringBuilder builder)
        {
            Object = obj;
            Exp = exp;
            Table = table;
            Builder = builder;
        }
 
   

        /// <summary>
        /// 得到别名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual string GetAsName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return Table.AsName;
            if (Table.Joins.ContainsKey(propertyName)) return Table.Joins[propertyName].AsName;
            return Table.AsName;
        }

        /// <summary>
        /// 得到查询字段名称
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual string GetFieldName(OrmPropertyInfo property, string propertyName)
        {
            FieldCount++;
            if (property.IsCustom)
            {
                var lastIndex = propertyName.LastIndexOf('.');
                var name = lastIndex < 0 ? propertyName : propertyName.Substring(0, lastIndex);
                return string.Format(property.FieldName, GetAsName(name));
            }
            return string.Format("{0}.{1}", GetAsName(string.IsNullOrEmpty(propertyName)?propertyName:
                propertyName.Replace(string.Format(".{0}", property.PropertyName),"")), property.FieldName);
        }

        /// <summary>
        /// 添加连接表
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertyName"></param>
        public virtual void AddJoin(OrmPropertyInfo property, string propertyName)
        {
            var index = propertyName.LastIndexOf('.');
            var lastName =propertyName.Substring(0,index) ;
            if (!Table.Joins.ContainsKey(propertyName))
            {
                Table.Joins.Add(propertyName, new JoinInfo
                    {
                        AsFieldName = property.Map.MapObjectProperty.FieldName,
                        AsName = Table.CreateAsName(),
                        JoinName = GetAsName(lastName),
                        JoinFieldName = property.Map.ObjectProperty.FieldName,
                        Object = property.Map.GetMapObject(),
                        Map= property.Map
                });
            }
        }

        /// <summary>
        /// 添加连接表
        /// </summary>
        /// <param name="chainProperties"></param>
        public virtual void AddJoins(IList<OrmPropertyInfo> chainProperties)
        {
            var lastName = "";
            var currentName = "";
            foreach (var chainProperty in chainProperties)
            {
                currentName = string.IsNullOrEmpty(currentName)
                                  ? chainProperty.PropertyName
                                  : string.Format("{0}.{1}", currentName, chainProperty.PropertyName);
                if (chainProperty.Map==null || chainProperty.Map.MapType==OrmMapType.OneToMany || chainProperty.Map.CheckRemote())
                    break;
                if (!Table.Joins.ContainsKey(currentName))
                {
                    Table.Joins.Add(currentName, new JoinInfo
                        {
                            AsFieldName = chainProperty.Map.MapObjectProperty.FieldName,
                            AsName = Table.CreateAsName(),
                            JoinName = GetAsName(lastName),
                            JoinFieldName = chainProperty.Map.ObjectProperty.FieldName,
                            Object = chainProperty.Map.GetMapObject(),
                            Map=chainProperty.Map
                    });
                }
                lastName = currentName;
            }
        }

        /// <summary>
        /// 得到连接表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual string GetJoinTable( QueryInfo query)
        {
            var builder = new StringBuilder();
            var getTableName =query.GetGetTableName(Object);
            if (!string.IsNullOrEmpty(Object.GetDefaultWhere))
                getTableName = string.Format("(select * from {0} where {1})", getTableName, Object.GetDefaultWhere);
            builder.AppendFormat("{0} {1}", getTableName, Table.AsName);
            BuilderJoinTable(builder);
            return builder.ToString();
        }

        /// <summary>
        /// 得到连接表
        /// </summary>
        /// <param name="save"></param>
        /// <returns></returns>
        public virtual string GetJoinTable(SaveInfo save)
        {
            var builder = new StringBuilder();
            var setTableName = save.SetTableName;
            if (!string.IsNullOrEmpty(Object.GetDefaultWhere))
                setTableName = string.Format("(select * from {0} where {1})", setTableName, Object.GetDefaultWhere);
            builder.AppendFormat("{0} {1}", setTableName, Table.AsName);
            BuilderJoinTable(builder);
            return builder.ToString();
        }
        /// <summary>
        /// 得到连接表
        /// </summary>
        /// <returns></returns>
        public virtual string GetJoinTable()
        {
            var builder = new StringBuilder();
            if (SubQuery == null)
            {
                if (string.IsNullOrEmpty(Object.GetDefaultWhere))
                    builder.AppendFormat(Query.GetGetTableName(Object));
                else
                    builder.AppendFormat("(select * from {0} where {1})", Query.GetGetTableName(Object), Object.GetDefaultWhere);
            }
            else
            {
                TranslateQuery(Object, SubQuery,this);
                if (SubQuery.SqlParameters != null)
                {
                    Query.SqlParameters = Query.SqlParameters ?? new Dictionary<string, object>(Query.Parameters);
                    foreach (var sqlParameter in SubQuery.SqlParameters)
                    {
                        if (!Query.SqlParameters.ContainsKey(sqlParameter.Key))
                            Query.SqlParameters.Add(sqlParameter.Key, sqlParameter.Value);
                    }
                    foreach (var parameter in SubQuery.Parameters)
                    {
                        if(!SubQuery.SqlParameters.ContainsKey(parameter.Key) && Query.Parameters.ContainsKey(parameter.Key))
                            Query.Parameters.Remove(parameter.Key);
                    }
                }
                SubQuery.Sql = Regex.Replace(SubQuery.Sql, @" as \w*", "");
                builder.AppendFormat("({0})", SubQuery.Sql);
            }
            builder.AppendFormat(" {0}", Table.AsName);
            BuilderJoinTable(builder);
            return builder.ToString();
        }

   

        /// <summary>
        /// 拼接表
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void BuilderJoinTable(StringBuilder builder)
        {
            foreach (var join in Table.Joins)
            {
                builder.AppendFormat(" left join {0} {1} on {1}.{2}={3}.{4}",
                                            GetJoinTableName(join.Value), join.Value.AsName,
                                             join.Value.AsFieldName,
                                             join.Value.JoinName, join.Value.JoinFieldName);
            }
        }
        /// <summary>
        /// 得到关联表名称
        /// </summary>
        /// <param name="join"></param>
        /// <returns></returns>
        protected virtual string GetJoinTableName(JoinInfo join)
        {
            if (string.IsNullOrEmpty(join.Object.GetDefaultWhere)) return Query.GetGetTableName(join);
            return string.Format("(select * from {0} where {1})", Query.GetGetTableName(join), join.Object.GetDefaultWhere);
        }
        /// <summary>
        /// 得到默认条件
        /// </summary>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public virtual string GetDefaultWhere(bool isSave)
        {
            var builder = new StringBuilder();
            var defaultWhere = isSave? Object.SetDefaultWhere: Object.GetDefaultWhere;
            if (!string.IsNullOrEmpty(defaultWhere))
            {
                builder.AppendFormat("{0}.{1}", Table.AsName, defaultWhere);
            }
            if (Table.Joins == null) return builder.ToString();
            foreach (var join in Table.Joins)
            {
                var joinDefaultWhere = isSave
                                           ? join.Value.Object.SetDefaultWhere
                                           : join.Value.Object.GetDefaultWhere;
                if (string.IsNullOrEmpty(joinDefaultWhere)) continue;
                builder.Append(" and ");
                builder.AppendFormat("{0}.{1}", join.Value.AsName, joinDefaultWhere);
            }
            if (builder.Length > 0)
            {
                builder.Insert(0, "(");
                builder.Append(")");
            }
            return builder.ToString();
        }
    }
}

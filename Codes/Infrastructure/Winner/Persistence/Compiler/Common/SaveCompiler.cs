using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections;
using Winner.Persistence.Exceptions;
using Winner.Persistence.Key;
using Winner.Persistence.Relation;
using Winner.Persistence.Translation;

namespace Winner.Persistence.Compiler.Common
{
    public class SaveCompiler : ISaveCompiler
    {
        #region 属性
        /// <summary>
        /// 条件实例
        /// </summary>
        public IWhereCompiler WhereCompiler { get; set; }
        /// <summary>
        /// 主键实例
        /// </summary>
        public IKey Key { get; set; }

        private string _paramterFlag = "@";
        /// <summary>
        /// ModelBase的属性名称
        /// </summary>
        public virtual string ParamterFlag
        {
            get { return _paramterFlag; }
            set { _paramterFlag = value; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public SaveCompiler()
        { 
        }

        /// <summary>
        /// 属性实例，条件实例，Orm实例，属性实例,主键实例
        /// </summary>
        /// <param name="where"></param>
        /// <param name="key"></param>
        public SaveCompiler(IWhereCompiler where,IKey key)
        {
            WhereCompiler = where;
            Key = key;
        }
        #endregion

        #region 接口实现

        /// <summary>
        /// 转换对象
        /// </summary>
        /// <param name="saveCompile"></param>
        public virtual void Save(SaveCompilerInfo saveCompile)
        {
            if (saveCompile == null || saveCompile.SaveInfo.Entity == null)return;
            switch (saveCompile.SaveInfo.Information.SaveType)
            {
                case SaveType.Add: AddInfo(saveCompile); break;
                case SaveType.Modify: ModifyInfo(saveCompile); break;
                case SaveType.Remove: DeleteInfo(saveCompile); break;
                case SaveType.Restore: RestoreInfo(saveCompile); break;
            }
        }
        #endregion

        #region 对象插入

        /// <summary>
        /// 转换插入
        /// </summary>
        /// <param name="saveCompile"></param>
        protected virtual void AddInfo(SaveCompilerInfo saveCompile)
        {
            if (saveCompile.SaveInfo.Entity == null) return;
            FillKey(saveCompile, false);
            IList<OrmPropertyInfo> maps = new List<OrmPropertyInfo>();
            saveCompile.Command.CommandText = GetAddSql(saveCompile, maps);
            Execute(saveCompile);
            AppendAddMap(saveCompile, maps);
        }

        /// <summary>
        /// 添加Map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="maps"></param>
        protected virtual void AppendAddMap(SaveCompilerInfo saveCompile, IList<OrmPropertyInfo> maps)
        {
            foreach (var p in maps)
            {
                ConvertAddMap(saveCompile, p);
            }
        }
        /// <summary>
        /// 得到添加SQL语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="maps"></param>
        /// <returns></returns>
        protected virtual string GetAddSql(SaveCompilerInfo saveCompile, IList<OrmPropertyInfo> maps)
        {
            var sbName = new StringBuilder();
            var sbValue = new StringBuilder();
            AppendAddSqlAndConvertMap(saveCompile, maps, sbName, sbValue);
            sbName.Remove(sbName.Length - 1, 1);
            sbValue.Remove(sbValue.Length - 1, 1);
            var tableName = saveCompile.SaveInfo.SetTableName;
            return string.Format("insert {0} ({1}) values ({2})", tableName, sbName, sbValue);
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="maps"></param>
        /// <param name="sbName"></param>
        /// <param name="sbValue"></param>
        protected virtual void AppendAddSqlAndConvertMap(SaveCompilerInfo saveCompile,IList<OrmPropertyInfo> maps, StringBuilder sbName, StringBuilder sbValue)
        {
            int i = 0;
            foreach (OrmPropertyInfo p in saveCompile.SaveInfo.Object.Properties)
            {
                if (!p.AllowAdd || p.IsIdentityKey) continue;
                if (p.Map != null) 
                    maps.Add(p);
                else
                    AppendAddSql(saveCompile, p, sbName, sbValue, string.Format("Add_{0}", i++));
            }
        }
        /// <summary>
        /// 是否插入
        /// </summary>
        /// <param name="property"></param>
        /// <param name="information"></param>
        /// <param name="pValue"></param>
        /// <returns></returns>
        protected virtual bool IsAllowAdd(OrmPropertyInfo property, EntityInfo information, object pValue)
        {
            if (pValue == null) return false;
            if (information.Properties != null && !property.InProperties(information.Properties) && !property.IsPrimaryKey)
                return false;
            if (property.UnAddValue != null && (property.HasUnAddValue && (property.UnAddValue.Equals(pValue))))
                return false;
            return true;
        }

        /// <summary>
        /// 添加插入语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        /// <param name="sbName"></param>
        /// <param name="sbValue"></param>
        /// <param name="pname"></param>
        protected virtual void AppendAddSql(SaveCompilerInfo saveCompile, OrmPropertyInfo property, StringBuilder sbName, StringBuilder sbValue, string pname)
        {
            object pValue = saveCompile.SaveInfo.Entity.GetProperty(property.PropertyName);
            if (!IsAllowAdd(property, saveCompile.SaveInfo.Information, pValue))
                return;
            sbName.AppendFormat("{0},", property.FieldName);
            sbValue.AppendFormat("{0}{1},",ParamterFlag, pname);
            AddParamter(saveCompile.Command, property, pname, pValue);
        }
        #endregion

        #region 对象更新

        /// <summary>
        /// 转换更新
        /// </summary>
        /// <param name="saveCompile"></param>
        protected virtual void ModifyInfo(SaveCompilerInfo saveCompile)
        {
            if (saveCompile.SaveInfo.Entity == null) return;
            var where = GetModifyWhereSql(saveCompile);
            if (saveCompile.SaveInfo.Entity == null) return;
            IList<OrmPropertyInfo> maps = new List<OrmPropertyInfo>();
            saveCompile.Command.CommandText = GetModifySql(saveCompile, where, maps);
            Execute(saveCompile);
            AppendModifyMap(saveCompile, maps);
        }

        /// <summary>
        /// 添加Map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="maps"></param>
        protected virtual void AppendModifyMap(SaveCompilerInfo saveCompile,IList<OrmPropertyInfo> maps)
        {
            foreach (var p in maps)
            {
                ConvertModifyMap(saveCompile, p);
            }
        }

        /// <summary>
        /// 得到更新语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        /// <param name="maps"></param>
        /// <returns></returns>
        protected virtual string GetModifySql(SaveCompilerInfo saveCompile,string where, IList<OrmPropertyInfo> maps)
        {
            var sql = new StringBuilder();
            var tableName = saveCompile.SaveInfo.SetTableName;
            sql.AppendFormat("update {0} set ", tableName);
            AppendSetSqlAndConvertMap(saveCompile, sql, maps);
            sql.Remove(sql.Length - 1, 1);
            sql.Append(where);
            return sql.ToString();
        }

        /// <summary>
        /// 拼接Set语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        /// <param name="sql"></param>
        /// <param name="pname"></param>
        protected virtual void AppendModifySetSql(SaveCompilerInfo saveCompile, OrmPropertyInfo property, StringBuilder sql, string pname)
        {
            object pValue = saveCompile.SaveInfo.Entity.GetProperty(property.PropertyName);
            if (!IsAllowModify(property, saveCompile.SaveInfo.Information, pValue))
                return;
            if (property.IsVersion && saveCompile.SaveInfo.IsSetVersion)
            {
                sql.AppendFormat("{0}={0}+1,", saveCompile.SaveInfo.Object.VersionProperty.FieldName);
            }
            else
            {
                sql.AppendFormat("{0}={1}{2},", property.FieldName, ParamterFlag, pname);
                AddParamter(saveCompile.Command, property, pname, pValue);
            }
         
        }

        /// <summary>
        /// 更新条件
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <returns></returns>
        protected virtual string GetModifyWhereSql(SaveCompilerInfo saveCompile)
        {
            var where = new StringBuilder();
            if (!string.IsNullOrEmpty(saveCompile.SaveInfo.Information.WhereExp))
                AppendCustomerWhere(saveCompile, where);
            else
                AppendPrimaryWhere(saveCompile, where);
            AppendVesionWhere(saveCompile, where);
            if(where.Length>0) where.Insert(0, " where (");
            AppendDefaultWhere(where, saveCompile.SaveInfo.Object);
            return where.ToString();
        }
        /// <summary>
        /// 添加版本控制
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        protected virtual void AppendVesionWhere(SaveCompilerInfo saveCompile,StringBuilder where)
        {
            if (saveCompile.SaveInfo.IsSetVersion)
            {
                const string pname = "where_Version";
                var pValue =
                    saveCompile.SaveInfo.Entity.GetProperty(saveCompile.SaveInfo.Object.VersionProperty.PropertyName);
                if (where.Length > 0)
                    where.Append(" and ");
                where.AppendFormat("{0}={1}{2}", saveCompile.SaveInfo.Object.VersionProperty.FieldName, ParamterFlag, pname);
                AddParamter(saveCompile.Command, saveCompile.SaveInfo.Object.VersionProperty, pname, pValue);
            }
        }
        /// <summary>
        /// 添加set语句和转换Map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="sql"></param>
        /// <param name="maps"></param>
        protected virtual void AppendSetSqlAndConvertMap(SaveCompilerInfo saveCompile, StringBuilder sql, IList<OrmPropertyInfo> maps)
        {
            int i = 0;
            foreach (var p in saveCompile.SaveInfo.Object.Properties)
            {
                if (!p.AllowModify) continue;
                if (p.Map != null)
                    maps.Add(p);
                else
                    AppendModifySetSql(saveCompile, p, sql, string.Format("Update_{0}", i++));
            }
        }

        /// <summary>
        /// 得到是否更新
        /// </summary>
        /// <param name="property"></param>
        /// <param name="information"></param>
        /// <param name="pValue"></param>
        /// <returns></returns>
        protected virtual bool IsAllowModify(OrmPropertyInfo property,EntityInfo information, object pValue)
        {
            if (pValue == null) return false;
            if (property.IsPrimaryKey || (information.Properties != null && !property.InProperties(information.Properties)))
            {
                return false;
            }
            if (property.UnModifyValue != null && (property.HasUnModifyValue &&(property.UnModifyValue.Equals(pValue))))
            {
                return false;
            }
            return true;
        }
        
       
        #endregion

        #region 对象删除

        /// <summary>
        /// 转换删除
        /// </summary>
        /// <param name="saveCompile"></param>
        protected virtual void DeleteInfo(SaveCompilerInfo saveCompile)
        {
            if (saveCompile.SaveInfo.Entity == null) return;
            string where = GetDeleteWhereSql(saveCompile);
            saveCompile.Command.CommandText = GetDeleteSql(saveCompile, where);
            ConvertDeleteMap(saveCompile, where);
            Execute(saveCompile);
        }

        /// <summary>
        /// 得到删除语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual string GetDeleteSql(SaveCompilerInfo saveCompile, string where)
        {
            var sql = new StringBuilder();
            var tableName = saveCompile.SaveInfo.SetTableName;
            sql.Append(string.IsNullOrEmpty(saveCompile.SaveInfo.Object.RemoveMark)
                           ? string.Format("delete from {0} ", tableName)
                           : GetDeleteMarkSql(saveCompile));
            if (!string.IsNullOrEmpty(where))
                sql.Append(where);
            return sql.ToString();
        }

        /// <summary>
        /// 得到标记删除语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <returns></returns>
        protected virtual string GetDeleteMarkSql(SaveCompilerInfo saveCompile)
        {
            var sql=new StringBuilder();
            var tableName = saveCompile.SaveInfo.SetTableName;
            sql.AppendFormat("update {0} set {1} ", tableName, saveCompile.SaveInfo.Object.RemoveMark);
            int i = 0;
            foreach (var property in saveCompile.SaveInfo.Object.Properties.Where(property => property.AllowRemove))
            {
                var pName = string.Format("Remove_{0} ", i++);
                sql.AppendFormat(",{0}={1}{2} ", property.FieldName,ParamterFlag, pName);
                AddParamter(saveCompile.Command, property, pName, saveCompile.SaveInfo.Entity.GetProperty(property.PropertyName));
            }
            return sql.ToString();
        }

        /// <summary>
        /// 得到删除的where语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <returns></returns>
        protected virtual string GetDeleteWhereSql(SaveCompilerInfo saveCompile)
        {
            var where = new StringBuilder();
            if (!string.IsNullOrEmpty(saveCompile.SaveInfo.Information.WhereExp))
                AppendCustomerWhere(saveCompile, where);
            else
                AppendPrimaryWhere(saveCompile, where);
            if (where.Length > 0) where.Insert(0, " where (");
            AppendDefaultWhere(where, saveCompile.SaveInfo.Object);
            return where.ToString();
        }
         
    
        #endregion

        #region 对象还原

        /// <summary>
        /// 转换删除
        /// </summary>
        /// <param name="saveCompile"></param>
        protected virtual void RestoreInfo(SaveCompilerInfo saveCompile)
        {
            if (saveCompile.SaveInfo.Entity == null)return;
            if (string.IsNullOrEmpty(saveCompile.SaveInfo.Object.RestoreMark)) return;
            string where = GetRestoreWhereSql(saveCompile);
            saveCompile.Command.CommandText = GetRestoreSql(saveCompile, where);
            ConvertRestoreMap(saveCompile, where);
            Execute(saveCompile);
        }

        /// <summary>
        /// 得到还原语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual string GetRestoreSql(SaveCompilerInfo saveCompile, string where)
        {
            var sql = new StringBuilder();
            sql.Append(GetRestoreMarkSql(saveCompile));
            if (!string.IsNullOrEmpty(where))
                sql.Append(where.ToString(CultureInfo.InvariantCulture));
            return sql.ToString();
        }

        /// <summary>
        /// 得到标记还原语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <returns></returns>
        protected virtual string GetRestoreMarkSql(SaveCompilerInfo saveCompile)
        {
            var sql = new StringBuilder();
            var tableName = saveCompile.SaveInfo.SetTableName;
            sql.AppendFormat("update {0} set {1} ", tableName, saveCompile.SaveInfo.Object.RestoreMark);
            int i = 0;
            foreach (var property in saveCompile.SaveInfo.Object.Properties.Where(property => property.AllowRestore))
            {
                var pName = string.Format("Restore_{0} ", i++);
                sql.AppendFormat(",{0}={1}{2} ", property.FieldName,ParamterFlag, pName);
                AddParamter(saveCompile.Command, property, pName, saveCompile.SaveInfo.Entity.GetProperty(property.PropertyName));
            }
            return sql.ToString();
        }

        /// <summary>
        /// 得到还原的where语句
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <returns></returns>
        protected virtual string GetRestoreWhereSql(SaveCompilerInfo saveCompile)
        {
            var where = new StringBuilder();
            if (!string.IsNullOrEmpty(saveCompile.SaveInfo.Information.WhereExp))
                AppendCustomerWhere(saveCompile, where);
            else
                AppendPrimaryWhere(saveCompile, where);
            if (where.Length > 0)
                where.Insert(0, " where (");
            AppendDefaultWhere(where, saveCompile.SaveInfo.Object);
            return where.ToString();
        }


        #endregion

        #region 转换添加Map

        /// <summary>
        /// 转换Map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        protected virtual void ConvertAddMap(SaveCompilerInfo saveCompile, OrmPropertyInfo property)
        {
            if (property.Map == null || !property.Map.IsAdd) return;
            var p = saveCompile.SaveInfo.Entity.GetType().GetProperties().FirstOrDefault(it => it.Name.Equals(property.PropertyName));
            if (p == null) return;
            var pValue = p.GetValue(saveCompile.SaveInfo.Entity, null);
            if (pValue != null && !saveCompile.IsInMap(property))
            {
                saveCompile.MapProperties.Add(property);
                SelectAddMap(saveCompile, property, pValue);
            }
        }

        /// <summary>
        /// 选择添加Map的执行方式
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        /// <param name="pValue"></param>
        protected virtual void SelectAddMap(SaveCompilerInfo saveCompile, OrmPropertyInfo property, object pValue)
        {
            switch (property.Map.MapType)
            {
                case OrmMapType.OneToOne:
                    AddOneToOneMap(saveCompile, property, pValue);
                    break;
                case OrmMapType.OneToMany:
                    AddOneToManyMap(saveCompile, property, pValue);
                    break;
            }
        }

        /// <summary>
        /// 添加1对1关系map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        /// <param name="pValue"></param>
        protected virtual void AddOneToOneMap(SaveCompilerInfo saveCompile, OrmPropertyInfo property, object pValue)
        {
            var pk = saveCompile.SaveInfo.Entity.GetType().GetProperty(property.Map.ObjectProperty.PropertyName).GetValue(saveCompile.SaveInfo.Entity, null);
            FillForeignProperty(pValue,property, pk);
            AddMapInfo(saveCompile, property, pValue);
        }

        /// <summary>
        /// 添加1对多关系map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        /// <param name="pValue"></param>
        protected virtual void AddOneToManyMap(SaveCompilerInfo saveCompile, OrmPropertyInfo property, object pValue)
        {
            System.Reflection.MethodInfo method = pValue.GetType().GetMethod("GetEnumerator");
            object pk = saveCompile.SaveInfo.Entity.GetType().GetProperty(property.Map.ObjectProperty.PropertyName).GetValue(saveCompile.SaveInfo.Entity, null);
            if (method != null)
            {
                FillOneToManyMap(saveCompile, property, pValue, method, pk);
            }
        }

        /// <summary>
        /// 填充1对多关系map对象
        /// </summary>
        /// <param name="saveCompile"/>
        /// <param name="property"></param>
        /// <param name="pValue"></param>
        /// <param name="method"></param>
        /// <param name="pk"></param>
        protected virtual void FillOneToManyMap(SaveCompilerInfo saveCompile, OrmPropertyInfo property, object pValue, System.Reflection.MethodInfo method, object pk)
        {
            var etor = (IEnumerator)method.Invoke(pValue, null);
            while (etor.MoveNext())
            {
                if (etor.Current == null)
                    continue;
                FillForeignProperty(etor.Current, property, pk);
                etor.Current.GetType().GetProperty(property.Map.MapObjectProperty.PropertyName).SetValue(etor.Current, pk, null);
                AddMapInfo(saveCompile, property, etor.Current);
            }
        }

        /// <summary>
        /// 填充外键
        /// </summary>
        /// <param name="info"></param>
        /// <param name="property"></param>
        /// <param name="pk"></param>
        protected virtual void FillForeignProperty(object info, OrmPropertyInfo property, object pk)
        {
            var propertyType =
                info.GetType()
                    .GetProperties()
                    .FirstOrDefault(it => it.Name.Equals(property.Map.MapObjectProperty.PropertyName));
            if (propertyType == null) return;
            if (propertyType.PropertyType.IsValueType && !pk.Equals(0)
                || !propertyType.PropertyType.IsValueType && pk != null) return;
            propertyType.SetValue(info, pk, null);
        }

        /// <summary>
        /// 添加map信息
        /// </summary>
        /// <param name="saveCompiler"></param>
        /// <param name="property"></param>
        /// <param name="entity"></param>
        protected virtual void AddMapInfo(SaveCompilerInfo saveCompiler, OrmPropertyInfo property, object entity)
        {
            if (((EntityInfo)entity).SaveType != SaveType.Modify || saveCompiler.ContentEntities.Contains(entity)) return;
            var mapSaveCompiler = new SaveCompilerInfo
                {
                    Command = saveCompiler.Command,
                    MapProperties = saveCompiler.MapProperties,
                    ContentEntities = saveCompiler.ContentEntities,
                    SaveInfo=new SaveInfo
                        {
                            Entity = entity,
                            Information = (EntityInfo)entity,
                            Object = property.Map.GetMapObject()
                        }
             };
            AddInfo(mapSaveCompiler);
        }
       

        #endregion

        #region 转换更新Map

        /// <summary>
        /// 更新map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        protected virtual void ConvertModifyMap(SaveCompilerInfo saveCompile, OrmPropertyInfo property)
        {
            if (property.Map == null || !property.Map.IsModify) return;
            var p = saveCompile.SaveInfo.Entity.GetType().GetProperties().FirstOrDefault(it => it.Name.Equals(property.PropertyName));
            if (p == null) return;
            var pValue = p.GetValue(saveCompile.SaveInfo.Entity, null);
            if (pValue != null && !saveCompile.IsInMap(property))
            {
                saveCompile.MapProperties.Add(property);
                SelectModifyMap(saveCompile, pValue, property);
            }
        }

        /// <summary>
        /// 选择更新map方式
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="pValue"></param>
        /// <param name="property"></param>
        protected virtual void SelectModifyMap(SaveCompilerInfo saveCompile, object pValue, OrmPropertyInfo property)
        {
            switch (property.Map.MapType)
            {
                case OrmMapType.OneToOne:
                    ModifyMapInfo(saveCompile, property, pValue);
                    break;
                case OrmMapType.OneToMany:
                    ModifyOneToManyMap(saveCompile, property, pValue);
                    break;
            }
        }

        /// <summary>
        /// 执行1对多map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        /// <param name="pValue"></param>
        protected virtual void ModifyOneToManyMap(SaveCompilerInfo saveCompile, OrmPropertyInfo property, object pValue)
        {
            System.Reflection.MethodInfo method = pValue.GetType().GetMethod("GetEnumerator");
            if (method != null)
            {
                var etor = (IEnumerator)method.Invoke(pValue, null);
                while (etor.MoveNext())
                {
                    ModifyMapInfo(saveCompile, property, etor.Current);
                }
            }
        }

        /// <summary>
        /// 添加map信息
        /// </summary>
        /// <param name="saveCompiler"></param>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        protected virtual void ModifyMapInfo(SaveCompilerInfo saveCompiler, OrmPropertyInfo property, object entity)
        {
            var mapModel = ((EntityInfo) entity);
            if (((EntityInfo)entity).SaveType != SaveType.Modify || saveCompiler.ContentEntities.Contains(entity)) return;
            var mapSaveCompiler = new SaveCompilerInfo
            {
                Command = saveCompiler.Command,
                MapProperties = saveCompiler.MapProperties,
                ContentEntities = saveCompiler.ContentEntities,
                SaveInfo=new SaveInfo
                        {
                            Entity = entity,
                            TableIndex = saveCompiler.SaveInfo.TableIndex,
                            Information = mapModel,
                            Object = property.Map.GetMapObject()
                        }
            };
            ModifyInfo(mapSaveCompiler);
        }

        #endregion

        #region 转换删除Map

        /// <summary>
        /// 删除map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        protected virtual void ConvertDeleteMap(SaveCompilerInfo saveCompile, string where)
        {
            foreach (var p in saveCompile.SaveInfo.Object.Properties)
            {
                SelectDeleteMap(saveCompile, where, p);
            }
        }

        /// <summary>
        /// 选择删除Map方式
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        /// <param name="property"></param>
        protected virtual void SelectDeleteMap(SaveCompilerInfo saveCompile, string where, OrmPropertyInfo property)
        {
            if (property.Map == null || !property.Map.IsRemove && !saveCompile.IsInMap(property)) return;
            saveCompile.MapProperties.Add(property);
            AppendDeleteMapSql(saveCompile, property, @where);
            var mapSaveCompiler = new SaveCompilerInfo
            {
                Command = saveCompile.Command,
                MapProperties = saveCompile.MapProperties,
                SaveInfo = new SaveInfo
                {
                    Entity = saveCompile.SaveInfo.Entity,
                    Information = saveCompile.SaveInfo.Information,
                    Object = property.Map.GetMapObject(),
                    TableIndex=saveCompile.SaveInfo.TableIndex
                }
            };
            ConvertDeleteMap(mapSaveCompiler, GetDeleteMapWhere(saveCompile, property, @where));
        }

        /// <summary>
        /// 得到map删除条件
        /// </summary>
        /// <param name="saveCompiler"></param>
        /// <param name="property"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual string GetDeleteMapWhere(SaveCompilerInfo saveCompiler, OrmPropertyInfo property,string where)
        {
            var sbwhere = new StringBuilder();
            sbwhere.AppendFormat(" where {0} in ( select ", property.Map.MapObjectProperty.FieldName);
            sbwhere.AppendFormat("{0} from ",property.Map.ObjectProperty.FieldName);
            sbwhere.AppendFormat("{0} {1} ", saveCompiler.SaveInfo.SetTableName, where);
            AppendDefaultWhere(sbwhere,property.Map.GetMapObject());
            return sbwhere.ToString();
        }

        /// <summary>
        /// 添加map删除语句
        /// </summary>
        /// <param name="saveCompiler"></param>
        /// <param name="property"></param>
        /// <param name="where"></param>
        protected virtual void AppendDeleteMapSql(SaveCompilerInfo saveCompiler, OrmPropertyInfo property, string where)
        {
            saveCompiler.Command.CommandText = string.Format("{0};{1}", GetDeleteMapSql(saveCompiler, property, @where), saveCompiler.Command.CommandText);
        }

        /// <summary>
        /// 得到删除map的语句
        /// </summary>
        /// <param name="saveCompiler"></param>
        /// <param name="property"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual string GetDeleteMapSql(SaveCompilerInfo saveCompiler, OrmPropertyInfo property, string where)
        {
            var sql = new StringBuilder();
            var mapObject = property.Map.GetMapObject();
            var tableName = saveCompiler.SaveInfo.GetSetTableName(mapObject);
            sql.Append(string.IsNullOrEmpty(property.Map.GetMapObject().RemoveMark)
                           ? string.Format("delete from {0}  ",tableName )
                           : string.Format("update {0} set {1}  ", tableName,
                                           property.Map.GetMapObject().RemoveMark));
            sql.AppendFormat("where {0} in ( select ", property.Map.MapObjectProperty.FieldName);
            sql.AppendFormat("{0} from ", property.Map.ObjectProperty.FieldName);
            sql.AppendFormat("{0} {1}) ", saveCompiler.SaveInfo.SetTableName, where);
            return sql.ToString();
        }
        #endregion

        #region 转换还原Map

        /// <summary>
        /// 还原map
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        protected virtual void ConvertRestoreMap(SaveCompilerInfo saveCompile, string where)
        {
            foreach (var p in saveCompile.SaveInfo.Object.Properties)
            {
                if (p.Map != null)
                {
                    SelectRestoreMap(saveCompile, where, p);
                }
            }
        }

        /// <summary>
        /// 选择还原Map方式
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        /// <param name="property"></param>
        protected virtual void SelectRestoreMap(SaveCompilerInfo saveCompile, string where, OrmPropertyInfo property)
        {
            if (property.Map == null || !property.Map.IsRestore && !saveCompile.IsInMap(property)) return;
            saveCompile.MapProperties.Add(property);
            AppendRestoreMapSql(saveCompile, property, @where);
            var mapSaveCompiler = new SaveCompilerInfo
            {
                Command = saveCompile.Command,
                MapProperties = saveCompile.MapProperties,
                  SaveInfo = new SaveInfo
                {
                    Entity = saveCompile.SaveInfo.Entity,
                    Information = saveCompile.SaveInfo.Information,
                    Object = property.Map.GetMapObject()
                }
            };
            ConvertRestoreMap(mapSaveCompiler, GetRestoreMapWhere(saveCompile, property, @where));
        }

        /// <summary>
        /// 得到map删除条件
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="property"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual string GetRestoreMapWhere(SaveCompilerInfo saveCompile, OrmPropertyInfo property, string where)
        {
            var sbwhere = new StringBuilder();
            sbwhere.AppendFormat(" where {0} in ( select ", property.Map.MapObjectProperty.FieldName);
            sbwhere.AppendFormat("{0} from ", property.Map.ObjectProperty.FieldName);
            sbwhere.AppendFormat("{0} {1} ", saveCompile.SaveInfo.SetTableName, where);
            AppendDefaultWhere(sbwhere, property.Map.GetMapObject());
            return sbwhere.ToString();
        }

        /// <summary>
        /// 添加map删除语句
        /// </summary>
        /// <param name="saveCompiler"></param>
        /// <param name="property"></param>
        /// <param name="where"></param>
        protected virtual void AppendRestoreMapSql(SaveCompilerInfo saveCompiler, OrmPropertyInfo property, string where)
        {
            saveCompiler.Command.CommandText = string.Format("{0};{1}", GetRestoreMapSql(saveCompiler, property, @where), saveCompiler.Command.CommandText);
        }

        /// <summary>
        /// 得到删除map的语句
        /// </summary>
        /// <param name="saveCompiler"></param>
        /// <param name="property"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected virtual string GetRestoreMapSql(SaveCompilerInfo saveCompiler, OrmPropertyInfo property, string where)
        {
            var sql = new StringBuilder();
            var mapObject = property.Map.GetMapObject();
            var tableName = saveCompiler.SaveInfo.GetSetTableName(mapObject);
            sql.AppendFormat("update {0} set {1} where ", tableName, property.Map.GetMapObject().RestoreMark);
            sql.AppendFormat("{0} in ( select ", property.Map.MapObjectProperty.FieldName);
            sql.AppendFormat("{0} from ", property.Map.ObjectProperty.FieldName);
            sql.AppendFormat("{0} {1}) ", saveCompiler.SaveInfo.SetTableName, where);
            return sql.ToString();
        }
        #endregion

        #region 设置条件和参数

    
        /// <summary>
        /// 添加主键条件
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        protected virtual void AppendPrimaryWhere(SaveCompilerInfo saveCompile, StringBuilder where)
        {
            object pvalue = saveCompile.SaveInfo.Entity.GetProperty(saveCompile.SaveInfo.Object.PrimaryProperty.PropertyName);
            if(pvalue==null) return;
            const string pname = "where_IsUnique";
            where.AppendFormat("{0}={1}{2}", saveCompile.SaveInfo.Object.PrimaryProperty.PropertyName, ParamterFlag, pname);
            AddParamter(saveCompile.Command, saveCompile.SaveInfo.Object.PrimaryProperty, pname, pvalue);
        }
        /// <summary>
        /// 设置默认查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="obj"></param>
        protected virtual void AppendDefaultWhere(StringBuilder where, OrmObjectInfo obj)
        {
            if (where.Length > 0) where.Append(")");
            if (string.IsNullOrEmpty(obj.SetDefaultWhere))//设置默认查询
                return;
            string key = where.Length > 0 ? "and" : "where";
            where.AppendFormat(" {0} {1}", key, obj.SetDefaultWhere);
        }

        /// <summary>
        /// 拼接自定义条件
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="where"></param>
        protected virtual void AppendCustomerWhere(SaveCompilerInfo saveCompile, StringBuilder where)
        {
            var whereCompile = new WhereCompilerInfo(saveCompile.SaveInfo.Object,
                                                     saveCompile.SaveInfo.Information.WhereExp, new TableInfo { Joins = new Dictionary<string, JoinInfo>() },
                                                     new StringBuilder(),true,saveCompile){Query=new QueryInfo{Object=saveCompile.SaveInfo.Object,Parameters = saveCompile.SaveInfo.Information.Parameters } };

            WhereCompiler.Translate(whereCompile);
            if (whereCompile.Query.SqlParameters != null)
            {
                foreach (var sqlParameter in whereCompile.Query.SqlParameters)
                {
                    AddParamter(saveCompile.Command,sqlParameter.Key,sqlParameter.Value);
                }
            }
            where.Append(whereCompile.Builder);
            saveCompile.IsSaveParameters = true;
        }

        #endregion

        #region 执行

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <returns></returns>
        protected virtual int Execute(SaveCompilerInfo saveCompile)
        {
            if (string.IsNullOrEmpty(saveCompile.Command.CommandText)) return 0;
            SaveParameters(saveCompile);
            var rev = saveCompile.Command.ExecuteNonQuery();
            if (saveCompile.SaveInfo.Information.SaveType == SaveType.Modify && saveCompile.SaveInfo.IsSetVersion && rev<=0)
            {
                throw new VersionException("Version Expired");
            }
            FillKey(saveCompile, true);
            saveCompile.Command.Parameters.Clear();
            saveCompile.Command.CommandText = null;
            return rev;
        }
        /// <summary>
        /// 存储参数
        /// </summary>
        /// <param name="saveCompile"></param>
        protected virtual void SaveParameters(SaveCompilerInfo saveCompile)
        {
            if (saveCompile.SaveInfo.Information.Parameters == null || saveCompile.IsSaveParameters) return;
            foreach (var parameter in saveCompile.SaveInfo.Information.Parameters)
            {
                AddParamter(saveCompile.Command, parameter.Key, parameter.Value);
            }
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="property"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected virtual void AddParamter(DbCommand command, OrmPropertyInfo property,string name,object value)
        {
            command.Parameters.Add(value);
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected virtual  void AddParamter(DbCommand command, string name, object value)
        {
            command.Parameters.Add(value);
        }

        /// <summary>
        /// 填充主键
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <param name="isExecute"></param>
        protected virtual void FillKey(SaveCompilerInfo saveCompile, bool isExecute)
        {
            if (!IsFillKey(saveCompile, isExecute)) return;
            object pValue = GetKey(saveCompile);
            saveCompile.SaveInfo.Entity.SetProperty(saveCompile.SaveInfo.Object.PrimaryProperty.PropertyName, pValue);
            if (saveCompile.SaveInfo.Information.Properties != null)
                saveCompile.SaveInfo.Information.SetProperty(saveCompile.SaveInfo.Object.PrimaryProperty.PropertyName);
        }

        /// <summary>
        /// 得到主键
        /// </summary>
        /// <param name="saveCompile"></param>
        /// <returns></returns>
        protected virtual object GetKey(SaveCompilerInfo saveCompile)
        {
            return Key.GetKey(saveCompile.SaveInfo.Entity.GetType().FullName);
        }

        /// <summary>
        /// 判断是否填充主键
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsFillKey(SaveCompilerInfo saveCompile, bool isExecute)
        {
            if (saveCompile.SaveInfo.Information.SaveType != SaveType.Add) return false;
            if (!isExecute && !saveCompile.SaveInfo.Object.PrimaryProperty.IsIdentityKey)
            {
                var propertyType = saveCompile.SaveInfo.Entity.GetType().GetProperties()
                   .FirstOrDefault(it => it.Name.Equals(saveCompile.SaveInfo.Object.PrimaryProperty.PropertyName));
                if (propertyType == null) return false;
                var propertyValue = propertyType.GetValue(saveCompile.SaveInfo.Entity, null);
                if (propertyType.PropertyType.IsValueType && !propertyValue.Equals(0)
                    || !propertyType.PropertyType.IsValueType && 
                    propertyValue != null && !"".Equals(propertyValue)) return false;
                return true;

            }
            return isExecute && saveCompile.SaveInfo.Object.PrimaryProperty.IsIdentityKey;
        }

        #endregion

    }
}

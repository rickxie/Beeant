using System;
using System.Linq;
using System.Xml;
using System.IO;

namespace Winner.Persistence.Relation
{
    /// <summary>
    /// 加载ORM
    /// </summary>
    public class XmlOrm: Orm
    {
        #region 属性
        private string _configFile;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFile
        {
            get { return _configFile; }
            set
            {
                _configFile = value;
                LoadConfig();
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 无参数
        /// </summary>
        public XmlOrm()
        { 
        }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        /// <param name="configFile"></param>
        public XmlOrm(string configFile)
        {
            ConfigFile = configFile;
        }
        #endregion

        #region 加载配置文件
        /// <summary>
        /// 加载配置文件
        /// </summary>
        protected virtual void LoadConfig()
        {
            XmlDocument doc = GetXmlDocument();
            FillOrmObjectByXmlNode(doc);
            FillOrmPropertyByXml(doc);
            SetOrmObjectCacheTime();
        }
        /// <summary>
        /// 得到XmlDocument
        /// </summary>
        /// <returns></returns> 
        protected virtual XmlDocument GetXmlDocument()
        {
            string filename = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, ConfigFile);
            return GetXmlDocument(filename);
        }
        /// <summary>
        ///  得到XmlDocument
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual XmlDocument GetXmlDocument(string fileName)
        {
            var doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }
        #endregion

        #region 填充对象信息
        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual XmlNodeList GetObjectNodesByXmlPath(string path)
        {
            string filename = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);
            var doc = GetXmlDocument(filename);
            var nodes = doc.SelectNodes("/configuration/Persistence/XmlOrm/Map/Object");
            return nodes;
        }
        /// <summary>
        /// 填充对象
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void FillOrmObjectByXmlNode(XmlDocument doc)
        {
            XmlNodeList ormPaths = doc.SelectNodes("/configuration/Persistence/XmlOrm/Model/Path");
             if (ormPaths == null || ormPaths.Count == 0)
                 return;
             foreach (XmlNode node in ormPaths)
             {
                 if (node.Attributes == null) continue;
                 XmlNodeList xnlObjects = GetObjectNodesByXmlPath(node.Attributes["Path"].Value);
                 if (xnlObjects == null || xnlObjects.Count == 0)
                     continue;
                 FillOrmObjectByObjectXmlNodes(xnlObjects);
             }
        }

        /// <summary>
        /// 填充对象信息根据节点
        /// </summary>
        /// <param name="nodes"></param>
        protected virtual void FillOrmObjectByObjectXmlNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                var info = new OrmObjectInfo();
                FillOrmObjectBaseByXmlNode(info, node);
                FillOrmObjectWhereByXmlNode(info, node);
                AddOrm(info);
            }
        }
 

        #endregion

        #region 填充对象表信息和同步信息
        /// <summary>
        /// 填充表
        /// </summary>
        /// <param name="info"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmObjectBaseByXmlNode(OrmObjectInfo info, XmlNode node)
        {
            if (node.Attributes == null) return;
            info.ObjectName = node.Attributes["ObjectName"] != null ? node.Attributes["ObjectName"].Value : null;
            info.Key = node.Attributes["KeyName"] != null ? node.Attributes["KeyName"].Value : null;
            info.GetTableName = node.Attributes["GetTableName"] != null ? node.Attributes["GetTableName"].Value.ToLower() : null;
            info.GetDataBase = node.Attributes["GetDataBase"] != null ? node.Attributes["GetDataBase"].Value.ToLower() : null; 
            info.NickObjectName = node.Attributes["NickObjectName"] != null ? node.Attributes["NickObjectName"].Value : null;
            info.SetTableName = node.Attributes["SetTableName"] == null ? info.GetTableName : node.Attributes["SetTableName"].Value.ToLower();
            info.IsCache = node.Attributes["IsCache"] != null && Convert.ToBoolean(node.Attributes["IsCache"].Value);
            info.CacheTime = node.Attributes["CacheTime"] == null ?1200:Convert.ToInt64(node.Attributes["CacheTime"].Value);
            info.SetDataBase = node.Attributes["SetDataBase"] == null ? info.GetDataBase : node.Attributes["SetDataBase"].Value.ToLower();
            info.RouteName = node.Attributes["RouteName"] == null ? "" : node.Attributes["RouteName"].Value.ToLower();
        }
        protected virtual void FillOrmObjectWhereByXmlNode(OrmObjectInfo info, XmlNode node)
        {
            if (node.Attributes != null && node.Attributes["SetDefaultWhere"] != null)
                info.SetDefaultWhere = node.Attributes["SetDefaultWhere"].Value.Trim();
            if (node.Attributes != null && node.Attributes["GetDefaultWhere"] != null)
                info.GetDefaultWhere = node.Attributes["GetDefaultWhere"].Value.Trim();
            if (node.Attributes != null && node.Attributes["RemoveMark"] != null)
                info.RemoveMark = node.Attributes["RemoveMark"].Value.Trim();
            if (node.Attributes != null && node.Attributes["RestoreMark"] != null)
                info.RestoreMark = node.Attributes["RestoreMark"].Value.Trim();
        }
    
      

        #endregion

        #region 填充属性
        /// <summary>
        /// 填充属性
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void FillOrmPropertyByXml(XmlDocument doc)
        {
            XmlNodeList ormPaths = doc.SelectNodes("/configuration/Persistence/XmlOrm/Model/Path");
            if (ormPaths == null || ormPaths.Count == 0)
                return;
            FillOrmPropertyByXmlPath(ormPaths);
            FillOrmPropertyMapByXmlPath(ormPaths);
        }
        /// <summary>
        ///填充属性
        /// </summary>
        /// <param name="ormPaths"></param>
        protected virtual void FillOrmPropertyByXmlPath(XmlNodeList ormPaths)
        {
            foreach (XmlNode node in ormPaths)
            {
                if (node.Attributes == null) continue;
                XmlNodeList xnlObjects = GetObjectNodesByXmlPath(node.Attributes["Path"].Value);
                FillOrmPropertyByObjectXmlNodes(xnlObjects);
            }
        }
        /// <summary>
        /// 填充Map属性
        /// </summary>
        /// <param name="ormPaths"></param>
        protected virtual void FillOrmPropertyMapByXmlPath(XmlNodeList ormPaths)
        {
            foreach (XmlNode node in ormPaths)
            {
                if (node.Attributes == null) continue;
                XmlNodeList xnlObjects = GetObjectNodesByXmlPath(node.Attributes["Path"].Value);
                FillOrmPropertyMapByObjectXmlNodes(xnlObjects);
            }
        }

        /// <summary>
        /// 填充属性基本信息
        /// </summary>
        /// <param name="xnlObjects"></param>
        protected virtual void FillOrmPropertyByObjectXmlNodes(XmlNodeList xnlObjects)
        {
            foreach (XmlNode xno in xnlObjects)
            {
                if (xno.ChildNodes.Count == 0)
                    continue;
                OrmObjectInfo info = GetOrmObjectByXmlNode(xno);
                if (info == null)
                    continue;
                XmlNodeList nodes = xno.SelectNodes("Property");
                FillOrmPropertyByPropertyXmlNodes(info,nodes);
            }
        }
        /// <summary>
        /// 根据property节点填充填充属性
        /// </summary>
        /// <param name="info"></param>
        /// <param name="nodes"></param>
        protected virtual void FillOrmPropertyByPropertyXmlNodes(OrmObjectInfo info, XmlNodeList nodes)
        {
            foreach (XmlNode nd in nodes)
            {
                var pi = new OrmPropertyInfo {ObjectName = info.ObjectName,Orms=Orms};
                info.Properties.Add(pi);
                FillOrmPropertyBaseByXmlNode(info,pi, nd);
                FillOrmPropertyDefaultByXmlNode(pi, nd);
                FillOrmPropertyOperatorModeByXmlNode(pi, nd);
            }
        }
        /// <summary>
        /// 根据objects节点填充map
        /// </summary>
        /// <param name="xnlObjects"></param>
        protected virtual void FillOrmPropertyMapByObjectXmlNodes(XmlNodeList xnlObjects)
        {
            foreach (XmlNode node in xnlObjects)
            {
                if (node.ChildNodes.Count == 0)
                    continue;
                OrmObjectInfo info = GetOrmObjectByXmlNode(node);
                if (info == null)
                    continue;
                XmlNodeList nodes = node.SelectNodes("Property");
                FillOrmPropertyMapByPropertyXmlNodes(info, nodes);
            }
        }
        /// <summary>
        /// 根据property节点填充map
        /// </summary>
        /// <param name="info"></param>
        /// <param name="nodes"></param>
        protected virtual void FillOrmPropertyMapByPropertyXmlNodes(OrmObjectInfo info,XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                OrmPropertyInfo pi = GetOrmPropertyByXmlNodeAndOrmObject(node, info);
                if (pi == null)
                {
                    continue;
                }
                FillOrmPropertyMapByXmlNode(info,pi, node);
            }
        }

        /// <summary>
        /// 填充属性基本信息
        /// </summary>
        /// <param name="ormObject"></param>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyBaseByXmlNode(OrmObjectInfo ormObject, OrmPropertyInfo property, XmlNode node)
        {
            if (node.Attributes == null) return;
            property.PropertyName = node.Attributes["PropertyName"].Value;
            property.NickPropertyName = node.Attributes["NickPropertyName"] == null ? null : node.Attributes["NickPropertyName"].Value.Split(',');
            property.FieldName = node.Attributes["FieldName"] == null ? property.PropertyName : node.Attributes["FieldName"].Value;
            property.FieldType = node.Attributes["FieldType"] == null ? null : node.Attributes["FieldType"].Value;
            property.PropertyType = node.Attributes["PropertyType"] == null ? null : Type.GetType(node.Attributes["PropertyType"].Value);
            property.Length = node.Attributes["Length"] == null ? 0 : Convert.ToInt32(node.Attributes["Length"].Value);
            property.IsPrimaryKey = node.Attributes["IsPrimaryKey"] != null && Convert.ToBoolean(node.Attributes["IsPrimaryKey"].Value);
            property.IsIdentityKey = node.Attributes["IsIdentityKey"] != null && Convert.ToBoolean(node.Attributes["IsIdentityKey"].Value);
            property.IsCustom = node.Attributes["IsCustom"] != null && Convert.ToBoolean(node.Attributes["IsCustom"].Value);
            property.IsOptimisticLocker = node.Attributes["IsOptimisticLocker"] != null && Convert.ToBoolean(node.Attributes["IsOptimisticLocker"].Value);
            property.IsVersion = node.Attributes["IsVersion"] != null && Convert.ToBoolean(node.Attributes["IsVersion"].Value);
            ormObject.PrimaryProperty = property.IsPrimaryKey ? property : ormObject.PrimaryProperty;
            ormObject.VersionProperty = property.IsVersion ? property : ormObject.VersionProperty;
        }
        /// <summary>
        /// 填充默认信息
        /// </summary>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyDefaultByXmlNode(OrmPropertyInfo property, XmlNode node)
        {
            FillOrmPropertyUnAddValueByXmlNode(property, node);
            FillOrmPropertyUnModifyValueByXmlNode(property, node);
            FillOrmPropertyReadNullValueByXmlNode(property, node);
        }
        /// <summary>
        /// 填充不添加的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyUnAddValueByXmlNode(OrmPropertyInfo property, XmlNode node)
        {
            if (node.Attributes == null || node.Attributes["UnAddValue"] == null || property.PropertyType==null) return;
            property.HasUnAddValue = true;
            if (!node.Attributes["UnAddValue"].Value.Equals("null"))
            {
                property.UnAddValue = Convert.ChangeType(node.Attributes["UnAddValue"].Value, property.PropertyType);
            }
        }
        /// <summary>
        /// 填充不更新的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyUnModifyValueByXmlNode(OrmPropertyInfo property, XmlNode node)
        {
            if (node.Attributes == null || node.Attributes["UnModifyValue"] == null) return;
            property.HasUnModifyValue = true;
            if (!node.Attributes["UnModifyValue"].Value.Equals("null"))
            {
                property.UnModifyValue = Convert.ChangeType(node.Attributes["UnModifyValue"].Value, property.PropertyType);
            }
        }
        /// <summary>
        /// 填充读取的默认值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyReadNullValueByXmlNode(OrmPropertyInfo property, XmlNode node)
        {
            if (node.Attributes == null || node.Attributes["ReadNullValue"] == null) return;
            property.HasReadNullValue = true;
            if (!node.Attributes["ReadNullValue"].Value.Equals("null"))
            {
                property.ReadNullValue = Convert.ChangeType(node.Attributes["ReadNullValue"].Value, property.PropertyType);
            }
        }

        /// <summary>
        /// 填充操作属性
        /// </summary>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyOperatorModeByXmlNode(OrmPropertyInfo property, XmlNode node)
        {
            if (node.Attributes != null && node.Attributes["OperatorMode"] != null)
            {
                if (node.Attributes["OperatorMode"].Value.Equals("None"))
                    return;
                string[] values = node.Attributes["OperatorMode"].Value.Split('|');
                FillOrmPropertyOperatorModeByValues(property, values);
            }
            else
            {
                FillOrmPropertyOperatorDefalutMode(property);
            }
        }
       /// <summary>
       /// 根据值设置属性操作
       /// </summary>
       /// <param name="property"></param>
       /// <param name="values"></param>
        protected virtual void FillOrmPropertyOperatorModeByValues(OrmPropertyInfo property, string[] values)
        {
            foreach (var s in values)
            {
                switch (s)
                {
                    case "Add": property.AllowAdd = true; break;
                    case "Modify": property.AllowModify = true; break;
                    case "Read": property.AllowRead = true; break;
                    case "Remove": property.AllowRemove = true; break;
                    case "Restore": property.AllowRestore = true; break;
                }
            }
        }
        /// <summary>
        /// 设置属性默认的操作
        /// </summary>
        /// <param name="property"></param>
        protected virtual void FillOrmPropertyOperatorDefalutMode(OrmPropertyInfo property)
        {
            property.AllowAdd = true;
            property.AllowModify = !property.IsPrimaryKey;
            property.AllowRead = true;
        }
        #endregion

        #region 填充属性map属性

        /// <summary>
        /// 填充属性map
        /// </summary>
        /// <param name="ormObject"></param>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyMapByXmlNode(OrmObjectInfo ormObject, OrmPropertyInfo property,XmlNode node )
        {
            XmlNode xnMap = node.SelectSingleNode("MapObject");
            if (xnMap != null)
            {
                property.Map = new OrmMapInfo { Orms = Orms, MapObjectName = xnMap.Attributes["Name"].Value };
                FillOrmPropertyMapKeyByXmlNode(ormObject,property, xnMap);
                FillOrmPropertyMapOperateByXmlNode(property, xnMap);
            }
        }
        

        /// <summary>
        /// 填充map基本属性
        /// </summary>
        /// <param name="ormObject"></param>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyMapKeyByXmlNode(OrmObjectInfo ormObject, OrmPropertyInfo property, XmlNode node)
        {
            if (node == null || node.Attributes == null) return;
            property.Map.ObjectProperty = ormObject.GetPropertyInfo(node.Attributes["ObjectProperty"].Value);
            property.Map.MapObjectProperty = property.Map.GetMapObject().GetPropertyInfo(node.Attributes["MapObjectProperty"].Value);
        }
        /// <summary>
        /// 填充map操作
        /// </summary>
        /// <param name="property"></param>
        /// <param name="node"></param>
        protected virtual void FillOrmPropertyMapOperateByXmlNode(OrmPropertyInfo property,XmlNode node )
        {
            if (node.Attributes == null) return;
            property.Map.IsAdd = node.Attributes["IsAdd"] != null && Convert.ToBoolean(node.Attributes["IsAdd"].Value);
            property.Map.IsRemove = node.Attributes["IsRemove"] != null && Convert.ToBoolean(node.Attributes["IsRemove"].Value);
            property.Map.IsModify = node.Attributes["IsModify"] != null && Convert.ToBoolean(node.Attributes["IsModify"].Value);
            property.Map.IsRestore = node.Attributes["IsRestore"] != null && Convert.ToBoolean(node.Attributes["IsRestore"].Value);
            property.Map.IsRemove = node.Attributes["IsRemove"] != null && Convert.ToBoolean(node.Attributes["IsRemove"].Value);
            property.Map.IsGreedyLoad = node.Attributes["IsGreedyLoad"] != null && Convert.ToBoolean(node.Attributes["IsGreedyLoad"].Value);
            property.Map.IsLazyLoad = node.Attributes["IsLazyLoad"] != null && Convert.ToBoolean(node.Attributes["IsLazyLoad"].Value);
            property.Map.IsRemote = node.Attributes["IsRemote"] != null && Convert.ToBoolean(node.Attributes["IsRemote"].Value);
            property.Map.IsRemoveCache = node.Attributes["IsRemoveCache"] != null && Convert.ToBoolean(node.Attributes["IsRemoveCache"].Value);
            property.Map.RemoteName = node.Attributes["RemoteName"] == null ? "" : node.Attributes["RemoteName"].Value.ToLower();
            if (node.Attributes["MapType"] != null)property.Map.MapType = (OrmMapType)Enum.Parse(typeof(OrmMapType), node.Attributes["MapType"].Value);
        }
        #endregion

        #region 得到对象和属性

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual OrmObjectInfo GetOrmObjectByXmlNode(XmlNode node)
        {
            var name=node.Attributes != null && node.Attributes["ObjectName"]!=null? node.Attributes["ObjectName"].Value:"";
            if (Orms.ContainsKey(name))
                return Orms[name];
            return null;
        }

        /// <summary>
        /// 返回属性
        /// </summary>
        /// <param name="node"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual OrmPropertyInfo GetOrmPropertyByXmlNodeAndOrmObject(XmlNode node, OrmObjectInfo info)
        {
            return info.Properties.FirstOrDefault(p => node.Attributes != null && p.PropertyName.Equals(node.Attributes["PropertyName"].Value));
        }

        #endregion

        #region 同步缓存机制
        /// <summary>
        /// 设置缓存时间
        /// </summary>
        protected virtual void SetOrmObjectCacheTime()
        {
            foreach (var orm in Orms)
            {
                if(!orm.Value.IsCache)continue;
                SetOrmObjectCacheTime(orm.Value);
            }
        }
        /// <summary>
        /// 同步缓存机制
        /// </summary>
        protected virtual void SetOrmObjectCacheTime(OrmObjectInfo orm)
        {
            var ormMaps =
                orm.Properties.Where(it => it.Map != null && !it.Map.IsGreedyLoad && it.Map.IsLazyLoad)
                   .Select(it => it.Map);
            foreach (var ormMap in ormMaps)
            {
                ormMap.GetMapObject().CacheTime = orm.CacheTime;
                ormMap.GetMapObject().IsCache = orm.IsCache;
                SetOrmObjectCacheTime(ormMap.GetMapObject());
            }
        }

        #endregion

    }
}

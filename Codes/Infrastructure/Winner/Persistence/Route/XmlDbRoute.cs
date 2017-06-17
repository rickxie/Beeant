using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Winner.Persistence.Route
{
    /// <summary>
    /// 加载ORM
    /// </summary>
    public class XmlDbRoute: DbRoute
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
        public XmlDbRoute()
        { 
        }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        /// <param name="configFile"></param>
        public XmlDbRoute(string configFile)
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
            LoadDbRoutes(doc);
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
        /// <summary>
        /// 加载DbRoutes
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void LoadDbRoutes(XmlDocument doc)
        {
            XmlNodeList xnPaths = doc.SelectNodes("/configuration/Persistence/XmlDbRoute/Path");
            if (xnPaths == null || xnPaths.Count == 0)
                return;
            var dbRoutes = new Dictionary<string, DbRouteInfo>();
            foreach (XmlNode node in xnPaths)
            {
                if (node.Attributes == null)
                    return ;
                string fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                         node.Attributes["Path"].Value);
                XmlDocument pathDoc = GetXmlDocument(fileName);
                LoadDbRouteByXml(dbRoutes, pathDoc);
            }
            DbRoutes = dbRoutes;
        }
        #endregion

        #region 得到配置信息

        /// <summary>
        /// 得到数据库信息
        /// </summary>
        /// <param name="dbRoutes"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected virtual void LoadDbRouteByXml(IDictionary<string, DbRouteInfo> dbRoutes,XmlDocument doc)
        {
            XmlNodeList nodes = doc.SelectNodes("/configuration/Persistence/XmlDbRoute/Info");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var dbroute = new DbRouteInfo { Rules = new List<RuleInfo>(), Shardings = new List<ShardingInfo>() };
                    if (node.Attributes == null) return;
                    dbroute.Name = node.Attributes["Name"].Value.ToLower();
                    if (node.Attributes["TopCount"] != null)
                        dbroute.TopCount = Convert.ToInt32(node.Attributes["TopCount"].Value);
                    if (node.Attributes["IsReturnAllShardings"] != null)
                        dbroute.IsReturnAllShardings = Convert.ToBoolean(node.Attributes["IsReturnAllShardings"].Value);
                    if (node.Attributes["IsMapTableAutoSharding"] != null)
                        dbroute.IsMapTableAutoSharding = Convert.ToBoolean(node.Attributes["IsMapTableAutoSharding"].Value);
                    if (node.Attributes["ClassName"] != null)
                    {
                        var obj = CreateClass(node.Attributes["ClassName"].Value);
                        if (obj != null)
                        {
                            if (node.Attributes["GetQueryShardingHandle"] != null)
                                dbroute.GetQueryShardingHandle = (Func<QueryInfo, IList<ShardingInfo>>)Delegate.CreateDelegate(typeof(Func<QueryInfo, IList<ShardingInfo>>), obj, node.Attributes["GetQueryShardingHandle"].Value);
                            if (node.Attributes["GetSaveShardingHandle"] != null)
                                dbroute.GetSaveShardingHandle = (Func<object, ShardingInfo>)Delegate.CreateDelegate(typeof(Func<object, ShardingInfo>), obj, node.Attributes["GetSaveShardingHandle"].Value);
                        }
                   }
                    LoadRulesByXmlNode(dbroute, node);
                    LoadShardingsByXmlNode(dbroute, node);
                    if (dbRoutes.ContainsKey(dbroute.Name))
                        dbRoutes.Remove(dbroute.Name);
                    dbRoutes.Add(dbroute.Name, dbroute);
                }
            }
        
        }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        protected virtual object CreateClass(string className)
        {
            var t = Type.GetType(className);
            if (t == null) return null;
            return Activator.CreateInstance(t);
        }
        /// <summary>
        /// 根据节点得到OrmDataBase
        /// </summary>
        /// <param name="dbRoute"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual void LoadRulesByXmlNode(DbRouteInfo dbRoute, XmlNode node)
        {
            var nodes = node.SelectNodes("Rule");
            if(nodes==null)return;
            foreach (XmlNode nd in nodes)
            {
                var rule = new RuleInfo();
                rule.IsHash = nd.Attributes["IsHash"] != null && Convert.ToBoolean(nd.Attributes["IsHash"].Value);
                rule.IsSave = nd.Attributes["IsSave"] != null && Convert.ToBoolean(nd.Attributes["IsSave"].Value);
                rule.PropertyName = nd.Attributes["PropertyName"] != null ? nd.Attributes["PropertyName"].Value : "";
                dbRoute.Rules.Add(rule);
            }
        }
        /// <summary>
        /// 根据节点得到OrmDataBase
        /// </summary>
        /// <param name="dbRoute"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual void LoadShardingsByXmlNode(DbRouteInfo dbRoute, XmlNode node)
        {
            var nodes = node.SelectNodes("Sharding");
            if (nodes == null) return;
            foreach (XmlNode nd in nodes)
            {
                var sharding = new ShardingInfo {ShardProperties=new List<ShardPropertyInfo>()};
                sharding.GetDataBase = nd.Attributes["GetDataBase"] != null
                                           ? nd.Attributes["GetDataBase"].Value.ToLower()
                                           : "";
                sharding.SetDataBase = nd.Attributes["SetDataBase"] != null
                                           ? nd.Attributes["SetDataBase"].Value.ToLower()
                                           : "";
                sharding.TableIndex = nd.Attributes["TableIndex"] != null
                                           ? nd.Attributes["TableIndex"].Value.ToLower()
                                           : "";
                sharding.MaxTableIndex = nd.Attributes["MaxTableIndex"] != null
                                       ? nd.Attributes["MaxTableIndex"].Value.ToLower()
                                       : "";
                sharding.TableTag= nd.Attributes["TableTag"] != null
                                       ? nd.Attributes["TableTag"].Value.ToLower()
                                       : "";
                sharding.IsWrite = nd.Attributes["IsWrite"] == null || Convert.ToBoolean(nd.Attributes["IsWrite"].Value);
                if (nd.Attributes["TableStepType"] != null)
                    sharding.TableStepType = (TableStepType)Enum.Parse(typeof(TableStepType), nd.Attributes["TableStepType"].Value);
                if (nd.Attributes["TableStep"] != null)
                    sharding.TableStep = Convert.ToInt32(nd.Attributes["TableStep"].Value);
                if (nd.Attributes["GetDataBaseTableCount"] != null)
                    sharding.GetDataBaseTableCount = Convert.ToInt32(nd.Attributes["GetDataBaseTableCount"].Value);
                if (nd.Attributes["SetDataBaseTableCount"] != null)
                    sharding.SetDataBaseTableCount = Convert.ToInt32(nd.Attributes["SetDataBaseTableCount"].Value);
                LoadShardingPropertiesByXmlNode(sharding, nd);
                dbRoute.Shardings.Add(sharding);
            }
        }
        /// <summary>
        /// 加载分片属性
        /// </summary>
        /// <param name="sharding"></param>
        /// <param name="node"></param>
        protected virtual void LoadShardingPropertiesByXmlNode(ShardingInfo sharding, XmlNode node)
        {
            var nodes = node.SelectNodes("Property");
            if (nodes == null) return;
            foreach (XmlNode nd in nodes)
            {
                var shardProperty = new ShardPropertyInfo();
                shardProperty.PropertyName = nd.Attributes["PropertyName"] != null
                                           ? nd.Attributes["PropertyName"].Value
                                          : "";
                shardProperty.Tag = nd.Attributes["Tag"] != null
                                           ? nd.Attributes["Tag"].Value
                                          : "";
                if (nd.Attributes["StartValue"] != null)
                    shardProperty.StartValue = Convert.ToInt64(nd.Attributes["StartValue"].Value);
                if (nd.Attributes["EndValue"] != null)
                    shardProperty.EndValue = Convert.ToInt64(nd.Attributes["EndValue"].Value);
                if (nd.Attributes["FixedValue"] != null)
                    shardProperty.FixedValue = nd.Attributes["FixedValue"].Value;
                if (nd.Attributes["DateFormat"] != null)
                    shardProperty.DateFormat = nd.Attributes["DateFormat"].Value;
                if (nd.Attributes["ShardingType"] != null)
                    shardProperty.ShardingType = (ShardingType)Enum.Parse(typeof(ShardingType), nd.Attributes["ShardingType"].Value);
                sharding.ShardProperties.Add(shardProperty);
            }
        }
        #endregion

    }
}

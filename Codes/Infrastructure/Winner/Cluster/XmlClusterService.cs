using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Winner.Cluster
{
    /// <summary>
    /// 加载ORM
    /// </summary>
    public class XmlClusterService : ClusterService
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
        public XmlClusterService()
        { 
        }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        /// <param name="configFile"></param>
        public XmlClusterService(string configFile)
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
            LoadHandles(doc);
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
        protected virtual void LoadHandles(XmlDocument doc)
        {
            XmlNodeList xnPaths = doc.SelectNodes("/configuration/Cluster/XmlClusterService/Path");
            if (xnPaths == null || xnPaths.Count == 0)
                return;
            var handles = new Dictionary<string, Action<ClusterArgsInfo>>();
            foreach (XmlNode node in xnPaths)
            {
                if (node.Attributes == null)
                    return ;
                string fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                         node.Attributes["Path"].Value);
                XmlDocument pathDoc = GetXmlDocument(fileName);
                LoadDbRouteByXml(handles, pathDoc);
            }
            Handles = handles;
        }
        #endregion

        #region 得到配置信息

        /// <summary>
        /// 得到数据库信息
        /// </summary>
        /// <param name="handles"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected virtual void LoadDbRouteByXml(Dictionary<string, Action<ClusterArgsInfo>> handles, XmlDocument doc)
        {
            XmlNodeList nodes = doc.SelectNodes("/configuration/Cluster/XmlClusterService/Info");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {

                    if (node.Attributes == null) return;
                    if (handles.ContainsKey(node.Attributes["Name"].Value))
                        continue;
                    if (node.Attributes["ClassName"] == null) continue;
                    var obj = CreateClass(node.Attributes["ClassName"].Value);
                    if (obj == null) continue;
                    if (node.Attributes["Method"] == null) continue;
                    Action<ClusterArgsInfo> action =
                        (Action<ClusterArgsInfo>)
                            Delegate.CreateDelegate(typeof (Action<ClusterArgsInfo>), obj,
                                node.Attributes["Method"].Value);
                    handles.Add(node.Attributes["ClassName"].Value, action);
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
       
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Winner.Wcf;

namespace Winner.Cache.Distributed
{

    public class XmlRemainderCache : RemainderCache
    {
        #region 属性
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 客户端文件
        /// </summary>
        public string ClientFile { get; set; }
        private string _configFile;
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
        public XmlRemainderCache()
        {
     
        }
        /// <summary>
        /// WCF客户端配置文件,虚拟节点配置文件路径
        /// </summary>
        /// <param name="clientFile"></param>
        /// <param name="configFile"></param>
        public XmlRemainderCache(string clientFile, string configFile)
        {
            ClientFile = clientFile;
            ConfigFile = configFile;
            
        }
        #endregion

        #region 加载配置文件
        /// <summary>
        /// 根据配置文件加载
        /// </summary>
        protected virtual void LoadConfig()
        {
            WcfService = new XmlWcfService(ClientFile, ConfigFile, NodeName);
            XmlDocument doc = GetXmlDocument();
            AddNodeByXml(doc, WcfService.EndPoints);
        }
        /// <summary>
        /// 得到XmlDocument
        /// </summary>
        /// <returns></returns>
        protected virtual XmlDocument GetXmlDocument()
        {
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, ConfigFile);
            var doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }

        /// <summary>
        /// 根据XML加载Node属性
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="endPoints"></param>
        protected virtual void AddNodeByXml(XmlDocument doc,IList<EndPointInfo> endPoints)
        {
            XmlNodeList nodes = doc.SelectNodes(NodeName);
            if (nodes == null || nodes.Count == 0) return;
            foreach (XmlNode nd in nodes)
            {
                if (nd.Attributes != null)
                    AddNode(endPoints.FirstOrDefault(it=>it.Name.Equals(nd.Attributes["Name"].Value)));
            }
        }
        #endregion

    }
}

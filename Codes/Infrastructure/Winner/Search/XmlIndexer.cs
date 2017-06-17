using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Winner.Search
{
    public class XmlIndexer : Indexer
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
        public XmlIndexer()
        { 
        }
        public XmlIndexer(string configFile)
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
            LoadStoreIndexsByXml(doc);
        }
        /// <summary>
        /// 得到XmlDocument
        /// </summary>
        /// <returns></returns>
        protected virtual XmlDocument GetXmlDocument()
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, ConfigFile);
            var doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }
        /// <summary>
        /// 得到词库信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected virtual void LoadStoreIndexsByXml(XmlDocument doc)
        {
            var storeIndexs=new Dictionary<string, StoreIndexInfo>();
            XmlNodeList nodes = doc.SelectNodes("/configuration/Search/XmlIndexer/Info");
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var storeDocumentNode = node.SelectSingleNode("StoreDocument");
                    var storeFieldNodes = node.SelectNodes("StoreField");
                    var storeSequences = node.SelectNodes("StoreSequence");
                    var storeIndex = new StoreIndexInfo
                        {
                            Name = node.Attributes["Name"].Value,
                            TopDocumentCount =
                                node.Attributes["TopDocumentCount"] == null
                                    ? 500
                                    : int.Parse(node.Attributes["TopDocumentCount"].Value),
                            StoreDocument = GetStoreDocumentByXmlNode(storeDocumentNode),
                            StoreFields = GetStoreFieldsByXmlNodes(storeFieldNodes),
                            StoreSequences = GetStoreSequencesByXmlNodes(storeSequences)
                        };
                    storeIndexs.Add(storeIndex.Name,storeIndex);
                   
                }
            }
            StoreIndexs = storeIndexs;
        }
        /// <summary>
        /// 得到文档信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual StoreDocumentInfo GetStoreDocumentByXmlNode(XmlNode node)
        {
            var storeDocument = new StoreDocumentInfo();
            if (node != null)
            {
                storeDocument.PageSize = node.Attributes["PageSize"] == null
                                             ? 500
                                             : int.Parse(node.Attributes["PageSize"].Value);
            }
            return storeDocument;
        }
        /// <summary>
        /// 得到文档信息
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected virtual IList<StoreFieldInfo> GetStoreFieldsByXmlNodes(XmlNodeList nodes)
        {
            var storeFields = new List<StoreFieldInfo>();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var storeField = new StoreFieldInfo
                        {
                            Index = int.Parse(node.Attributes["Index"].Value),
                            Name = node.Attributes["Name"].Value,
                            StoreType = (FieldIndexType)Enum.Parse(typeof(FieldIndexType), node.Attributes["StoreType"].Value)
                        };
                    storeFields.Add(storeField);
                }
            }
            return storeFields;
        }
        /// <summary>
        /// 得到文档信息
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected virtual IList<StoreSequenceInfo> GetStoreSequencesByXmlNodes(XmlNodeList nodes)
        {
            var storeSequences = new List<StoreSequenceInfo>();
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var storeSequence = new StoreSequenceInfo
                    {
                        Index = int.Parse(node.Attributes["Index"].Value),
                        Density = double.Parse(node.Attributes["Density"].Value),
                        Percentage = float.Parse(node.Attributes["Percentage"].Value),
                    };
                    storeSequences.Add(storeSequence);
                }
            }
            return storeSequences;
        }
        #endregion
    }
}

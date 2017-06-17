using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Winner.Wcf;

namespace Winner.Storage.Distributed
{
    public class XmlDistributedStore : DistributedStore
    {

        #region 属性
    
        /// <summary>
        /// 客户端文件
        /// </summary>
        public string ClientFile { get; set; }
        public string NodeName { get; set; }
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
        public XmlDistributedStore()
        { 
        }

        /// <summary>
        /// WCF客户端配置文件,虚拟节点配置文件路径
        /// </summary>
        /// <param name="clientFile"></param>
        /// <param name="configFile"></param>
        public XmlDistributedStore(string clientFile, string configFile)
        {
            ClientFile = clientFile;
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
            WcfService = new XmlWcfService(ClientFile, ConfigFile,NodeName);
            LoadStoreByXml(doc);
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
        /// 加载缩略图信息
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void LoadStoreByXml(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("/configuration/Storage/XmlDistributedStore");
            if (node == null || node.Attributes == null)
                return;
            DataServiceGroups = GetDataServiceGroupsByXmlNode(node);
        }
  

        /// <summary>
        /// 根据节点得到缩略图
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual IList<DataServiceGroupInfo> GetDataServiceGroupsByXmlNode(XmlNode node)
        {
            IList<DataServiceGroupInfo> dataServiceGroups = new List<DataServiceGroupInfo>();
            XmlNodeList nodes = node.SelectNodes("DataServiceGroup");
            if (nodes == null || nodes.Count == 0)
                return dataServiceGroups;
            var endPoints = WcfService.EndPoints;
            foreach (XmlNode nd in nodes)
            {
                AddDataServiceByXmlNode(dataServiceGroups, endPoints, nd);
            }
            return dataServiceGroups;
        }

        /// <summary>
        ///  根据节点添加缩略图
        /// </summary>
        /// <param name="dataServiceGroups"></param>
        /// <param name="endPoints"></param>
        /// <param name="node"></param>
        protected virtual void AddDataServiceByXmlNode(IList<DataServiceGroupInfo> dataServiceGroups, IList<EndPointInfo> endPoints, XmlNode node)
        {
            var dsp = new DataServiceGroupInfo{DataServices=new List<DataServiceInfo>()};
            if (node != null && node.Attributes != null)
            {
                dsp.Path = node.Attributes["Path"] == null ? null : node.Attributes["Path"].Value;
                dsp.Name = node.Attributes["Name"] == null ? null : node.Attributes["Name"].Value;
                dsp.IsClose = node.Attributes["IsClose"] != null && Convert.ToBoolean(node.Attributes["IsClose"].Value);
                dsp.Addresses = node.Attributes["Addresses"] == null ? null : node.Attributes["Addresses"].Value.Split(',');
                XmlNodeList dsNodes = node.SelectNodes("DataService");
                if (dsNodes != null && dsNodes.Count > 0)
                {
                    foreach (XmlNode dsNode in dsNodes)
                    {
                        var ds = new DataServiceInfo
                            {
                                Type = dsNode.Attributes["Type"] == null
                                           ? DataServiceType.Master
                                           : (DataServiceType)
                                             Enum.Parse(typeof(DataServiceType), dsNode.Attributes["Type"].Value),
                                EndPoint =
                                    endPoints.FirstOrDefault(it => it.Name.Equals(dsNode.Attributes["EndPointName"].Value))
                            };
                        dsp.DataServices.Add(ds);
                    }
                }
              
            }
            dataServiceGroups.Add(dsp);
        }
        #endregion
    }
}

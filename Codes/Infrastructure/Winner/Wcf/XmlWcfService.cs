using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Winner.Wcf
{
    public class XmlWcfService : WcfService
    {

        #region 属性

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

        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
        #endregion

        #region 构造函数

        /// <summary>
        /// 无参数
        /// </summary>
        public XmlWcfService()
        {
        }

        /// <summary>
        /// 配置信息文件路径,节点名称
        /// </summary>
        /// <param name="clientFile"></param>
        /// <param name="configFile"></param>
        /// <param name="nodeName"></param>
        public XmlWcfService(string clientFile, string configFile, string nodeName)
        {
            ClientFile = clientFile;
            NodeName = nodeName;
            ConfigFile = configFile;
        }

        #endregion

        #region 加载配置文件

        /// <summary>
        /// 加载配置文件
        /// </summary>
        protected virtual void LoadConfig()
        {
            var doc = GetXmlDocument();
            LoadEndPointsByXml(doc);
            SetGetEndPointsHandle();
        }
        /// <summary>
        /// 设置委托
        /// </summary>
        protected virtual void SetGetEndPointsHandle()
        {
            if (string.IsNullOrEmpty(ClassName) || string.IsNullOrEmpty(MethodName))
                return;
            var t = Type.GetType(ClassName);
            if (t == null) return;
            var obj = Activator.CreateInstance(t);
            GetEndPointsHandle = (GetEndPointsDelegate)Delegate.CreateDelegate(typeof(GetEndPointsDelegate), obj, MethodName);
        }
        /// <summary>
        /// 得到XmlDocument
        /// </summary>
        /// <returns></returns>
        protected virtual XmlDocument GetXmlDocument()
        {
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                     ConfigFile);
            var doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }

        /// <summary>
        /// 加载EndPoints
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void LoadEndPointsByXml(XmlDocument doc)
        {
            if(string.IsNullOrEmpty(NodeName))return;
            XmlNodeList nodes = doc.SelectNodes(NodeName);
            SetEndPointsByXmlNodes(nodes);
        }
        /// <summary>
        /// 设置节点
        /// </summary>
        /// <param name="nodes"></param>
        public virtual void SetEndPointsByXmlNodes(XmlNodeList nodes)
        {
            if (nodes == null) return;
            var temp = GetEndPointsByXmlNodes(nodes);
            SetEndPointsFailoverByXmlNodes(nodes, temp);
        }

        /// <summary>
        /// 根据节点得到EndPointInfo
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected virtual IList<EndPointInfo> GetEndPointsByXmlNodes(XmlNodeList nodes)
        {
            var temp = new List<EndPointInfo>();
            EndPoints = new List<EndPointInfo>();
            foreach (XmlNode node in nodes)
            {
                var t = new EndPointInfo();
                if (node.Attributes == null) continue;
                t.Name = node.Attributes["Name"].Value;
                t.Nickname = node.Attributes["Nickname"] != null ? node.Attributes["Nickname"].Value : null;
                t.CheckAlivePeriod = node.Attributes["CheckAlivePeriod"] == null ||
                                         string.IsNullOrEmpty(node.Attributes["CheckAlivePeriod"].Value)
                                             ? t.CheckAlivePeriod
                                             : Convert.ToInt32(node.Attributes["CheckAlivePeriod"].Value);
                temp.Add(t);
                if (node.Attributes["IsUsed"] == null || !"false".Equals(node.Attributes["IsUsed"].Value))
                    EndPoints.Add(t);
            }
            return temp;
        }

        /// <summary>
        /// 设置故障转移
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="endPoints"></param>
        protected virtual void SetEndPointsFailoverByXmlNodes(XmlNodeList nodes, IList<EndPointInfo> endPoints)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes == null || node.Attributes["FailoverName"] == null) continue;
                var endPoint = endPoints.FirstOrDefault(it => it.Name.Equals(node.Attributes["Name"].Value));
                if (endPoint == null) continue;
                var failoverName = node.Attributes["FailoverName"].Value.Split(',');
                endPoint.Failovers =
                    endPoints.Where(it => failoverName.Contains(it.Name) && !endPoint.Name.Equals(it.Name)).ToList();

            }

        }

        #endregion
    }
}

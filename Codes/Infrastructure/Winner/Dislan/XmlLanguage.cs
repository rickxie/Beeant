using System;
using System.Linq;
using System.Xml;

namespace Winner.Dislan
{
    public class XmlLanguage : Language
    {
        #region 配置文件
        private string _configFile;
        /// <summary>
        /// �����ļ�·��
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

        #region 加载配置
        /// <summary>
        /// ���������ļ�
        /// </summary>
        protected virtual void LoadConfig()
        {
            XmlDocument doc = GetXmlDocument();
            LoadLanguageByXml(doc);
        }
        /// <summary>
        /// �õ�XmlDocument
        /// </summary>
        /// <returns></returns>
        protected virtual XmlDocument GetXmlDocument()
        {
            string filename = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, ConfigFile);
            return GetXmlDocument(filename);
        }
        /// <summary>
        ///  �õ�XmlDocument
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

        #region 加载配置

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void LoadLanguageByXml(XmlDocument doc)
        {
            XmlNodeList xnPaths = doc.SelectNodes("/configuration/Dislan/XmlLanguage/LanguagePath");
            if (xnPaths == null || xnPaths.Count == 0)
                return ;
            foreach (XmlNode node in xnPaths)
            {
                XmlNodeList nodes = GetLanguageXmlNodes(node);
                AddLanguageByXmlNodes(nodes);
            }
        }

        /// <summary>
        /// �õ�����ڵ�
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual XmlNodeList GetLanguageXmlNodes(XmlNode node)
        {
            if (node.Attributes == null)
                return null;
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                     node.Attributes["Path"].Value);
            XmlDocument doc = GetXmlDocument(fileName);
            XmlNodeList nodes = doc.SelectNodes("/configuration/Dislan/XmlLanguage/Language");
            return nodes;
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="nodes"></param>
        protected virtual void AddLanguageByXmlNodes(XmlNodeList nodes)
        {
            if (nodes == null || nodes.Count == 0)return;
            foreach (XmlNode node in nodes)
            {
                FillNamesByXmlNode(node);
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="node"></param>
        protected virtual void FillNamesByXmlNode(XmlNode node)
        {
            var nodes = node.SelectNodes("Message");
            if (nodes == null || nodes.Count == 0) return;
            if (node.Attributes == null) return;
            var languages = (from XmlNode nd in nodes select GetLanguageInfoByXmlNode(nd)).ToList();
            AddNames(node.Attributes["Name"].Value, languages);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual LanguageInfo GetLanguageInfoByXmlNode( XmlNode node)
        {
            var info = new LanguageInfo();
            if (node.Attributes != null)
            {
                info.Name = node.Attributes["Name"].Value;
                info.Message = node.Attributes["Value"].Value;
            }
            return info;
        }

        #endregion
    }
}

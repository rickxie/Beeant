using System;
using System.Xml;
using System.IO;
using Winner.Persistence.Data;

namespace Winner.Persistence.Key
{
    /// <summary>
    /// 加载ORM
    /// </summary>
    public class XmlKey: Key
    {
        #region 属性
        /// <summary>
        /// 数据库实例
        /// </summary>
        public IDataBase DataBase { get; set; }
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
        public XmlKey()
        { 
        }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        /// <param name="configFile"></param>
        public XmlKey(string configFile)
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
            AddOrmKeyByXml(doc);
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
   
        #region 得到配置信息
        /// <summary>
        /// 得到key的信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected virtual void AddOrmKeyByXml(XmlDocument doc)
        {
            XmlNodeList nodes = doc.SelectNodes("/configuration/Persistence/XmlKey/Info");
            if (nodes != null)
                foreach (XmlNode node in nodes)
                {
                    AddOrmKeyByXmlNode(node);
                }
        }
        /// <summary>
        /// 得到OrmKey
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected virtual void AddOrmKeyByXmlNode(XmlNode node)
        {
            var key = new OrmKeyInfo();
            if (node.Attributes != null)
            {
                key.Name = node.Attributes["Name"].Value;
                key.Flag =node.Attributes["Flag"]==null?"": node.Attributes["Flag"].Value;
                key.Recovery = node.Attributes["Recovery"] == null ? "" : node.Attributes["Recovery"].Value;
                key.RightLength = Convert.ToInt32(node.Attributes["RightLength"].Value);
            }
           OrmKeys.Add(key);
        }
       
        #endregion

      

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Xml;
using Component.Extension;
using Winner;
using Winner.Base;
using Winner.Persistence.Key;

namespace Configuration
{
    static public class ConfigurationManager
    {
        #region 初始化配置文件
        public static object Locker=new object();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="appName"></param>
        public static void Initialize(string appName)
        {
            lock (Locker)
            {
                ReplaceConfigFile(appName);
                ReplaceIocFile(appName);
                ReplaceValidationFile(appName);
                ReplaceDbRouteFile(appName);
                LoadSettings(appName, @"Config\Url.config");
                LoadSettings(appName, @"Config\App.config");
                LoadSettings(appName, @"Config\ThirdParty.config");
                Creator.ConfigFile = @"Config\config.config";
                LoadSettings(appName);
                Creator.Get<IKey>().Initialize();
                Creator.AddHandle(Creator.Get<IKey>().Initialize);
            }
        }
        /// <summary>
        /// 得到节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static XmlNodeList GetNodes(string xpath)
        {
            var doc = GetXmlDocument();
            return doc.SelectNodes(xpath);
        }

        /// <summary>
        /// 得到配置文档
        /// </summary>
        /// <returns></returns>
        private static XmlDocument GetXmlDocument(string fileName = null)
        {
            fileName = string.IsNullOrEmpty(fileName) ? @"Config\Config.config" : fileName;
            var doc = new XmlDocument();
            fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, fileName);
            doc.Load(fileName);
            return doc;
        }

     
        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="appName"></param>
        private static void ReplaceConfigFile(string appName)
        {
            if (string.IsNullOrEmpty(appName)) return;
            var doc = GetXmlDocument();
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/Merged/App");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if (xmlNode.Attributes == null || xmlNode.Attributes["Name"] == null || !appName.StartsWith(xmlNode.Attributes["Name"].Value) )
                    continue;
                ReplaceNodes(doc.ChildNodes, xmlNode);
                break;
            }
            var fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Config\config.config");
            doc.Save(fileName);
        }
        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="appName"></param>
        private static void ReplaceIocFile(string appName)
        {
            if (string.IsNullOrEmpty(appName)) return;
            var doc = GetXmlDocument(@"Config\Ioc.config");
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/Merged/App");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if(xmlNode.Attributes==null ||xmlNode.Attributes["Name"]==null || !appName.StartsWith(xmlNode.Attributes["Name"].Value))
                    continue;
                ReplaceNodes(doc.ChildNodes, xmlNode);
                break;
            }
            var fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Config\Ioc.config");
            doc.Save(fileName);
        }
        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="appName"></param>
        private static void ReplaceValidationFile(string appName)
        {
            if (string.IsNullOrEmpty(appName)) return;
            var doc = GetXmlDocument(@"Config\Validation.config");
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/Merged/App");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if (xmlNode.Attributes == null || xmlNode.Attributes["Name"] == null || !appName.StartsWith(xmlNode.Attributes["Name"].Value))
                    continue;
                ReplaceNodes(doc.ChildNodes, xmlNode);
                break;
            }
            var fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Config\Validation.config");
            doc.Save(fileName);
        }
        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="appName"></param>
        private static void ReplaceDbRouteFile(string appName)
        {
            if (string.IsNullOrEmpty(appName)) return;
            var doc = GetXmlDocument(@"Config\DbRoute.config");
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/Merged/App");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if (xmlNode.Attributes == null || xmlNode.Attributes["Name"] == null || !appName.StartsWith(xmlNode.Attributes["Name"].Value))
                    continue;
                ReplaceNodes(doc.ChildNodes, xmlNode);
                break;
            }
            var fileName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"Config\DbRoute.config");
            doc.Save(fileName);
        }
        /// <summary>
        /// 替换节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="appNode"></param>
        private static void ReplaceNodes(XmlNodeList nodes, XmlNode appNode)
        {
            if (appNode.ChildNodes.Count == 0) return;
            foreach (XmlNode node in nodes)
            {
                ReplaceNode(node, appNode);
                if (node.ChildNodes.Count > 0)
                    ReplaceNodes(node.ChildNodes, appNode);
            }
        }

        /// <summary>
        /// 替换节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="appNode"></param>
        private static void ReplaceNode(XmlNode node, XmlNode appNode)
        {
            if (node.Attributes == null || node.Attributes["Merged"] == null)
                return ;
            for (int i = 0; i < appNode.ChildNodes.Count;)
            {
                var childNode = appNode.ChildNodes[i];
                if (childNode.Attributes == null || childNode.Attributes["MergedValue"] == null
                    || childNode.Attributes["MergedValue"].Value != node.Attributes["Merged"].Value)
                {
                    i++;
                    continue;
                }
                var type = childNode.Attributes["MergedType"].Value;
                switch (type)
                {
                    case "Replace":
                        node.ParentNode.ReplaceChild(childNode, node);
                        break;
                    case "Append":
                        node.AppendChild(childNode);
                        break;
                    case "InsertBefore":
                        node.ParentNode.InsertBefore(childNode, node);
                        break;
                    case "InsertAfter":
                        node.ParentNode.InsertAfter(childNode, node);
                        break;
                    default:
                        i++;
                        break;
                }
                childNode.Attributes.Remove(childNode.Attributes["MergedType"]);
                childNode.Attributes.Remove(childNode.Attributes["MergedValue"]);
            }
        }


        #endregion

        #region 得到配置信息
        private static readonly IDictionary<string,string> Settings=new Dictionary<string, string>();

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="fileName"></param>
        private static void LoadSettings(string appName,string fileName=null)
        {
            var doc = GetXmlDocument(fileName);
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/Settings/App");
            if(xmlNodes==null || xmlNodes.Count==0)
                return;
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if (xmlNode.Attributes == null ||
                    xmlNode.Attributes["Name"] != null && !string.IsNullOrEmpty(xmlNode.Attributes["Name"].Value) &&
                    xmlNode.Attributes["Name"].Value != appName)
                    continue;
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.Attributes == null || node.Attributes["key"] == null || node.Attributes["value"] == null)
                        continue;
                    if (Settings.ContainsKey(node.Attributes["key"].Value))
                        Settings.Remove(node.Attributes["key"].Value);
                    Settings.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            }
        }
    
        /// <summary>
        /// 得到配置信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetSetting<T>(string name)
        {
            if (Settings.ContainsKey(name))
                return Settings[name].Convert<T>();
            return default(T);
        }

        #endregion

        #region 注册码
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ValidatePollCode(string fileName)
        {
            var text = File.ReadAllText(fileName).Split(new [] { "\r\n" },StringSplitOptions.None);
            var mac = GetMac();
            return Creator.Get<ISecurity>().DecryptRsa(mac, text[0], text[1]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetMac()
        {
            try
            {
                var query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                var queryCollection = query.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    if (mo["IPEnabled"].ToString() == "True")
                        return mo["MacAddress"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}

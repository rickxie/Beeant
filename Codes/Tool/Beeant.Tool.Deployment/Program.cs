using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Beeant.Tool.Deployment
{
    class Program
    {
        static void Main(string[] args)
        {
            var urls=GetUrls();
            var storageUrls= GetStorageUrls();
            if (urls != null)
            {
                AppendHost(urls);
            }
            if (storageUrls != null)
            {
                AppendHost(storageUrls);
            }
            AppendApplicationHost(urls, storageUrls);
        }
        /// <summary>
        /// 添加本地host
        /// </summary>
        /// <param name="urls"></param>
        private static void AppendHost(IDictionary<string, string> urls)
        {
            var file = @"C:\Windows\System32\drivers\etc\hosts";
            var text = File.ReadAllText(file);
            var builder=new StringBuilder(text);
            foreach (var url in urls)
            {
                var value = GetUrlValue(url.Value);
                if(string.IsNullOrWhiteSpace(value))
                    continue;
                if (text.Contains(value))
                    continue;
                builder.AppendLine(string.Format("127.0.0.1 {0}", value));
            }
            File.WriteAllText(file,builder.ToString());
        }
        /// <summary>
        /// 得到值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetUrlValue(string value)
        {
            value = value.ToLower();
            if (!value.StartsWith("http://") && !value.StartsWith("https://") || value[value.Length - 1] != '/' && value.LastIndexOf("/") > 8)
                return null;
            value = value.Replace("http://", "").Replace("https://", "").Replace("/", "");
            return value;
        }
        #region 替换IIS

        /// <summary>
        /// 添加本地host
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="storageUrls"></param>
        private static void AppendApplicationHost(IDictionary<string, string> urls,
            IDictionary<string, string> storageUrls)
        {
            var rootPath = GetRootPath();
            var filename = $@"{rootPath.Replace(@"\Codes\","")}\Publish\IIS\applicationhost.config";
            var doc = new XmlDocument();
            doc.Load(filename);
            AppendApplicationPools(doc, urls);
            AppendApplicationSites(doc, urls);
            AppendApplicationImageSites(doc, storageUrls);
            AppendApplicationTempSites(doc, storageUrls);
            AppendApplicationDocSites(doc, storageUrls);
            doc.Save(filename);
            FileInfo file=new FileInfo(filename);
            file.CopyTo(@"C:\Windows\System32\inetsrv\config\applicationhost.config", true);
        }

        /// <summary>
        /// 判断得到名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string GetName(string name,string c)
        {
            var builder=new StringBuilder();
            foreach (var n in name)
            {
                if (n < 97)
                    builder.Append(c);
                builder.Append(n);
            }
            builder.Remove(0, 1);
            builder.Remove(builder.Length - 4, 4);
            return builder.ToString();
        }
        /// <summary>
        /// 新增应用程序池
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="urls"></param>
        private static void AppendApplicationPools(XmlDocument doc, IDictionary<string, string> urls)
        {
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/system.applicationHost/applicationPools/add");
            if (xmlNodes == null || xmlNodes.Count==0)
                return ;
            foreach (var url in urls)
            {
                var value = GetUrlValue(url.Value);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                var name = $"Beeant.{GetName(url.Key, ".")}";
                var isAppend = true;
                foreach (XmlNode node in xmlNodes)
                {
                    if (node.Attributes == null || node.Attributes["name"] == null ||
                        node.Attributes["name"].Value != name)
                        continue;
                    isAppend = false;
                    break;
                }
                if (!isAppend)
                    continue;
                var newNode = xmlNodes[0].Clone();
                newNode.Attributes["name"].Value = name;
                newNode.Attributes["managedRuntimeVersion"].Value = "v4.0";
                newNode.Attributes["managedPipelineMode"].Value = "Classic";
                xmlNodes[0].ParentNode.InsertBefore(newNode, xmlNodes[0].ParentNode.LastChild);
            }
        }
        /// <summary>
        /// 新增应用程序池
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="urls"></param>
        private static void AppendApplicationSites(XmlDocument doc, IDictionary<string, string> urls)
        {
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/system.applicationHost/sites/site");
            if (xmlNodes == null || xmlNodes.Count == 0)
                return ;
            var i = 1;
            var rootPath = GetRootPath();
            foreach (var url in urls)
            {
                var value = GetUrlValue(url.Value);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                var name = $"Beeant.{GetName(url.Key, ".")}";
                var isAppend = true;
                foreach (XmlNode node in xmlNodes)
                {
                    if (node.Attributes == null || node.Attributes["name"] == null ||
                        node.Attributes["name"].Value != name)
                        continue;
                    isAppend = false;
                    break;
                }
                if (!isAppend)
                    continue;
                var path = name;
                var index = path.IndexOf(".");
                if (index > -1)
                    path = path.Substring(index + 1);
                index = path.LastIndexOf(".");
                if (index > -1)
                    path = path.Substring(0,index+1);
                path = path.Replace(".", "\\");
                var newNode = xmlNodes[0].ParentNode.Clone();
                var builder=new StringBuilder();
                builder.AppendLine($"            <site name=\"{name}\" id=\"{xmlNodes.Count + i}\">");
                builder.AppendLine($"                <application path=\"/\" applicationPool=\"{name}\">");
                builder.AppendLine($"                    <virtualDirectory path = \"/\" physicalPath = \"{rootPath}{path}{name}\" />");
                builder.AppendLine("                </application >");
                builder.AppendLine("                <bindings>");
                builder.AppendLine($"                    <binding protocol = \"http\" bindingInformation = \"*:80:{value}\" />");
                builder.AppendLine("                </bindings>");
                builder.AppendLine($"            </site>");
                newNode.InnerXml = builder.ToString();
                var insertNode = xmlNodes[0].ParentNode.SelectSingleNode("siteDefaults");
                xmlNodes[0].ParentNode.InsertBefore(newNode.ChildNodes[1], insertNode);
                i++;
            }
        }

        /// <summary>
        /// 新增应用程序池
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="storageUrls"></param>
        private static void AppendApplicationImageSites(XmlDocument doc, IDictionary<string, string> storageUrls)
        {
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/system.applicationHost/sites/site");
            if (xmlNodes == null || xmlNodes.Count == 0)
                return;
            var rootPath = GetRootPath();
            var name = "Beeant.Images";
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes == null || node.Attributes["name"] == null ||
                    node.Attributes["name"].Value != name)
                    continue;
                return;
            }
            var newNode = xmlNodes[0].ParentNode.Clone();
            var builder = new StringBuilder();
            builder.AppendLine($"            <site name=\"{name}\" id=\"{xmlNodes.Count + 1}\">");
            builder.AppendLine($"                <application path=\"/\" applicationPool=\"DefaultAppPool\">" );
            builder.AppendLine($"                    <virtualDirectory path=\"/\" physicalPath=\"{rootPath}Distributed\\Outside\\Beeant.Distributed.Outside.Image\" />");
            builder.AppendLine($"                    <virtualDirectory path = \"/Files\" physicalPath = \"{rootPath.Replace(@"\Codes\","")}\\Publish\\Hosts\\IImageFileService\\Files\" />");
            builder.AppendLine("                </application >");
            builder.AppendLine("                <bindings>");
            foreach (var url in storageUrls)
            {
                if(!url.Key.ToLower().StartsWith("images"))
                    continue;
                var value = GetUrlValue(url.Value);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                builder.AppendLine($"                    <binding protocol = \"http\" bindingInformation = \"*:80:{value}\" />");
            }
            builder.AppendLine("                </bindings>");
            builder.AppendLine($"            </site>");
            newNode.InnerXml = builder.ToString();
            var insertNode = xmlNodes[0].ParentNode.SelectSingleNode("siteDefaults");
            xmlNodes[0].ParentNode.InsertBefore(newNode.ChildNodes[1], insertNode);
        }

        /// <summary>
        /// 新增应用程序池
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="storageUrls"></param>
        private static void AppendApplicationTempSites(XmlDocument doc, IDictionary<string, string> storageUrls)
        {
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/system.applicationHost/sites/site");
            if (xmlNodes == null || xmlNodes.Count == 0)
                return ;
            var rootPath = GetRootPath();
            var name = "Beeant.Temps";
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes == null || node.Attributes["name"] == null ||
                    node.Attributes["name"].Value != name)
                    continue;
                return ;
            }
            var newNode = xmlNodes[0].ParentNode.Clone();
            var builder = new StringBuilder();
            builder.AppendLine($"            <site name=\"{name}\" id=\"{xmlNodes.Count + 1}\">");
            builder.AppendLine($"                <application path=\"/\" applicationPool=\"DefaultAppPool\">");
            builder.AppendLine($"                    <virtualDirectory path=\"/\" physicalPath=\"%SystemDrive%\\inetpub\\wwwroot\" />");
            builder.AppendLine($"                    <virtualDirectory path = \"/Files\" physicalPath = \"{rootPath.Replace(@"\Codes\", "")}\\Publish\\Hosts\\IImageTempFileService\\Files\" />");
            builder.AppendLine("                </application >");
            builder.AppendLine("                <bindings>");
            foreach (var url in storageUrls)
            {
                if (!url.Key.ToLower().StartsWith("temp"))
                    continue;
                var value = GetUrlValue(url.Value);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                builder.AppendLine($"                    <binding protocol = \"http\" bindingInformation = \"*:80:{value}\" />");
            }
            builder.AppendLine("                </bindings>");
            builder.AppendLine($"            </site>");
            newNode.InnerXml = builder.ToString();
            var insertNode = xmlNodes[0].ParentNode.SelectSingleNode("siteDefaults");
            xmlNodes[0].ParentNode.InsertBefore(newNode.ChildNodes[1], insertNode);
        }

        /// <summary>
        /// 新增应用程序池
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="storageUrls"></param>
        private static void AppendApplicationDocSites(XmlDocument doc, IDictionary<string, string> storageUrls)
        {
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/system.applicationHost/sites/site");
            if (xmlNodes == null || xmlNodes.Count == 0)
                return;
            var rootPath = GetRootPath();
            var name = "Beeant.Docs";
            foreach (XmlNode node in xmlNodes)
            {
                if (node.Attributes == null || node.Attributes["name"] == null ||
                    node.Attributes["name"].Value != name)
                    continue;
                return;
            }
            var newNode = xmlNodes[0].ParentNode.Clone();
            var builder = new StringBuilder();
            builder.AppendLine($"            <site name=\"{name}\" id=\"{xmlNodes.Count + 1}\">");
            builder.AppendLine($"                <application path=\"/\" applicationPool=\"DefaultAppPool\">");
            builder.AppendLine($"                    <virtualDirectory path=\"/\" physicalPath=\"%SystemDrive%\\inetpub\\wwwroot\" />");
            builder.AppendLine($"                    <virtualDirectory path = \"/Files\" physicalPath = \"{rootPath.Replace(@"\Codes\", "")}\\Publish\\Hosts\\IDocFileService\\Files\" />");
            builder.AppendLine("                </application >");
            builder.AppendLine("                <bindings>");
            foreach (var url in storageUrls)
            {
                if (!url.Key.ToLower().StartsWith("doc"))
                    continue;
                var value = GetUrlValue(url.Value);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                builder.AppendLine($"                    <binding protocol = \"http\" bindingInformation = \"*:80:{value}\" />");
            }
            builder.AppendLine("                </bindings>");
            builder.AppendLine($"            </site>");
            newNode.InnerXml = builder.ToString();
            var insertNode = xmlNodes[0].ParentNode.SelectSingleNode("siteDefaults");
            xmlNodes[0].ParentNode.InsertBefore(newNode.ChildNodes[1], insertNode);
        }
        #endregion

        /// <summary>
        /// 得到Url文件
        /// </summary>
        /// <returns></returns>
        private static IDictionary<string, string> GetUrls()
        {
            string filename = string.Format(@"{0}Infrastructure\Configuration\Config\Url.config", GetRootPath());
            var doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/Settings/App");
            if (xmlNodes == null || xmlNodes.Count == 0)
                return null;
            IDictionary<string, string> urls=new Dictionary<string, string>();
            foreach (XmlNode xmlNode in xmlNodes)
            {
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.Attributes == null || node.Attributes["key"] == null || node.Attributes["value"] == null)
                        continue;
                    if (urls.ContainsKey(node.Attributes["key"].Value))
                        urls.Remove(node.Attributes["key"].Value);
                    urls.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            }
            return urls;
        }
        /// <summary>
        /// 得到Url文件
        /// </summary>
        /// <returns></returns>
        private static IDictionary<string, string> GetStorageUrls()
        {
            string filename = string.Format(@"{0}Infrastructure\Configuration\Config\Storage.config", GetRootPath());
            var doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList xmlNodes = doc.SelectNodes("configuration/Storage/XmlAddress");
            if (xmlNodes == null || xmlNodes.Count == 0)
                return null;
            IDictionary<string, string> urls=new Dictionary<string, string>();
            foreach (XmlNode xmlNode in xmlNodes)
            {
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.Attributes == null || node.Attributes["Name"] == null || node.Attributes["Name"] == null)
                        continue;
                    if (urls.ContainsKey(node.Attributes["Name"].Value))
                        urls.Remove(node.Attributes["Name"].Value);
                    urls.Add(node.Attributes["Name"].Value, node.Attributes["Url"].Value);
                }
            }
            return urls;
        }
        /// <summary>
        /// 得到根目录
        /// </summary>
        /// <returns></returns>
        private static string GetRootPath()
        {
            var tag = @"\Codes\";
            var path = Directory.GetCurrentDirectory();
            var index = path.IndexOf(tag);
            if (index == -1)
                return null;
            return path.Substring(0, index + tag.Length);
        }
    }
}

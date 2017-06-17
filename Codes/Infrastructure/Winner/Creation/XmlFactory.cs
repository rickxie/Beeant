using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Winner.Creation
{
    

    public class XmlFactory : Factory 
    {
        #region 属性
        private string _configFile;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public virtual string ConfigFile
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
        /// 加载配置文件
        /// </summary>
        protected virtual void LoadConfig()
        {
            XmlDocument doc = GetXmlDocument();
            IDictionary<string, DelegateAopMethod> methods = new Dictionary<string, DelegateAopMethod>();
            LoadAopByXml(doc, methods);
            LoadIocByXml(doc, methods);
            LoadPropertyByXml(doc);
        }
        /// <summary>
        /// 得到XmlDocument
        /// </summary>
        /// <returns></returns>
        protected virtual XmlDocument GetXmlDocument()
        {
            string filename = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, ConfigFile);
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

        #region 加载AOP
        /// <summary>
        /// 加载AOP
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="methods"></param>
        protected virtual void LoadAopByXml(XmlDocument doc, IDictionary<string, DelegateAopMethod> methods)
        {
            XmlNodeList xnPaths = doc.SelectNodes("/configuration/Creation/XmlFactory/AopPath");
            if (xnPaths == null || xnPaths.Count == 0)
                return;
            foreach (XmlNode node in xnPaths)
            {
                if (node.Attributes != null)
                {
                    XmlNodeList nodes = GetAopXmlNodesByXmlPath(node.Attributes["Path"].Value);
                    AddAopByXmlNodes(nodes, methods);
                }
            }
        }

        /// <summary>
        /// 得到AOP节点
        /// </summary>
        /// <returns></returns>
        protected virtual XmlNodeList GetAopXmlNodesByXmlPath(string path)
        {
            if (path == null) return null;
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);
            XmlDocument doc = GetXmlDocument(fileName);
            XmlNodeList nodes = doc.SelectNodes("/configuration/Creation/XmlFactory/Aop/Instance");
            return nodes;
        }
        /// <summary>
        /// 添加AOP到集合中
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="methods"></param>
        protected virtual void AddAopByXmlNodes(XmlNodeList nodes, IDictionary<string, DelegateAopMethod> methods)
        {
            if (nodes == null || nodes.Count == 0)
                return;
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes == null) continue;
                var obj = CreateClass(node.Attributes["ClassName"].Value);
                if(obj==null) continue;
                var properties = GetFactoryPropertiesByXmlNodes(node.SelectNodes("Property"));
                TrySetProperty(obj,properties);
                if (node.Attributes == null) continue;
                var dt = (DelegateAopMethod)Delegate.CreateDelegate(typeof(DelegateAopMethod), obj, node.Attributes["Method"].Value);
                methods.Add(node.Attributes["Name"].Value, dt);
            }
        }
        #endregion

        #region 加载IOC

        /// <summary>
        /// 加载IOC
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="methods"></param>
        protected virtual void LoadIocByXml(XmlDocument doc, IDictionary<string, DelegateAopMethod> methods)
        {
            XmlNodeList xnPaths = doc.SelectNodes("/configuration/Creation/XmlFactory/IocPath");
            if (xnPaths == null || xnPaths.Count == 0)
                return;
            foreach (XmlNode node in xnPaths)
            {
                if (node.Attributes == null) continue;
                var nodes = GetIocXmlNodesByXmlPath(node.Attributes["Path"].Value);
                AddIocByXmlNodes(nodes, methods);
            }
        }

        /// <summary>
        /// 得到IOC节点
        /// </summary>
        /// <returns></returns>
        protected virtual XmlNodeList GetIocXmlNodesByXmlPath(string path)
        {
            if (path == null) return null;
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);
            XmlDocument doc = GetXmlDocument(fileName);
            XmlNodeList nodes = doc.SelectNodes("/configuration/Creation/XmlFactory/Ioc/Instance");
            return nodes;
        }

        /// <summary>
        /// 创建IOC实例
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="methods"></param>
        protected virtual void AddIocByXmlNodes(XmlNodeList nodes,IDictionary<string, DelegateAopMethod> methods)
        {
            if (nodes == null || nodes.Count == 0)return;
            foreach (XmlNode node in nodes)
            {
                var info = new FactoryInfo
                    {
                        BeforeCuts = GetBeforeMothedByXmlNode(node, methods),
                        AfterCuts = GetAfterMothedByXmlNode(node, methods)
                    };
                CreateByXmlNode(node, info);
            }
        }
        /// <summary>
        /// 得到注入开始方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methods"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> GetBeforeMothedByXmlNode(XmlNode node, IDictionary<string, DelegateAopMethod> methods)
        {
            IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> exeMethods = new Dictionary<string, KeyValuePair<DelegateAopMethod, bool>>();
            XmlNodeList nodes = node.SelectNodes("Before");
            FillMethodsfromXmlNodes(methods, nodes, exeMethods);
            return exeMethods;
        }
     
        /// <summary>
        /// 得到注入结束方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methods"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> GetAfterMothedByXmlNode(XmlNode node, IDictionary<string, DelegateAopMethod> methods)
        {
            IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> exeMethods = new Dictionary<string, KeyValuePair<DelegateAopMethod, bool>>();
            XmlNodeList nodes = node.SelectNodes("After");
            FillMethodsfromXmlNodes(methods, nodes, exeMethods);
            return exeMethods;
        }
        /// <summary>
        /// 填充注入方法
        /// </summary>
        /// <param name="methods"></param>
        /// <param name="nodes"></param>
        /// <param name="exeMethods"></param>
        protected virtual void FillMethodsfromXmlNodes(IDictionary<string, DelegateAopMethod> methods, XmlNodeList nodes, IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> exeMethods)
        {
            if (nodes == null || nodes.Count == 0)
                return;
            foreach (XmlNode node in nodes)
            {
                AddMethodsByXmlNode(methods, node, exeMethods);
            }
        }
        /// <summary>
        /// 添加注入方法
        /// </summary>
        /// <param name="methods"></param>
        /// <param name="node"></param>
        /// <param name="exeMethods"></param>
        protected virtual void AddMethodsByXmlNode(IDictionary<string, DelegateAopMethod> methods, XmlNode node, IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> exeMethods)
        {
            if (node.Attributes != null && !methods.ContainsKey(node.Attributes["AopName"].Value))
                return;
            bool isasncy = node.Attributes != null && (node.Attributes["IsAsynchronous"] != null && Convert.ToBoolean(node.Attributes["IsAsynchronous"].Value));
            if (node.Attributes == null) return;
            string thismothod = node.Attributes["ThisMethod"].Value;
            var value = new KeyValuePair<DelegateAopMethod, bool>(methods[node.Attributes["AopName"].Value], isasncy);
            exeMethods.Add(thismothod, value);
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="node"></param>
        /// <param name="info"></param>
        protected virtual void CreateByXmlNode(XmlNode node,FactoryInfo info)
        {
            if (node.Attributes == null) return;
            var obj = CreateClass(node.Attributes["ClassName"].Value);
            if ((info.BeforeCuts != null && info.BeforeCuts.Count > 0) || (info.AfterCuts != null && info.AfterCuts.Count > 0))
                info.Target = GetTransparentProxy(obj, info.BeforeCuts, info.AfterCuts);
            else
                info.Target = obj;
            if (info.Target == null) return;
            info.IsSingle = node.Attributes["IsSingle"] == null || Convert.ToBoolean(node.Attributes["IsSingle"].Value);
            info.Name = node.Attributes["Name"].Value;
            Set(info);
        }
        #endregion

        #region 加载Ioc属性
        /// <summary>
        /// 加载属性
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void LoadPropertyByXml(XmlDocument doc)
        {
            XmlNodeList xnPaths = doc.SelectNodes("/configuration/Creation/XmlFactory/IocPath");
            if (xnPaths == null || xnPaths.Count == 0)
                return;
            var alreadyFactories = new List<FactoryInfo>();
            foreach (XmlNode node in xnPaths)
            {
                XmlNodeList nodes = GetPropertyXmlNodes(node);
                SetFactoryPropertiesByXmlNodes(nodes,alreadyFactories);
            }
            foreach (var info in Factories)
            {
                TrySetProperty(info.Target,info.Properties);
            }
        }

        /// <summary>
        /// 得到属性节点
        /// </summary>
        /// <returns></returns>
        protected virtual XmlNodeList GetPropertyXmlNodes(XmlNode node)
        {
            if (node.Attributes == null)
                return null;
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,node.Attributes["Path"].Value);
            XmlDocument doc = GetXmlDocument(fileName);
            XmlNodeList nodes = doc.SelectNodes("/configuration/Creation/XmlFactory/Ioc/Instance");
            return nodes;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="alreadyFactories"></param>
        protected virtual void SetFactoryPropertiesByXmlNodes(XmlNodeList nodes, IList<FactoryInfo> alreadyFactories)
        {
            if (nodes == null || nodes.Count == 0) return ;
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes == null) continue;
                var info = Factories.FirstOrDefault(it => it.Name.Equals(node.Attributes["Name"].Value));
                if (info == null || alreadyFactories.Count(it=>it.Name==info.Name)>0) continue;
                alreadyFactories.Add(info);
                info.Properties = GetFactoryPropertiesByXmlNodes(node.SelectNodes("Property"));
            }
        }


        #endregion

        #region 得到属性

        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="nodes"></param>
        protected virtual IList<FactoryPropertyInfo> GetFactoryPropertiesByXmlNodes(XmlNodeList nodes)
        {
            if (nodes == null || nodes.Count == 0) return null;
            return (from XmlNode node in nodes
                    let attributes = node.Attributes
                    where attributes != null
                    where attributes != null
                    select new FactoryPropertyInfo
                        {
                            Name = attributes["Name"].Value, Value = attributes["Value"].Value, 
                            IsShare = attributes["IsShare"] != null && 
                            Convert.ToBoolean(attributes["IsShare"].Value),
                            Properties = GetFactoryPropertiesByXmlNodes(node.SelectNodes("Property"))
                        }).ToList();
        }


        #endregion

    

    }
}

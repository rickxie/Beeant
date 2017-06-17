using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Winner.Base;
using Winner.Cache;
using Winner.Creation;
using Winner.Dislan;
using Winner.Filter;
using Winner.Log;
using Winner.Mail;
using Winner.Persistence;
using Winner.Persistence.Compiler.Common;
using Winner.Persistence.Compiler.Reverse;
using Winner.Persistence.Compiler.SqlServer;
using Winner.Persistence.ContextStorage;
using Winner.Persistence.Data;
using Winner.Persistence.Key;
using Winner.Persistence.Relation;
using Winner.Persistence.Route;
using Winner.Persistence.Translation;
using Winner.Persistence.Works;
using Winner.Queue;
using Winner.Reverse;
using Winner.Search;
using Winner.Search.Analysis;
using Winner.Search.Document;
using Winner.Search.Store;
using Winner.Search.Word;
using Winner.Storage;
using Winner.Storage.Address;
using Winner.Storage.Route;
using Winner.Storage.Synchronization;
using Winner.Wcf;
using IProperty = Winner.Base.IProperty;
using Property = Winner.Base.Property;

namespace Winner
{
    public sealed class Creator
    {
        #region 声明

        public delegate void RemoveCacheHandle();
        static private readonly IList<RemoveCacheHandle> RemoveCacheHandles=new List<RemoveCacheHandle>();
 
        #endregion

        #region 静态变量
        static private string _configFile;
        static public string ConfigFile
        {
            get { return _configFile; }
            set
            {
                _configFile = value;
                LoadInstance();
            }
        }

        static private  IDictionary<string, object> _instances = new Dictionary<string, object>();

        #endregion

        #region 得到和创建实例
        /// <summary>
        /// 得到实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        static public T Get<T>(string name=null)
        {
            if (name == null)
            {
                return (T)_instances[typeof(T).ToString()];
            }
            return (T)_instances[name];
        }
        /// <summary>
        /// 添加实例
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        static public bool Set(string name, object obj)
        {
            if (_instances.ContainsKey(name))
            {
                return false;
            }
            _instances.Add(name, obj);
            return true;
        }
        /// <summary>
        /// 添加缓存变化调用实例
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool AddHandle(RemoveCacheHandle handle)
        {
            if (RemoveCacheHandles.Contains(handle)) return false;
            RemoveCacheHandles.Add(handle);
            return true;
        }

        #endregion

        #region 默认配置
        /// <summary>
        /// 加载实例
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        private static void LoadInstatnce(IDictionary<string, object> instances, string name, object obj)
        {
            if (!instances.ContainsKey(name))
            {
                instances.Add(name, obj);
            }
          
        }
        /// <summary>
        /// 加载Base模块
        /// </summary>
        private static void LoadBaseInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances,typeof(IComponent).FullName, new Component());
            LoadInstatnce(instances, typeof(ISecurity).FullName, new Security());
            LoadInstatnce(instances, typeof(IProperty).FullName, new Property());
        }
        /// <summary>
        /// 加载Queue模块
        /// </summary>
        private static void LoadQueueInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IQueue).FullName, new LocalQueue());
           
        }
        /// <summary>
        /// 加载Mail模块
        /// </summary>
        private static void LoadMailInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IMail).FullName, new Mail.Mail());
        }
        /// <summary>
        /// 加载Cache模块
        /// </summary>
        private static void LoadCacheInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(ICache).FullName, new LocalCache());
            LoadInstatnce(instances, typeof(ICacheContract).FullName, new CacheService());
        }
        /// <summary>
        /// 加载Creation模块
        /// </summary>
        private static void LoadCreationInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IFactory).FullName, new XmlFactory());
        }
        /// <summary>
        /// 加载Dislan模块
        /// </summary>
        private static void LoadDislanInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(ILanguage).FullName, new XmlLanguage());
        }
        /// <summary>
        /// 加载Dislan模块
        /// </summary>
        private static void LoadFilterInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IValidation).FullName, new XmlValidation());
        }
        /// <summary>
        /// 加载Log模块
        /// </summary>
        private static void LoadLogInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(ILog).FullName, new FileLog());
        }
        /// <summary>
        /// 加载Persistence模块
        /// </summary>
        private static void LoadPersistenceInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IContext).FullName, new Context());
            LoadInstatnce(instances, typeof(ITransaction).FullName, new Transaction());
            LoadInstatnce(instances, typeof(IExecutor).FullName, new Executor());
            LoadInstatnce(instances, typeof(IOrm).FullName, new XmlOrm());
            LoadInstatnce(instances, typeof(IKey).FullName, new XmlKey());
            LoadInstatnce(instances, typeof(IDataBase).FullName, new XmlDataBase());
            LoadInstatnce(instances, typeof(IDbRoute).FullName, new XmlDbRoute());
            LoadInstatnce(instances, typeof(IContextStorage).FullName, new HttpContextStorage());
            LoadInstatnce(instances, typeof(ICompiler).FullName, new SqlServerCompiler());
            LoadInstatnce(instances, typeof(IFill).FullName, new JsonFill());
            LoadInstatnce(instances, typeof(IQueryCompiler).FullName, new SqlServer2005QueryCompiler());
            LoadInstatnce(instances, typeof(IGroupbyCompiler).FullName, new GroupbyCompiler());
            LoadInstatnce(instances, typeof(IHavingCompiler).FullName, new HavingCompiler());
            LoadInstatnce(instances, typeof(IOrderbyCompiler).FullName, new OrderbyCompiler());
            LoadInstatnce(instances, typeof(ISaveCompiler).FullName, new SqlServerSaveCompiler());
            LoadInstatnce(instances, typeof(ISelectCompiler).FullName, new SqlServer2005SelectCompiler());
            LoadInstatnce(instances, typeof(IWhereCompiler).FullName, new WhereCompiler());
        }
   
        /// <summary>
        /// 加载Reverse模块
        /// </summary>
        private static void LoadReverseInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IMapper).FullName, new Mapper());
        }
        /// <summary>
        /// 加载Search模块
        /// </summary>
        private static void LoadSearchInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IAnalyzer).FullName, new StandardAnalyzer());
            LoadInstatnce(instances, typeof(IDocumentor).FullName, new Documentor());
            LoadInstatnce(instances, typeof(IWorder).FullName, new Worder());
            LoadInstatnce(instances, typeof(IStorer).FullName, new FileStorer());
            LoadInstatnce(instances, typeof(IIndexer).FullName, new Indexer());
        }
        /// <summary>
        /// 加载Storage模块
        /// </summary>
        private static void LoadStorageInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(Storage.Cache.ICache).FullName, new Storage.Cache.LocalCache());
            LoadInstatnce(instances, typeof(IFile).FullName, new FileStore());
            LoadInstatnce(instances, typeof(IFileContract).FullName, new FileService());
            LoadInstatnce(instances, typeof(IMaster).FullName, new Master());
            LoadInstatnce(instances, typeof(IAddress).FullName, new Address());
            LoadInstatnce(instances, typeof(IFileRoute).FullName, new FileRoute());
        }
        /// <summary>
        /// 加载Wcf模块
        /// </summary>
        private static void LoadWcfInstance(IDictionary<string, object> instances)
        {
            LoadInstatnce(instances, typeof(IWcfService).FullName, new XmlWcfService());
            LoadInstatnce(instances, typeof(IWcfHost).FullName, new WcfHost());
        }
        /// <summary>
        /// 加载默认实例
        /// </summary>
        public static void LoadDefaultInstance(IDictionary<string, object> instances)
        {
           LoadBaseInstance(instances);
           LoadQueueInstance(instances);
           LoadMailInstance(instances);
           LoadCacheInstance(instances);
           LoadCreationInstance(instances);
           LoadDislanInstance(instances);
           LoadFilterInstance(instances);
           LoadLogInstance(instances);
           LoadPersistenceInstance(instances);
           LoadReverseInstance(instances);
           LoadSearchInstance(instances);
           LoadStorageInstance(instances);
           LoadWcfInstance(instances);
           SetDefaultInstanceProperty(instances);
        }
        /// <summary>
        /// 设置默认实例
        /// </summary>
        public static void SetDefaultInstanceProperty(IDictionary<string, object> instances)
        {
            foreach (var instance in instances)
            {
                var properties = instance.Value.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (instances.ContainsKey(property.PropertyType.FullName))
                    {
                        property.SetValue(instance.Value, instances[property.PropertyType.FullName], null);
                        if (instance.Value is ICompiler)
                            ((IDataBase)instances[typeof(IDataBase).FullName]).AddCompiler(typeof(ICompiler).FullName, instance.Value as ICompiler);
                    }

                }
            }
          
        }

        #endregion

        #region 加载实例
        static private object _locker=new object();
        /// <summary>
        /// 加载实例
        /// </summary>
        static private void LoadInstance()
        {
            lock (_locker)
            {
                var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var filename = Path.Combine(dir, _configFile);
                var directs = new List<string>();
                LoadDirectory(directs, Path.GetDirectoryName(filename));
                var instances = new Dictionary<string, object>();
                LoadDefaultInstance(instances);

                var doc = new XmlDocument();
                doc.Load(filename);
                var nodes = doc.SelectNodes("/configuration/Winner/Instance");
                FillInstance(instances, nodes);
                _instances = instances;

            }
        }
        /// <summary>
        /// 加载目录
        /// </summary>
        /// <param name="directs"></param>
        /// <param name="path"></param>
        private static void LoadDirectory(IList<string> directs, string path)
        {
            directs.Add(path);
            var directory=new DirectoryInfo(path);
            foreach (var dir in directory.GetDirectories())
            {
                LoadDirectory(directs, dir.FullName);
            }
            
        }
 
        /// <summary>
        /// 填充实例
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="nodes"></param>
        private static void FillInstance(IDictionary<string, object> instances, XmlNodeList nodes)
        {
            if (nodes == null || nodes.Count <= 0) return;
            foreach (XmlNode node in nodes)
                TryCreateInstance(instances,node);
            SetDefaultInstanceProperty(instances);
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes == null || !instances.ContainsKey(node.Attributes["Name"].Value)) continue;
                var target = instances[node.Attributes["Name"].Value];
                TrySetProperty(instances,target, node);
            }
                
           
        }

        ///  <summary>
        /// 尝试创建实例
        ///  </summary>
        /// <param name="instances"></param>
        /// <param name="node"></param>
        private static void TryCreateInstance(IDictionary<string, object> instances, XmlNode node)
        {
            try
            {
                CreateInstance(instances,node);
            }
            catch (Exception ex)
            {
                throw new Exception(node.OuterXml, ex);
            }
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static void CreateInstance(IDictionary<string, object> instances, XmlNode node)
        {
            if (node.Attributes == null || node.Attributes["ClassName"] == null || string.IsNullOrEmpty(node.Attributes["ClassName"].Value)) return;
            var type = Type.GetType(node.Attributes["ClassName"].Value);
            if (type == null) return;
            var obj = Activator.CreateInstance(type);
            if (obj is ICompiler)
                ((IDataBase)instances[typeof(IDataBase).FullName]).AddCompiler(node.Attributes["Name"].Value, obj as ICompiler);
            if (instances.ContainsKey(node.Attributes["Name"].Value))
                instances.Remove(node.Attributes["Name"].Value);
            instances.Add(node.Attributes["Name"].Value, obj);
        }

        ///  <summary>
        /// 尝试创建实例
        ///  </summary>
        /// <param name="instances"></param>
        /// <param name="target"></param>
        /// <param name="node"></param>
        private static void TrySetProperty(IDictionary<string, object> instances, object target, XmlNode node)
        {
            try
            {
                SetProperty(instances,target, node);
            }
            catch (Exception ex)
            {
                throw new Exception(node.OuterXml, ex);
            }
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="target"></param>
        /// <param name="node"></param>
        static private void SetProperty(IDictionary<string, object> instances, object target, XmlNode node)
        {
            var nodes = node.SelectNodes("Property");
            if (nodes == null) return;
            foreach (XmlNode nd in nodes)
            {
                if (nd.Attributes == null) continue;
                var property =
                    target.GetType().GetProperties().FirstOrDefault(it => it.Name.Equals(nd.Attributes["Name"].Value));
                if (property == null) continue;
                var value = GetPropertyValue(instances,nd.Attributes["Value"].Value, property);
                if (value == null) continue;
                property.SetValue(target, value, null);
                TrySetProperty(instances,value, nd);
            }
        }

        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="propertyValue"></param>
        /// <param name="property"></param>
        static private object GetPropertyValue(IDictionary<string, object> instances, string propertyValue, PropertyInfo property)
        {
            object value;
            if (property.PropertyType.IsInterface)
            {
                value = !instances.ContainsKey(propertyValue) ? CreateClass(propertyValue) : instances[propertyValue];
            }
            else
                value = TryConvertValue(propertyValue, property.PropertyType);
            return value;
        }
         /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static private object TryConvertValue(object value, Type type)
        {
            if (value == null) return null;
            if (type == typeof(object)) return value;
            try
            {
                if (type.IsEnum)
                {
                    var charValue = Convert.ChangeType(value, typeof(char));
                    if (charValue == null) return Enum.Parse(type, value.ToString());
                    var intValue = Convert.ChangeType(charValue, typeof(int));
                    if (intValue == null) return null;
                    return Enum.Parse(type, intValue.ToString());
                }
                return Convert.ChangeType(value, type);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        static private object CreateClass(string className)
        {
            var t = Type.GetType(className);
            if (t == null) return null;
            return Activator.CreateInstance(t);
        }
        #endregion

    }
}

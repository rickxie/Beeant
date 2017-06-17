using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Winner.Creation
{
    

    public class Factory :  IFactory
    {
        #region 属性
        IList<FactoryInfo> _factories =new List<FactoryInfo>();
        /// <summary>
        ///工厂集合
        /// </summary>
        public IList<FactoryInfo> Factories
        {
            get { return _factories; }
            set { _factories = value; }
        }
        /// <summary>
        /// 代理实例
        /// </summary>
        public virtual string ProxyName { get; set; }
        #endregion

        #region 接口的实现
        /// <summary>
        /// 得到实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T Get<T>(string name = null)
        {
            name = name ?? typeof(T).ToString();
            var info = Factories.FirstOrDefault(it => it.Name.Equals(name));
            if (info==null)
                return default(T);
            if (info.IsSingle)
                return (T)info.Target;
            return (T)Create(info);
        }

        /// <summary>
        /// 创建实例，isSingle表示是否为单例
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="isSingle"></param>
        public virtual bool Set(string name, object obj, bool isSingle)
        {
            var info =new FactoryInfo{Name=name,Target=obj,IsSingle=isSingle,AfterCuts=null,BeforeCuts=null};
            return Set(info);
        }

        /// <summary>
        /// 添加实例
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool Set(FactoryInfo info)
        {
            if (info == null) return false;
            Factories = Factories.Where(it => !it.Name.Equals(info.Name)).ToList();
            Factories.Add(info);
            return true;
        }

        
        #endregion

        #region 创建代理实例和添加对象

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual object Create(FactoryInfo info)
        {
            object target;
            var obj = Activator.CreateInstance(info.Target.GetType());
            if ((info.BeforeCuts != null && info.BeforeCuts.Count>0) || (info.AfterCuts != null && info.AfterCuts.Count>0))
                target = GetTransparentProxy(obj, info.BeforeCuts, info.AfterCuts);
            else
                target = obj;
            TrySetProperty(target, info.Properties);
            return target;
        }

        ///  <summary>
        /// 尝试创建实例
        ///  </summary>
        /// <param name="target"></param>
        /// <param name="properties"></param>
        protected virtual void TrySetProperty(object target, IList<FactoryPropertyInfo> properties)
        {
            try
            {
                if (properties == null || properties.Count == 0) return;
                SetProperty(target, properties);
            }
            catch (Exception ex)
            {
                throw new Exception(target.GetType().FullName, ex);
            }
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="target"></param>
        /// <param name="properties"></param>
        protected virtual void SetProperty(object target, IList<FactoryPropertyInfo> properties)
        {
            foreach (var p in properties)
            {
                var property = target.GetType().GetProperties().FirstOrDefault(it => it.Name.Equals(p.Name));
                if (property == null) continue;
                var value = GetPropertyValue(property, p);
                if (value == null) continue;
                property.SetValue(target, value, null);
                if(!p.IsShare) TrySetProperty(value, p.Properties);
            }
        }

        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="p"></param>
        protected virtual object GetPropertyValue(PropertyInfo property, FactoryPropertyInfo p)
        {
            object value;
            if (property.PropertyType.IsInterface)
            {
                var t = Factories.FirstOrDefault(it => it.Name.Equals(p.Value));
                value = t == null ? CreateClass(p.Value)
                            : p.IsShare ? t.Target : Create(t);
            }
            else
                value = TryConvertValue(p.Value, property.PropertyType);
            return value;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual object TryConvertValue(object value, Type type)
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
        protected virtual object CreateClass(string className)
        {
            var t = Type.GetType(className);
            if (t == null) return null;
            return Activator.CreateInstance(t);
        }
        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <param name="target"></param>
        /// <param name="befores"></param>
        /// <param name="afters"></param>
        /// <returns></returns>
        protected virtual object GetTransparentProxy(object target,
                                                     IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> befores,
                                                     IDictionary<string, KeyValuePair<DelegateAopMethod, bool>> afters)
        {
            if (string.IsNullOrEmpty(ProxyName))
                return null;
            var type = Type.GetType(ProxyName);
            if (type != null)
            {
                var proxy = Activator.CreateInstance(type, target, befores, afters) as IProxy;
                if (proxy != null) return proxy.GetProxy();
            }
            return null;
        }


        #endregion

    }
}

using System;
using System.Linq;
using System.Reflection;
using Winner.Persistence.Declare;

namespace Winner.Persistence.Compiler.Common
{
    public static class CompilerHelper 
    {
     

        #region 读取和设置属性值
        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetProperty(this object info, string name)
        {
            if (name.Contains("."))
            {
                return GetRelatePropertyValue(info, name);
            }
            return GetPropertyValue(info, name);
        }

        /// <summary>
        /// 填充属性值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetProperty(this object info, string name, object value)
        {
            if (name.Contains('.'))
            {
                SetRelatePropertyValue(info, name, value);
            }
            else
            {
                SetPropertyValue(info, name, value);
            }
        }
        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object GetPropertyValue(object info, string name)
        {
            var property = info.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(it => it.Name.Equals(name));
            if (property == null) return null;
            return property.GetValue(info, null);
        }
        /// <summary>
        /// 得到关联属性对象
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object GetRelatePropertyValue(object info, string name)
        {
            string[] str = name.Split('.');
            object obj = info;
            foreach (string t in str)
            {
                obj = GetPropertyValue(obj, t);
                if (obj == null)
                    return null;
            }
            return obj;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SetPropertyValue(object info, string name, object value)
        {
            var property = info.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                               .FirstOrDefault(it => it.Name.Equals(name));
            if (property == null) return;
            value = value == DBNull.Value
                        ? property.GetValue(info, null)
                        : property.PropertyType.TryConvertValue(value);
            if (value == null) return;
            property.SetValue(info, value, null);
        }

        /// <summary>
        /// 设置关联属性
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SetRelatePropertyValue(object info, string name, object value)
        {
            string[] str = name.Split('.');
            object obj = info;
            for (int i = 0; i < str.Length; i++)
            {
                if (obj == null) break;
                if (i != str.Length - 1)
                    obj = GetAndFillProperty(obj, str[i]);
                else
                    SetPropertyValue(obj, str[i], value);
            }
        }
        /// <summary>
        /// 填充对象属性并返回属性值
        /// </summary>
        public static object GetAndFillProperty(object info, string name)
        {
            object obj = GetPropertyValue(info, name);
            if (obj == null)
            {
                var pi = info.GetType()
                   .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                   .FirstOrDefault(it => it.Name.Equals(name));
                obj = pi == null ? null : Activator.CreateInstance(pi.PropertyType);
                SetPropertyValue(info, name, obj);
            }
            return obj;
        }
        /// <summary>
        /// 试着将value转换为type类型的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object TryConvertValue(this Type type,object value)
        {
            if (value == null) return null;
            if (type == typeof(object)) return value;
            try
            {
                if (type.IsEnum)
                {
                    var chars = type.GetCustomAttributes(typeof (CharEnumAttribute), true);
                    if (chars.Length >0)
                    {
                        var c = Convert.ChangeType(value, typeof(char));
                        var i = Convert.ChangeType(c, typeof(long));
                        if (i != null)
                            return Enum.Parse(type, i.ToString());
                    }
                    return Enum.Parse(type, value.ToString());
                }
                if (type == typeof (bool))
                {
                    if (value.ToString().Equals("1")) return true;
                    if (value.ToString().Equals("0")) return false;
                }
                return Convert.ChangeType(value, type);
            }
            catch
            {
                return value;
            }
        }
       

        #endregion

   


    }
}

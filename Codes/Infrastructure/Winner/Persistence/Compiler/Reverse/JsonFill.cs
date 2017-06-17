using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using Winner.Persistence.Compiler.Common;
using Winner.Persistence.Relation;

namespace Winner.Persistence.Compiler.Reverse
{

    public class JsonFill : Fill
    {
        public override T Reverse<T>(System.Data.IDataReader reader, OrmObjectInfo obj)
        {
            var arry = new List<string>();
            while (reader.Read())
            {
                var properties = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    AppendJsonObject(obj, properties, reader.GetName(i), reader[i]);
                }
                arry.Add(JsonConvert.SerializeObject(properties));
            }
            var names = obj.ObjectName.Split(',');
            var typeName = string.Format("{0}[],{1}",names[0],names[1]);
            var type = Type.GetType(typeName);
            return (T)JsonConvert.DeserializeObject(string.Format("[{0}]", string.Join(",", arry)), type);
           
        }

        /// <summary>
        /// 拼接json
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected virtual void AppendJsonObject(OrmObjectInfo obj, IDictionary<string, object> properties, string name,
                                                object value)
        {
            var propertyName = GetPropertyName(name);
            var chainPropertys = obj.GetChainProperties(propertyName);
            OrmPropertyInfo property = chainPropertys.Count > 0 ? chainPropertys[chainPropertys.Count - 1] : null;
            if (property == null) return;
            if (!property.AllowRead) return;
            if (value == DBNull.Value && property.HasReadNullValue)
                value = property.ReadNullValue;
            if (value == DBNull.Value)
                return;
            SetPropertyDictionary(properties, propertyName);
            var names = propertyName.Split('.');
            var tempProperties = GetPropertyDictionary(properties, names);
            if (property.Map != null && property.Map.MapType == OrmMapType.OneToMany)
            {
                if (!tempProperties.ContainsKey(names[names.Length - 1]))
                    tempProperties.Add(names[names.Length - 1],
                                       GetManyProperties(property.Map.GetMapObject(), value.ToString()));
            }
            else if (!tempProperties.ContainsKey(names[names.Length - 1]))
            {
                tempProperties.Add(names[names.Length - 1], GetPropertyTypeByName(obj, propertyName).TryConvertValue(value));
            }
        }

        /// <summary>
        /// 得到词典
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> GetPropertyDictionary(IDictionary<string, object> properties, string[] names)
        {
            if (names.Length==1)
                return properties;
            var result = properties;
            for (int i = 0; i < names.Length-1; i++)
            {
                result = result[names[i]] as IDictionary<string, object>;
            }
            return result;
        }
        /// <summary>
        /// 设置属性词典
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="propertyName"></param>
        protected virtual void SetPropertyDictionary(IDictionary<string, object> properties, string propertyName)
        {
            var tempProperties = properties;
            var pNames = propertyName.Split('.');
            for (int j = 0; j < pNames.Length - 1; j++)
            {
                if (!tempProperties.ContainsKey(pNames[j]))
                {
                    var val = new Dictionary<string, object>();
                    tempProperties.Add(pNames[j], val);
                    tempProperties = val;
                }
                else
                {
                    tempProperties = tempProperties[pNames[j]] as IDictionary<string, object>;
                }
            }
        }

        /// <summary>
        /// 得到子集合属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual ArrayList GetManyProperties(OrmObjectInfo obj, string value)
        {
            var elements = new ArrayList();
            var doc = new XmlDocument();
            doc.LoadXml(string.Format("<sysroot>{0}</sysroot>", value));
            foreach (XmlNode node in doc.FirstChild.ChildNodes)
            {
                var properties = new Dictionary<string, object>();
                foreach (XmlNode nd in node.ChildNodes)
                {
                    AppendJsonObject(obj, properties, nd.Name, nd.ChildNodes.Count == 1 && nd.FirstChild.NodeType == XmlNodeType.Text?nd.InnerText: nd.InnerXml);
                }
                elements.Add(properties);
            }
            return elements;
        }
        /// <summary>
        /// 得到属性类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual Type GetPropertyTypeByName(OrmObjectInfo obj, string name)
        {
            var names = name.Split('.');
            Type property = Type.GetType(obj.ObjectName).GetProperties().First(it => it.Name.Equals(names[0])).PropertyType;
            for (int i = 1; i < names.Length; i++)
            {
                property = property.GetProperties().First(it => it.Name.Equals(names[i])).PropertyType;
            }
            return property;
        }
    }
}

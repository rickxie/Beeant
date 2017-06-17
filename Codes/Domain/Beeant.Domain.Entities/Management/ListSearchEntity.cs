using System;
using System.Collections.Generic;
using System.Xml;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Management
{
    [Serializable]
    public class ListSearchEntity : BaseEntity<ListSearchEntity>
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 得到控件
        /// </summary>
        public IDictionary<string, string> Controls
        {
            get 
            {
                var controls = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(Detail)) return controls;
                var doc = new XmlDocument();
                doc.LoadXml(string.Format("<root>{0}</root>", Detail));
                foreach (XmlNode node in doc.FirstChild.ChildNodes)
                {
                    if(node.Attributes==null)continue;
                    controls.Add(node.Attributes["Id"].Value,node.InnerText);
                }
                return controls;
            }
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public virtual bool AddControl(string id, string value)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(value)) return false;
            Detail = string.Format("{0}<Control Id=\"{1}\">{2}</Control>", Detail, id, value);
            return true;
        }
    }
}

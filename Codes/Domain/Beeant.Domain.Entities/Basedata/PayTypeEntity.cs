using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class PayTypeEntity : BaseEntity<PayTypeEntity>
    {
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 线上方式
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 设置地址
        /// </summary>
        public virtual void SetUrl()
        {
            var url = Url;
            if (string.IsNullOrWhiteSpace(url) || Url.ToLower().StartsWith("http"))
                return;
            var index = Url.IndexOf("/");
            if (index == -1)
            {
                url = Configuration.ConfigurationManager.GetSetting<string>(url);
            }
            else
            {
                url = Url.Substring(0, index);
                url = Configuration.ConfigurationManager.GetSetting<string>(url);
                url = string.Format("{0}{1}", url, Url.Substring(index));
            }
            Url = url;
        }
    }
}

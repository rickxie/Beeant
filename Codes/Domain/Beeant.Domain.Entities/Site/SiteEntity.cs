using System;
using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Site
{
    [Serializable]
    public class SiteEntity : BaseEntity<SiteEntity>
    {
        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 图标路径
        /// </summary>
        public string FaviconFileName { get; set; }
        /// <summary>
        /// logo地址
        /// </summary>
        
        public string LogoFileName { get; set; }
        /// <summary>
        /// 图标路径
        /// </summary>
        public string LogoFullFileName
        {
            get { return this.GetFullFileName(LogoFileName); }
        }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] LogoFileByte { get; set; }
        /// <summary>
        /// 图标路径
        /// </summary>
        public string FaviconFullFileName
        {
            get { return this.GetFullFileName(FaviconFileName); }
        }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FaviconFileByte { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
 

        #region 配置属性
        /// <summary>
        /// 是否开启手机端
        /// </summary>
        public string WechatToken
        {
            get { return GetSetting<string>("WechatToken"); }
            set { SetSetting("WechatToken", value); }
        }
        /// <summary>
        /// 是否开启手机端
        /// </summary>
        public string WechatAppId
        {
            get { return GetSetting<string>("WechatAppId"); }
            set { SetSetting("WechatAppId", value); }
        }
        /// <summary>
        /// 是否开启手机端
        /// </summary>
        public string WechatSecret
        {
            get { return GetSetting<string>("WechatSecret"); }
            set { SetSetting("WechatSecret", value); }
        }
        /// <summary>
        /// 是否开启手机端
        /// </summary>
        public bool IsOpenMobile
        {
            get { return GetSetting<bool>("IsOpenMobile"); }
            set { SetSetting("IsOpenMobile", value); }
        }
        /// <summary>
        /// 是否开启手机端
        /// </summary>
        public bool IsOpenWebsite
        {
            get { return GetSetting<bool>("IsOpenWebsite"); }
            set { SetSetting("IsOpenWebsite", value); }
        }
        /// <summary>
        /// 是否显示作者
        /// </summary>
        public bool IsShowAuthor
        {
            get { return GetSetting<bool>("IsShowAuthor"); }
            set { SetSetting("IsShowAuthor", value); }
        }
        /// <summary>
        /// 是否启用密保
        /// </summary>
        public bool IsPassword
        {
            get { return GetSetting<bool>("IsPassword"); }
            set { SetSetting("IsPassword", value); }
        }
        /// <summary>
        /// 是否开通细节图
        /// </summary>
        public bool IsOpenImages
        {
            get { return GetSetting<bool>("IsOpenImages"); }
            set { SetSetting("IsOpenImages", value); }
        }
        /// <summary>
        /// 是否开消息
        /// </summary>
        public bool IsOpenMessage
        {
            get { return GetSetting<bool>("IsOpenMessage"); }
            set { SetSetting("IsOpenMessage", value); }
        }
        /// <summary>
        /// 是否开启一件分享
        /// </summary>
        public bool IsOpenMultiShare
        {
            get { return GetSetting<bool>("IsOpenMultiShare"); }
            set { SetSetting("IsOpenMultiShare", value); }
        }
        /// <summary>
        /// 是否开启一件分享
        /// </summary>
        public bool IsOpenSubscribeUser
        {
            get { return GetSetting<bool>("IsOpenSubscribeUser"); }
            set { SetSetting("IsOpenSubscribeUser", value); }
        }
        /// <summary>
        /// 是否启用密保
        /// </summary>
        public string MobileStyle
        {
            get { return GetSetting<string>("MobileStyle"); }
            set { SetSetting("MobileStyle", value); }
        }
        /// <summary>
        /// 是否启用密保
        /// </summary>
        public string WebsiteStyle
        {
            get { return GetSetting<string>("WebsiteStyle"); }
            set { SetSetting("WebsiteStyle", value); }
        }
        /// <summary>
        /// 是否启用图册
        /// </summary>
        public bool IsPrint
        {
            get { return GetSetting<bool>("IsPrint"); }
            set { SetSetting("IsPrint", value); }
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public string IsShowAuthorName
        {
            get { return this.GetStatusName(IsShowAuthor); }
        }
        /// <summary>
        /// 是否启用密保
        /// </summary>
        public string IsPasswordName
        {
            get { return this.GetStatusName(IsPassword); }
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public string IsOpenMobileName
        {
            get { return this.GetStatusName(IsOpenMobile); }
        }
        /// <summary>
        /// 是否启用密保
        /// </summary>
        public string IsOpenWebsiteName
        {
            get { return this.GetStatusName(IsOpenWebsite); }
        }
       

        private string _setting;

        /// <summary>
        /// 设置
        /// </summary>
        public string Setting
        {
            get
            {
                if (string.IsNullOrEmpty(_setting) && SettingDictionary != null)
                    _setting = SettingDictionary.SerializeJson();
                return _setting;
            }
            set
            {
                _setting = value;
                if (string.IsNullOrEmpty(value))
                    return;
                SettingDictionary = value.DeserializeJson<Dictionary<string, object>>();
            }
        }
        public IDictionary<string, object> SettingDictionary { get; set; }

        /// <summary>
        /// 得到设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T GetSetting<T>(string key)
        {
            if (SettingDictionary == null || !SettingDictionary.ContainsKey(key))
                return default(T);
            return SettingDictionary[key].Convert<T>();
        }


        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetSetting(string key, object value)
        {
            SettingDictionary = SettingDictionary ?? new Dictionary<string, object>();
            if (SettingDictionary.ContainsKey(key))
                SettingDictionary[key] = value;
            else
                SettingDictionary.Add(key, value);
        }


        #endregion
    }
}

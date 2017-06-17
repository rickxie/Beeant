using System;
using System.Collections.Generic;
using Component.Extension;
using Beeant.Domain.Entities.Account;

namespace Beeant.Domain.Entities.Crm
{
    [Serializable]
    public class CrmEntity : BaseEntity<CrmEntity>
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
        /// 有效日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
       
 

        #region 配置属性
        /// <summary>
        /// 是否开启手机端
        /// </summary>
        public int StaffCount
        {
            get { return GetSetting<object>("StaffCount").Convert<int>(); }
            set { SetSetting("StaffCount", value); }
        }
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

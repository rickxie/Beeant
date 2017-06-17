using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Account;
using Component.Extension;

namespace Beeant.Domain.Entities.Agent
{
    [Serializable]
    public class AgentEntity : BaseEntity<AgentEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public AccountEntity Account { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        #region 配置属性

        /// <summary>
        /// 加价幅度
        /// </summary>
        public decimal Discount
        {
            get { return GetSetting<object>("Discount").Convert<decimal>(); }
            set { SetSetting("Discount", value); }
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

        /// <summary>
        /// 得到价格
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public virtual decimal GetPrice(decimal price)
        {
            return Discount *price;
        }

    }
}

using System;
using System.Collections.Generic;
using Component.Extension;
using Winner;
using Winner.Base;

namespace Beeant.Domain.Entities.Account
{


    [Serializable]
    public class AccountEntity : BaseEntity<AccountEntity>
    {
 
        /// <summary>
        /// 预收款
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Payword { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 是否激活手机
        /// </summary>
        public bool IsActiveMobile { get; set; }
        /// <summary>
        /// 是否激活邮箱
        /// </summary>
        public bool IsActiveEmail { get; set; }
        /// <summary>
        /// 是否实名
        /// </summary>
        public bool IsReality { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetVerifyName(IsUsed); }
        }
        /// <summary>
        /// 是否激活手机
        /// </summary>
        public string IsActiveMobileName
        {
            get { return this.GetStatusName(IsActiveMobile); }
        }
        /// <summary>
        /// 是否激活邮箱
        /// </summary>
        public string IsActiveEmailName
        {
            get { return this.GetStatusName(IsActiveEmail); }
        }
        /// <summary>
        /// 是否激活邮箱
        /// </summary>
        public string IsRealityName
        {
            get { return this.GetStatusName(IsReality); }
        }
        /// <summary>
        /// 得到加密密码
        /// </summary>
        /// <returns></returns>
        public virtual void SetEncryptPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                Password = "";
                return;
            }
            var pass = string.IsNullOrEmpty(Password)
                                       ? "" : Creator.Get<ISecurity>().EncryptMd5(Password);
            pass = string.Format("{0}{1}{2}", pass.Substring(1, pass.Length - 2),
                pass.Substring(0, 1), pass.Substring(pass.Length - 1, 1));
            Password = Creator.Get<ISecurity>().Encrypt3Des(pass, Configuration.ConfigurationManager.GetSetting<string>("Keys").DeserializeJson<dynamic>().AccountKey.ToString());

        }
        /// <summary>
        /// 得到加密密码
        /// </summary>
        /// <returns></returns>
        public virtual void SetEncryptPayword()
        {
            if (string.IsNullOrEmpty(Payword))
            {
                Payword = "";
                return;
            }
            var pass = string.IsNullOrEmpty(Payword)
                                       ? "" : Creator.Get<ISecurity>().EncryptMd5(Payword);
            pass = string.Format("{0}{1}{2}", pass.Substring(1, pass.Length - 2),
                pass.Substring(0, 1), pass.Substring(pass.Length - 1, 1));
            Payword = Creator.Get<ISecurity>().Encrypt3Des(pass, Configuration.ConfigurationManager.GetSetting<string>("Keys").DeserializeJson<dynamic>().AccountKey.ToString());

        }
        /// <summary>
        /// 账户编号
        /// </summary>
        public virtual IList<AccountNumberEntity> AccountNumbers { get; set; }
        /// <summary>
        /// 身份信息
        /// </summary>
        public virtual IList<AccountIdentityEntity> AccountIdentites { get; set; }

    }
    
}

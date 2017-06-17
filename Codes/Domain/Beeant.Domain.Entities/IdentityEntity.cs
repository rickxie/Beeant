using System.Collections.Generic;
using Component.Extension;

namespace Beeant.Domain.Entities
{
    public class IdentityEntity
    {
        public const string LockTag = "IdentityLogin";
        public const string MobileLoginTag = "MobileLogin";
        /// <summary>
        /// Id
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 对应编号
        /// </summary>
        public IDictionary<string,string> Numbers { get; set; } 
    }

    public static class IdentityEntityExtension
    {
        public static T GetNumber<T>(this IdentityEntity identity,string key)
        {
            if (identity.Numbers == null || !identity.Numbers.ContainsKey(key))
                return default(T);
            return identity.Numbers[key].Convert<T>();
        }
    }
}

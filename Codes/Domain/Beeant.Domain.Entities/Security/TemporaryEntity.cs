using System;
using Component.Extension;

namespace Beeant.Domain.Entities.Security
{
    [Serializable]
    public class TemporaryEntity : BaseEntity<TemporaryEntity>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 锁住截止时间
        /// </summary>
        public DateTime EffectiveTime { get; set; }

        protected override void SetAddBusiness()
        {
            if (EffectiveTime == DateTime.MinValue)
            {
                var json =
                    Configuration.ConfigurationManager.GetSetting<string>("SecurityTemporary")
                        .DeserializeJson<dynamic>();
                int time = json == null || json.CacheTime == null ? 1800 : json.CacheTime;
                EffectiveTime = DateTime.Now.AddSeconds(time);
            }
        }
    }
}

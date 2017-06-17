using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class CurrencyEntity : BaseEntity<CurrencyEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 货币代码
        /// </summary>
        public string Code { get; set; }
    }
}

using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class CountryEntity : BaseEntity<CountryEntity>
    {
        /// <summary>
        /// 中文名
        /// </summary>
        public string Name { get; set; }
        
    }
}

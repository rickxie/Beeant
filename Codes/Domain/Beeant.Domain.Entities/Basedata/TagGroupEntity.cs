using System;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class TagGroupEntity : BaseEntity<TagGroupEntity>
    {
        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

    }
}

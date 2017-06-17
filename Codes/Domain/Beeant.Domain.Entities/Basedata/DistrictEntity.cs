using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Basedata
{
    [Serializable]
    public class DistrictEntity : BaseEntity<DistrictEntity>
    {
        /// <summary>
        ///父菜单
        /// </summary>
        public DistrictEntity Parent { get; set; }
   
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        public string Pinyin { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
   
        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsUsedName
        {
            get { return this.GetStatusName(IsUsed); }
        }
        /// <summary>
        ///子类
        /// </summary>
        public IList<DistrictEntity> Children { get; set; }
 
   
    }
}

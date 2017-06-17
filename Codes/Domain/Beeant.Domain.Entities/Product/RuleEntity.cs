using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class RuleEntity : BaseEntity<RuleEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 规则
        /// </summary>
        public string Pattern { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否范围验证
        /// </summary>
        public bool IsRange { get; set; }
        /// <summary>
        /// 是否范围验证
        /// </summary>
        public string IsRangeName
        {
            get { return this.GetStatusName(IsRange); }
        }
        /// <summary>
        /// 规则
        /// </summary>
        public IList<PropertyRuleEntity> PropertyRules { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class PropertyEntity : BaseEntity<PropertyEntity>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类目
        /// </summary>
        public CategoryEntity Category { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public PropertyType Type { get; set; }
        /// <summary>
        /// 是否SKU
        /// </summary>
        public bool IsSku { get; set; }
        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool IsAllowEdit { get; set; }
        /// <summary>
        /// 搜索类型
        /// </summary>
        public PropertySearchType SearchType { get; set; }
        /// <summary>
        /// 错误提醒
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 自定义扩展数量
        /// </summary>
        public int CustomCount { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 搜索值
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUsed { get; set; }
        /// <summary>
        /// 值集合
        /// </summary>
        public string[] ValueArray
        {
            get
            {
                if (string.IsNullOrEmpty(Value))
                    return null;
                return Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        /// <summary>
        /// 搜索值集合
        /// </summary>
        public string[] SearchValueArray
        {
            get
            {
                if (string.IsNullOrEmpty(SearchValue))
                    return null;
                return SearchValue.Split(',');
            }
        }
        /// <summary>
        /// 规则
        /// </summary>
        public IList<PropertyRuleEntity> PropertyRules { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get
            {
                return Type.GetName();
            }
        }
        /// <summary>
        /// 是否SKU
        /// </summary>
        public string IsSkuName
        {
            get
            {
                return this.GetStatusName(IsSku);
            }
        }
        /// <summary>
        /// 是否SKU
        /// </summary>
        public string IsAllowEditName
        {
            get
            {
                return this.GetStatusName(IsAllowEdit);
            }
        }

        /// <summary>
        /// 是否搜索
        /// </summary>
        public string SearchTypeName
        {
            get { return SearchType.GetName(); }
        }
        /// <summary>
        /// 是否下单选择项
        /// </summary>
        public string IsUsedName
        {
            get
            {
                return this.GetStatusName(IsUsed);
            }
        }

        public const string SearchSavePropertyName = "Properties";
    }
}

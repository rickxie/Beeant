using System;
using Component.Extension;

namespace Beeant.Domain.Entities.Product
{
    [Serializable]
    public class PropertyRuleEntity : BaseEntity<PropertyRuleEntity>
    {

        /// <summary>
        /// 属性
        /// </summary>
        public PropertyEntity Property { get; set; }
        /// <summary>
        /// 规则
        /// </summary>
        public RuleEntity Rule { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Paramter { get; set; }
        /// <summary>
        /// 错误提示
        /// </summary>
        public string Message { get; set; }

        private int _type;

        /// <summary>
        /// 验证规则
        /// </summary>
        public int Type
        {

            get { return _type; }
            set
            {
                _type = value;
                _typeValue = value.GetEnumComboStringValue<PropertyRuleType>();
            }
        }

        /// <summary>
        /// 是否多行
        /// </summary>
        public bool IsMultiline { get; set; }
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        public bool IsIgnoreCase { get; set; }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public string TypeName
        {
            get
            {
                return  Type.GetEnums<PropertyRuleType>().BuildeName();
            }
        }

        private string _typeValue = "";
        /// <summary>
        /// 值
        /// </summary>
        public string TypeValue
        {
            set 
            {
                _typeValue = value;
                _type = value.GetEnumSumValue<PropertyRuleType>();
            }
            get
            {
                return _typeValue;
            }
        }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public string IsMultilineName
        {
            get
            {
                return this.GetStatusName(IsMultiline);
            }
        }
        /// <summary>
        /// 是否自定义
        /// </summary>
        public string IsIgnoreCaseName
        {
            get
            {
                return this.GetStatusName(IsIgnoreCase);
            }
        }

        /// <summary>
        /// 得到表达式
        /// </summary>
        /// <returns></returns>
        public virtual string GetPattern()
        {
            if (Rule == null) return null;
            if (string.IsNullOrEmpty(Paramter)) return Rule.Pattern;
            var arr = Paramter.Replace("\r","").Split('\n');
            var pattern = Rule.Pattern;
            for (int i = 0; i < arr.Length; i++)
            {
                pattern = pattern.Replace(string.Format("P{0}", i), arr[i]);
            }
            return pattern;
        }

    }
}

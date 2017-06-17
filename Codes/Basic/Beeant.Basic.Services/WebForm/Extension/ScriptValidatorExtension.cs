using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Relation;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class ScriptValidatorExtension
    {


        /// <summary>
        /// 输出到页面
        /// </summary>
        /// <returns></returns>
        public static string Initialize<T>( IList<string> propertyNames,SaveType saveType)
        {
            var ormObject = Winner.Creator.Get<IOrm>().GetOrm(typeof(T).FullName);
            var validations = Winner.Creator.Get<IValidation>().GetValidations(typeof(T).FullName);
            var infos = new List<string>();
            foreach (var propertyName in propertyNames)
            {
                AppendValidationScript(infos,saveType, ormObject.Properties.FirstOrDefault(it => it.PropertyName.Equals(propertyName))
                    , validations.FirstOrDefault(it => it.PropertName.Equals(propertyName)));
            }
            return string.Join(",", infos.ToArray());
        }

        /// <summary>
        /// 加载验证信息
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="saveType"></param>
        /// <param name="ormProperty"></param>
        /// <param name="validation"></param>
        private static void AppendValidationScript(IList<string> infos,SaveType saveType, OrmPropertyInfo ormProperty, ValidationInfo validation)
        {
            if (validation == null) return;
            var isEnable = true;
            if (ormProperty != null)
                isEnable = saveType == SaveType.Add && ormProperty.AllowAdd
                           || saveType == SaveType.Modify && ormProperty.AllowModify
                           || saveType == SaveType.Remove && ormProperty.AllowRemove;
            if (!isEnable) return;
            var builder = new StringBuilder();
            builder.Append("{Name:");
            builder.AppendFormat("'{0}',Message:", validation.PropertName);
            builder.AppendFormat("'{0}',Rules:[{1}]", validation.Message, GetRulesArray(saveType, validation));
            builder.Append("}");
            infos.Add(builder.ToString());
        }

        /// <summary>
        /// 得到验证正则表达式集合
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="validation"></param>
        /// <returns></returns>
        private static string GetRulesArray(SaveType saveType, ValidationInfo validation)
        {
            var validateType = ValidationType.Add;
            if (saveType == SaveType.Add) validateType = ValidationType.Add;
            if (saveType == SaveType.Modify) validateType = ValidationType.Modify;
            if (saveType == SaveType.Remove) validateType = ValidationType.Remove;
            var ruleBuilder = new StringBuilder();
            foreach (var rule in validation.Rules)
            {
                AppendRules(ruleBuilder, rule, validateType);
            }
            if (ruleBuilder.Length > 0) ruleBuilder.Remove(ruleBuilder.Length - 1, 1);
            return ruleBuilder.ToString();
        }

        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="rule"></param>
        /// <param name="type"></param>
        private static void AppendRules(StringBuilder ruleBuilder, RuleInfo rule, ValidationType type)
        {
            if (rule.ValidationTypes == null || !rule.ValidationTypes.Contains(type)) return;
            ruleBuilder.Append("{Pattern:");
            ruleBuilder.AppendFormat("'{0}'", rule.Pattern.Replace("\\", "\\\\"));
            ruleBuilder.Append(",Options:\"");
            if (rule.IsMultiline) ruleBuilder.Append("m");
            if (!rule.IsIgnoreCase) ruleBuilder.Append("i");
            ruleBuilder.Append("\",");
            ruleBuilder.AppendFormat("Message:\"{0}\"", rule.Message);
            ruleBuilder.AppendFormat(",IsRange:{0}", rule.IsRange.ToString().ToLower());
            ruleBuilder.Append("},");
        }

    }
}

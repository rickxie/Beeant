using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Relation;

namespace Beeant.Basic.Services.Mvc.Script
{
    public class ScriptValidator<T>
    {

        /// <summary>
        /// 验证类型
        /// </summary>
        public SaveType SaveType { get; set; }
        /// <summary>
        /// 验证的属性
        /// </summary>
        public string[] PropertyNames { get; set; }
        /// <summary>
        /// 实体
        /// </summary>
        protected OrmObjectInfo OrmObject { get; set; }
        /// <summary>
        /// 验证机制
        /// </summary>
        public IList<ValidationInfo> Validations { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="propertyNames"></param>
        public ScriptValidator(SaveType saveType, string[] propertyNames)
        {
            if (propertyNames == null || propertyNames.Length == 0) return;
            OrmObject = Winner.Creator.Get<IOrm>().GetOrm(typeof(T).FullName);
            Validations = Winner.Creator.Get<IValidation>().GetValidations(typeof(T).FullName);
            SaveType = saveType;
            PropertyNames = propertyNames;
        }

      
        /// <summary>
        /// 加载验证信息
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="ormProperty"></param>
        /// <param name="validation"></param>
        protected virtual void AppendValidationScript(IList<string> infos, OrmPropertyInfo ormProperty, ValidationInfo validation)
        {
            if(validation==null)return;
            var isEnable = true;
            if(ormProperty!=null)
                isEnable=SaveType == SaveType.Add && ormProperty.AllowAdd
                           || SaveType == SaveType.Modify && ormProperty.AllowModify
                           || SaveType == SaveType.Remove && ormProperty.AllowRemove;
            if (!isEnable) return;
            var builder = new StringBuilder();
            builder.Append("{Name:");
            builder.AppendFormat("'{0}',Message:", validation.PropertName);
            builder.AppendFormat("'{0}',Rules:[{1}]", validation.Message, GetRulesArray(validation));
            builder.Append("}");
            infos.Add(builder.ToString());
        }

        /// <summary>
        /// 得到验证正则表达式集合
        /// </summary>
        /// <param name="validation"></param>
        /// <returns></returns>
        protected virtual string GetRulesArray(ValidationInfo validation)
        {
            var validateType = ValidationType.Add;
            if (SaveType == SaveType.Add) validateType = ValidationType.Add;
            if (SaveType == SaveType.Modify) validateType = ValidationType.Modify;
            if (SaveType == SaveType.Remove) validateType = ValidationType.Remove;
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
        protected virtual void AppendRules(StringBuilder ruleBuilder, RuleInfo rule, ValidationType type)
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
        /// <summary>
        /// 输出到页面
        /// </summary>
        /// <returns></returns>
        public virtual MvcHtmlString Gets()
        {
            var infos = new List<string>();
            foreach (var propertyName in PropertyNames)
            {
                AppendValidationScript(infos,OrmObject.Properties.FirstOrDefault(it => it.PropertyName.Equals(propertyName))
                    ,Validations.FirstOrDefault(it=>it.PropertName.Equals(propertyName)));
            }
            return new MvcHtmlString(string.Format("[{0}]", string.Join(",",infos.ToArray())));
        }
        /// <summary>
        /// 输出到页面
        /// </summary>
        /// <returns></returns>
        public virtual MvcHtmlString Initialize(string clientContainerId, string clientSaveButtonId)
        {
        
            var infos = new List<string>();
            if (Validations != null)
            {
                foreach (var propertyName in PropertyNames)
                {
                    AppendValidationScript(infos,
                        OrmObject.Properties.FirstOrDefault(it => it.PropertyName.Equals(propertyName))
                        , Validations.FirstOrDefault(it => it.PropertName.Equals(propertyName)));
                }
            }
            var builder = new StringBuilder();
            builder.AppendFormat("var validateEntities=[{0}];", string.Join(",", infos.ToArray()));
            builder.Append("var validator = new Winner.Validator({ PropertyName: \"ValidateName\", StyleFile: \"\", IsShowMessage: false });");
            builder.AppendFormat("validator.Initialize(); validator.InitializeControl(validateEntities, $(\"#{0}\")[0]);", clientContainerId);
            if (!string.IsNullOrEmpty(clientSaveButtonId))
            {
                builder.AppendFormat("$(\"#{0}\").click(function()", clientSaveButtonId);
                builder.Append("{return validator.ValidateSubmit();});");
            }
            return new MvcHtmlString(builder.ToString());
        }
        /// <summary>
        /// 输出到页面
        /// </summary>
        /// <returns></returns>
        public virtual MvcHtmlString GetValidateEntities(string name)
        {
            var infos = new List<string>();
            foreach (var propertyName in PropertyNames)
            {
                AppendValidationScript(infos, OrmObject.Properties.FirstOrDefault(it => it.PropertyName.Equals(propertyName))
                    , Validations.FirstOrDefault(it => it.PropertName.Equals(propertyName)));
            }
            var builder = new StringBuilder();
            builder.AppendFormat("var {0}=[{1}];", name, string.Join(",", infos.ToArray()));
            return new MvcHtmlString(builder.ToString());
        }

    }
}

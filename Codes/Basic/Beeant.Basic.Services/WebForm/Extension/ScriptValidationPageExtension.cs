using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Beeant.Basic.Services.WebForm.Controls;
using Winner.Filter;
using Winner.Persistence.Relation;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class ScriptValidationPageExtension
    {


        #region 扩展方法

        /// <summary>
        /// 绑定客户端脚本验证
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="ormProperties"></param>
        /// <param name="validations"></param>
        /// <param name="type"></param>
        /// <param name="ctrlValidateName"></param>
        public static void BindControlClientValidate(ControlCollection controls, StringBuilder script,IList<OrmPropertyInfo> ormProperties, IList<ValidationInfo> validations, ValidationType type, string validatorName="validator",string ctrlValidateName="ValidateName")
        {
            foreach (Control ctrl in controls)
            {
                SetControlEnableAndClientValidate(ctrl, script, ormProperties, validations, type, validatorName, ctrlValidateName);
                if (ctrl.Controls.Count > 0)
                    BindControlClientValidate(ctrl.Controls, script,  ormProperties, validations, type, validatorName, ctrlValidateName );
            }
        }
        /// <summary>
        /// 得到验证脚本对象
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetValidateScript(Button saveButton)
        {
            var script = new StringBuilder();
            script.Append("var validator=new Winner.Validator(); validator.Initialize();");
            script.AppendFormat("validator.AddButtonClick(document.getElementById('{0}'));", saveButton.ClientID);
            return script;
        }

        /// <summary>
        /// 添加客户端验证脚本并设置控件是否可编辑
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="ormProperties"></param>
        /// <param name="validations"></param>
        /// <param name="type"></param>
        /// <param name="ctrlValidateName"></param>
        public static void SetControlEnableAndClientValidate(Control ctrl, StringBuilder script,  IList<OrmPropertyInfo> ormProperties, IList<ValidationInfo> validations, ValidationType type, string validatorName = "validator",string ctrlValidateName = "ValidateName")
        {
            string saveName = ctrl.GetAttributeValue("SaveName");
            string validateName = ctrl.GetAttributeValue(ctrlValidateName);
            validateName = validateName ?? saveName;
            if (EnableControlExtension.SetControlEnable(ctrl, ormProperties,  validateName , type) || !string.IsNullOrEmpty(validateName))
            {
                if (!ctrl.Visible) return;
                SetControlClientValidate(validateName ?? saveName, ctrl, script, validatorName,validations, type);
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 添加脚本验证
        /// </summary>
        /// <param name="validName"></param>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="validations"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static void SetControlClientValidate(string validName, Control ctrl, StringBuilder script,string validatorName,
                                                     IList<ValidationInfo> validations, ValidationType type)
        {
            if (string.IsNullOrEmpty(validName)) return;
            foreach (
                var validate in
                    validations.Where(
                        validate =>
                        validate.PropertName.Equals(validName) &&
                        validate.Rules.Count(it => it.ValidationTypes.Contains(type)) > 0))
            {
                InvokeSetClientValidate(ctrl, script, validatorName, validations, validate, type);
                break;
            }
        }

        /// <summary>
        /// 验证上传控件
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="validations"></param>
        /// <param name="type"></param>
        private static void SetUploaderClientValidate(UploaderControlBase ctrl, StringBuilder script,string validatorName, IList<ValidationInfo> validations, ValidationType type)
        {
            var validate = validations.FirstOrDefault(it => it.PropertName.Equals(ctrl.FileNameSaveName));
            if(validate==null)
                return;
            script.AppendFormat("{0}.AddFileRegularValidate", validatorName);
            script.Append("({");
            script.AppendFormat("Control:document.getElementById('{0}'),Rules:[{1}],Message:'{2}',IsEventValidate:true", ctrl.FileByteControl.ClientID, GetRulesArray(validate, type), validate.Message);
            script.Append("});");
            var byteValidate = validations.FirstOrDefault(it => it.PropertName.Equals(ctrl.FileByteSaveName));
            if(byteValidate==null)
                return;
            script.AppendFormat("{0}.AddFileSizeValidate", validatorName);
            script.Append("({");
            script.AppendFormat("Control:document.getElementById('{0}'),Rules:[{1}],Message:'{2}',IsEventValidate:true", ctrl.FileByteControl.ClientID, GetRulesArray(byteValidate, type, byteValidate.Message), byteValidate.Message);
            script.Append("});");
             
            
        }

        /// <summary>
        /// 调用客户端验证
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="validates"></param>
        /// <param name="validate"></param>
        /// <param name="type"></param>
        private static void InvokeSetClientValidate(Control ctrl, StringBuilder script, string validatorName,IList<ValidationInfo> validates, ValidationInfo validate, ValidationType type)
        {
            if (validate == null || validate.Rules == null || validate.Rules.Count == 0) return;
            if (ctrl.Parent.Parent is UploaderControlBase)
            {
                if (ctrl is HtmlInputFile)
                    SetUploaderClientValidate(ctrl.Parent.Parent as UploaderControlBase, script, validatorName, validates, type);
            }
            else if (ctrl is HtmlInputFile)
                SetFileClientValidate(ctrl, script, validatorName, validate, type);
            else if (ctrl is HtmlInputText || ctrl is TextBox || ctrl is HtmlTextArea || ctrl is HtmlInputHidden)
                SetInputClientValidate(ctrl, script, validatorName, validate, type);
            else if (ctrl is DropDownList)
                SetDropDownListClientValidate(ctrl, script, validatorName, validate, type);
            else if (ctrl is CheckBoxList)
                SetCheckBoxListClientValidate(ctrl, script, validatorName, validate, type);
        }

        /// <summary>
        /// 添加输入框验证
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="validate"></param>
        /// <param name="type"></param>
        private static void SetFileClientValidate(Control ctrl, StringBuilder script,string validatorName, ValidationInfo validate, ValidationType type)
        {
            script.AppendFormat("{0}.AddFileRegularValidate", validatorName);
            script.Append("({");
            script.AppendFormat("Control:document.getElementById('{0}'),Rules:[{1}],Message:'{2}',IsEventValidate:true", ctrl.ClientID, GetRulesArray(validate, type), validate.Message);
            script.Append("});");
        }

        /// <summary>
        /// 添加输入框验证
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="validate"></param>
        /// <param name="type"></param>
        private static void SetInputClientValidate(Control ctrl, StringBuilder script, string validatorName, ValidationInfo validate, ValidationType type)
        {
            script.AppendFormat("{0}.AddInputRegularValidate", validatorName);
            script.Append("({");
            script.AppendFormat("Control:document.getElementById('{0}'),Rules:[{1}],Message:'{2}',IsEventValidate:true", ctrl.ClientID, GetRulesArray(validate, type), validate.Message);
            script.Append("});");
        }


        /// <summary>
        /// 得到验证正则表达式集合
        /// </summary>
        /// <param name="validate"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string GetRulesArray(ValidationInfo validate, ValidationType type,string message=null)
        {
            var ruleBuilder = new StringBuilder();
            foreach (var rule in validate.Rules)
            {
                AppendRules(ruleBuilder, rule, type, message);
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
        /// <param name="message"></param>
        private static void AppendRules(StringBuilder ruleBuilder, RuleInfo rule, ValidationType type, string message)
        {
            if (rule.ValidationTypes == null || !rule.ValidationTypes.Contains(type)) return;
            ruleBuilder.Append("{Pattern:");
            ruleBuilder.AppendFormat("'{0}'", rule.Pattern.Replace("\\", "\\\\"));
            ruleBuilder.Append(",Options:\"");
            if (rule.IsMultiline) ruleBuilder.Append("m");
            if (!rule.IsIgnoreCase) ruleBuilder.Append("i");
            ruleBuilder.Append("\",");
            ruleBuilder.AppendFormat("Message:\"{0}\"",string.IsNullOrEmpty(rule.Message)?message:rule.Message);
            ruleBuilder.AppendFormat(",IsRange:{0}", rule.IsRange.ToString().ToLower());
            ruleBuilder.Append("},");
        }

        /// <summary>
        /// 添加下拉框验证
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="validate"></param>
        /// <param name="type"></param>
        private static void SetDropDownListClientValidate(Control ctrl, StringBuilder script,string validatorName, ValidationInfo validate, ValidationType type)
        {
            script.AppendFormat("{0}.AddDrowDownListValidate", validatorName);
            script.Append("({");
            script.AppendFormat("Control:document.getElementById('{0}'),Rules:[{1}],Message:'{2}',IsEventValidate:true", ctrl.ClientID, GetRulesArray(validate, type), validate.Message);
            script.Append("});");
        }

        /// <summary>
        /// 设置checkboxlist客户端验证
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="validatorName"></param>
        /// <param name="validate"></param>
        /// <param name="type"></param>
        private static void SetCheckBoxListClientValidate(Control ctrl, StringBuilder script, string validatorName,ValidationInfo validate, ValidationType type)
        {
            script.AppendFormat("{0}.AddCheckBoxListValidate", validatorName);
            script.Append("({");
            script.AppendFormat("Control:document.getElementById('{0}'),Rules:[{1}],Message:'{2}',IsEventValidate:true", ctrl.ClientID, GetRulesArray(validate, type), validate.Message);
            script.Append("});");
            var boxList = ctrl as CheckBoxList;
            if (boxList == null) return;
            foreach (ListItem li in boxList.Items)
            {
                li.Attributes.Add("RealValue", li.Value);
            }
        }


        #endregion


    }
}

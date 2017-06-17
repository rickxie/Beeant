using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Winner;
using Winner.Base;
using Winner.Filter;
using Winner.Persistence.Relation;


namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class BindPageExtension
    {
        #region 声明

        /// <summary>
        /// 绑定委托
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private delegate void SetControlValueHandler(Control ctrl, object value);

        #endregion

        #region 扩展方法

        /// <summary>
        /// 绑定对象
        /// </summary>
        /// <param name="page"></param>
        /// <param name="info"></param>
        /// <param name="saveButton"></param>
        /// <param name="validationType"></param>
        public static string BindEntity(this Page page, object info, Button saveButton, ValidationType validationType)
        {
            var orm = Creator.Get<IOrm>().GetOrm(info.GetType().FullName);
            return BindEntity(page, info, saveButton, validationType, orm == null ? null : orm.Properties);
        }

        /// <summary>
        /// 绑定对象
        /// </summary>
        /// <param name="page"></param>
        /// <param name="info"></param>
        /// <param name="saveButton"></param>
        /// <param name="validationType"></param>
        /// <param name="ormProperties"></param>
        public static string BindEntity(this Page page, object info, Button saveButton, ValidationType validationType, IList<OrmPropertyInfo> ormProperties)
        {
            if (info == null || page.Form == null)
                return "";
            var script = saveButton != null ? ScriptValidationPageExtension.GetValidateScript(saveButton) : new StringBuilder();
            IList<ValidationInfo> valids = Creator.Get<IValidation>().GetValidations(info.GetType());
            BindControlValue(info, page.Form.Controls, script, ormProperties, valids, validationType);
            return script == null ? "" : script.ToString();
        }
        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        public static void InvokeSetControlValueHandler(Control ctrl, object value)
        {
            IDictionary<Type, SetControlValueHandler> handlers = new Dictionary<Type, SetControlValueHandler>
                {
                {typeof(DropDownList),SetDropDownListValue}, {typeof(TextBox),SetTextBoxValue},{typeof(HtmlInputHidden),SetHtmlInputHiddenValue},
                {typeof(HtmlInputText),SetHtmlInputTextValue},{typeof(HtmlTextArea),SetHtmlTextAreaValue}, {typeof(CheckBoxList),SetCheckBoxListValue},
                {typeof(Label),SetLabelValue},{typeof(RadioButtonList),SetRadioButtonListValue},{typeof(HtmlAnchor),SetHtmlAnchorValue},
                {typeof(RadioButton),SetRadioButtonValue},{typeof(CheckBox),SetCheckBoxValue},{typeof(HtmlImage),SetHtmlImageValue}
            };
            if (handlers.ContainsKey(ctrl.GetType()))
                handlers[ctrl.GetType()](ctrl, value);
        }
        #endregion

        #region 绑定数据

        /// <summary>
        /// 绑定控件值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="controls"></param>
        /// <param name="script"></param>
        /// <param name="ormProperties"></param>
        /// <param name="valids"></param>
        /// <param name="type"></param>
        private static void BindControlValue(object info, ControlCollection controls, StringBuilder script, IList<OrmPropertyInfo> ormProperties, IList<ValidationInfo> valids, ValidationType type)
        {
            foreach (Control ctrl in controls)
            {
                SetControlValue(info, ctrl, script, ormProperties, valids, type);
                if (ctrl.Controls.Count > 0)
                    BindControlValue(info, ctrl.Controls, script, ormProperties, valids, type);
            }
        }

        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctrl"></param>
        /// <param name="script"></param>
        /// <param name="ormProperties"></param>
        /// <param name="valids"></param>
        /// <param name="type"></param>
        private static void SetControlValue(object info, Control ctrl, StringBuilder script, IList<OrmPropertyInfo> ormProperties, IList<ValidationInfo> valids, ValidationType type)
        {
            if (valids != null)
            {
                ScriptValidationPageExtension.SetControlEnableAndClientValidate(ctrl, script, ormProperties, valids, type);
            }
            object value = GetBindValue(info, ctrl);
            if (value == null)
                return;
            InvokeSetControlValueHandler(ctrl, value);
        }
      

        /// <summary>
        /// 设置下拉框值
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetDropDownListValue(Control ctrl, object value)
        {
            ((DropDownList) ctrl).SelectedIndex = ((DropDownList) ctrl).Items.IndexOf(((DropDownList) ctrl).Items.FindByValue(value.ToString()));
        }

        /// <summary>
        /// 设置TextBox
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetTextBoxValue(Control ctrl, object value)
        {
            ((TextBox)ctrl).Text = value.ToString();
        }
        /// <summary>
        /// 设置HtmlInputHidden
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetHtmlInputHiddenValue(Control ctrl, object value)
        {
            ((HtmlInputHidden)ctrl).Value = value.ToString();
        }
        /// <summary>
        /// 设置HtmlTextArea值
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetHtmlTextAreaValue(Control ctrl, object value)
        {
            ((HtmlTextArea)ctrl).Value = value.ToString();
        }
        /// <summary>
        /// 设置Label
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetLabelValue(Control ctrl, object value)
        {
            ((Label)ctrl).Text = value.ToString();
        }

        /// <summary>
        /// 设置HtmlInputText
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetHtmlInputTextValue(Control ctrl, object value)
        {
            ((HtmlInputText)ctrl).Value = value.ToString();
        }
        /// <summary>
        /// 设置图片路径
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetHtmlImageValue(Control ctrl, object value)
        {
            var src = value.ToString();
            src =string.IsNullOrEmpty(src)?"": string.Format(src.Contains("?") ? "{0}&{1}" : "{0}?{1}", src, DateTime.Now.ToString("yyyyMMddHHmmss"));
            ((HtmlImage)ctrl).Src = src;
        }
        /// <summary>
        /// 设置图片路径
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetHtmlAnchorValue(Control ctrl, object value)
        {

            ((HtmlAnchor)ctrl).HRef = string.Format("{0}{1}", ((HtmlAnchor)ctrl).HRef,value); 
        }
        /// <summary>
        /// 设置RadioButtonList
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetRadioButtonListValue(Control ctrl, object value)
        {
            ((RadioButtonList)ctrl).SelectedValue = value.ToString();
        }
        /// <summary>
        /// 设置RadioButton
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetRadioButtonValue(Control ctrl, object value)
        {
            bool result;
            bool.TryParse(value.ToString(), out result);
            ((RadioButton)ctrl).Checked = result;
        }
        /// <summary>
        /// 设置CheckBox
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetCheckBoxValue(Control ctrl, object value)
        {
            bool result;
            bool.TryParse(value.ToString(), out result);
            ((CheckBox)ctrl).Checked = result;
        }
        /// <summary>
        /// 设置CheckBoxList
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="value"></param>
        private static void SetCheckBoxListValue(Control ctrl, object value)
        {
            var list = ctrl as CheckBoxList;
            if(list==null) return;
            IList<string> arr = value is string[] ? (string[])value : value.ToString().Split(',');
            foreach (ListItem li in list.Items)
            {
                if (li.Value.IndexOf(",") >= 0)
                {
                    var sr = li.Value.Split(new char[] { ',' });
                    li.Selected = arr.Contains(sr[0]);
                }
                else
                    li.Selected = arr.Contains(li.Value);
            }

        }


        /// <summary>
        /// 得到绑定值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private static object GetBindValue(object info, Control ctrl)
        {
            string name = ctrl.GetAttributeValue("BindName");
            if (string.IsNullOrEmpty(name))
                return null;
            return GetFormatValue(info, name);
        }
        /// <summary>
        /// 得到格式化值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static object GetFormatValue(object info, string name)
        {
            var value = Creator.Get<IProperty>().GetValue<object>(info, name);
            if (value is DateTime && ((DateTime)value).Minute==0 && ((DateTime)value).Hour==0 && ((DateTime)value).Second==0 )
            {
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd");
            }
            return value;
        }
        #endregion



    }
}

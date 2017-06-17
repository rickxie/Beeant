using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Basic.Services.WebForm.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Winner.Base;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class SavePageExtension
    {
        #region 声明
        /// <summary>
        /// 得到控件值委托
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private delegate object GetControlValueHandler(Control ctrl);
        #endregion

        #region 扩展方法

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="isFillAllEntity"></param>
        /// <returns></returns>
        public static T GetEntity<T>(this Page page, bool isFillAllEntity) where T : BaseEntity, new()
        {
            if (page.Form == null) return null;
            var info = new T();
            FillEntity(info, page.Form.Controls, isFillAllEntity);
            return info;
        }

        /// <summary>
        /// 得到子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="itemsValues"></param>
        /// <returns></returns>
        public static IList<T> GetItemEntities<T>(this Page page, string itemsValues, SaveType SaveType) where T : BaseEntity, new()
        {
            if (string.IsNullOrWhiteSpace(itemsValues)) return null;
            IList<T> infos = new List<T>();
            var jarray = JsonConvert.DeserializeObject(itemsValues) as JArray;
            foreach (JObject jobject in jarray)
            {
                var info = jobject.ToString().DeserializeJson<T>();
                IEnumerable<JProperty> properties = jobject.Properties();
                foreach (JProperty jproperty in properties)
                {
                    if (jproperty.Name.Contains("."))
                    {
                        Winner.Creator.Get<IProperty>().SetValue(info, jproperty.Name, jproperty.Value.Value<string>());
                    }
                }

                info.SaveType = SaveType;                
                infos.Add(info);
            }
            return infos;
        }

        #endregion

        #region 填充对象

        /// <summary>
        /// 填充对象
        /// </summary>
        /// <param name="info"></param>
        /// <param name="controls"></param>
        /// <param name="isFillAllEntity"></param>
        private static void FillEntity(object info, ControlCollection controls, bool isFillAllEntity)
        {
            foreach (Control ctrl in controls)
            {
                SetEntityValue(info, ctrl, isFillAllEntity);
                if (ctrl.Controls.Count > 0)
                    FillEntity(info, ctrl.Controls, isFillAllEntity);
            }
        }

        /// <summary>
        /// 得到控件值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctrl"></param>
        /// <param name="isFillAllEntity"></param>
        /// <returns></returns>
        private static void SetEntityValue(object info, Control ctrl, bool isFillAllEntity)
        {
            string saveName = ctrl.GetAttributeValue("SaveName");
            if (ctrl is HtmlControl && ((HtmlControl)ctrl).Disabled && !(ctrl is HtmlInputHidden)) return;
            if (string.IsNullOrEmpty(saveName)) return;
            if (ctrl is WebControl && !((WebControl)ctrl).Enabled) return;
            object value = InvokeGetControlValueHandler(ctrl);
            string unSaveValue = ctrl.GetAttributeValue("UnSaveValue");
            if (value == null || (unSaveValue != null && unSaveValue.Equals(value.ToString()))) return;
            Winner.Creator.Get<IProperty>().SetValue(info, saveName, value);
            if (!isFillAllEntity)
                FillPartEntity(info, ctrl, saveName);
        }
        /// <summary>
        /// 填充部分
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctrl"></param>
        /// <param name="saveName"></param>
        private static void FillPartEntity(object info, Control ctrl, string saveName)
        {
            var ormSaveName = ctrl.GetAttributeValue("OrmSaveName");
            ormSaveName = string.IsNullOrEmpty(ormSaveName) ? saveName : ormSaveName;
            ((BaseEntity)info).SetProperty(ormSaveName);
        }


        /// <summary>
        /// 得到控件值
        /// </summary>
        /// <param name="ctrl"></param>
        public static object InvokeGetControlValueHandler(Control ctrl)
        {
            IDictionary<Type, GetControlValueHandler> handlers = new Dictionary<Type, GetControlValueHandler>
                {
                {typeof(DropDownList),GetDropDownListValue}, {typeof(TextBox),GetTextBoxValue},{typeof(HtmlInputHidden),GetHtmlInputHiddenValue},
                {typeof(HtmlInputText),GetHtmlInputTextValue},{typeof(HtmlTextArea),GetHtmlTextAreaValue},{typeof(HtmlInputPassword),GetHtmlInputPasswordValue},
                {typeof(CheckBoxList),GetCheckBoxListValue}, {typeof(RadioButtonList),GetRadioButtonListValue},
                {typeof(CheckBox),GetCheckBoxValue},{typeof(RadioButton),GetRadioButtonValue},{typeof(HtmlInputFile),GetHtmlInputFileValue}
            };
            if (handlers.ContainsKey(ctrl.GetType())) return handlers[ctrl.GetType()](ctrl);
            return null;
        }
        /// <summary>
        /// 得到下拉框值
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetDropDownListValue(Control ctrl)
        {
            return ((DropDownList)ctrl).SelectedValue;
        }
        /// <summary>
        /// 得到TextBox
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetTextBoxValue(Control ctrl)
        {
            return ((TextBox)ctrl).Text.Trim();
        }
        /// <summary>
        /// 得到HtmlInputPassword的值
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private static string GetHtmlInputPasswordValue(Control ctrl)
        {
            return ((HtmlInputPassword)ctrl).Value.Trim();
        }
        /// <summary>
        /// 得到HtmlInputHidden的值
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private static string GetHtmlInputHiddenValue(Control ctrl)
        {
            if (ctrl.GetAttributeValue("disabled") == "disabled")
                return null;
            if (ctrl.Parent == null)
                return ((HtmlInputHidden)ctrl).Value.Trim();
            var @base = ctrl.Parent.Parent as UploaderControlBase;
            if (@base == null) return ((HtmlInputHidden)ctrl).Value.Trim();
            return @base.GetFileName();
        }
        /// <summary>
        /// 得到HtmlInputText
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetHtmlInputTextValue(Control ctrl)
        {
            return ((HtmlInputText)ctrl).Value.Trim();
        }
        /// <summary>
        /// 得到HtmlTextArea
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetHtmlTextAreaValue(Control ctrl)
        {
            return ((HtmlTextArea)ctrl).Value.Trim();
        }
        /// <summary>
        /// 得到CheckBoxList
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetCheckBoxListValue(Control ctrl)
        {
            var t = ctrl as CheckBoxList;
            var value = new StringBuilder();
            if (t != null)
                foreach (ListItem li in t.Items)
                {
                    if (li.Selected)
                        value.AppendFormat("{0},", li.Value);
                }
            if (value.Length > 0) value.Remove(value.Length - 1, 1);
            return value.ToString();
        }
        /// <summary>
        /// 得到RadioButtonList
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetRadioButtonListValue(Control ctrl)
        {
            return ((RadioButtonList)ctrl).SelectedValue;
        }
        /// <summary>
        /// 得到CheckBox
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetCheckBoxValue(Control ctrl)
        {
            return ((CheckBox)ctrl).Checked.ToString();
        }
        /// <summary>
        /// 得到RadioButton
        /// </summary>
        /// <param name="ctrl"></param>
        private static string GetRadioButtonValue(Control ctrl)
        {
            return ((RadioButton)ctrl).Checked.ToString();
        }

        /// <summary>
        /// 得到RadioButton
        /// </summary>
        /// <param name="ctrl"></param>
        private static byte[] GetHtmlInputFileValue(Control ctrl)
        {
            if (((HtmlInputFile)ctrl).PostedFile == null)
                return null;
            if (ctrl.Parent == null)
            {
                var stream = ((HtmlInputFile)ctrl).PostedFile.InputStream;
                if (stream.Length != 0)
                {
                    var fileByte = new byte[stream.Length];
                    stream.Read(fileByte, 0, fileByte.Length);
                    return fileByte;
                }
                return null;
            }
            var @base = ctrl.Parent.Parent as UploaderControlBase;
            if (@base != null) return @base.GetFileByte();
            return null;
        }
        #endregion




    }
}

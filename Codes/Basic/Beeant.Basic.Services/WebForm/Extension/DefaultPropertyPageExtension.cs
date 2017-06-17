using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class DefaultPropertyPageExtension
    {
        #region 声明
        /// <summary>
        /// 设置默认属性委托
        /// </summary>
        /// <param name="ctrl"></param>
        private delegate void SetControlPropertyHandler(Control ctrl);

        #endregion

        #region 扩展方法

        /// <summary>
        /// 设置控件默认值
        /// </summary>
        /// <param name="page"></param>
        public static void SetControlProperty(this Page page)
        {
            if(page.Form==null)
                return;
            SetControlProperty(page.Form.Controls);
        }
       
        #endregion
 

        #region 设置默认值

        private static void SetControlProperty(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                InvokeSetControlPropertyHandler(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    SetControlProperty(ctrl.Controls);
                }
            }
        }
        /// <summary>
        /// 得到设置实例
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private static void InvokeSetControlPropertyHandler(Control ctrl)
        {
            IDictionary<Type, SetControlPropertyHandler> handlers = new Dictionary<Type, SetControlPropertyHandler>
                {
                {typeof(DropDownList),SetDropDownListProperty},{typeof(GridView),SetGridViewProperty},{typeof(CheckBoxList),SetCheckBoxListProperty}
            };
            if (handlers.ContainsKey(ctrl.GetType()))
                handlers[ctrl.GetType()](ctrl);
        }

        /// <summary>
        /// 设置下拉框
        /// </summary>
        /// <param name="ctrl"></param>
        private static void SetDropDownListProperty(Control ctrl)
        {
            var t = ctrl as DropDownList;
            if (t == null || (t.Items.Count > 0 && t.Items[0].Text == "=请选择="))
                return;
            t.Items.Insert(0, new ListItem("=请选择=", ""));
        }

        /// <summary>
        /// 设置girdview
        /// </summary>
        /// <param name="ctrl"></param>
        private static void SetGridViewProperty(Control ctrl)
        {
            var t = ctrl as GridView;
            if (t == null) return;
            t.AllowPaging = false;
            t.ShowFooter = false;
            t.EmptyDataText = "没有相关记录";
            t.ShowHeader = true;
            t.EmptyDataRowStyle.VerticalAlign = VerticalAlign.Middle;
            t.AutoGenerateColumns = false;
        }
        /// <summary>
        /// 设置CheckBoxList
        /// </summary>
        /// <param name="ctrl"></param>
        private static void SetCheckBoxListProperty(Control ctrl)
        {
            var t = ctrl as CheckBoxList;
            if (t != null) t.RepeatDirection = RepeatDirection.Horizontal;
        }

        #endregion 

  
    }
}

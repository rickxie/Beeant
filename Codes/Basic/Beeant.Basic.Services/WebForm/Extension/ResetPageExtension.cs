using System.Web.UI;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class ResetPageExtension
    {


        #region 扩展方法
        /// <summary>
        /// 清空数据
        /// </summary>
        /// <param name="page"></param>
        public static void ResetControl(this Page page)
        {
            ResetControlValue(page.Form.Controls);
        }

        #endregion

        #region 清空数据
        /// <summary>
        /// 重置控件值
        /// </summary>
        /// <param name="controls"></param>
        private static void ResetControlValue(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                InvokeResetControlValueHandler(ctrl);
                if (ctrl.Controls.Count > 0)
                    ResetControlValue(ctrl.Controls);
            }
        }
        /// <summary>
        /// 添加后重新设置控件值
        /// </summary>
        /// <param name="ctrl"></param>
        private static void InvokeResetControlValueHandler(Control ctrl)
        {
            if(string.IsNullOrEmpty(ctrl.GetAttributeValue("SaveName")))return;
            BindPageExtension.InvokeSetControlValueHandler(ctrl,GetResetControlValue(ctrl));

        }
        /// <summary>
        /// 得到控件值,如果没有默认值返回控
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private static string GetResetControlValue(Control ctrl)
        {
            string value = ctrl.GetAttributeValue("DefaultValue");
            if (string.IsNullOrEmpty(value))
                value = "";
            return value;
        }

        #endregion

    

  
    }
}

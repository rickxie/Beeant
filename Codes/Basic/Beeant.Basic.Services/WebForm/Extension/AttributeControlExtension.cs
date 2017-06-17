using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class AttributeControlExtension
    {
     

        #region 扩展方法

        /// <summary>
        /// 得到Attributes属性值
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this Control ctrl, string name)
        {
            System.Reflection.PropertyInfo p = ctrl.GetType().GetProperty("Attributes");
            if (p != null)
            {
                object value = p.GetValue(ctrl, null);
                if (value != null)
                {
                    return ((AttributeCollection)value)[name];
                }
            }
            return null;
        }
        /// <summary>
        /// 移除控件特性
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="name"></param>
        public static void RemoveAttributeValue(Control ctrl, string name)
        {
            var control = ctrl as WebControl;
            if (control != null)
                control.Attributes.Remove(name);
            else
            {
                var htmlControl = ctrl as HtmlControl;
                if (htmlControl != null) 
                    htmlControl.Attributes.Remove(name);
            }
        }
        #endregion


  
    }
}

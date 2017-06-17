using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Winner.Filter;
using Winner.Persistence.Relation;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class EnableControlExtension
    {
  
        #region 扩展方法

        /// <summary>
        /// 设置控件是否可以编辑
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ormProperties"></param>
        /// <param name="saveName"></param>
        /// <param name="type"></param>
        public static bool SetControlEnable(Control ctrl, IList<OrmPropertyInfo> ormProperties, string saveName, ValidationType type)
        {
            if (ormProperties== null || string.IsNullOrEmpty(saveName)) return true;
            var property = ormProperties.FirstOrDefault(it => it.PropertyName == saveName);
            if (property != null)
            {
                var isEnabled = (ValidationType.Add == type && property.AllowAdd) || (ValidationType.Modify == type && property.AllowModify);
                SetControlEditStatus(ctrl, isEnabled);
                return isEnabled;
            }
            return true;
        }
        /// <summary>
        /// 设置控件是否可以编辑
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="isEnabled"></param>
        public static void SetControlEditStatus(Control ctrl, bool isEnabled)
        {
            var control = ctrl as WebControl;
            if (control != null) control.Enabled = isEnabled;
            else
            {
                var htmlControl = ctrl as HtmlControl;
                if (htmlControl != null) htmlControl.Disabled = !isEnabled;
            }
            if (!isEnabled) AttributeControlExtension.RemoveAttributeValue(ctrl, "SaveName");
        }
        #endregion


  
    }
}

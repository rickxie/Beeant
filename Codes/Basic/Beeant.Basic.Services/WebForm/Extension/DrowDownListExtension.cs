using System.Web.UI.WebControls;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class DrowDownListExtension
    {
     

        #region 扩展方法

        /// <summary>
        /// 得到Attributes属性值
        /// </summary>
        /// <param name="drowDownList"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static void DataBind(this DropDownList drowDownList,object source)
        {
            drowDownList.DataSource = source;
            drowDownList.DataBind();
            drowDownList.Items.Insert(0,new ListItem("=请选择=",""));
        }
      
        #endregion


  
    }
}

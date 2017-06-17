using System;
using System.Web;
using System.Web.UI;

namespace Beeant.Basic.Services.WebForm.Extension
{
    /// <summary>
    ///Script主要分装客户端语句
    /// </summary>
    static public class ScriptControlExtension
    {

        #region 扩展方法

        /// <summary>
        /// 得到跳出框脚本
        /// </summary>
        /// <param name="control"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetShowMessageScript(this Control control,string subject, string message)
        {
            return string.Format("var dialog=new Winner.Dialog(\"{0}\",\"{1}\");dialog.Initialize();", subject.Replace("\"", "\\\"").Replace("\r\n", ""), message.Replace("\"", "\\\"").Replace("\r\n", ""));
        }

        /// <summary>
        /// 现在消息
        /// </summary>
        /// <param name="control"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="isScriptManager"></param>
        public static void ShowMessage(this Control control, string subject, string message, bool isScriptManager = true)
        {
            if (isScriptManager)
                ScriptManager.RegisterStartupScript(control, control.GetType(), "", GetShowMessageScript(control, subject, message), true);
            else
                ((Page)HttpContext.Current.Handler).Page.ClientScript.RegisterClientScriptBlock(typeof(Page), Guid.NewGuid().ToString(), GetShowMessageScript(control, subject, message), true);
        }

        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="control"></param>
        /// <param name="jsString"></param>
        /// <param name="isScriptManager"></param>
        /// <param name="isStartup"></param>
        public static void ExecuteScript(this Control control, string jsString, bool isScriptManager = true, bool isStartup = true)
        {
            if (isScriptManager)
                if (isStartup)
                    ScriptManager.RegisterStartupScript(control, control.GetType(), Guid.NewGuid().ToString(), jsString,true);
                else
                    ScriptManager.RegisterClientScriptBlock(control, control.GetType(), Guid.NewGuid().ToString(), jsString, true);
            else
                ((Page) HttpContext.Current.Handler).Page.ClientScript.RegisterClientScriptBlock(typeof (Page), Guid.NewGuid().ToString(), jsString, true);
        }

        /// <summary>
        /// 注册脚本
        /// </summary>
        /// <param name="control"></param>
        /// <param name="path"></param>
        public static void RegisterScript(this Control control, string path)
        {
            ScriptManager.RegisterClientScriptBlock(control, control.GetType(), path, string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", path), false);
        }
        #endregion

 

 
 

    }
}

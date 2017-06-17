using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Winner.Filter;

namespace Beeant.Basic.Services.WebForm.Controls
{

    /// <summary>
    ///MessageControlBase 的摘要说明
    /// </summary>
    public class MessageControlBase : UserControl
    {
        /// <summary>
        /// 消息
        /// </summary>
        public virtual string Message { get; set; }
        public virtual void ShowMessage(IList<ErrorInfo> errors)
        {
            if (errors == null || errors.Count == 0)
            {
                ShowSuccessMessage();
            }
            else
            {
                ShowFailureMessage(errors);
            }
        }
        /// <summary>
        /// 显示保存成功
        /// </summary>
        public virtual void ShowSuccessMessage()
        {
            Message = "保存成功！";
        }
        /// <summary>
        /// 显示失败信息
        /// </summary>
        public virtual void ShowFailureMessage(IList<ErrorInfo> errors)
        {
            var sb = new StringBuilder();
            sb.Append("您的输入有误,请仔细检查！");
            foreach (var error in errors)
            {
                sb.Append("</br>");
                sb.Append(error.Message);
            }
            Message = sb.ToString();
        }
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="error"></param>
        public virtual void ShowMessage(ErrorInfo error)
        {
            IList<ErrorInfo> errors = new List<ErrorInfo>();
            errors.Add(error);
            ShowMessage(errors);
        }
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message"></param>
        public virtual void ShowMessage(string message)
        {
            Message = message;
        }
    }
        
}
using System.Web.UI;
using System.Web.UI.WebControls;
using Beeant.Application.Services.Authority;
using Dependent;
using Beeant.Domain.Entities.Authority;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class AuthorizePageExtension
    {


        #region 扩展方法

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        public static bool VerifyResource(this Page page,long userId)
        {
            var info = GetVerification(page, userId);
            if (info == null) return false;
            SetResourceControl(page, info);
            return info.IsPass;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        public static VerificationEntity GetVerification(this Page page, long userId)
        {
            var info = Ioc.Resolve<IAuthorityEngineApplicationService>()
                          .GeVerificationEntity(userId, page.Request.RawUrl);
            return info;

        }

        /// <summary>
        /// 设置控件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="info"></param>
        private static void SetResourceControl(this Page page,VerificationEntity info)
        {
            if (!info.IsPass) return;
            foreach (var ctrl in info.Controls)
            {
                SetPageControl(page, ctrl.Key, ctrl.Value);
            }
        }
        /// <summary>
        /// 设置控件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="controlId"></param>
        /// <param name="value"></param>
        private static void SetPageControl(this Page page,string controlId, bool value)
        {
            var control = page.FindControl(controlId);
            if (control != null) control.Visible = value;
            else if (!value)
            {
                var jsScript = string.Format("$(document).find(\"*[name={0}]\").remove();", controlId);
                page.ExecuteScript(jsScript);
            }
        }
        /// <summary>
        /// 设置girdview的显示
        /// </summary>
        /// <param name="page"></param>
        /// <param name="gridView"></param>
        /// <param name="ctrl"></param>
        public static void SetGridViewColumnVisible(this Page page, GridView gridView, ListControl ctrl)
        {
            for (int i = 0; i < ctrl.Items.Count; i++)
            {
                if (!ctrl.Items[i].Selected)
                foreach (DataControlField field in gridView.Columns)
                {
                    if (field.HeaderText.Equals(ctrl.Items[i].Text))
                    {
                        field.Visible = false;
                        break;
                    }
                }
            }
        }
        #endregion


  
    }
}

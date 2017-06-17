
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Home.Controls
{


    public partial class Pager : PagerControlBase
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        protected override HiddenField PageIndexControl
        {
            get { return hfPageIndex; }
            set { hfPageIndex = value; }
        }

        /// <summary>
        /// 总共数量
        /// </summary>
        protected override HiddenField DataCountControl
        {
            get { return hfDataCount; }
            set { hfDataCount = value; }
        }
        /// <summary>
        /// 容器
        /// </summary>
        protected override HtmlGenericControl PanelControl
        {
            get { return divPanel; }
            set { divPanel = value; }
        }
        /// <summary>
        /// 输入分页框
        /// </summary>
        protected override TextBox PageControl
        {
            get { return txtPage; }
            set { txtPage = value; }
        }
        /// <summary>
        /// 上一页
        /// </summary>
        protected override LinkButton PreviousControl
        {
            get { return lnkPrevious; }
            set { lnkPrevious = value; }
        }
        /// <summary>
        /// 最后一页
        /// </summary>
        protected override LinkButton LastControl
        {
            get { return lnkLast; }
            set { lnkLast = value; }
        }
        /// <summary>
        /// 下一页
        /// </summary>
        protected override LinkButton NextControl
        {
            get { return lnkNext; }
            set { lnkNext = value; }
        }
        /// <summary>
        /// 第一页
        /// </summary>
        protected override LinkButton FirstControl
        {
            get { return lnkFirst; }
            set { lnkFirst = value; }
        }
        /// <summary>
        /// 输入框检索按钮
        /// </summary>
        protected override Button JumpControl
        {
            get { return btnPage; }
            set { btnPage = value; }
        }
        protected override DropDownList PageSizeControl
        {
            get
            {
                return ddlPageSize;
            }
            set
            {
                base.PageSizeControl = value;
            }
        }

    }
}
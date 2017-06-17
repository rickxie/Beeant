using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Component.Extension;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Controls
{

    /// <summary>
    ///PagerControl 的摘要说明
    /// </summary>
    public class PagerControlBase : UserControl 
    {
        #region 声明
        public event PagerChangedEventHandler PagerChanged;
        public delegate void PagerChangedEventHandler(object sender, EventArgs e);
        #endregion

        #region 具体控件
        /// <summary>
        /// 分页索引
        /// </summary>
        protected virtual HiddenField PageIndexControl { get; set; }
        /// <summary>
        /// 总共数量
        /// </summary>
        protected virtual HiddenField DataCountControl { get; set; }
        /// <summary>
        /// 容器
        /// </summary>
        protected virtual HtmlGenericControl PanelControl { get; set; }
        /// <summary>
        /// 输入分页框
        /// </summary>
        protected virtual TextBox PageControl { get; set; }
        /// <summary>
        /// 上一页
        /// </summary>
        protected virtual LinkButton PreviousControl { get; set; }
        /// <summary>
        /// 最后一页
        /// </summary>
        protected virtual LinkButton LastControl { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        protected virtual LinkButton NextControl { get; set; }
        /// <summary>
        /// 第一页
        /// </summary>
        protected virtual LinkButton FirstControl { get; set; }
        /// <summary>
        /// 输入框检索按钮
        /// </summary>
        protected virtual Button JumpControl { get; set; }
         /// <summary>
        /// 输入框检索按钮
        /// </summary>
        protected virtual DropDownList PageSizeControl { get; set; }

        private int _linkButtonCount=15;
        public int LinkButtonCount
        {
            get { return _linkButtonCount; }
            set { _linkButtonCount = value; }
        }
        #endregion

        #region JS脚本路径
        private string _pagingScriptPath = "/scripts/Winner/Pager/Winner.Pager.js";
        /// <summary>
        /// 脚本路径
        /// </summary>
        public string PagerScriptPath
        {
            get { return _pagingScriptPath; }
            set { _pagingScriptPath = value; }
        }
        #endregion

        #region 属性

        private bool _visible = true;
        /// <summary>
        /// 是否显示
        /// </summary>
        public new bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        private int _pageSize = 15;
        /// <summary>
        /// 页的大小
        /// </summary>
        public int PageSize
        {
            set { _pageSize = value; }
            get
            {
                if (PageSizeControl == null || string.IsNullOrEmpty(PageSizeControl.SelectedValue))
                    return _pageSize;
                return PageSizeControl.SelectedValue.Convert<int>();
            }
        }
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex
        {
            set { PageIndexControl.Value = value.ToString(CultureInfo.InvariantCulture); }
            get { return Convert.ToInt32(PageIndexControl.Value); }
        }
        /// <summary>
        /// 数据量
        /// </summary>
        public int DataCount
        {
            set { DataCountControl.Value = value.ToString(CultureInfo.InvariantCulture); }
            get { return Convert.ToInt32(DataCountControl.Value); }
        }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageCount
        {
            get { return Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(DataCount) / PageSize)); }
        }
        /// <summary>
        /// 查询内容
        /// </summary>
        public string SelectExp { get; set; }
        /// <summary>
        /// 排序内容
        /// </summary>
        public string OrderByExp { get; set; }
        /// <summary>
        /// 分组内容
        /// </summary>
        public string GroupByExp { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public string WhereExp { get; set; }
        
        /// <summary>
        /// 分组条件
        /// </summary>
        public string HavingExp { get; set; }
 
        /// <summary>
        /// 查询对象
        /// </summary>
        public string FromExp { get; set; }
        private QueryInfo _query = new QueryInfo();
        public QueryInfo Query
        {
            get
            {
                _query.SelectExp = SelectExp;
                _query.OrderByExp = OrderByExp;
                _query.HavingExp = HavingExp;
                _query.GroupByExp = GroupByExp;
                _query.FromExp = FromExp;
                _query.PageIndex = PageIndex;
                _query.PageSize = PageSize;
                _query.WhereExp = WhereExp;
                return _query;
            }
            set
            {
                _query = value;
                SelectExp = _query.SelectExp;
                OrderByExp = _query.OrderByExp;
                GroupByExp = _query.GroupByExp;
                FromExp = _query.FromExp;
                PageSize = _query.PageSize;
                HavingExp = _query.HavingExp;
                WhereExp = _query.WhereExp;
            }
        }
        #endregion

        #region 重写页面加载事件

        protected virtual void Page_Load(object sender, EventArgs e)
        {
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            CreateScriptPager();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 创建客户端脚本
        /// </summary>
        protected virtual void CreateScriptPager()
        {
            this.RegisterScript(PagerScriptPath);
            this.ExecuteScript(string.Format("var {0}=new Winner.Pager('{1}',{2},{3});{0}.Initialize();", ClientID, PanelControl.ClientID, PageIndex + 1, PageCount));
        }
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnPagerPageChanged(EventArgs e)
        {
            if (PagerChanged != null)
            {
                _query.DataCount = DataCount;
                PagerChanged(this, e);
            }
        }
        /// <summary>
        /// 绑定
        /// </summary>
        public override void DataBind()
        {
            DataCount = _query.DataCount;
            PageIndex = _query.PageIndex;
            if (DataCount > 0 && Visible)
            {
                PanelControl.Visible = true;
                SetButton();
            }
            else
            {
                PanelControl.Visible = false;
            }
        }
        #endregion

        #region 设置按钮
        /// <summary>
        /// 设置按钮
        /// </summary>
        protected virtual void SetButton()
        {
            if (PageCount < LinkButtonCount)
                SetNotFullButton();
            else
                SetFullButton();
            SetFirstPageControlButton();
            SetLastPageControlButton();
            SetOnlyPageControlButton();
            SetButtonClass();
        }
        /// <summary>
        /// 设置样式
        /// </summary>
        protected virtual void SetButtonClass()
        {
            if (PageIndex == 0)
            {
                FirstControl.CssClass = "link disable";
                PreviousControl.CssClass = "link disable";
            }
            else
            {
                FirstControl.CssClass = "link";
                PreviousControl.CssClass = "link";
            }
            if (PageIndex == PageCount - 1)
            {
                LastControl.CssClass = "link disable";
                NextControl.CssClass = "link disable";
            }
            else
            {
                LastControl.CssClass = "link";
                NextControl.CssClass = "link";
            }
            for (int i = 1; i <= LinkButtonCount; i++)
            {
                var ctrl = (LinkButton)FindControl(string.Format("LinkButton{0}", i.ToString(CultureInfo.InvariantCulture)));
                ctrl.CssClass = ctrl.Text==(PageIndex+1).ToString() ? "link disable" : "link";
            }
        }

        /// <summary>
        /// 设置第一按钮
        /// </summary>
        protected virtual void SetFirstPageControlButton()
        {
            bool value = PageIndex == 0 ;
            FirstControl.Enabled = !value;
            PreviousControl.Enabled = !value;
        }

        /// <summary>
        /// 设置最后一页按钮
        /// </summary>
        protected virtual void SetLastPageControlButton()
        {
            bool value = PageIndex >= PageCount - 1;
            LastControl.Enabled = !value;
            NextControl.Enabled = !value;
        }

        /// <summary>
        /// 设置只有一页按钮
        /// </summary>
        protected virtual void SetOnlyPageControlButton()
        {
            bool value = PageCount <= 1 ;
            JumpControl.Enabled = !value;
            PageControl.Enabled = !value;
            PageControl.Text = Convert.ToString(PageIndex + 1);
        }
        /// <summary>
        /// 设置没有超过LinkButtonCount数量时LinkButton按钮
        /// </summary>
        protected virtual void SetNotFullButton()
        {
            for (int i = PageCount + 1; i <= LinkButtonCount; i++)
            {
                FindControl(string.Format("LinkButton{0}", i.ToString(CultureInfo.InvariantCulture))).Visible = false;
            }
            SetNotFullButtonText();
        }
        /// <summary>
        /// 设置没有超过LinkButtonCount的LinkButton的显示值
        /// </summary>
        protected virtual void SetNotFullButtonText()
        {
            for (int i = 1; i <= PageCount; i++)
            {
                var lnk = ((LinkButton)FindControl(string.Format("LinkButton{0}", i.ToString(CultureInfo.InvariantCulture))));
                lnk.Text = i.ToString(CultureInfo.InvariantCulture);
                lnk.Visible = true;
                lnk.Enabled = i != PageIndex + 1 ;
            }
        }
        /// <summary>
        /// 设置超过LinkButtonCount数量时LinkButton按钮
        /// </summary>
        protected virtual void SetFullButton()
        {
            int text;
            if (PageIndex < LinkButtonCount / 2)
                text = 1;
            else if (PageCount > PageIndex + LinkButtonCount / 2 + 1)
                text = PageIndex - LinkButtonCount / 2 + 1;
            else
                text = PageCount - LinkButtonCount + 1;
            SetFullButtonText(text);
        }
        /// <summary>
        /// 设置超过LinkButtonCount的LinkButton的显示值
        /// </summary>
        /// <param name="text"></param>
        protected virtual void SetFullButtonText(int text)
        {
            for (int i = 1; i <= LinkButtonCount; i++, text++)
            {
                var lnk = ((LinkButton)FindControl(string.Format("LinkButton{0}", i.ToString(CultureInfo.InvariantCulture))));
                lnk.Text = text.ToString(CultureInfo.InvariantCulture);
                lnk.Visible = true;
                lnk.Enabled = Convert.ToInt32(lnk.Text) != PageIndex + 1;
            }
        }
        #endregion

        #region 事件
        protected virtual void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageIndex = 0;
            OnPagerPageChanged(e);
        }
        protected virtual void lnkFirst_Click(object sender, EventArgs e)
        {
            PageIndex = 0;
            OnPagerPageChanged(e);
        }
        protected virtual void lnkPrevious_Click(object sender, EventArgs e)
        {
            if(PageIndex>0)
                PageIndex = PageIndex - 1;
            OnPagerPageChanged(e);
        }
        protected virtual void lnkNext_Click(object sender, EventArgs e)
        {
            if(PageIndex<PageCount-1)
                PageIndex = PageIndex + 1;
            OnPagerPageChanged(e);
        }
        protected virtual void lnkLast_Click(object sender, EventArgs e)
        {
            PageIndex = PageCount - 1;
            OnPagerPageChanged(e);
        }
        protected virtual void LinkButton_Click(object sender, EventArgs e)
        {
            PageIndex = Convert.ToInt32(((LinkButton)sender).Text) - 1;
            OnPagerPageChanged(e);
        }
        protected virtual void btnPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PageControl.Text))
            {
                PageIndex = Convert.ToInt32(PageControl.Text) - 1;
                OnPagerPageChanged(e);
            }
        }
        #endregion

    }
        
}
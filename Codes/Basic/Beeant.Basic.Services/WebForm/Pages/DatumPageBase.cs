using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;
using Dependent;
using Beeant.Application.Services;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class DatumPageBase<T> : AuthorizePageBase where T : BaseEntity, new()
    {
        #region 具体控件
        private long _requestId = System.Web.HttpContext.Current.Request.QueryString["id"].Convert<long>();
        public virtual long RequestId
        {
            get { return _requestId; }
            set { _requestId = value; }
        }

         /// <summary>
         /// 消息控件
         /// </summary>
         public virtual MessageControlBase MessageControl { get; set; }

         /// <summary>
         /// 保存控件
         /// </summary>
         public virtual Button SaveButton { get; set; }
    
         private string _validScriptPath = "/scripts/Winner/Validator/Winner.Validator.js";
         /// <summary>
         /// 脚本路径
         /// </summary>
         public virtual string ValidScriptPath
         {
             get { return _validScriptPath; }
             set { _validScriptPath = value; }
         }
         private string _tableScriptPath = "/scripts/Winner/Table/Winner.Table.js";
         /// <summary>
         /// 脚本路径
         /// </summary>
         public string TableScriptPath
         {
             get { return _tableScriptPath; }
             set { _tableScriptPath = value; }
         }
         private string _checkScriptPath = "/scripts/Winner/CheckBox/Winner.CheckBox.js";
         /// <summary>
         /// 脚本路径
         /// </summary>
         public virtual string CheckScriptPath
         {
             get { return _checkScriptPath; }
             set { _checkScriptPath = value; }
         }
         /// <summary>
         /// 控件容器
         /// </summary>
         public IList<Control> ItemControls { get; set; }

         public virtual HtmlInputHidden ItemsValueControl { get; set; }
        #endregion

        #region 属性

         private bool _isFillAllEntity = true;
         /// <summary>
         /// 是否是部分绑定
         /// </summary>
         public virtual bool IsFillAllEntity
         {
             get
             {
                 return _isFillAllEntity;
             }
             set
             {
                 _isFillAllEntity = value;
             }
         }

        private bool _isUpdatePanel = true;
        /// <summary>
        /// 是否UpdatePanel
        /// </summary>
        public virtual bool IsUpdatePanel
        {
            get
            {
                return _isUpdatePanel;
            }
            set
            {
                _isUpdatePanel = value;
            }
        }
        /// <summary>
        /// 是否重置
        /// </summary>
        public virtual bool IsResetControl
        {
            get
            {
                if (SaveType.Add == SaveType && RequestId==0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 验证脚本
        /// </summary>
         protected string ValidScriptString { get; set; }
         /// <summary>
         /// 存储类型
         /// </summary>
         public virtual SaveType SaveType { get; set; }
         #endregion

        #region 重写页面事件
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            CheckControl();
            if (!IsPostBack)
                this.SetControlProperty();
            if(Form==null || RequestId==0)
                return;
            LoadItemControls(Form.Controls);
        }
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                if (!IsUpdatePanel) SetResult(false,null);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AddClientScript();

        }
        #endregion

        #region 检查控件
        /// <summary>
        /// 检查MessageControlBase
        /// </summary>
        protected virtual void CheckControl()
        {
            LoadMessageControl();
            LoadSaveButtonControl();
            LoadItemsValueControl();
        }
        /// <summary>
        /// 加载消息
        /// </summary>
        protected virtual void LoadMessageControl()
        {
            if (MessageControl == null)
                MessageControl = Page.FindControl("ctl00$Body$Message1") as MessageControlBase;
        }
        /// <summary>
        /// 加载存储按钮
        /// </summary>
        protected virtual void LoadSaveButtonControl()
        {
            if (SaveButton == null)
                SaveButton = Page.FindControl("ctl00$Body$Edit1$btnSave") as Button;
            if (SaveButton != null)
            {
                SaveButton.Click += Save_Click;
                //this.ExecuteScript(string.Format("$(\"#{0}\").click(function() {{$(this).hide();}});", SaveButton.ClientID));
            }
                
        }
        /// <summary>
        /// 加载消息
        /// </summary>
        protected virtual void LoadItemsValueControl()
        {
            if (ItemsValueControl == null)
                ItemsValueControl = Page.FindControl("ctl00$Body$hiddentextorderitem") as HtmlInputHidden;
        }
        #endregion

        #region 注册脚本

        /// <summary>
        /// 创建客户端脚本
        /// </summary>
        protected virtual void AddClientScript()
        {
            if (SaveButton != null)
            {
                this.RegisterScript(ValidScriptPath);
                SetValidScriptString();
                this.ExecuteScript(ValidScriptString);
            }
            this.RegisterScript(TableScriptPath);
            this.RegisterScript(CheckScriptPath);
            this.ExecuteScript("$('#btnClose').click(function(){window.close();});");
            var setScript = new StringBuilder();
            setScript.Append("function SetEntityBody(id) { var sender = $('#' + id);");
            setScript.Append("if (sender.css('display') == 'none')sender.show();else sender.hide();}");
            this.ExecuteScript(setScript.ToString());
        }
        /// <summary>
        /// 设置验证脚步
        /// </summary>
        protected virtual void SetValidScriptString()
        {
            ValidScriptString = string.IsNullOrEmpty(ValidScriptString) ?
                  this.SetEntity(SaveButton, typeof(T), SaveType == SaveType.Add ? ValidationType.Add : ValidationType.Modify) : ValidScriptString;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 加载数据
        /// </summary>
        protected virtual void LoadData()
        {
            LoadEntity();
            if (ItemControls == null) return;
            foreach (var item in ItemControls)
            {
                LoadItemEntities(item, false);
            }
        }
        /// <summary>
        /// 加载方法
        /// </summary>
        protected virtual void LoadEntity()
        {
            var info = GetEntity();
            if (info==null && SaveType==SaveType.Modify)
            {
                InvalidateData("您访问的信息不存在或者已经被删除");
            }
            BindEntity(info);
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="info"></param>
        protected virtual void BindEntity(T info)
        {
            if (info == null) return;
            ValidScriptString = this.BindEntity(info, SaveButton,
                                              SaveType == SaveType.Add ? ValidationType.Add : ValidationType.Modify);
        }

        /// <summary>
        /// 加载实体
        /// </summary>
        /// <returns></returns>
        protected virtual T GetEntity()
        {
            if (RequestId==0)
                return null;
            return Ioc.Resolve<IApplicationService,T>().GetEntity<T>(RequestId);
        }

        /// <summary>
        /// 得到对象
        /// </summary>
        /// <returns></returns>
        protected virtual T FillEntity()
        {
            var info = this.GetEntity<T>(IsFillAllEntity);
            info.SaveType = SaveType;
            if(SaveType==SaveType.Modify)
                info.Id = RequestId;
            return info;
        }


        /// <summary>
        /// 得到子对象
        /// </summary>
        /// <returns></returns>
        protected virtual IList<TItem> GetItems<TItem>() where TItem : BaseEntity, new()
        {
            var info = this.GetItemEntities<TItem>(ItemsValueControl.Value, SaveType);    
            return info;
        }
        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Save()
        {
            T info = FillEntity();
            Save(info);
        }

        /// <summary>
        /// 存储对象
        /// </summary>
        /// <param name="info"></param>
        protected virtual void Save(T info)
        {
            if(info==null) return;
            var rev=Ioc.Resolve<IApplicationService,T>().Save(info);
            SetResult(rev,info.Errors);
        }

        /// <summary>
        /// 设置返回结果
        /// </summary>
        /// <param name="rev"></param>
        /// <param name="errors"></param>
        protected virtual void SetResult(bool rev,IList<ErrorInfo> errors)
        {
            if (MessageControl == null) return;
            if (IsUpdatePanel)
            {
                MessageControl.ShowMessage(errors);
                if (rev && IsResetControl)
                    this.ResetControl();
                if (rev)
                    SetSuccessControl();
            }
            else
            {
                if ("true".Equals(Request.QueryString["result"]))
                {
                    MessageControl.ShowMessage(errors);
                    if (IsResetControl)
                        this.ResetControl();
                    SetSuccessControl();
                }
                else if (rev)
                {
                    SetRedirectResult();
                }
                else if (errors != null && errors.Count > 0)
                {
                    MessageControl.ShowMessage(errors);
                }
            }
        }
        /// <summary>
        /// 设置控件
        /// </summary>
        protected virtual void SetSuccessControl()
        {
            if (SaveButton != null)
                SaveButton.Enabled = false;
        }

        /// <summary>
        /// 跳转正确结果
        /// </summary>
        protected virtual void SetRedirectResult()
        {
            string url = Request.Url.ToString();
            if (string.IsNullOrEmpty(Request.QueryString["result"]))
                url = string.Format("{0}{1}", url, url.Contains("?") ? "&result=true" : "?result=true");
            this.Redirect(url);
        }

        #endregion

        #region 保存事件
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Save_Click(object sender, EventArgs e)
        {
           Save();
        }
        #endregion

        #region 加载相关项
        /// <summary>
        /// 加载附属数据
        /// </summary>
        protected virtual void LoadItemControls(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                var gridView = ctrl as GridView;
                if (gridView != null)
                {
                    ItemControls = ItemControls ?? new List<Control>();
                    ItemControls.Add(gridView.Parent);
                    BindControlEvent(gridView.Parent);
                    this.ExecuteScript(string.Format("var table=new Winner.Table('{0}');table.Initialize();", gridView.ClientID));
                    var builder = new StringBuilder();
                    builder.AppendFormat("var ck{0}=new Winner.CheckBox('{0}',", gridView.ClientID);
                    builder.Append("{StyleFile:null});");
                    builder.AppendFormat("ck{0}.Initialize();", gridView.ClientID);
                    this.ExecuteScript(builder.ToString());
                }
                else LoadItemControls(ctrl.Controls);
            }
        }

        /// <summary>
        /// 绑定控件事件
        /// </summary>
        /// <param name="container"></param>
        protected virtual void BindControlEvent(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                var paging = ctrl as PagerControlBase;
                if (paging != null)
                    paging.PagerChanged += Pager_PagerChanged;
                var search = ctrl as Button;
                if (search != null) search.Click += Search_Click;

            }
        }

        /// <summary>
        /// container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isResetPageIndex"></param>
        protected virtual void LoadItemEntities(Control container, bool isResetPageIndex)
        {
            GridView gridView = null;
            PagerControlBase paging = null;
            foreach (var ctrl in container.Controls)
            {
                var view = ctrl as GridView;
                if (view != null) gridView = view;
                var @base = ctrl as PagerControlBase;
                if (@base != null) paging = @base;
                if (gridView != null && paging != null) break;
            }
            BindItemEntities(container, gridView, paging, isResetPageIndex);
        }

        /// <summary>
        /// 绑定集合
        /// </summary>
        /// <param name="container"></param>
        /// <param name="gridView"></param>
        /// <param name="paging"></param>
        /// <param name="isResetPageIndex"></param>
        protected virtual void BindItemEntities(Control container, GridView gridView, PagerControlBase paging,
                                             bool isResetPageIndex)
        {
            if (gridView == null || paging == null) return;
            if (isResetPageIndex) paging.PageIndex = 0;
            SetFindQuery(container, paging);
            var infos = GetItemEntities(paging.Query);
            paging.DataBind();
            gridView.DataSource = infos;
            gridView.DataBind();
           
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="container"></param>
        /// <param name="paging"></param>
        protected virtual void SetFindQuery(Control container, PagerControlBase paging)
        {
            var query = paging.Query;
            this.SetFindQueryByControls(Type.GetType(paging.FromExp), query, container.Controls);
            paging.Query = query;
        }
        /// <summary>
        /// 得到集合控件
        /// </summary>
        /// <param name="query"></param>
        protected virtual IList<BaseEntity> GetItemEntities(QueryInfo query)
        {
            query.SetParameter("Id", RequestId);
            return Ioc.Resolve<IApplicationService>().GetEntities<BaseEntity>(query);
        }


        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Pager_PagerChanged(object sender, EventArgs e)
        {
            LoadItemEntities(((Control)sender).Parent, false);
        }
        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Search_Click(object sender, EventArgs e)
        {
            LoadItemEntities(((Control)sender).Parent, true);
        }
        #endregion

        #region 导出Excel

        /// <summary>
        /// 格式输出Excel数据
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="gridView"></param>
        /// <returns></returns>
        protected virtual HtmlTextWriter GetExcelString<TEntityType>(IList<TEntityType> infos, GridView gridView)
        {
            using (var writer = new StringWriter())
            {
                HideExcelGridViewColumn();
                gridView.DataSource = infos;
                gridView.DataBind();
                var htmlTextWriter = new HtmlTextWriter(writer);
                gridView.RenderControl(htmlTextWriter);
                return htmlTextWriter;
            }
        }

        /// <summary>
        /// 拼接Excel标题
        /// </summary>
        protected virtual void HideExcelGridViewColumn()
        {

        }
        /// <summary>
        /// 设置导出格式样式
        /// </summary>
        /// <param name="column"></param>
        protected virtual void SetExcelGridViewColumnItemStyle(DataControlField column)
        {
            var styles = new[] { "xlstext", "xlsfloat", "xlsdate", "xlsdatetime" };
            var css = "";
            foreach (var style in styles)
            {
                if (column.ItemStyle.CssClass.Contains(style))
                {
                    css = style;
                }
            }
            column.ItemStyle.CssClass = css;
        }

        /// <summary>
        /// 输出Excel
        /// </summary>
        /// <typeparam name="TEntityType"></typeparam>
        /// <param name="query"></param>
        /// <param name="gridView"></param>
        protected virtual void ExportExcel<TEntityType>(QueryInfo query, GridView gridView) where TEntityType : BaseEntity, new()
        {
            var infos = Ioc.Resolve<IApplicationService, TEntityType>().GetEntities<TEntityType>(query);
            HtmlTextWriter writer = GetExcelString(infos, gridView);
            WriteExcel("export", writer);
        }

        /// <summary>
        /// 输出Excel
        /// </summary>
        /// <param name="exportFileName"></param>
        /// <param name="writer"></param>
        public virtual void WriteExcel(string exportFileName, HtmlTextWriter writer)
        {
            Response.Charset = "UTF-8";
            string fileName = HttpUtility.UrlEncode(exportFileName, Encoding.UTF8);
            string str = "attachment;filename=" + fileName + ".xls";
            Response.AppendHeader("Content-Disposition", str);
            Response.ContentType = "application/ms-excel";
            const string style = @"<style>td{white-space: nowrap; text-overflow: ellipsis; word-break: keep-all;}" +
                                 ".xlstext {vnd.ms-excel.numberformat:@;}" +
                                 ".xlsnum {vnd.ms-excel.numberformat:#,##0;}" +
                                 ".xlsfloat {vnd.ms-excel.numberformat:#,##0.00;}" +
                                 ".xlsdate {vnd.ms-excel.numberformat:yyyy-mm-dd}" +
                                 ".xlsdatetime {vnd.ms-excel.numberformat:yyyy-mm-dd}</style>";
            Response.Write(style);
            Response.Write(writer.InnerWriter.ToString());
            Response.End();
        }


        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        #endregion
    }
}

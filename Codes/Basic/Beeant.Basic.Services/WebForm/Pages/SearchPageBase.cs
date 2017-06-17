using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Management;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class SearchPageBase<T>:AuthorizePageBase
    {
        /// <summary>
        /// 导出excel的数量
        /// </summary>
        public virtual int ExcelRowCount
        {
            get { return 10000; }
        }
        #region 脚本、控件
        /// <summary>
        /// Girdview控件
        /// </summary>
        public virtual GridView GridView { get; set; }

        /// <summary>
        /// 分页控件
        /// </summary>
        public virtual PagerControlBase Pager { get; set; }
        /// <summary>
        /// 搜索容器
        /// </summary>
        public virtual HtmlGenericControl SearchPanel { get; set; }
        /// <summary>
        /// Excel导出
        /// </summary>
        public virtual Button ExcelButton { get; set; }
        /// <summary>
        /// 搜索按钮
        /// </summary>
        public virtual Button SearchButton { get; set; }
        /// <summary>
        /// 存储个性化设置
        /// </summary>
        public virtual Button SavePersonalization { get; set; }
        /// <summary>
        /// 存储个性化清除
        /// </summary>
        public virtual Button ClearPersonalization { get; set; }
        /// <summary>
        /// 排序内容
        /// </summary>
        public virtual DropDownList OrderbyList { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public virtual RadioButtonList OrderbyTypeList { get; set; }
        /// <summary>
        /// 选择内容
        /// </summary>
        public virtual CheckBoxList SelectList { get; set; }

        private string _tableScriptPath = "/scripts/Winner/Table/Winner.Table.js";
        /// <summary>
        /// 脚本路径
        /// </summary>
        public virtual string TableScriptPath
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
        private string _confirmBoxScriptPath = "/scripts/Winner/ConfirmBox/Winner.ConfirmBox.js";
        /// <summary>
        /// 脚本路径
        /// </summary>
        public virtual string ConfirmBoxScriptPath
        {
            get { return _confirmBoxScriptPath; }
            set { _confirmBoxScriptPath = value; }
        }
        #endregion


        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            CheckControl();
            if (!IsPostBack)
            {
                this.SetControlProperty();
            }

        }
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadListPage();
                LoadData();
            }

        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AddClientScript();

        }

        #region 加载数据

        /// <summary>
        /// 加载数据
        /// </summary>
        protected virtual void LoadData()
        {
            //try
            //{
                LoadEntities();
                SetGridViewSequence();
                SetGridViewColumn();
            //}
            //catch (Exception ex)
            //{
            //    this.ShowMessage("查询异常", string.Format("异常信息为：{0}", ex.Message));
            //    this.AddExceptionLog(ex);
            //}
        }


        /// <summary>
        /// 加载对象
        /// </summary>
        protected virtual void LoadEntities()
        {
            var infos = GetEntities();
            BindEntities(infos);
        }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void BindEntities(IList<T> infos)
        {
            if (infos == null)
                return;
            if (Pager != null)
                Pager.DataBind();
            if (GridView != null)
            {
                GridView.DataSource = infos;
                GridView.DataBind();
            }

        }

        /// <summary>
        /// 得到对象集合
        /// </summary>
        /// <returns></returns>
        protected virtual IList<T> GetEntities()
        {
            if (Pager != null)
            {
                QueryInfo query = Pager.Query;
                SetQuery(query);
                var infos = Ioc.Resolve<IApplicationService, T>().GetEntities<T>(query);
                Pager.Query = query;
                return infos;
            }
            return null;
        }

        #region 设置girdview

        /// <summary>
        /// 设置gridview显示的列
        /// </summary>
        protected virtual void SetGridViewColumn()
        {
            if (GridView == null) return;
            foreach (DataControlField column in GridView.Columns)
            {
                column.Visible = true;
            }

        }
        /// <summary>
        /// 设置序号
        /// </summary>
        protected virtual void SetGridViewSequence()
        {
            if (GridView == null) return;
            var index = Pager == null ? 1 : Pager.PageIndex * Pager.PageSize + 1;
            foreach (GridViewRow row in GridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                    row.Cells[0].Text = (row.RowIndex + index).ToString(CultureInfo.InvariantCulture);
            }
        }
        #endregion

        /// <summary>
        /// 设置查询
        /// </summary>
        /// <param name="query"></param>
        protected virtual void SetQuery(QueryInfo query)
        {
            query.From<T>();
            SetQueryWhere(query);
            SetQuerySelect(query);
            SetQueryOrderby(query);
        }

  
        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Pager_PagerChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region 检查控件
        /// <summary>
        /// 检查GridView和PagerControlBase
        /// </summary>
        protected virtual void CheckControl()
        {
            LoadGridView();
            LoadPagerControl();
            LoadSearchPanel();
            LoadSearchButton();
            LoadSelectList();
            LoadOrderbyList();
            LoadOrderbyTypeList();
            LoadExcelButton();
            LoadSavePersonalization();
            LoadClearPersonalization();
        }
        /// <summary>
        /// 加载GirdView
        /// </summary>
        protected virtual void LoadGridView()
        {
            if (GridView == null)
                GridView = Page.FindControl("ctl00$Body$GridView1") as GridView;
        }
        /// <summary>
        /// 加载分页控件
        /// </summary>
        protected virtual void LoadPagerControl()
        {
            if (Pager == null)
                Pager = Page.FindControl("ctl00$Body$Pager1") as PagerControlBase;
            if (Pager != null)
                Pager.PagerChanged += Pager_PagerChanged;
        }
        protected virtual void LoadSearchPanel()
        {
            if (SearchPanel == null)
                SearchPanel = Page.FindControl("ctl00$Body$divSearch") as HtmlGenericControl;
        }
        /// <summary>
        /// 搜索按钮
        /// </summary>
        protected virtual void LoadSearchButton()
        {
            if (SearchButton == null)
            {
                SearchButton = Page.FindControl("ctl00$Body$btnSearch") as Button;
            }
            if (SearchButton != null)
            {
                SearchButton.Click += Search_Click;
                Form.DefaultButton = SearchButton.UniqueID;
            }
        }
        /// <summary>
        /// 排序内容
        /// </summary>
        protected virtual void LoadOrderbyList()
        {
            if (OrderbyList == null)
            {
                OrderbyList = Page.FindControl("ctl00$Body$ddlOrderbyList") as DropDownList;
            }
        }
        /// <summary>
        /// 排序方式
        /// </summary>
        protected virtual void LoadOrderbyTypeList()
        {
            if (OrderbyTypeList == null)
            {
                OrderbyTypeList = Page.FindControl("ctl00$Body$rdOrderbyType") as RadioButtonList;
            }
        }
        /// <summary>
        /// 显示内容
        /// </summary>
        protected virtual void LoadSelectList()
        {
            if (SelectList == null)
            {
                SelectList = Page.FindControl("ctl00$Body$ckSelectList") as CheckBoxList;
                if (SelectList == null) return;
                if (SelectList.RepeatColumns == 0)
                    SelectList.RepeatColumns = 12;
                if (SelectList != null)
                {
                    var js = new StringBuilder();
                    js.AppendFormat("$('#{0}').before(\"<input type='button' value='显示选项' onclick='if(this.value==\\\"显示选项\\\")", SelectList.ClientID);
                    js.Append("{");
                    js.AppendFormat("$(\\\"#{0}\\\").show();this.value=\\\"隐藏选项\\\";", SelectList.ClientID);
                    js.Append("}else{");
                    js.AppendFormat("$(\\\"#{0}\\\").hide();this.value=\\\"显示选项\\\";", SelectList.ClientID);
                    js.Append("}'  class='btn' />\");");
                    this.ExecuteScript(js.ToString());
                    SelectList.Attributes.Add("style", "display:none");
                }
            }
        }
        /// <summary>
        /// Excel按钮
        /// </summary>
        protected virtual void LoadExcelButton()
        {
            if (ExcelButton == null)
            {
                ExcelButton = Page.FindControl("ctl00$Body$btnExcel") as Button;
                if (ExcelButton != null)
                {
                    ExcelButton.Click += Excel_Click;
                    ExcelButton.Attributes.Add("onclick", "return confirm('您确定要导出当前数据吗？');");
                }
            }
        }
        /// <summary>
        /// 保存个性化设置按钮
        /// </summary>
        protected virtual void LoadSavePersonalization()
        {
            if (SavePersonalization != null) return;
            SavePersonalization = Page.FindControl("ctl00$Body$btnSavePersonalization") as Button;
            if (SavePersonalization == null) return;
            SavePersonalization.Click += SavePersonalization_Click;
        }
        /// <summary>
        /// 保存个性化清除按钮
        /// </summary>
        protected virtual void LoadClearPersonalization()
        {
            if (ClearPersonalization != null) return;
            ClearPersonalization = Page.FindControl("ctl00$Body$btnClearPersonalization") as Button;
            if (ClearPersonalization == null) return;
            ClearPersonalization.Click += ClearPersonalization_Click;
        }
        #endregion

        #region 脚本
        /// <summary>
        /// 创建客户端脚本
        /// </summary>
        protected virtual void AddClientScript()
        {
            if (GridView != null)
            {
                this.RegisterScript(TableScriptPath);
                this.RegisterScript(CheckScriptPath);
                this.RegisterScript(ConfirmBoxScriptPath);
                var builder = new StringBuilder(string.Format("var table=new Winner.Table('{0}',", GridView.ClientID));
                builder.Append("{StyleFile:null});");
                builder.AppendFormat("var checkbox=new Winner.CheckBox('{0}',", GridView.ClientID);
                builder.Append("{StyleFile:null});");
                builder.AppendFormat("var confirmBox=new Winner.ConfirmBox();");
                builder.Append("checkbox.Initialize();confirmBox.Initialize();table.Initialize();");
                this.ExecuteScript(builder.ToString());
            }
        }
        #endregion

        #region 搜索
        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="query"></param>
        protected virtual void SetQueryWhere(QueryInfo query)
        {
            if (SearchPanel == null)
                return;
            this.SetFindQueryByControls<T>(query, SearchPanel.Controls);
        }
        /// <summary>
        /// 设置查询内容
        /// </summary>
        /// <param name="query"></param>
        protected virtual void SetQuerySelect(QueryInfo query)
        {
            if (SelectList == null)
            {
                query.SelectExp = Pager.SelectExp;
                return;
            }
            var builder = new StringBuilder();
            var selects =string.IsNullOrEmpty(query.SelectExp)?null: query.SelectExp.Split(',');
            foreach (ListItem item in SelectList.Items)
            {
                if (!item.Selected) continue;
                if (string.IsNullOrEmpty(query.SelectExp) || selects != null && !selects.Contains(item.Value))
                    builder.AppendFormat("{0},", item.Value);
            }
            if (string.IsNullOrEmpty(query.SelectExp) && builder.Length > 0) builder.Remove(builder.Length - 1, 1);
            else builder.Append(Pager.SelectExp);
            query.SelectExp = builder.ToString();
        }
        /// <summary>
        /// 设置排序内容
        /// </summary>
        /// <param name="query"></param>
        protected virtual void SetQueryOrderby(QueryInfo query)
        {
            if (OrderbyList == null || OrderbyTypeList == null
                || OrderbyList.SelectedItem == null || OrderbyTypeList.SelectedItem == null
                || string.IsNullOrEmpty(OrderbyList.SelectedItem.Value)) return;
            if (string.IsNullOrEmpty(query.OrderByExp))
                query.OrderByExp = string.Format("{0} {1}", OrderbyList.SelectedItem.Value,
                                                 OrderbyTypeList.SelectedItem.Value);
            else
                query.OrderByExp = string.Format("{0} {1},{2}", OrderbyList.SelectedItem.Value,
                                                     OrderbyTypeList.SelectedItem.Value, query.OrderByExp);
        }
        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Search_Click(object sender, EventArgs e)
        {
            ResetPagerIndex();
            LoadData();
        }
        /// <summary>
        /// 重置页码
        /// </summary>
        protected virtual void ResetPagerIndex()
        {
            if (Pager != null)
                Pager.PageIndex = 0;
        }

        #endregion

        #region 导出Excel
        /// <summary>
        /// 得到Excel查询
        /// </summary>
        /// <returns></returns>
        protected virtual QueryInfo GetExecelQuery()
        {
            if (Pager != null)
            {
                QueryInfo query = Pager.Query;
                query.PageIndex = 0;
                query.PageSize = ExcelRowCount;
                SetQuery(query);
                return query;
            }
            return null;
        }
        /// <summary>
        /// 得到Excel集合
        /// </summary>
        /// <returns></returns>
        protected virtual IList<T> GetExcelEntities()
        {
            var query = GetExecelQuery();
            if (query != null)
            {
                return Ioc.Resolve<IApplicationService, T>().GetEntities<T>(query);
            }
            return null;
        }
        /// <summary>
        /// 得到Excel集合
        /// </summary>
        /// <returns></returns>
        protected virtual DataTable GetExcelDataTable()
        {
            var query = GetExecelQuery();
            if (query != null)
            {
                return Ioc.Resolve<IApplicationService, T>().Execute<DataTable>(query);
            }
            return null;
        }

        /// <summary>
        /// 设置导出格式
        /// </summary>
        protected virtual DataTable SetExcelDataTable(DataTable dt)
        {
            if (SelectList != null)
            {
                var i = 0;
                foreach (ListItem item in SelectList.Items)
                {
                    if (!item.Selected)
                        continue;
                    dt.Columns[i].ColumnName = item.Text;
                    dt.Columns[i].Caption = item.Text;
                    i++;
                }
            }
            else if (GridView != null)
            {
                for (int i = 1; i < GridView.Columns.Count; i++)
                {
                    dt.Columns[i - 1].ColumnName = GridView.Columns[i].HeaderText;
                    dt.Columns[i - 1].Caption = GridView.Columns[i].HeaderText;
                }
            }
            return dt;
        }

        /// <summary>
        /// 格式输出Excel数据
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        protected virtual HtmlTextWriter GetExcelString(IList<T> infos)
        {
            using (var writer = new StringWriter())
            {
                HideExcelGridViewColumn();
                GridView.DataSource = infos;
                GridView.DataBind();
                SetGridViewSequence();
                var htmlTextWriter = new HtmlTextWriter(writer);
                GridView.RenderControl(htmlTextWriter);
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
        protected virtual void ExportExcel(bool isPlus, string excelName)
        {
            var hasCount = false;
            if (isPlus)
            {
                var dt = GetExcelDataTable();
                if (dt.Rows.Count > 0)
                {
                    hasCount = true;
                    dt = SetExcelDataTable(dt);
                    WritePlusExcel(excelName, dt);
                }
            }
            else
            {
                var infos = GetExcelEntities();
                if (infos.Count > 0)
                {
                    hasCount = true;
                    HtmlTextWriter writer = GetExcelString(infos);
                    WriteExcel(excelName, writer);
                }
            }
            if (hasCount)
            {
                this.ShowMessage("未找到数据", "未找到相关记录, 请改变检索条件后重试!");
            }

        }

        /// <summary>
        /// 输出Excel
        /// </summary>
        /// <param name="exportFileName"></param>
        /// <param name="writer"></param>
        public virtual void WriteExcel(string exportFileName, HtmlTextWriter writer)
        {
            string fileName = exportFileName;
            //string str = "attachment;filename=" + fileName + ".xls";
            //Response.AppendHeader("Content-Disposition", str);
            Response.ContentType = "application/ms-excel";
            //Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition", "attachment;filename=\"" + System.Web.HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls\"");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.UTF8;
            const string style = @"<style>td{white-space: nowrap; text-overflow: ellipsis; word-break: keep-all;}" +
                                 ".xlstext {vnd.ms-excel.numberformat:@;}" +
                                 ".xlsnum {vnd.ms-excel.numberformat:#,##0;}" +
                                 ".xlsfloat {vnd.ms-excel.numberformat:#,##0.00;}" +
                                 ".xlsdate {vnd.ms-excel.numberformat:yyyy-mm-dd}" +
                                 ".xlsdatetime {vnd.ms-excel.numberformat:yyyy-mm-dd}</style>";
            Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            Response.Write(style);
            Response.Write(writer.InnerWriter.ToString().Replace("\r\n", "").Replace("\t", "").Replace("  ", ""));
            Response.End();
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="exportFileName"></param>
        /// <param name="dt"></param>
        public virtual void WritePlusExcel(string exportFileName, DataTable dt)
        {

            Response.AddHeader("content-disposition", $"attachment;  filename={exportFileName}.xlsx");
            Response.ContentType = "applicationnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(ExcelHelper.ExportExcel(dt));
            Response.Flush();
            Response.End();
        }

        protected virtual void Excel_Click(object sender, EventArgs e)
        {
            ExportExcel(((Button)sender).GetAttributeValue("IsPlus").Convert<bool>(), ((Button)sender).Attributes["excelname"] ?? "export");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        #endregion

        #region 个性化设置

        /// <summary>
        /// 存储个性化设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SavePersonalization_Click(object sender, EventArgs e)
        {
            SaveListPage(((Button)sender).CommandArgument.Convert<long>());
        }
        /// <summary>
        /// 清除个性化设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ClearPersonalization_Click(object sender, EventArgs e)
        {
            ClearListPage(((Button)sender).CommandArgument.Convert<long>());
        }
        /// <summary>
        /// 加载个性化搜索设置
        /// </summary>
        protected virtual void LoadListPage()
        {
            try
            {
                if (SearchPanel == null) return;
                var info = GetListSearchEntity();
                if (info == null) return;
                if (SavePersonalization != null)
                {
                    SavePersonalization.CommandArgument = info.Id.ToString();
                }
                if (ClearPersonalization != null)
                {
                    ClearPersonalization.CommandArgument = info.Id.ToString();
                }
                SetListSearchControls(info);
            }
            catch (Exception)
            {
            }

        }
        /// <summary>
        /// 设置搜索控件
        /// </summary>
        /// <param name="info"></param>
        protected virtual void SetListSearchControls(ListSearchEntity info)
        {
            if (info.Controls == null) return;
            foreach (var c in info.Controls)
            {
                var ctrl = Page.FindControl(c.Key);
                if (ctrl == null) continue;
                BindPageExtension.InvokeSetControlValueHandler(ctrl, c.Value);
            }
        }

        /// <summary>
        /// 得到搜索
        /// </summary>
        /// <returns></returns>
        protected virtual ListSearchEntity GetListSearchEntity()
        {
            var query = new QueryInfo();
            query.Query<ListSearchEntity>().Where(it => it.Account.Id == Identity.Id && it.Website == Request.Url.Host
                                                      &&
                                                      it.Url == Request.RawUrl);
            return Ioc.Resolve<IApplicationService, ListSearchEntity>().GetEntities<ListSearchEntity>(query).FirstOrDefault();
        }

        /// <summary>
        /// 清除个性化设置
        /// </summary>
        /// <param name="id"></param>
        protected virtual void ClearListPage(long id)
        {
            var info = new ListSearchEntity { SaveType = SaveType.Remove, Id = id };
            var rev = Ioc.Resolve<IApplicationService, ListSearchEntity>().Save(info);
            this.ShowMessage("操作提醒", rev ? "清除成功" : "清除失败");
            LoadListPage();
        }

        /// <summary>
        /// 保存个性化设置
        /// </summary>
        /// <param name="id"></param>
        protected virtual void SaveListPage(long id)
        {
            var info = GetSaveSearchEntity(id);
            var rev = Ioc.Resolve<IApplicationService, ListSearchEntity>().Save(info);
            this.ShowMessage("操作提醒", rev ? "保存成功" : "保存失败");
            LoadListPage();
        }

        /// <summary>
        /// 得到存储信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual ListSearchEntity GetSaveSearchEntity(long id)
        {
            var info = new ListSearchEntity
            {
                Id = id,
                Website = Request.Url.Host,
                Url = Request.RawUrl,
                Account = new AccountEntity { Id = Identity.Id },
                Detail = "",
                SaveType = id==0 ? SaveType.Add : SaveType.Modify
            };
            if (SearchPanel == null) return info;
            AddSearchControls(info, SearchPanel.Controls);
            return info;
        }
        /// <summary>
        /// 添加搜索
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctrls"></param>
        protected virtual void AddSearchControls(ListSearchEntity info, ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                var value = SavePageExtension.InvokeGetControlValueHandler(ctrl);
                var rev = info.AddControl(ctrl.UniqueID, value == null ? null : value.ToString());
                if (!rev && ctrl.Controls.Count > 0)
                    AddSearchControls(info, ctrl.Controls);
            }
        }


        #endregion
    }
}

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class CreateSelectProduct : SearchPageBase<ProductEntity>
    {
        public override Basic.Services.WebForm.Controls.PagerControlBase Pager
        {
            get
            {
                return Pager1;
            }
            set
            {
                base.Pager = value;
            }
        }
        public override System.Web.UI.HtmlControls.HtmlGenericControl SearchPanel
        {
            get
            {
                return divSearch;
            }
            set
            {
                base.SearchPanel = value;
            }
        }
        public override Button SearchButton
        {
            get
            {
                return btnSearch;
            }
            set
            {
                base.SearchButton = value;
            }
        }
        public override GridView GridView
        {
            get { return GridView1; }
            set
            {
                base.GridView = value;
            }
        }
        public override bool IsVerifyResource
        {
            get { return false; }
        }
        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            Page.AddExceptionLog();
        }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories(ddlCategoryOne,0);
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 重写查询
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            if (!string.IsNullOrEmpty(ddlCategoryTree.SelectedValue))
            {
                query.Query<ProductEntity>()
                     .Where(it => it.IsSales && it.Goods.Category.Id == ddlCategoryTree.SelectedValue.Convert<long>());
            }
            else if (!string.IsNullOrEmpty(ddlCategoryTow.SelectedValue))
            {
                query.Query<ProductEntity>()
                     .Where(it => it.IsSales && it.Goods.Category.Parent.Id == ddlCategoryTow.SelectedValue.Convert<long>());
            }
            else if (!string.IsNullOrEmpty(ddlCategoryOne.SelectedValue))
            {
                query.Query<ProductEntity>()
                     .Where(it => it.IsSales && it.Goods.Category.Parent.Parent.Id == ddlCategoryOne.SelectedValue.Convert<long>());
            }
            else
            {
                query.Query<ProductEntity>()
                     .Where(it => it.IsSales);
            }
            base.SetQueryWhere(query);
        }
        
        /// <summary>
        /// 加载类目
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="parentId"></param>
        protected virtual void LoadCategories(DropDownList dropDownList, long parentId)
        {
            var query = new QueryInfo();
            query.Query<CategoryEntity>()
                 .Where(it => it.Parent.Id == parentId)
                 .Select(it => new object[] {it.Id, it.Name});
            var infos = Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntities<CategoryEntity>(query);
            dropDownList.DataTextField = "Name";
            dropDownList.DataValueField = "Id";
            dropDownList.DataBind(infos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCategoryOne_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlCategoryTow.Items.Clear();
            ddlCategoryTree.Items.Clear();
            if (!string.IsNullOrEmpty(ddlCategoryOne.SelectedValue))
            {
                LoadCategories(ddlCategoryTow, ddlCategoryOne.SelectedValue.Convert<long>());
            }
        }

        protected void ddlCategoryTow_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlCategoryTree.Items.Clear();
            if (!string.IsNullOrEmpty(ddlCategoryTow.SelectedValue))
            {
                LoadCategories(ddlCategoryTree, ddlCategoryTow.SelectedValue.Convert<long>());
            }
        }
        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript("Init();");
        }
    }
}
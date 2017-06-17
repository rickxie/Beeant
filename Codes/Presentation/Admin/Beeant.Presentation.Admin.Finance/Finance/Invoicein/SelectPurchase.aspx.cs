using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Component.Extension;
using Beeant.Domain.Entities.Purchase;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Finance.Finance.Invoicein
{
    public partial class SelectPurchase : SearchPageBase<PurchaseEntity>
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

        public long AccountId
        {
            get { return Request.QueryString["accountid"].Convert<long>(); }
        }

        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            Page.AddExceptionLog();
        }
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
             
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 重写查询
        /// </summary>
        /// <param name="query"></param>
        protected override void SetQueryWhere(QueryInfo query)
        {
            if (AccountId!=0)
            {
                query.Query<PurchaseEntity>()
                     .Where(it => it.Account.Id == AccountId);
            }
            base.SetQueryWhere(query);
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
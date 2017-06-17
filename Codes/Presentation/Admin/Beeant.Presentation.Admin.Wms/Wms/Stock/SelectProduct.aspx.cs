using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Product;

namespace Beeant.Presentation.Admin.Wms.Wms.Stock
{
    public partial class SelectProduct : SearchPageBase<ProductEntity>
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
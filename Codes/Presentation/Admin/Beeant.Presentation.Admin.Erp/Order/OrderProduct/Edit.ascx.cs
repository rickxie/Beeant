using System.Web.UI.HtmlControls;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Order.OrderProduct
{
    public partial class Edit : System.Web.UI.UserControl
    {
        public virtual SaveType UploaderSaveType
        {
            get { return Uploader1.SaveType; }
            set
            {
                Uploader1.SaveType = value;
            }
        }
        public virtual HtmlInputText PriceInput 
        {
            get { return txtPrice; }
        }


        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
            {
               ddlType.LoadData();
            }

            base.OnInit(e);
        }

    }
}
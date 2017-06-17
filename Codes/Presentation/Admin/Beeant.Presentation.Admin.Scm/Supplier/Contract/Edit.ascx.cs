using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Supplier.Contract
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

        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlPaymentType.LoadData();
                ddlDispatchType.LoadData();
                ddlBillType.LoadData();
            }
            hidSupplierId.Value = Request.Params["SupplierId"] ?? "0";
            base.OnInit(e);
        }

      
    }
}
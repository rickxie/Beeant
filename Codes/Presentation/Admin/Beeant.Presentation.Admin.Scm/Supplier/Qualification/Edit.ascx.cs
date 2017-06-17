

using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Supplier.Qualification
{
    public partial class Edit : System.Web.UI.UserControl
    {
        public virtual SaveType UploaderSaveType
        {
            get { return Uploader1.SaveType; }
            set
            {
                Uploader1.SaveType = value;
                Uploader2.SaveType = value;
                Uploader4.SaveType = value;
                Uploader5.SaveType = value;
                Uploader6.SaveType = value;
            }
        }
        protected override void OnInit(System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlBrandAuthorization.LoadData();
            }
            hidSupplier.Value = Request.Params["Supplierid"] ?? "0";
            base.OnInit(e);
        }








      

       












    }
}
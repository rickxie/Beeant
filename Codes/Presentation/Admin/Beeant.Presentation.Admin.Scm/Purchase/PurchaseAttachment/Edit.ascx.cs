using System;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Purchase.PurchaseAttachment
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            base.OnInit(e);
        }
        public virtual SaveType UploaderSaveType
        {
            get { return Uploader2.SaveType; }
            set
            {
                Uploader2.SaveType = value;
            }
        }
    }
}
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Scm.Supplier.Certification
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


      
      
    }
}
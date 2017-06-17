using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cloud.Site.Company
{
    public partial class Edit : System.Web.UI.UserControl
    {
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
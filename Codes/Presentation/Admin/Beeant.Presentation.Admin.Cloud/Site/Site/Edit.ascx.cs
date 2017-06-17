using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cloud.Site.Site
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
            }
        }
 
    }
}
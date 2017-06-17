using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Controls;

namespace Beeant.Presentation.Admin.Editor.Controls
{
    public partial class Uploader : UploaderControlBase
    {
        public override HtmlInputHidden FileNameControl
        {
            get
            {
                return hfFileName;
            }
            set
            {
                hfFileName = value;
            }
        }
        public override HtmlInputFile FileByteControl
        {
            get
            {
               return fileFileByte;
            }
            set
            {
                fileFileByte = value;
            }
        }
        public override HtmlImage ImageControl
        {
            get
            {
                return imgImage;
            }
            set { imgImage = value; }
        }
        public override HtmlGenericControl ContainerControl
        {
            get
            {
                return divContainer;
            }
            set { divContainer=value; }
        }
        public override HtmlGenericControl ViewControl
        {
            get
            {
                return divView;
            }
            set { divView = value; }
        }
    }
}
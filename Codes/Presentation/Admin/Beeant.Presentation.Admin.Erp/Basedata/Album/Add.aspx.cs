using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Album
{
    public partial class Add : AddPageBase<AlbumEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }

    }
}
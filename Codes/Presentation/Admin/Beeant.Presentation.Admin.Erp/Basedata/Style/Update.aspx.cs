using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Style
{
    public partial class Update : UpdatePageBase<StyleEntity>
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
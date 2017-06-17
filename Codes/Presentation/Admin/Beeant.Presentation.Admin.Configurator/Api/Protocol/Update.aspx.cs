using Beeant.Domain.Entities.Api;

namespace Beeant.Presentation.Admin.Configurator.Api.Protocol
{
    public partial class Update : Basic.Services.WebForm.Pages.UpdatePageBase<ProtocolEntity>
    {
        public override bool IsUpdatePanel
        {
            get
            {
                return false;
            }
            set
            {
                base.IsUpdatePanel = value;
            }
        }
      
    }
}
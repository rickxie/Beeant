using Beeant.Domain.Entities.Api;

namespace Beeant.Presentation.Admin.Configurator.Api.Protocol
{
    public partial class Add : Basic.Services.WebForm.Pages.AddPageBase<ProtocolEntity>
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
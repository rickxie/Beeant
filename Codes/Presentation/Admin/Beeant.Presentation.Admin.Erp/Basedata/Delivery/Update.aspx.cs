using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Delivery
{
    public partial class Update : UpdatePageBase<DeliveryEntity>
    {
        
        public override bool IsUpdatePanel
        {
            get { return false; }
            set { base.IsUpdatePanel = value; }
        }

    }
}
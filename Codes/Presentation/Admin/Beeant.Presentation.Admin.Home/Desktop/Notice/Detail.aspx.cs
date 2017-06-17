using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Home.Desktop.Notice
{
    public partial class Detail : DetailPageBase<ContentEntity>
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }
    }
}
using Beeant.Basic.Services.WebForm.Pages;
namespace Beeant.Presentation.Admin.Home.Desktop.Index
{
    public partial class List : AuthorizePageBase
    {
        public override bool IsVerifyResource
        {
            get { return false; }
        }
    }
}
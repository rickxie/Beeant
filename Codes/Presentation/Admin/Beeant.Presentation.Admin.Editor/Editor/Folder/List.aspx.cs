using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Editor;
using Beeant.Domain.Entities.Finance;


namespace Beeant.Presentation.Admin.Editor.Editor.Folder
{
    public partial class List : Basic.Services.WebForm.Pages.MaintenPageBase<FolderEntity>
    {
        public override bool IsVerifyResource
        {
            get
            {
                return false;
            }
        }
   
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlSearchType.LoadData();
                ddlType.LoadData();
            }
            base.Page_Load(sender, e);
        }
        /// <summary>
        /// 填充
        /// </summary>
        /// <returns></returns>
        protected override FolderEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Account = new AccountEntity {Id = 0};
            }
            return info;
        }
    }
}
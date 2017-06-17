using Beeant.Domain.Entities.Search;
using Winner.Persistence;


namespace Beeant.Presentation.Admin.Configurator.Search.Word
{
    public partial class List : Basic.Services.WebForm.Pages.MaintenPageBase<WordEntity>
    {


     
        protected void btnIsForbid_Click(object sender, System.EventArgs e)
        {
            SaveEntities(SaveType.Modify, "修改成功", "修改失败", ddlIsForbid);
        }
   
    }
}
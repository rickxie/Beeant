using System;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Product.Comment
{
    public partial class List : ListPageBase<CommentEntity>
    {
 
        protected void btnIsShow_Click(object sender, EventArgs e)
        {
            SaveEntities(SaveType.Modify, "修改成功", "修改失败", ddlIsShow);
        }
     
    }
}
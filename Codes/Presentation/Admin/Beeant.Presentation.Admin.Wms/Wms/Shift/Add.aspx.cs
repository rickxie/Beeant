using System;
using System.Web.UI;
using Dependent;
using Component.Extension;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Wms;

namespace Beeant.Presentation.Admin.Wms.Wms.Shift
{
    public partial class Add : AuthorizePageBase
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual long ShelfId
        {
            get { return Request.QueryString["ShelfId"].Convert<long>(); }
        }

        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            Page.AddExceptionLog();
        }
   
    
        /// <summary>
        /// 保存
        /// </summary>
        public virtual void Save()
        {
            var info = new ShiftEntity
                {
                    Remark = txtContent.Value.Trim(),
                    Count= txtCount.Value.Convert<int>(),
                     Shelf = new ShelfEntity { Id = ShelfId },
                    SaveType = SaveType.Add
                };
        
            Message1.ShowMessage(info.Errors);
         
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }


    }
}
using System;
using System.Linq;
using Component.Extension;
using Beeant.Domain.Entities.Order;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class List : ListPageBase<OrderEntity>
    {
       

        protected override void SetQueryWhere(Winner.Persistence.QueryInfo query)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                var type = (OrderType)Request.QueryString["type"].Convert<int>();
                if (string.IsNullOrEmpty(query.WhereExp))
                {
                    query.Query<OrderEntity>().Where(it => it.Type == type);
                }
            }
            base.SetQueryWhere(query);
      
        }

      

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript("var note=new Winner.Note();note.Initialize();");
        }
   

    
    }
}
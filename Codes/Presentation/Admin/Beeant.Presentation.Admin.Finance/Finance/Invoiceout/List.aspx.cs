using Beeant.Domain.Entities.Finance;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Finance.Finance.Invoiceout
{
    public partial class List : ListPageBase<InvoiceoutEntity>
    {


        //protected override void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    base.GridView_RowDataBound(sender, e);
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (GridView.DataKeys[e.Row.RowIndex]["IsFlush"].Convert<bool>())
        //        {
        //            e.Row.Attributes.Add("style", "color:#FF0000");
        //        }
        //    }

        //}
    }
}
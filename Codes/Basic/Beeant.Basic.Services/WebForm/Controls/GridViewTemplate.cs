using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Beeant.Basic.Services.WebForm.Controls
{

    public class GridViewTemplate : ITemplate
    {
        private string ColumnName { get; set; }

        public GridViewTemplate()
        {

        }

        public GridViewTemplate(string strColumnName)
        {
            ColumnName = strColumnName;
        }

        public void InstantiateIn(Control container)
        {
            var lit = new Literal();
            container.Controls.Add(lit);
            lit.DataBinding += lit_DataBinding;
        }
        void lit_DataBinding(object sender, EventArgs e)
        {
            var lit = (Literal) sender;
            var row = (GridViewRow)lit.NamingContainer;
            lit.Text = DataBinder.Eval(row.DataItem, ColumnName).ToString();
        }

    }

}

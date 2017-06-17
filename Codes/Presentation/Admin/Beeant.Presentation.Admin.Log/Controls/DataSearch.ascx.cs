using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Beeant.Presentation.Admin.Log.Controls
{
    public partial class DataSearch : UserControl
    {
        public TextBox BeginInsertTimeTextBox
        {
            get { return txtBeginInsertTime; }
        }
        public TextBox EndInsertTimeTextBox
        {
            get { return txtEndInsertTime; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
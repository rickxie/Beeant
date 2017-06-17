
using System;
using Beeant.Presentation.Admin.Erp.Controls.Basedata;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Basedata.Brand
{
    public partial class Edit : System.Web.UI.UserControl
    {
        public virtual SaveType UploaderSaveType
        {
            get { return Uploader1.SaveType; }
            set
            {
                Uploader1.SaveType = value;
            }
        }

        /// <summary>
        /// 标签
        /// </summary>
        public TagRadioButtonList TagCheckBoxList
        {
            get { return ckTag; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                ckTag.LoadData();
            }
        }
    }
}
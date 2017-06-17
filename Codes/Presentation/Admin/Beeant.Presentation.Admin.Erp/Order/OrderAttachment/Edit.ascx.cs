using System;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Finance;
using Beeant.Domain.Entities.Order;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Erp.Order.OrderAttachment
{
    public partial class Edit : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        public virtual SaveType UploaderSaveType
        {
            get { return Uploader2.SaveType; }
            set
            {
                Uploader2.SaveType = value;
            }
        }
    }
}
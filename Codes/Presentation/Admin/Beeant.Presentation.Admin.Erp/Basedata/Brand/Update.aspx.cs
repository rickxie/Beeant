using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Basedata.Brand
{
    public partial class Update : UpdatePageBase<BrandEntity>
    {
        public override bool IsUpdatePanel
        {
            get { return false; }
            set
            {
                base.IsUpdatePanel = value;
            }
        }

        protected override BrandEntity FillEntity()
        {
            var info = base.FillEntity();
            if (info != null)
            {
                var values = Edit1.TagCheckBoxList.GetSelectedValues();
                if (values.Length > 0)
                {
                    info.Tag = values;
                    info.SetProperty(it => it.Tag);
                }
            }
            return info;
        }
    }
}
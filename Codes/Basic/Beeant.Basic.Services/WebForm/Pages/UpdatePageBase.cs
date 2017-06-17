using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class UpdatePageBase<T> : DatumPageBase<T> where T : Domain.Entities.BaseEntity, new()
    {
        public override SaveType SaveType
        {
            get
            {
                return SaveType.Modify;
            }
            set
            {
                base.SaveType = value;
            }
        }
    }
}

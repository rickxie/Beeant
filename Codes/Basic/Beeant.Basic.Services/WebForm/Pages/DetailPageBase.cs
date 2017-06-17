using Winner.Persistence;


namespace Beeant.Basic.Services.WebForm.Pages
{
    public class DetailPageBase<T> : DatumPageBase<T> where T : Domain.Entities.BaseEntity, new()
    {
        public override SaveType SaveType
        {
            get
            {
                return SaveType.None;
            }
            set
            {
                base.SaveType = value;
            }
        }


    }
}

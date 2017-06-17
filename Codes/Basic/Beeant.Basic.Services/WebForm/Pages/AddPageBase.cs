using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Pages
{
    public class AddPageBase<T> : DatumPageBase<T> where T : BaseEntity, new()
    {


        public override SaveType SaveType
        {
            get
            {
                return SaveType.Add;
            }
            set
            {
                base.SaveType = value;
            }
        }
 
    }
}

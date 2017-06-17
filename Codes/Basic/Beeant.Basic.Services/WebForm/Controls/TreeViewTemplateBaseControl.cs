using Beeant.Domain.Entities;

namespace Beeant.Basic.Services.WebForm.Controls
{
    public class TreeViewTemplateBaseControl<T> : TreeViewBaseControl where T : BaseEntity, new()
    {
        public override string EntityName
        {
            get
            {
                return typeof(T).FullName;
            }
            set
            {
                base.EntityName = value;
            }
        }
    }
}

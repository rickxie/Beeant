namespace Beeant.Basic.Services.WebForm.Controls
{
    public abstract class DropDownListTemplateBaseControl<T> : DropDownListBaseControl 
    {
       
        public override string ObjectName
        {
            get { return typeof(T).FullName; }
        }

    }
}

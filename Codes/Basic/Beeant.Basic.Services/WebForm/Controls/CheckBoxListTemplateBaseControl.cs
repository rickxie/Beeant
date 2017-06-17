namespace Beeant.Basic.Services.WebForm.Controls
{

    /// <summary>
    ///CheckBoxListControlBase 的摘要说明
    /// </summary>
    public class CheckBoxListTemplateBaseControl<T> : CheckBoxListBaseControl 
    {

        public override string ObjectName
        {
            get { return typeof (T).FullName; }
        }

     
    }
        
}
using System.Web.Mvc;
using Winner.Persistence;

namespace Beeant.Basic.Services.Mvc.Script
{
    static public class ScriptValidatorExtension
    {
        public static ScriptValidator<T> ScriptValidator<T>(this HtmlHelper htmlHelper,
                                             SaveType saveType, string[] propertyNames)
        {

            return new ScriptValidator<T>(saveType, propertyNames);
        }

        
 
    }
}

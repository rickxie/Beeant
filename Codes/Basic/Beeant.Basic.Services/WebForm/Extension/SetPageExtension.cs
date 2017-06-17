using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Component.Extension;
using Winner.Filter;
using Winner.Persistence.Relation;

namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class SetPageExtension
    {


        #region 扩展方法

        /// <summary>
        /// 绑定客户端脚本验证
        /// </summary>
        /// <param name="page"></param>
        /// <param name="saveButton"></param>
        /// <param name="entityType"></param>
        /// <param name="type"></param>
        public static string SetEntity(this Page page, Button saveButton, Type entityType, ValidationType type)
        {
            OrmObjectInfo orm = Winner.Creator.Get<IOrm>().GetOrm(entityType.FullName);
            return SetEntity(page, saveButton, entityType, type, orm == null ? null : orm.Properties);
        }

        /// <summary>
        /// 绑定客户端脚本验证
        /// </summary>
        /// <param name="page"></param>
        /// <param name="saveButton"></param>
        /// <param name="entityType"></param>
        /// <param name="type"></param>
        /// <param name="properties"></param>
        public static string SetEntity(this Page page, Button saveButton, Type entityType, ValidationType type, IList<OrmPropertyInfo> properties)
        {
            if (saveButton == null)
                return "";
            IList<ValidationInfo> valids = Winner.Creator.Get<IValidation>().GetValidations(entityType);
            if (valids == null || valids.Count == 0) return "";
            StringBuilder script = ScriptValidationPageExtension.GetValidateScript(saveButton);
            ScriptValidationPageExtension.BindControlClientValidate(page.Form.Controls, script, properties, valids, type);
            return script.ToString();
        }

        /// <summary>
        /// 绑定客户端脚本验证
        /// </summary>
        /// <param name="page"></param>
        /// <param name="saveButton"></param>
        /// <param name="entityTypes"></param>
        /// <param name="type"></param>
        public static string SetEntity(this Page page, Button saveButton, IDictionary<Type,string> entityTypes, ValidationType type)
        {
            if (saveButton == null)
                return "";
            var validateNames = new List<string>();
            var script = new StringBuilder();
            var i = 0;
            foreach (var entityType in entityTypes)
            {
                OrmObjectInfo orm = Winner.Creator.Get<IOrm>().GetOrm(entityType.Key.FullName);
                if(orm==null)
                    continue;
                var validateName = i == 0 ? "validator" : string.Format("validator{0}", i);
                IList<ValidationInfo> valids = Winner.Creator.Get<IValidation>().GetValidations(entityType.Key);
                if (valids == null || valids.Count == 0) return "";
                ScriptValidationPageExtension.BindControlClientValidate(page.Form.Controls, script, orm.Properties, valids, type, validateName, entityType.Value);
                script.Insert(0, string.Format("var {0}=new Winner.Validator(); {0}.Initialize();", validateName));
                validateNames.Add(validateName);
                i++;
            }
            script.AppendFormat("$('#{0}').click(function()", saveButton.ClientID);
            script.Append("{");
            i = 0;
            foreach (var validateName in validateNames)
            {
                script.AppendFormat("{0} {1}.ValidateSubmit(){2}",i==0?"":"&&", validateName,i== validateNames.Count-1?";":"");
                i++;
            }
            script.Append("});");
            return script.ToString();
        }
        #endregion



    }
}

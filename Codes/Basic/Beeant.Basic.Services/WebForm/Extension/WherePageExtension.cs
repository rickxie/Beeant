using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Component.Extension;
using Winner;
using Winner.Base;
using Winner.Persistence;
namespace Beeant.Basic.Services.WebForm.Extension
{
    static public class WherePageExtension
    {


        #region 扩展方法
        /// <summary>
        /// 得到搜索条件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="query"></param>
        /// <param name="ctrls"></param>
        public static void SetFindQueryByControls<T>(this Page page, QueryInfo query, ControlCollection ctrls)
        {
            SetFindQueryByControls(page, typeof(T), query, ctrls);
        }

        /// <summary>
        /// 得到搜索条件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <param name="query"></param>
        /// <param name="ctrls"></param>
        public static void SetFindQueryByControls(this Page page, Type type, QueryInfo query, ControlCollection ctrls)
        {
            var where = new StringBuilder();
            var having = new StringBuilder();
            AddQueryByControls(type, query, where, having, ctrls);
            SetFindWhere(query, where);
            SetFindHaving(query, having);
        }
        /// <summary>
        /// 设置条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="where"></param>
        private static void SetFindWhere(QueryInfo query, StringBuilder where)
        {
            if (!string.IsNullOrEmpty(query.WhereExp))
            {
                if (where.Length > 0) where.Append(" && ");
                where.Append(query.WhereExp);
            }
            query.WhereExp = where.ToString();
        }
        /// <summary>
        /// 设置条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="having"></param>
        private static void SetFindHaving(QueryInfo query, StringBuilder having)
        {
            if (!string.IsNullOrEmpty(query.HavingExp))
            {
                if (having.Length > 0) having.Append(" && ");
                having.Append(query.HavingExp);
            }
            query.HavingExp = having.ToString();
        }

        /// <summary>
        /// 添加查询
        /// </summary>
        /// <param name="type"></param>
        /// <param name="query"></param>
        /// <param name="where"></param>
        /// <param name="having"></param>
        /// <param name="ctrls"></param>
        private static void AddQueryByControls(Type type, QueryInfo query, StringBuilder where, StringBuilder having, ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                AddQueryByControl(type, query, where, having, ctrl);
                if (ctrl.Controls.Count > 0)
                    AddQueryByControls(type, query, where, having, ctrl.Controls);
                    
            }
        }


        /// <summary>
        /// 添加添加
        /// </summary>
        /// <param name="type"></param>
        /// <param name="query"></param>
        /// <param name="where"></param>
        /// <param name="having"></param>
        /// <param name="ctrl"></param>
        private static void AddQueryByControl(Type type, QueryInfo query, StringBuilder where, StringBuilder having, Control ctrl)
        {
            string searchWhere = ctrl.GetAttributeValue("SearchWhere");
            string searchHaving = ctrl.GetAttributeValue("SearchHaving");
            if (string.IsNullOrEmpty(searchWhere) && string.IsNullOrEmpty(searchHaving)) return;
            object value = GetSearchControlValue(ctrl);
            if (value == null || value.ToString().Equals("")) return;
            string searchParamterName = ctrl.GetAttributeValue("SearchParamterName");
            string unSearchValue = ctrl.GetAttributeValue("UnSearchValue");
            string searchPropertyTypeName = ctrl.GetAttributeValue("SearchPropertyTypeName");
            AddQueryString(type,query, string.IsNullOrEmpty(searchWhere) ? having : where,
                string.IsNullOrEmpty(searchWhere) ? searchHaving : searchWhere, value, searchParamterName, unSearchValue, searchPropertyTypeName);
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="query"></param>
        /// <param name="where"></param>
        /// <param name="searchWhere"></param>
        /// <param name="value"></param>
        /// <param name="searchParamterName"></param>
        /// <param name="unSearchValue"></param>
        /// <param name="searchPropertyTypeName"></param>
        private static void AddQueryString(Type type, QueryInfo query, StringBuilder where, string searchWhere,
                                           object value, string searchParamterName, string unSearchValue,
                                           string searchPropertyTypeName)
        {
            if (!string.IsNullOrEmpty(unSearchValue) && unSearchValue.Equals(value)) return;
            if (where.Length > 0) where.Append(" && ");
            type = GetSearchPropertyType(type, searchPropertyTypeName);
            if (value.GetType().IsArray)
            {
                var array = value as Array;
                if (array.Length == 0) return;
                where.Append("(");
                for (int i = 0; i < array.Length; i++)
                {
                    where.AppendFormat("{0}_{1}", searchWhere, i);
                    if (i != array.Length - 1) where.Append(" || ");
                    query.SetParameter(string.Format("{0}_{1}", searchParamterName, i), type == null
                                                                                            ? array.GetValue(i)
                                                                                            : Creator.Get<IProperty>()
                                                                                                     .TryConvertValue(
                                                                                                         array.GetValue(
                                                                                                             i), type));
                }
                where.Append(")");
            }
            else if (type == typeof (DateTime))
            {
                var dt = value.Convert<DateTime>();
                if (dt == DateTime.MinValue)
                    dt = DateTime.Parse("1900-01-01");
                query.SetParameter(searchParamterName, dt);
                where.Append(searchWhere);
            }
            else
            {
                where.Append(searchWhere);
                if (type != null) value = Creator.Get<IProperty>().TryConvertValue(value, type);
                query.SetParameter(searchParamterName, value);
            }

        }

        /// <summary>
        /// 得到类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchPropertyTypeName"></param>
        /// <returns></returns>
        private static Type GetSearchPropertyType(Type type, string searchPropertyTypeName)
        {
            if (string.IsNullOrEmpty(searchPropertyTypeName) || type==null) return null;
            var typeNames = searchPropertyTypeName.Split('.');
            var oType = type;
            foreach (var typeName in typeNames)
            {
                var propertyType = type.GetProperty(typeName);
                if (propertyType != null) type = propertyType.PropertyType;
            }
            if (type != oType) return type;
            return null;
        }

        /// <summary>
        /// 得到搜索控件值
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private static object GetSearchControlValue(Control ctrl)
        {
            var list = ctrl as DropDownList;
            if (list != null) return list.SelectedValue;
            var box = ctrl as TextBox;
            if (box != null) return box.Text.Trim();
            var input = ctrl as HtmlInputText;
            if (input != null) return input.Value.Trim();
            var hidden = ctrl as HtmlInputHidden;
            if (hidden != null) return hidden.Value.Trim();
            var ckboxlist = ctrl as CheckBoxList;
            if (ckboxlist != null)
            {
                var names = (from ListItem item in ckboxlist.Items where item.Selected select item.Value).ToArray();
                if (names.Length > 0) return names;
                return null;
            }
            var ckbox = ctrl as CheckBox;
            if (ckbox != null) return ckbox.Checked;
            return null;
        }



        #endregion


  
    }
}

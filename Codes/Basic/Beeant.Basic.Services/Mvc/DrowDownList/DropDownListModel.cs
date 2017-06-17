using System.ComponentModel;
using System.Web.Mvc;

namespace Beeant.Basic.Services.Mvc.DrowDownList
{
    public class DropDownListModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 验证名称
        /// </summary>
        public string ValidatePropertyName { get; set; } = "ValidateName";
        /// <summary>
        /// 验证名称
        /// </summary>
        public string ValidateName { get; set; }
        /// <summary>
        /// 选择的值
        /// </summary>
        public string SelectValue { get; set; }
        /// <summary>
        /// 绑定值
        /// </summary>
        public BindingList<SelectListItem> Items { get; set; }
    }
}
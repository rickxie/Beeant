namespace Beeant.Basic.Services.Mvc.ComboBox
{
    public class ComboBoxModel
    {
        /// <summary>
        /// 文本框Id
        /// </summary>
        public string TextId { get; set; }
        /// <summary>
        /// 值Id
        /// </summary>
        public string ValueId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string TextName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ValueName { get; set; }
        /// <summary>
        /// 验证名称
        /// </summary>
        public string TextValidateName { get; set; }
        /// <summary>
        /// 验证名称
        /// </summary>
        public string ValueValidateName { get; set; }
        /// <summary>
        /// 选择的值
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 选择的值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 异步请求
        /// </summary>
        public string AjaxUrl { get; set; }
    }
}
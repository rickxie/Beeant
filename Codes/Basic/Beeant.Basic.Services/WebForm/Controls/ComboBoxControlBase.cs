using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Basic.Services.WebForm.Controls
{

    /// <summary>
    ///ComboBoxControlBase 的摘要说明
    /// </summary>
    public abstract class ComboBoxControlBase : UserControl
    {

        #region JS脚本路径
        private string _scriptPath = "/scripts/Winner/ComboBox/Winner.ComboBox.js";
        /// <summary>
        /// 脚本路径
        /// </summary>
        public string ScriptPath
        {
            get { return _scriptPath; }
            set { _scriptPath = value; }
        }
        #endregion



        /// <summary>
        /// 输入控件
        /// </summary>
        public virtual HtmlInputText InputText { get; set; }

        /// <summary>
        /// 值控件
        /// </summary>
        public virtual HtmlInputHidden InputHidden { get; set; }

        /// <summary>
        /// 存储名称
        /// </summary>
        public string TextSaveName
        {
            get { return InputText.Attributes["SaveName"]; }
            set { InputText.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public string TextBindName
        {
            get { return InputText.Attributes["BindName"]; }
            set { InputText.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 验证名称
        /// </summary>
        public string HiddenValidateName
        {
            get { return InputHidden.Attributes["ValidateName"]; }
            set { InputHidden.Attributes.Add("ValidateName", value); }
        }

        /// <summary>
        /// 存储名称
        /// </summary>
        public string HiddenSaveName
        {
            get { return InputHidden.Attributes["SaveName"]; }
            set { InputHidden.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public string HiddenBindName
        {
            get { return InputHidden.Attributes["BindName"]; }
            set { InputHidden.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 验证名称
        /// </summary>
        public string TextValidateName
        {
            get { return InputText.Attributes["ValidateName"]; }
            set { InputText.Attributes.Add("ValidateName", value); }
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string TextSearchWhere
        {
            get { return InputText.Attributes["SearchWhere"]; }
            set { InputText.Attributes.Add("SearchWhere", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public string TextSearchParamterName
        {
            get { return InputText.Attributes["SearchParamterName"]; }
            set { InputText.Attributes.Add("SearchParamterName", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public string TextSearchPropertyTypeName
        {
            get { return InputText.Attributes["SearchPropertyTypeName"]; }
            set { InputText.Attributes.Add("SearchPropertyTypeName", value); }
        }
        /// <summary>
        /// 不搜索的值
        /// </summary>
        public string TextUnSearchValue
        {
            get { return InputText.Attributes["UnSearchValue"]; }
            set { InputText.Attributes.Add("UnSearchValue", value); }
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string HiddenSearchWhere
        {
            get { return InputHidden.Attributes["SearchWhere"]; }
            set { InputHidden.Attributes.Add("SearchWhere", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public string HiddenSearchParamterName
        {
            get { return InputHidden.Attributes["SearchParamterName"]; }
            set { InputHidden.Attributes.Add("SearchParamterName", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public string HiddenSearchPropertyTypeName
        {
            get { return InputHidden.Attributes["SearchPropertyTypeName"]; }
            set { InputHidden.Attributes.Add("SearchPropertyTypeName", value); }
        }
        /// <summary>
        /// 不搜索的值
        /// </summary>
        public string HiddenUnSearchValue
        {
            get { return InputHidden.Attributes["UnSearchValue"]; }
            set { InputHidden.Attributes.Add("UnSearchValue", value); }
        }
        private bool _isValidateHidden = true;
        /// <summary>
        /// 是否验证隐藏控件
        /// </summary>
        public bool IsValidateHidden
        {
            get { return _isValidateHidden; }
            set { _isValidateHidden = value; }
        }
        /// <summary>
        /// Url
        /// </summary>
        public virtual string AjaxUrl { get; set; }

        private string _urlData = "'name=' + value";
        /// <summary>
        /// 参数
        /// </summary>
        public virtual string UrlData
        {
            get { return _urlData; }
            set { _urlData = value; }
        }

        public string HiddenInputClientId
        {
            get { return InputHidden.ClientID; }
        }


        #region 重写页面加载事件

        protected virtual void Page_Load(object sender, EventArgs e)
        {
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            CreateScriptPager();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 创建客户端脚本
        /// </summary>
        protected virtual void CreateScriptPager()
        {
            Page.RegisterScript(ScriptPath);
            var script = new StringBuilder();
            script.AppendFormat("var {0}=new Winner.ComboBox('{1}', '{2}');{0}.Initialize();", ClientID, InputText.ClientID,
                                        InputHidden.ClientID);
            script.AppendFormat("{0}.GetInfos=function (value) ", ClientID);
            script.Append("{var rev = [];$.ajax({ type: 'GET',");
            script.AppendFormat("url:{0}.AjaxUrl,", ClientID);
            script.Append("async: false,");
            script.AppendFormat("data: {0},", UrlData);
            script.Append("success: function (msg) {rev = eval(msg); }});return rev;};");
            if (IsValidateHidden)
            {
                script.Append("if(typeof validator!=\"undefined\"){");
                script.AppendFormat("$('#{0}').blur(", InputText.ClientID);
                script.Append("function(){");
                script.AppendFormat("validator.Validate(validator.GetValidateInfo($('#{0}')[0]));", InputHidden.ClientID);
                script.Append("});};");
            }
            script.AppendFormat("{0}.AjaxUrl='{1}';", ClientID, AjaxUrl);
            Page.ExecuteScript(script.ToString());

        }

        #endregion
  
    }
        
}
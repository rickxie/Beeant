using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Beeant.Basic.Services.WebForm.Extension;

namespace Beeant.Basic.Services.WebForm.Controls
{

    /// <summary>
    ///EditorControlBase 的摘要说明
    /// </summary>
    public abstract class EditorControlBase : UserControl
    {
        #region JS脚本路径
        private string _scriptPath = "/scripts/Winner/Editor/Winner.Editor.js";
        /// <summary>
        /// 脚本路径
        /// </summary>
        public string ScriptPath
        {
            get { return _scriptPath; }
            set { _scriptPath = value; }
        }
        #endregion

        
        public virtual HtmlTextArea TextArea { get; set; }
        #region 路径属性

        private string _imageUploadUrl ="/Editor/Image/add.aspx";
        /// <summary>
        /// 图片快速上传
        /// </summary>
        public virtual string ImageUploadUrl 
        {
            get {return string.Format("{0}{1}?path={2}",Page.GetUrl("PresentationAdminEditorUrl"), _imageUploadUrl, ImagePath); }
            set { _imageUploadUrl = value; }
        }
        private string _imageBrowserUrl = "/Editor/Image/list.aspx";
        /// <summary>
        /// 图片列表
        /// </summary>
        public virtual string ImageBrowserUrl
        {
            get { return string.Format("{0}{1}?path={2}", Page.GetUrl("PresentationAdminEditorUrl"), _imageBrowserUrl, ImagePath); }
            set { _imageBrowserUrl = value; }
        }
        private string _flashUploadUrl = "/Editor/Flash/add.aspx";
        /// <summary>
        /// falsh快速上传
        /// </summary>
        public virtual string FlashUploadUrl
        {
            get { return string.Format("{0}{1}?path={2}", Page.GetUrl("PresentationAdminEditorUrl"), _flashUploadUrl, FlashPath); }
            set { _flashUploadUrl = value; }
        }
        private string _flashBrowserUrl = "/Editor/Flash/list.aspx";
        /// <summary>
        /// falsh列表
        /// </summary>
        public virtual string FlashBrowserUrl
        {
            get { return string.Format("{0}{1}?path={2}", Page.GetUrl("PresentationAdminEditorUrl"), _flashBrowserUrl, ImagePath); }
            set { _flashBrowserUrl = value; }
        }
        private string _templateUploadUrl = "/Editor/Template/add.aspx";
        /// <summary>
        /// 模板快速上传
        /// </summary>
        public virtual string TemplateUploadUrl
        {
            get { return _templateUploadUrl; }
            set { _templateUploadUrl = value; }
        }
        private string _templateBrowserUrl = "/Editor/Template/list.aspx";
        /// <summary>
        /// 模板列表
        /// </summary>
        public virtual string TemplateBrowserUrl
        {
            get { return string.Format("{0}{1}", Page.GetUrl("PresentationAdminEditorUrl"), _templateBrowserUrl);  }
            set { _templateBrowserUrl = value; }
        }
        private int _width = 800;
        /// <summary>
        /// 模板列表
        /// </summary>
        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _height = 600;
        /// <summary>
        /// 模板列表
        /// </summary>
        public virtual int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        #endregion

        #region 配置属性

        private string _imagePath;
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = Server.UrlEncode(value); }
        }
        private string _flashPath;
        /// <summary>
        /// 图片路径
        /// </summary>
        public string FlashPath
        {
            get { return _flashPath; }
            set { _flashPath = Server.UrlEncode(value); }
        }
        /// <summary>
        /// 存储名称
        /// </summary>
        public string SaveName
        {
            get { return TextArea.Attributes["SaveName"]; }
            set { TextArea.Attributes.Add("SaveName", value); }
        }
        /// <summary>
        /// 绑定名称
        /// </summary>
        public string BindName
        {
            get { return TextArea.Attributes["BindName"]; }
            set { TextArea.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 绑定名称
        /// </summary>
        public string ValidateName
        {
            get { return TextArea.Attributes["ValidateName"]; }
            set { TextArea.Attributes.Add("ValidateName", value); }
        }
        #endregion

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            CreateScriptPager();
        }


        /// <summary>
        /// 创建客户端脚本
        /// </summary>
        protected virtual void CreateScriptPager()
        {
            Page.RegisterScript(ScriptPath);
            Page.ExecuteScript(string.Format("var {0}=new Winner.Editor('{0}');{0}.Initialize({1});document.domain='{2}';", TextArea.ClientID, GetEditorConfig(), Configuration.ConfigurationManager.GetSetting<string>("Domain")));
            var script=new StringBuilder();
            script.Append("$(document).ready(function () {");
            script.Append("var validate= function (){");
            script.AppendFormat("validator.BindControlValidateEvent($('#{0}').next()[0], 'mouseout', $('#{0}')[0]);", TextArea.ClientID);
            script.Append("};setTimeout(validate,1500);});");
            Page.ExecuteScript(script.ToString());
        }

        public virtual string GetEditorConfig()
        {
            var sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("ImageUploadUrl:'{0}'", ImageUploadUrl);
            sb.AppendFormat(",ImageBrowserUrl:'{0}'", ImageBrowserUrl);
            sb.AppendFormat(",FlashUploadUrl:'{0}'", FlashUploadUrl);
            sb.AppendFormat(",FlashBrowserUrl:'{0}'", FlashBrowserUrl);
            sb.AppendFormat(",TemplateUploadUrl:'{0}'", TemplateUploadUrl);
            sb.AppendFormat(",TemplateBrowserUrl:'{0}'", TemplateBrowserUrl);
            sb.AppendFormat(",Height:{0}",Height);
            sb.AppendFormat(",Width:{0}", Width);
            sb.AppendFormat(",Domain:'{0}'", Configuration.ConfigurationManager.GetSetting<string>("Domain"));
            sb.Append("}");
            return sb.ToString();
        }
    }

}
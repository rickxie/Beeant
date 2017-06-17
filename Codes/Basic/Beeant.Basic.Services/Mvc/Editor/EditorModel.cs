using Configuration;

namespace Beeant.Basic.Services.Mvc.Editor
{
    public class EditorModel
    {
        public string Id { get; set; } = "txtEditor";
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string FlashPath { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; } = 700;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; } = 600;

        private string _imageUploadUrl;
        /// <summary>
        /// 图片快速上传
        /// </summary>
        public virtual string ImageUploadUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_imageUploadUrl) && !string.IsNullOrWhiteSpace(ImagePath))
                    _imageUploadUrl = string.Format("{0}/Image/Upload?path={1}", ConfigurationManager.GetSetting<string>("PresentationWebsiteEditorUrl"), ImagePath);
                return _imageUploadUrl;
            }
            set { _imageUploadUrl = value; }
        }
        private string _imageBrowserUrl;
        /// <summary>
        /// 图片列表
        /// </summary>
        public virtual string ImageBrowserUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_imageBrowserUrl) && !string.IsNullOrWhiteSpace(ImagePath))
                    _imageBrowserUrl = string.Format("{0}/Image/Index?path={1}", ConfigurationManager.GetSetting<string>("PresentationWebsiteEditorUrl"), ImagePath);
                return _imageBrowserUrl;
            }
            set { _imageBrowserUrl = value; }
        }
        private string _flashUploadUrl ;
        /// <summary>
        /// falsh快速上传
        /// </summary>
        public virtual string FlashUploadUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_flashUploadUrl)&& !string.IsNullOrWhiteSpace(FlashPath))
                    _flashUploadUrl = string.Format("{0}/Flash/Upload?path={1}", ConfigurationManager.GetSetting<string>("PresentationWebsiteEditorUrl"), FlashPath);
                return _flashUploadUrl;
            }
            set { _flashUploadUrl = value; }
        }
        private string _flashBrowserUrl;
        /// <summary>
        /// falsh列表
        /// </summary>
        public virtual string FlashBrowserUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_flashBrowserUrl) && !string.IsNullOrWhiteSpace(FlashPath))
                    _flashBrowserUrl = string.Format("{0}/Flash/Index?path={1}", ConfigurationManager.GetSetting<string>("PresentationWebsiteEditorUrl"), FlashPath);
                return _flashBrowserUrl;
            }
            set { _flashBrowserUrl = value; }
        }
        private string _templateUploadUrl;
        /// <summary>
        /// 模板快速上传
        /// </summary>
        public virtual string TemplateUploadUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_templateUploadUrl))
                    _templateUploadUrl = string.Format("{0}/Template/Upload", ConfigurationManager.GetSetting<string>("PresentationWebsiteEditorUrl"));
                return _templateUploadUrl;
            }
            set { _templateUploadUrl = value; }
        }
        private string _templateBrowserUrl ;
        /// <summary>
        /// 模板列表
        /// </summary>
        public virtual string TemplateBrowserUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_templateBrowserUrl))
                    _templateBrowserUrl = string.Format("{0}/Template/Index", ConfigurationManager.GetSetting<string>("PresentationWebsiteEditorUrl"));
                return _templateBrowserUrl;
            }
            set { _templateBrowserUrl = value; }
        }
    }
}
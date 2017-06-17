using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Beeant.Application.Services.Utility;
using Dependent;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;

namespace Beeant.Basic.Services.WebForm.Controls
{

    /// <summary>
    ///UploaderControlBase 的摘要说明
    /// </summary>
    public abstract class UploaderControlBase : UserControl
    {
        #region JS脚本路径
        private string _scriptPath = "/scripts/Winner/Uploader/Winner.Uploader.js";
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
        /// 存储类型
        /// </summary>
        public virtual SaveType SaveType { get; set; }

        /// <summary>
        /// 绑定控件
        /// </summary>
        public virtual HtmlInputFile FileByteControl { get; set; }
        /// <summary>
        /// 绑定控件
        /// </summary>
        public virtual HtmlInputHidden FileNameControl { get; set; }
        /// <summary>
        /// 图片控件
        /// </summary>
        public virtual HtmlImage ImageControl { get; set; }
        /// <summary>
        /// 容器
        /// </summary>
        public virtual HtmlGenericControl ContainerControl { get; set; }
        /// <summary>
        /// 容器
        /// </summary>
        public virtual HtmlGenericControl ViewControl { get; set; }

        /// <summary>
        /// 是否显示图片
        /// </summary>
        public virtual bool IsShowViewControl
        {
            get { return ViewControl.Visible; }
            set { ViewControl.Visible = value; }
        }
        /// <summary>
        /// 设置是否多选
        /// </summary>
        public virtual bool IsMultiple
        {
            get { return FileByteControl.Attributes["multiple"] == "true"; }
            set
            {
                if (value)
                {
                    FileByteControl.Attributes.Add("multiple", "true");
                }
                else
                {
                    FileByteControl.Attributes.Remove("multiple");
                }
            }
        }
        /// <summary>
        /// 设置是否多选
        /// </summary>
        public virtual string Accept
        {
            get { return FileByteControl.Attributes["Accept"]; }
            set
            {
                FileByteControl.Attributes.Add("Accept", value);
            }
        }
        /// <summary>
        /// 文件流存储名称
        /// </summary>
        public string FileByteSaveName
        {
            get { return FileByteControl.Attributes["SaveName"]; }
            set { FileByteControl.Attributes.Add("SaveName", value); }
        }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 文件名存储名称
        /// </summary>
        public string FileNameSaveName
        {
            get { return FileNameControl.Attributes["SaveName"]; }
            set { FileNameControl.Attributes.Add("SaveName", value); }
        }
        /// <summary>
        /// 文件名绑定名称
        /// </summary>
        public string FileNameBindName
        {
            get { return FileNameControl.Attributes["BindName"]; }
            set { FileNameControl.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 图片的路径绑定名称
        /// </summary>
        public string FullFileNameBindName
        {
            get { return ImageControl.Attributes["BindName"]; }
            set { ImageControl.Attributes.Add("BindName", value); }
        }


 
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetControlUploaderProperty();
            CreateScriptPager();
            if (string.IsNullOrEmpty(ImageControl.Src))
                ImageControl.Src = string.Format("{0}/Images/Nopic.jpg", Page.GetUrl("PresentationAdminHomeUrl"));
        }
        #region 方法

        /// <summary>
        /// 设置控件上传属性
        /// </summary>
        protected virtual void SetControlUploaderProperty()
        {
            if (FileByteControl != null) FileByteControl.Attributes.Add("Uploader", "File");
            if (ViewControl != null) ViewControl.Attributes.Add("Uploader", "View");
        }

        /// <summary>
        /// 创建客户端脚本
        /// </summary>
        protected virtual void CreateScriptPager()
        {
            Page.RegisterScript(ScriptPath);
            Page.ExecuteScript(string.Format("var {0}=new Winner.Uploader('{0}');{0}.Initialize();", ContainerControl.ClientID));
        }
        
        #endregion

        /// <summary>
        /// 设置文件名
        /// </summary>
        public virtual string GetFileName()
        {
            if (FileByteControl == null)
                return null;
            if (FileByteControl.PostedFile != null && FileByteControl.PostedFile.InputStream.Length > 0)
                return string.Format("{0}{1}", Path, System.IO.Path.GetFileName(FileByteControl.PostedFile.FileName));
            return FileNameControl.Value;
        }

        /// <summary>
        /// 得到文件流
        /// </summary>
        /// <returns></returns>
        public virtual byte[] GetFileByte()
        {
            if (FileByteControl != null && FileByteControl.PostedFile!=null && FileByteControl.PostedFile.InputStream.Length>0)
            {
                var stream = FileByteControl.PostedFile.InputStream;
                if (stream.Length != 0)
                {
                    var fileByte = new byte[stream.Length];
                    stream.Read(fileByte, 0, fileByte.Length);
                    return fileByte;
                }
            }
            if (string.IsNullOrEmpty(FileNameControl.Value) || SaveType!=SaveType.Add)
                return null;
            return Ioc.Resolve<IFileApplicationService>().Grab(FileNameControl.Value);
        }
 
    }

}
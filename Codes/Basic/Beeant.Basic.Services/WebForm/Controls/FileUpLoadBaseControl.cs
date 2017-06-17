using System;
using System.Web.UI;

namespace Beeant.Basic.Services.WebForm.Controls
{
    public class FileUpLoadBaseControl : UserControl
    {
        protected int chunkSize = 1*1024*1024;
        /// <summary>
        /// 分片大小
        /// </summary>
        public int ChunkSize
        {
            set { chunkSize = value; }
            get { return chunkSize; }
        }

        protected string token = Guid.NewGuid().ToString();
        /// <summary>
        /// 全局Token
        /// </summary>
        public string Token
        {
            set { token = value; }
            get { return token; }
        }

        protected string completeMessage = "文件上传成功!";
        /// <summary>
        /// 上传成功后显示文本信息
        /// </summary>
        public string CompleteMessage
        {
            set { completeMessage = value; }
            get { return completeMessage; }
        }

        /// <summary>
        /// 附件上传地址
        /// </summary>
        public string UpLoadUrl { set; get; }

        /// <summary>
        /// 暂停按钮图片地址
        /// </summary>
        public virtual string PauseImgUrl { set; get; }
        /// <summary>
        /// 继续按钮图片地址
        /// </summary>
        public virtual string ResumeImgUrl { set; get; }

        /// <summary>
        /// 对应上传路径
        /// </summary>
        public string Target { set; get; }

        protected int shardSize = 1048576;
        /// <summary>
        /// 分片大小
        /// </summary>
        public int ShardSize
        {
            set { shardSize = value; } 
            get { return shardSize; }
        }

        private string uploadBtnName = "上传文件";
        /// <summary>
        /// 文件上传按钮名称
        /// </summary>
        public string UploadBtnName
        {
            set { uploadBtnName = value; }
            get { return uploadBtnName; }
        }

        /// <summary>
        /// 输出转换后的ID
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        private string GetId(string sid)
        {
            return string.Format("{0}_{1}", sid, ClientID);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            #region 上传文件按钮
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute("href", "#");
            writer.AddAttribute("id", GetId("browseButton"));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.WriteLine(UploadBtnName);
            writer.RenderEndTag();
            writer.RenderEndTag();
	        #endregion

            #region Table
            writer.AddAttribute("class", "resumable-progress");
            writer.AddAttribute("id", GetId("resumable-progress"));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //Table
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            //Tr
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            #region 第一个TD
            //Td
            writer.AddAttribute("width", "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //progress-container
            writer.AddAttribute("class", "progress-container");
            writer.AddAttribute("id", GetId("progress-container"));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
           
            //progress-bar
            writer.AddAttribute("class", "progress-bar");
            writer.AddAttribute("id", GetId("progress-bar"));
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            //progress-container End
            writer.RenderEndTag();


            //Td End
            writer.RenderEndTag();
            #endregion

            #region 第二个TD
            writer.AddAttribute("class", "progress-text");
            writer.AddAttribute("id", GetId("progress-text"));
            writer.AddAttribute("nowrap", "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
            #endregion

            #region 第三个TD
            writer.AddAttribute("class", "progress-pause");
            writer.AddAttribute("id", GetId("progress-pause"));
            writer.AddAttribute("nowrap", "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //继续
            writer.AddAttribute("href", "#");
            writer.AddAttribute("onclick", "r.upload(); return(false);");
            writer.AddAttribute("class", "progress-resume-link");
            writer.AddAttribute("id", GetId("progress-resume-link"));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute("src", ResumeImgUrl);
            writer.AddAttribute("title", "继续上传");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //暂停
            writer.AddAttribute("href", "#");
            writer.AddAttribute("onclick", "r.pause(); return(false);");
            writer.AddAttribute("class", "progress-pause-link");
            writer.AddAttribute("id", GetId("progress-pause-link"));
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute("src", PauseImgUrl);
            writer.AddAttribute("title", "暂停");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
            #endregion

            //Tr End
            writer.RenderEndTag();
            //Table End
            writer.RenderEndTag();
            //Div End
            writer.RenderEndTag();
            #endregion

            #region Span
            writer.AddAttribute("class", "resumable-list");
            writer.AddAttribute("id", GetId("resumable-list"));
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.RenderEndTag();
            #endregion
            
            RenderScripts(writer);
        }

        private void RenderScripts(HtmlTextWriter writer)
        {
            writer.WriteLine("<script>");
            RenderResumable(writer);
            writer.WriteLine("</script>");
        }

        private void RenderResumable(HtmlTextWriter writer)
        {
            writer.WriteLine(string.Format("var {0} = new Resumable({{", GetId("r")));
            writer.WriteLine(string.Format("   target: '{0}',", Target));
            writer.WriteLine("   chunkSize: {0},", ShardSize);
            writer.WriteLine("   simultaneousUploads: 1,");
            writer.WriteLine("   testChunks: false,");
            writer.WriteLine("   throttleProgressCallbacks: 1,");
            writer.WriteLine("   method: \"octet\",");
            writer.WriteLine(string.Format("   query: {{ upload_token: '{0}' }}", token));
            writer.WriteLine("});");

            
            // Resumable.js isn't supported, fall back on a different method
            writer.WriteLine(string.Format("if (!{0}.support) {{" , GetId("r")));
            writer.WriteLine("    alert('您的浏览器不支持Html5，请更换支持Html5的浏览器');");
            writer.WriteLine("} else {");
            writer.WriteLine("    // Show a place for dropping/selecting files");
            writer.WriteLine(string.Format("    {0}.assignBrowse(document.getElementById('{1}'));", GetId("r"), GetId("browseButton")));
            writer.WriteLine("");
            writer.WriteLine("    // Handle file add event");
            writer.WriteLine(string.Format("    {0}.on('fileAdded', function (file) {{", GetId("r")));
            writer.WriteLine("        file.startchunkindex = 0; // 设置当前文件开始上传的块数");
            writer.WriteLine("        // Show progress pabr");
            writer.WriteLine(string.Format("        $('#{0}, #{1}').show();", GetId("resumable-progress"), GetId("resumable-list")));
            writer.WriteLine("        // Show pause, hide resume");
            writer.WriteLine(string.Format("        $('#{0}').hide();", GetId("progress-resume-link")));
            writer.WriteLine(string.Format("        $('#{0}').show();", GetId("progress-pause-link")));
            writer.WriteLine("        // Add the file to the list");
            writer.WriteLine(string.Format("        $('#{0}').html('文件上传...');", GetId("resumable-list")));
            writer.WriteLine(string.Format("        {0}.upload();", GetId("r")));
            writer.WriteLine("    });");
            writer.WriteLine("");
            writer.WriteLine(string.Format("    {0}.on('uploadStart', function () {{", GetId("r")));
            writer.WriteLine(string.Format("        $('#{0}').hide();", GetId("progress-resume-link")));
            writer.WriteLine(string.Format("        $('#{0}').show();", GetId("progress-pause-link")));
            writer.WriteLine(string.Format("        $('#{0}').hide(); //开始上传时，选择文件按钮禁用，每次只允许传一个文件", GetId("resumable-browse")));
            writer.WriteLine("    });");
            writer.WriteLine(string.Format("    {0}.on('pause', function () {{", GetId("r")));
            writer.WriteLine("        // Show resume, hide pause");
            writer.WriteLine(string.Format("        $('#{0}').show();", GetId("progress-resume-link")));
            writer.WriteLine(string.Format("        $('#{0}').hide();", GetId("progress-pause-link")));
            writer.WriteLine("    });");
            writer.WriteLine(string.Format("    {0}.on('complete', function () {{", GetId("r")));
            writer.WriteLine("        // Hide pause/resume when the upload has completed");
            writer.WriteLine(string.Format("        $('#{0}, #{1}').hide();", GetId("progress-resume-link"), GetId("progress-pause-link")));
            writer.WriteLine(string.Format("        $('#{0}').show(); //上传成功后选择文件按钮可用", GetId("resumable-browse")));
            writer.WriteLine("    });");
            writer.WriteLine(string.Format("    {0}.on('fileSuccess', function (file, message) {{", GetId("r")));
            writer.WriteLine("        // Reflect that the file upload has completed");
            writer.WriteLine(string.Format("        $('#{0}').html('{1}');", GetId("resumable-list"), CompleteMessage));
            writer.WriteLine("    });");
            writer.WriteLine(string.Format("    {0}.on('fileError', function (file, message) {{", GetId("r")));
            writer.WriteLine("        // Reflect that the file upload has resulted in error");
            writer.WriteLine(string.Format("        $('#{0}').html('文件上传失败:' + message);", GetId("resumable-list")));
            writer.WriteLine("    });");
            writer.WriteLine(string.Format("    {0}.on('fileProgress', function (file) {{", GetId("r")));
            writer.WriteLine("        // Handle progress for both the file and the overall upload");
            writer.WriteLine(string.Format("        $('#{0}').html('文件上传...' + Math.floor(file.progress() * 100) + '%');", GetId("resumable-list")));
            writer.WriteLine(string.Format("        $('#{0}').css({{ width: Math.floor({1}.progress() * 100) + '%' }});", GetId("progress-bar"), GetId("r")));
            writer.WriteLine("    });");
            writer.WriteLine("}");
        }
    }
}

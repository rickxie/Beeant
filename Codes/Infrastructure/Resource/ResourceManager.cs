using System;
using System.IO;
using System.Text;
using Yahoo.Yui.Compressor;

namespace Resource
{
    public static class ResourceManager
    {
        #region 压缩文件
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="path"></param>
        public static void Compress(string path)
        {
            if(!Directory.Exists(path))
                return;
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var finfo = new FileInfo(file);
                if(finfo.Attributes == FileAttributes.Compressed)
                    continue;
                string strContent = File.ReadAllText(file, Encoding.UTF8);
                try
                {
                    File.SetAttributes(file, FileAttributes.Compressed);
                    if (!file.Contains(".min."))
                    {
                        if (finfo.Extension.ToLower() == ".js")
                        {
                            var js = new JavaScriptCompressor
                            {
                                CompressionType = CompressionType.Standard,
                                Encoding = Encoding.UTF8,
                                IgnoreEval = false,
                                ThreadCulture = System.Globalization.CultureInfo.CurrentCulture
                            };
                            strContent = js.Compress(strContent);
                            File.WriteAllText(file, strContent, Encoding.UTF8);
                        }
                        else if (finfo.Extension.ToLower() == ".css")
                        {
                            var css = new CssCompressor();
                            strContent = css.Compress(strContent);
                            File.WriteAllText(file, strContent, Encoding.UTF8);
                        }
   
                    }
                }
                catch (Exception ex)
                {
               
                }
               
            }
        }
        #endregion
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Component.Extension
{
    public static class WebsiteImageHelper
    {
        /// <summary>
        /// 生成
        /// </summary>
        public static WebsiteImage Create()
        {
            var wi=new WebsiteImage();
            return wi;
        }
    }
    public class WebsiteImage
    {
      
        public Action<string,byte[]> Handle { get; set; }
        /// <summary>
        /// 网站
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 质量值
        /// </summary>
        public long QuityValue { get; set; } = 100L;

        /// <summary>
        /// 生成图片快照
        /// </summary>
        /// <param name="url"></param>
        /// <param name="handle"></param>
        /// <param name="quityValue"></param>
        /// <returns></returns>
        public void Generate(string url, Action<string,byte[]> handle, long quityValue)
        {
            Handle = handle;
            Url = url;
            QuityValue = quityValue;
            var thread = new Thread(ThreadGenerate);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    
        /// <summary>
        /// 线程
        /// </summary>
        protected virtual void ThreadGenerate()
        {
            var browser = new WebBrowser { ScrollBarsEnabled = false };
            browser.Navigate(Url);
            browser.DocumentCompleted += Browser_DocumentCompleted;
            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            browser.Dispose();
        }
        /// <summary>
        /// 得到编码
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        protected virtual ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = (WebBrowser)sender;
            if (browser == null || browser.Document==null || browser.Document.Body==null)
                return ;
            browser.ClientSize = new Size(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Bottom);
            browser.ScrollBarsEnabled = false;
            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, QuityValue);
            myEncoderParameters.Param[0] = myEncoderParameter;
            var bitmap = new Bitmap(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Bottom);
            browser.BringToFront();
            browser.DrawToBitmap(bitmap, browser.Bounds);
            byte[] bs;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, GetEncoderInfo("image/jpeg"), myEncoderParameters);
                bs = ms.GetBuffer();
            }
            Handle(Url,bs);
       
        }
 
    }
}

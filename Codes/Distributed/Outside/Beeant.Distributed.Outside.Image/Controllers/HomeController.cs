using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Extension;
using Winner.Storage;

namespace Beeant.Distributed.Outside.Image.Controllers
{
    public class HomeController : Controller
    {
      
        //
        // GET: /Mobile/
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public virtual ActionResult Index(string filename,string tag)
        {
            var url = Winner.Creator.Get<IFile>().GetFullFileName(filename);
            if (url == filename)
                return DefaultImage();
            if (!string.IsNullOrEmpty(tag))
                url = string.Format("{0}.{1}{2}", url, tag, System.IO.Path.GetExtension(filename));
            return Redirect(url);
        }
        /// <summary>
        /// 返回默认图片
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DefaultImage()
        {
            return Redirect(this.GetNoPicture());
        }


    
    }
}

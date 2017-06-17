using Component.Extension;
using Beeant.Domain.Entities.Cms;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Cms.Cms.Postcard
{
    public partial class List : ListPageBase<PostcardEntity>
    {

       
        /// <summary>
        /// 生成二维码 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreate_Click(object sender, System.EventArgs e)
        {
            CreateFileName();
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        protected virtual void CreateFileName()
        {
            var infos = GetSaveEntities<PostcardEntity>(SaveType.Modify);
            if (infos != null)
            {
                foreach (var info in infos)
                {
                    info.FileByte =
                        QrEncodHelper.Create(string.Format("{0}/home/index/{1}",
                            this.GetUrl("PresentationMobilePostcardUrl"), info.Id));
                    info.FileName = "1.jpg";
                    info.SetProperty(it => it.FileName);
                }
            }
            SaveEntities(infos, "生成成功", "生成失败");
        }

    }
}
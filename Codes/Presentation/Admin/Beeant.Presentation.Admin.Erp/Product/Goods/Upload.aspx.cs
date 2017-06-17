using System;
using System.Collections.Generic;
using Configuration;
using Dependent;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Winner.Filter;

namespace Beeant.Presentation.Admin.Erp.Product.Goods
{
    public partial class Upload : AuthorizePageBase
    {
        public override bool IsVerifyResource
        {
            get
            {
                return false;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var fileroute = Winner.Creator.Get<Winner.Storage.IFile>();
                var fileName = string.Format("Files/Temp/{0}", Request.Files[0].FileName);
                fileName = fileroute.CreateFileName(fileName);
                var fileByte = new byte[Request.Files[0].InputStream.Length];
                Request.Files[0].InputStream.Read(fileByte, 0, fileByte.Length);
                var goodsImage = new GoodsImageEntity
                    {
                        FileByte = fileByte,
                        FileName = fileName
                    };
                var errors= Winner.Creator.Get<IValidation>()
                      .ValidateInfo(goodsImage, ValidationType.Modify, new List<string> {"FileName", "FileByte"});
                if (errors != null && errors.Count > 0)
                {
                    Response.Write(string.Format("<script type='text/javascript'>document.domain='{0}';parent.Goods.Publisher.Image.UploadFailure('{1}','{2}');</script>",ConfigurationManager.GetSetting<string>("Domain"), Request.QueryString["ctrlid"], errors[0].Message));
                    return;
                }
                Ioc.Resolve<IFileApplicationService>().Save(fileName, fileByte);
                Response.Write(string.Format("<script type='text/javascript'>document.domain='{0}';parent.Goods.Publisher.Image.UploadSucess('{1}','{2}');</script>", ConfigurationManager.GetSetting<string>("Domain"), Request.QueryString["ctrlid"], goodsImage.FullFileName));
            }
        }
    }
}
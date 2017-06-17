using System;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Gis;
using Beeant.Basic.Services.WebForm.Pages;
using Component.Extension;
using Dependent;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Gis.Gis.Area
{
    public partial class SaveAjax : AuthorizePageBase
    {

        public long RequestId
        {
            get { return Request.QueryString["id"].Convert<long>(); }
        }

        protected  void Page_Load(object sender, EventArgs e)
        {
            Save();
        }

        protected virtual AreaEntity FillEntity()
        {
            var info = new AreaEntity
            {
                Id = RequestId,
                Name = Server.UrlDecode(Request["name"]),
                Type = Server.UrlDecode(Request["type"]),
                Path = Server.UrlDecode(Request["path"]),
                City = Server.UrlDecode(Request["city"]),
                Color = Server.UrlDecode(Request["color"]),
                Tag = Server.UrlDecode(Request["tag"]),
                Value= Server.UrlDecode(Request["value"]),
                IsUsed = Request["isUsed"].Convert<bool>(),
                SaveType = RequestId==0?SaveType.Add : SaveType.Modify
            };
            if (Request["savetype"] == "remove")
            {
                info.SaveType=SaveType.Remove;
            }
            info.Import();
            if (info.SaveType == SaveType.Add)
            {
                info.Path = "";
            }
            else if (info.SaveType == SaveType.Modify || info.SaveType == SaveType.Remove)
            {
                info.SetProperty(it => it.Origin);
                info.SaveType = SaveType.Modify;
            }
            return info;
        }
        
        protected virtual void Save()
        {
            AreaEntity info = FillEntity();
            var rev = Ioc.Resolve<IApplicationService, AreaEntity>().Save(info);
            if (rev)
            {
                if (info.SaveType == SaveType.Add)
                {
                    var id = info.Id;
                    info.Publish();
                    info.Id = id;
                    info.Import();
                    info.Properties = null;
                    info.SetProperty(it => it.Origin);
                    info.SaveType = SaveType.Modify;
                    Ioc.Resolve<IApplicationService, AreaEntity>().Save(info);
                }
                Response.Write("{Code:\"true\",Message:\""+info.Id+"\"}");
         
            }
            else if (info.Errors != null && info.Errors.Count > 0)
            {
                Response.Write("{Code:\"false\",Message:\"" + info.Errors[0].Message + "\"}");
        
            }
            else
            {
                Response.Write("{Code:\"false\",Message:\"保存失败\"}");
            }
        }

    }

}
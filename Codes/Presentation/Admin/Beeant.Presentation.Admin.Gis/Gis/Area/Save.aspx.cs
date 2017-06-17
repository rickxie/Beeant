using System;
using System.Collections.Generic;
using Beeant.Domain.Entities.Gis;
using Beeant.Basic.Services.WebForm.Pages;
using Configuration;
using Winner.Filter;
using Winner.Persistence;

namespace Beeant.Presentation.Admin.Gis.Gis.Area
{
    public partial class Save : DatumPageBase<AreaEntity>
    {
        public override bool IsImageRecover
        {
            get { return false; }
        }

        public override bool IsVerifyResource
        {
            get { return false; }
        }

        public override SaveType SaveType
        {
            get { return RequestId == 0 ? SaveType.Add : SaveType.Modify; }
            set { base.SaveType = value; }
        }

        protected override AreaEntity FillEntity()
        {
            var info= base.FillEntity();
            if (info != null)
            {
                info.Id = RequestId;
                info.Name = Server.UrlDecode(Request["name"]);
                info.Type= Server.UrlDecode(Request["type"]);
                info.Path= Server.UrlDecode(Request["path"]);
                info.City = Server.UrlDecode(Request["city"]);
                info.Color = Server.UrlDecode(Request["color"]);
            }
            return info;
        }

        protected override void SetResult(bool rev, IList<ErrorInfo> errors)
        {
            if (rev)
            {
                Response.Write("true");
            }
            else if (errors != null && errors.Count > 0)
            {
                Response.Write(errors[0].Message);
            }
            else
            {
                Response.Write("保存失败");
            }
            
        }
    }

}
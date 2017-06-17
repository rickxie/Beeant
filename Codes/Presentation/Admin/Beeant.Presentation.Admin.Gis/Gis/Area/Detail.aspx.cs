using System;
using Beeant.Domain.Entities.Gis;
using Beeant.Basic.Services.WebForm.Pages;
using Configuration;

namespace Beeant.Presentation.Admin.Gis.Gis.Area
{
    public partial class Detail : DetailPageBase<AreaEntity>
    {
        public override bool IsImageRecover
        {
            get { return false; }
        }

        public override bool IsVerifyResource
        {
            get { return false; }
        }

        protected override void BindEntity(AreaEntity info)
        {
            if (info != null)
            {
                var tempEntity = info;
                if (Request["ispublish"] != "true")
                    tempEntity = info.GetOrigin();
                var value = string.Format("name:'{0}',tag:'{1}',value:'{2}',color:'{3}',isused:{4}", tempEntity.Name, tempEntity.Tag, tempEntity.Value, tempEntity.Color, tempEntity.IsUsed.ToString().ToLower());
                Response.Write("{"+ value + "}");
            }
        }
    }

}
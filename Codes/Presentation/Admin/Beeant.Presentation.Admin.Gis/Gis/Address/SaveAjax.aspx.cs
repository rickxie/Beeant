using System;
using System.Linq;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Gis;
using Beeant.Basic.Services.WebForm.Pages;
using Component.Extension;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Gis.Gis.Address
{
    public partial class SaveAjax : AuthorizePageBase
    {

     

        protected  void Page_Load(object sender, EventArgs e)
        {
            Save();
        }
        /// <summary>
        /// 得到地址
        /// </summary>
        /// <returns></returns>
        protected virtual AddressEntity GetAddress()
        {
            var query=new QueryInfo();
            query.Query<AddressEntity>()
                .Where(
                    it => it.Name == Server.UrlDecode(Request["name"]) )
                .Select(it => it.Id);
            var infos = Ioc.Resolve<IApplicationService>().GetEntities<AddressEntity>(query);
            return infos?.FirstOrDefault();

        }

        protected virtual AddressEntity FillEntity()
        {
            var info = new AddressEntity
            {
                Name = Server.UrlDecode(Request["name"]),
                Point = Server.UrlDecode(Request["point"]),
                City = Server.UrlDecode(Request["city"]),
                IsStartWith = Request["IsStartWith"].Convert<bool>()
            };

            return info;
        }
        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Save()
        {
            var dataEntity = GetAddress();
            if (Request["savetype"] == "remove" && dataEntity==null)
            {
                return;
            }
            AddressEntity info = FillEntity();
           
            if (dataEntity == null)
            {
                info.SaveType=SaveType.Add;
            }
            else
            {
                info.Id = dataEntity.Id;
                info.SaveType=SaveType.Modify;
            }
            if (Request["savetype"] == "remove")
            {
                info.SaveType = SaveType.Remove;
            }
            var rev = Ioc.Resolve<IApplicationService, AddressEntity>().Save(info);
            if (rev)
            {
                Response.Write("保存成功");
            }
            else if (info.Errors != null && info.Errors.Count > 0)
            {
                Response.Write(info.Errors[0].Message);
        
            }
            else
            {
                Response.Write("保存失败");
            }
        }

    }

}
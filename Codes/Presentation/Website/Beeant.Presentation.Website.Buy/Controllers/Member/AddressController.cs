using System.Collections.Generic;
using System.Web.Mvc;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Presentation.Website.Buy.Models.Member;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Winner.Persistence;

namespace Beeant.Presentation.Website.Buy.Controllers.Member
{

    public class AddressController : BaseController
    {


        /// <summary>
        /// 保存地址
        /// </summary>
        /// <param name="model"></param>
        /// <param name="saveType"></param>
        /// <returns></returns>
        protected virtual AddressEntity SaveAddress(AddressModel model, SaveType saveType)
        {

            var info = new AddressEntity
            {
                Id = model.Id,
                Address = model.Address,
                Postcode = model.Postcode,
                Country="国家",
                Province = model.Province,
                City=model.City,
                County = model.County,
                Mobile = model.Mobile,
                Recipient = model.Recipient,
                SaveType = saveType
            };
            if (saveType == SaveType.Add)
            {
                info.Account = new AccountEntity { Id = Identity.Id };
            }
            this.SaveEntity(info);
            return info;
        }

        #region 对话框
        [AuthorizeFilter]
        public virtual ActionResult Dialog(long id)
        {
            var model = GetModel(id);
            return View("~/Views/Member/Address/Dialog.cshtml", model);
        }

        /// <summary>
        /// 添加新地址
        /// </summary>
        /// <param name="model"></param>
        [AuthorizeFilter]
        [HttpPost]
        public virtual string AddDialog(AddressModel model)
        {
            var info = SaveAddress(model,SaveType.Add);
            var dis = new Dictionary<string, object>();
            string value = info.Errors == null || info.Errors.Count == 0 ? "true" : info.Errors[0].Message;
            dis.Add("Result", value);
            dis.Add("Id", info.Id);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dis);
        }
        /// <summary>
        /// 添加新地址
        /// </summary>
        /// <param name="model"></param>
        [AuthorizeFilter]
        [DataFilter(EntityType = typeof(AddressEntity))]
        [HttpPost]
        public virtual string ModifyDialog(AddressModel model)
        {
            var info = SaveAddress(model, SaveType.Modify);
            var dis = new Dictionary<string, object>();
            string value = info.Errors == null || info.Errors.Count == 0 ? "true" : info.Errors[0].Message;
            dis.Add("Result", value);
            dis.Add("Id", info.Id);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dis);
        }
        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeFilter]
        [DataFilter(EntityType = typeof(AddressEntity))]
        public virtual string Remove(long id)
        {
            var info = new AddressEntity
                {
                    Id = id,
                    SaveType = SaveType.Remove
                };
            var rev = this.SaveEntity(info);
            if (rev)
                return "true";
            return info.Errors != null && info.Errors.Count > 0 ? info.Errors[0].Message : "";
        }

        /// <summary>
        /// 得到地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual AddressModel GetModel(long id)
        {
            if (id <= 0) return null;
            var info =this.GetEntityByIdentity<AddressEntity>(id);
            if (info == null) return null;
            var model = new AddressModel
                {
                    Id = info.Id,
                    Address = info.Address,
                    Province=info.Province,
                    City=info.City,
                    County=info.County,
                    Recipient = info.Recipient,
                    Mobile = info.Mobile,
                    Postcode = info.Postcode
                };
            return model;
        }
        #endregion
    }
}

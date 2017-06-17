using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Member;
using Beeant.Presentation.Mobile.Member.Models.Member;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Mobile.Member.Controllers.Member
{
    [AuthorizeFilter]
    public class AddressController : MobileBaseController
    {

        #region 首页

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var model = new AddressModel
            {
                Addresses = GetAddresses()
            };
            return View("~/Views/Member/Address/Index.cshtml", model);
        }

        /// <summary>
        /// 得到会员地址信息
        /// </summary>
        /// <returns></returns>
        protected virtual IList<AddressEntity> GetAddresses()
        {
            var query = new QueryInfo();
            query.Query<AddressEntity>()
                .OrderByDescending(it => it.Id)
                .Select(
                    it =>
                        new object[]
                        {
                            it.Id,it.Recipient, it.Address, it.Country, it.Province, it.City, it.County, it.Mobile, it.Email, it.County,
                            it.IsDefault
                        });
            return this.GetEntitiesByIdentity<AddressEntity>(query);

        }

        #endregion

        #region 更新默认

        /// <summary>
        /// 更新购物车
        /// </summary>
        /// <param name="id">
        /// <param></param>
        /// </param>
        /// <returns></returns>
        [DataFilter(IdParamterName = "id", EntityType = typeof (AddressEntity))]
        public virtual ActionResult UpdateDefault(long id)
        {
            var result = new Dictionary<string, object>();
            var infos = new List<AddressEntity>();
            var defaultEntities = GetDefaultInfos();
            if (defaultEntities != null)
            {
                foreach (var entity in defaultEntities)
                {
                    entity.IsDefault = false;
                    entity.SaveType = SaveType.Modify;
                    entity.SetProperty(it => it.IsDefault);
                    infos.Add(entity);
                }
            }
            var info = new AddressEntity
            {
                Id = id,
                IsDefault = true,
                SaveType = SaveType.Modify
            };
            info.SetProperty(it => it.IsDefault);
            infos.Add(info);
            var rev = this.SaveEntities(infos);
            result.Add("Status", rev);
            return this.Jsonp(result);
        }

        /// <summary>
        /// 得到默认
        /// </summary>
        /// <returns></returns>
        protected virtual IList<AddressEntity> GetDefaultInfos()
        {
            var query = new QueryInfo();
            query.Query<AddressEntity>().Where(it=>it.IsDefault)
                .Select(
                    it =>
                        new object[]
                        {
                            it.Id
                        });
            return this.GetEntitiesByIdentity<AddressEntity>(query);
        }

        #endregion

        #region 新增
        [HttpGet]
        public virtual ActionResult Add()
        {
            return View("~/Views/Member/Address/Edit.cshtml");
        }
        [HttpPost]
        public virtual ActionResult Add(AddressModel model)
        {
            var infos = new List<AddressEntity>();
            var defaultEntities = GetDefaultInfos();
            if (defaultEntities != null)
            {
                foreach (var entity in defaultEntities)
                {
                    entity.IsDefault = false;
                    entity.SaveType = SaveType.Modify;
                    entity.SetProperty(it => it.IsDefault);
                    infos.Add(entity);
                }
            }
            var info = model.CreateEntity(SaveType.Add);
            info.Account = new AccountEntity { Id = Identity.Id };
            info.IsDefault = true;
            infos.Add(info);
            var result = new Dictionary<string, object>();
            var rev = this.SaveEntities(infos);
            model.AddressEntity = info;
            if (rev)
            {
                return RedirectToAction("Index");
            }
            return View("~/Views/Member/Address/Edit.cshtml", model);
        }
        #endregion

        #region 修改
        [HttpGet]
        public virtual ActionResult Update(long id)
        {
            var query=new QueryInfo();
            query.Query<AddressEntity>().Where(it => it.Id == id)
                .Select(it=>new object[] {it.Id,it.Recipient,it.Mobile,it.Address,it.Postcode,it.Province,it.City,it.County});
            var infos = this.GetEntitiesByIdentity<AddressEntity>(query);
            var info = infos?.FirstOrDefault();
            var model = new AddressModel {AddressEntity = info};
            return View("~/Views/Member/Address/Edit.cshtml",model);
        }
        [DataFilter(EntityType = typeof(AddressEntity))]
        [HttpPost]
        public virtual ActionResult Update(AddressModel model)
        {
            var entity = model.CreateEntity(SaveType.Modify);
            var result = new Dictionary<string, object>();
            var rev = this.SaveEntity(entity);
            model.AddressEntity = entity;
            if (rev)
            {
                return RedirectToAction("Index");
            }
            return View("~/Views/Member/Address/Edit.cshtml", model);
        }
        #endregion

        #region 删除
        [DataFilter(EntityType = typeof(AddressEntity))]
        public virtual ActionResult Remove(long[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<AddressEntity>();
                foreach (var i in id)
                {
                    var info = new AddressEntity
                    {
                        Id = i,
                        SaveType = SaveType.Remove
                    };
                    infos.Add(info);
                }
                rev = this.SaveEntities(infos);
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }
        #endregion


    }
}

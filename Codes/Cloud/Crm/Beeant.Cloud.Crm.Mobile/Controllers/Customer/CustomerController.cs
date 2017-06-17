using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Crm.Mobile.Models.Customer;
using Beeant.Cloud.Crm.Mobile.Models.Staff;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Crm.Mobile.Controllers.Customer
{
    [CrmAuthorizeFilter]
    public class CustomerController : CrmAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new CustomerListModel();
            model.CustomerChannels = GetCustomerChannels();
            model.CustomerTypes = GetCustomerTypes();
            return View("~/Views/Customer/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string key,string channelId,string typeId,bool? isReadSelf, int? page)
        {
            var model = new CustomerListModel
            {
                PageIndex = page.HasValue ? page.Value : 0,
                ChannelId=channelId,
                TypeId=typeId,
                IsReadSelf = isReadSelf,
                Key = key
            };

            model.Customers = GetCustomers(model);
            if (model.Customers == null || model.Customers.Count == 0)
                return Content("");
            return View("~/Views/Customer/_Customer.cshtml", model);
        }

 
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CustomerChannelEntity> GetCustomerChannels()
        {
            var query = new QueryInfo();
            query.Query<CustomerChannelEntity>().Where(it => it.Crm.Id == CrmId).Select(it => new object[] { it.Id, it.Name });
            return this.GetEntities<CustomerChannelEntity>(query);
        }

 
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CustomerTypeEntity> GetCustomerTypes()
        {
            var query = new QueryInfo();
            query.Query<CustomerTypeEntity>().Where(it => it.Crm.Id == CrmId).Select(it => new object[] { it.Id, it.Name });
            return this.GetEntities<CustomerTypeEntity>(query);
        }
        /// <summary>
        /// 客户类型
        /// </summary>
        /// <returns></returns>
        [CrmAssignAuthorizeFilter]
        public virtual ActionResult Staffs(int? page)
        {
            var model = new StaffListModel
            {
                PageIndex=page.HasValue?page.Value:0
            };
            model.Staffs = GetStaffs(model);
            return View("~/Views/Customer/_Staff.cshtml", model);
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual IList<StaffEntity> GetStaffs(StaffListModel model)
        {
            if (Staff == null || Staff.ReadCustomerType==ReadCustomerType.Self)
                return null;
            var query = new QueryInfo();
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex).Query<StaffEntity>()
                .Where(it => it.Crm.Id == CrmId).Select(it => new object[] { it.Id, it.Name });
            if (Staff.ReadCustomerType == ReadCustomerType.Department)
            {
                query.SetParameter("DepartmentId", Staff.Department == null ? 0 : Staff.Department.Id)
                       .Where(string.Format("{0} && Department.Id==@DepartmentId", query.WhereExp));
            }
            return this.GetEntities<StaffEntity>(query);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [CrmDataFilter(EntityType = typeof (CustomerEntity))]
        public virtual ActionResult Get(long id)
        {
            var entity = this.GetEntity<CustomerEntity>(id);
            if (entity == null)
                return null;
            var model = new CustomerModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Mobile = entity.Mobile,
                TypeId = entity.Type == null ? "" : entity.Type.Id.ToString(),
                ChannelId = entity.Channel == null ? "" : entity.Channel.Id.ToString(),
                Gender = entity.Gender,
                Qq = entity.Qq,
                Linkman = entity.Linkman,
                Weixin = entity.Weixin,
                Address = entity.Address,
                Telephone = entity.Telephone,
                Remark = entity.Remark,
                RemindNoteDate =
                    entity.RemindNoteDate == entity.GetMinDateTime() ? "" : entity.RemindNoteDate.ToString("yyyy-MM-dd"),
                Email = entity.Email,


            };
            return this.Jsonp(model);
        }

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<CustomerEntity> GetCustomers(CustomerListModel model)
        {
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex)
                .Query<CustomerEntity>().Where(it => it.Crm.Id == CrmId).OrderByDescending(it=>it.Id)
                .Select(it => new object[] { it.Id, it.Name, it.Mobile,it.Staff.Id});
            if (!string.IsNullOrWhiteSpace(model.Key))
            {
                query.Where(string.Format("{0} && (Name.Contains(@Key) || Mobile.Contains(@Key) || Email.Contains(@Key))", query.WhereExp))
                    .SetParameter("Key", model.Key);
            }
            if (!string.IsNullOrWhiteSpace(model.ChannelId))
            {
                query.Where(string.Format("{0} && Channel.Id==@ChannelId", query.WhereExp))
                    .SetParameter("ChannelId", model.ChannelId.Convert<long>());
            }
            if (!string.IsNullOrWhiteSpace(model.TypeId))
            {
                query.Where(string.Format("{0} && Type.Id==@TypeId", query.WhereExp))
                    .SetParameter("TypeId", model.TypeId.Convert<long>());
            }
            var readCustomerType =ViewBag.IsMainAccount? ReadCustomerType.All : Staff == null ? ReadCustomerType.Self : Staff.ReadCustomerType;
            if (model.IsReadSelf == true || readCustomerType == ReadCustomerType.Self)
            {
                query.Where(string.Format("{0} && Staff.Id==@StaffId", query.WhereExp)).SetParameter("StaffId", Staff == null ? 0 : Staff.Id);
            }
            else if (readCustomerType == ReadCustomerType.Department)
            {
                var statffIds = GetStaffIds();
                query.Where(string.Format("{0} && @StaffIds.Contains(Staff.Id)", query.WhereExp)).SetParameter("StaffIds", statffIds);
            }
            return this.GetEntities<CustomerEntity>(query);
        }
        /// <summary>
        /// 得到员工编号
        /// </summary>
        /// <returns></returns>
        protected virtual long[] GetStaffIds()
        {
            var departmentId = Staff == null || Staff.Department == null ? 0 : Staff.Department.Id;
            var query=new QueryInfo();
            query.Query<StaffEntity>().Where(it => it.Department.Id == departmentId).Select(it => it.Id);
            var infos = this.GetEntities<StaffEntity>(query);
            var ids = infos == null || infos.Count == 0 ? new [] { Staff == null ? 0 : Staff.Id } : infos.Select(it => it.Id).ToArray();
            return ids;
        }
        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(CustomerModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add);
            var result = new Dictionary<string, object>();
            entity.Crm = new CrmEntity { Id = CrmId };
            entity.Staff = new StaffEntity {Id = Staff == null ? 0 : Staff.Id};
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Id", entity.Id);
            result.Add("Message", mess);
            result.Add("StaffId", entity.Staff.Id);
            return this.Jsonp(result);
        }

        #endregion

        #region 修改
        [HttpPost]
        public virtual ActionResult Modify(CustomerModel model)
        {
            if (model == null || Staff==null)
                return null;
            var dataEntity = this.GetEntity<CustomerEntity>(model.Id.Convert<long>());
            if (dataEntity == null || dataEntity.Staff == null || Staff.Id != dataEntity.Staff.Id)
                return null;
            var entity = model.CreateEntity(SaveType.Modify);
            var result = new Dictionary<string, object>();
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }
        #endregion

        #region 删除
        [CrmDataFilter(EntityType = typeof(CustomerEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<CustomerEntity>();
                foreach (var i in id)
                {
                    var info = new CustomerEntity
                    {
                        Id = i.Convert<long>(),
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

        #region 修改员工
        [CrmDataFilter(EntityType = typeof(CustomerEntity))]
        [CrmAssignAuthorizeFilter]
        public virtual ActionResult UpdataStaff(string[] id, string staffId)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<CustomerEntity>();
                foreach (var i in id)
                {
                    var info = new CustomerEntity
                    {
                        Id = i.Convert<long>(),
                        Staff = new StaffEntity { Id = staffId.Convert<long>() },
                        SaveType = SaveType.Modify
                    };
                    info.SetProperty(it => it.Staff.Id);
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

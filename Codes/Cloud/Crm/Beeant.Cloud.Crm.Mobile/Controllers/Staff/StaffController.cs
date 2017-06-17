using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Application.Services.Account;
using Component.Extension;
using Dependent;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Crm.Mobile.Models.Department;
using Beeant.Cloud.Crm.Mobile.Models.Staff;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Crm.Mobile.Controllers.Staff
{
    [CrmAdminAuthorizeFilterAttribute]
    public class StaffController : CrmAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
        
            return View("~/Views/Staff/index.cshtml");
        }

        public virtual ActionResult List(int? page)
        {
            var model = new StaffListModel
            {
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Staffs = GetStaffs(model);
            return View("~/Views/Staff/_Staff.cshtml", model);
        }
        /// <summary>
        /// 部门
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual ActionResult Department(int? page)
        {
            var model = new DepartmentListModel
            {
               Departments = GetDepartments()
            };
            return View("~/Views/Staff/_Department.cshtml", model);
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual IList<DepartmentEntity> GetDepartments()
        {
            var query = new QueryInfo();
            query.Query<DepartmentEntity>().Where(it=>it.Crm.Id==CrmId).Select(it => new object[] { it.Id, it.Name});
            return this.GetEntities<DepartmentEntity>(query);
        }
        /// <summary>
        /// 得到
        /// </summary>
        /// <returns></returns>
        protected virtual IList<StaffEntity> GetStaffs(StaffListModel model)
        {
            var query = new QueryInfo();
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex)
                .Query<StaffEntity>().Select(it => new object[] { it.Id, it.Name, it.Account.Id });
            return this.GetEntities<StaffEntity>(query);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [CrmDataFilter(EntityType = typeof(StaffEntity))]
        public virtual ActionResult Get(long id)
        {
            var entity = this.GetEntity<StaffEntity>(id);
            if (entity == null)
                return null;
            var model = new StaffModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                DepartmentId = entity.Department==null?"":entity.Department.Id.ToString(),
                ReadCustomerType=entity.ReadCustomerType
            };
            return this.Jsonp(model);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(StaffModel model)
        {
            var result=new Dictionary<string,object>();
            var rev = true;
            AccountEntity account=null;
            if (!string.IsNullOrWhiteSpace(model.LoginName))
            {
                rev = Ioc.Resolve<IPasswordApplicationService>().CheckPassword(model.LoginName, model.LoginPassword,out account);
            }
            if (!rev)
            {
                result.Add("Message", "绑定的用户名或密码错误");
            }
            else
            {
                var entity = model.CreateEntity(SaveType.Add, account);
                entity.Crm = new CrmEntity {Id = CrmId};
                 rev = this.SaveEntity(entity);
                if (rev)
                {
                    result.Add("Id", entity.Id.ToString());
                    result.Add("AccountId", entity.Account==null?0:entity.Account.Id);
                }
                else
                {
                    result.Add("Message", entity.Errors?.FirstOrDefault()?.Message);
                }
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CrmDataFilter(EntityType = typeof(StaffEntity))]
        public virtual ActionResult Modify(StaffModel model)
        {
            var result = new Dictionary<string, object>();
            var rev = true;
            AccountEntity account = null;
            if (!string.IsNullOrWhiteSpace(model.LoginName) && !string.IsNullOrEmpty(model.LoginPassword))
            {
                rev = Ioc.Resolve<IPasswordApplicationService>().CheckPassword(model.LoginName, model.LoginPassword, out account);
            }
            if (!rev)
            {
                result.Add("Message", "绑定的用户名或密码错误");
            }
            else
            {
                var entity = model.CreateEntity(SaveType.Modify, account);
                rev = this.SaveEntity(entity);
                if (rev)
                {
                    result.Add("Id", entity.Id.ToString());
                    result.Add("AccountId", entity.Account == null ? 0 : entity.Account.Id);
                }
                else
                {
                    result.Add("Message", entity.Errors?.FirstOrDefault()?.Message);
                }
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CrmDataFilter(EntityType = typeof (StaffEntity))]
        public virtual ActionResult Unbind(StaffModel model)
        {
            var result = new Dictionary<string, object>();
            var entity = new StaffEntity
            {
                Id = model.Id.Convert<long>(),
                Account = new AccountEntity {Id = 0},
                SaveType = SaveType.Modify
            };
            entity.SetProperty(it => it.Account.Id);
            var rev = this.SaveEntity(entity);
            if (rev)
            {
                result.Add("Id", entity.Id.ToString());
            }
            else
            {
                result.Add("Message", entity.Errors?.FirstOrDefault()?.Message);
            }
            result.Add("Status", rev);
            return this.Jsonp(result);
        }

        #region 删除
        [CrmDataFilter(EntityType = typeof(StaffEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<StaffEntity>();
                foreach (var i in id)
                {
                    var info = new StaffEntity
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
  
         
    }
}

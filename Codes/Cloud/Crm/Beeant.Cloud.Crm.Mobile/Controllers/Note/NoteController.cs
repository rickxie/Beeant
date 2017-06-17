using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Component.Extension;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Cloud.Crm.Mobile.Models.Note;
using Beeant.Domain.Entities.Crm;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Cloud.Crm.Mobile.Controllers.Note
{
    [CrmAuthorizeFilter]
    public class NoteController : CrmAuthorizeBaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new NoteListModel();

            return View("~/Views/Note/index.cshtml", model);
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string customerId, int? page)
        {
            var model = new NoteListModel
            {
                CustomerId=customerId,
                PageIndex = page.HasValue ? page.Value : 0
            };

            model.Notes = GetNotes(model);
            if (model.Notes == null || model.Notes.Count == 0)
                return Content("");
            return View("~/Views/Note/_Note.cshtml", model);
        }

 

        /// <summary>
        /// 得到类目
        /// </summary>
        /// <returns></returns>
        protected virtual IList<NoteEntity> GetNotes(NoteListModel model)
        {
            var staffId = Staff == null ? 0 : Staff.Id;
            var query = new QueryInfo { IsReturnCount = false };
            query.SetPageSize(model.PageSize).SetPageIndex(model.PageIndex)
                .Query<NoteEntity>().Where(it => it.Crm.Id == CrmId &&　it.Customer.Staff.Id== staffId).OrderByDescending(it=>it.Id)
                .Select(it => new object[] { it.Id, it.Name, it.Content,it.InsertTime });
            if (!string.IsNullOrWhiteSpace(model.CustomerId))
            {
                query.Where(string.Format("{0} && Customer.Id==@CustomerId"))
                    .SetParameter("CustomerId", model.CustomerId.Convert<long>());
            }
            return this.GetEntities<NoteEntity>(query);
        }

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add(NoteModel model)
        {
            if (model == null)
                return null;
            var staff = Staff==null?null:this.GetEntity<StaffEntity>(Staff.Id);
            var entity = model.CreateEntity();
            var result = new Dictionary<string, object>();
            entity.Crm = new CrmEntity { Id = CrmId };
            entity.Name = staff==null?"": staff.Name;
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Id", entity.Id);
            result.Add("Message", mess);
            result.Add("Name", entity.Name);
            result.Add("InsertTime", entity.InsertTime.ToString("yyyy-MM-dd HH:mm"));
            if (rev && model.RemindNoteDate != null)
            {
                var customer = new CustomerEntity
                {
                    Id = model.CustomerId.Convert<long>(),
                    RemindNoteDate = model.RemindNoteDate.Value,
                    SaveType = SaveType.Modify
                };
                customer.SetProperty(it => it.RemindNoteDate);
                this.SaveEntity(customer);
            }
            return this.Jsonp(result);
        }

        #endregion

     

        #region 删除
        [CrmDataFilter(EntityType = typeof(NoteEntity))]
        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            if (id != null)
            {
                var infos = new List<NoteEntity>();
                foreach (var i in id)
                {
                    var info = new NoteEntity
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

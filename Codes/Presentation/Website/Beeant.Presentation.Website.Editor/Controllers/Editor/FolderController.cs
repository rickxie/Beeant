using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Editor;
using Beeant.Presentation.Website.Editor.Models.Editor;
using Component.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Editor.Controllers.Editor
{
    [AuthorizeFilter]
    public class FolderController : BaseController
    {

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
 
        public ActionResult Index(string name, int? page)
        {
            var model = new FolderListModel
            {
                Name = name,
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Folders = GetFolders(model);
            return View("~/Views/Editor/Folder/index.cshtml", model);
        }



        /// <summary>
        /// 得到标签
        /// </summary>
        /// <returns></returns>
        protected virtual IList<FolderEntity> GetFolders(FolderListModel model)
        {
            var query = new QueryInfo();
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<FolderEntity>()
                .Where(it => it.Account.Id == Identity.Id)
                .Select(it => new object[]
                {
                    it.Id, it.Name,it.Type,it.Sequence
                });
            if (!string.IsNullOrWhiteSpace(model.Name))
                query.AppendWhere("Name.Contains(@Name)").SetParameter("Name", model.Name);
            var infos = this.GetEntities<FolderEntity>(query);
            model.DataCount = query.DataCount;
            return infos;
        }

        /// <summary>
        /// 加载详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
   
        public virtual ActionResult Get(long id)
        {
            var query = new QueryInfo();
            query.Query<FolderEntity>().Where(it => it.Account.Id == Identity.Id && it.Id == id)
                .Select(
                    it =>
                        new object[]
                        {
                            it
                        });
            var entity = this.GetEntities<FolderEntity>(query)?.FirstOrDefault();
            if (entity == null)
                return null;
            var model = new FolderModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Type=entity.Type,
                Sequence=entity.Sequence
            };
            return this.Jsonp(model);
        }


        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
    
        public virtual ActionResult Add(FolderModel model)
        {
            if (model == null)
                return null;
            var entity = model.CreateEntity(SaveType.Add);
            var result = new Dictionary<string, object>();
            entity.Account = new AccountEntity { Id = Identity.Id };
            var rev = this.SaveEntity(entity);
            var mess = rev ? "" : entity.Errors?.FirstOrDefault()?.Message;
            result.Add("Status", rev);
            result.Add("Id", entity.Id);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }


        #endregion

        #region 修改
 
        [DataFilter(EntityType = typeof(FolderEntity))]
        [HttpPost]

        public virtual ActionResult Modify(FolderModel model)
        {
            if (model == null)
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

        [DataFilter(EntityType = typeof(FolderEntity))]

        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            var mess = "";
            if (id != null)
            {
                var infos = new List<FolderEntity>();
                foreach (var i in id)
                {
                    var info = new FolderEntity
                    {
                        Id = i.Convert<long>(),
                        SaveType = SaveType.Remove
                    };
                    infos.Add(info);
                }
                rev = this.SaveEntities(infos);
                mess = rev ? "" : infos.FirstOrDefault()?.Errors?.FirstOrDefault()?.Message;

            }
            result.Add("Status", rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }

        #endregion
    }
}

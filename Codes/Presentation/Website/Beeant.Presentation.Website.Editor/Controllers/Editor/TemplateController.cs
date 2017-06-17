using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Beeant.Application.Services;
using Beeant.Basic.Services.Mvc.Bases;
using Beeant.Basic.Services.Mvc.Extension;
using Beeant.Basic.Services.Mvc.FilterAttribute;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Editor;
using Beeant.Presentation.Website.Editor.Models.Editor;
using Component.Extension;
using Configuration;
using Dependent;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Website.Editor.Controllers.Editor
{
    [AuthorizeFilter]
    public class TemplateController : BaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index(string folderId, int? page)
        {
            var model = new TemplateListModel
            {
                FolderId = folderId,
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Folders = GetFolders(model);
            model.Templates = GetTemplates(model);
            return View("~/Views/Editor/Template/Index.cshtml", model);
        }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual IList<TemplateEntity> GetTemplates(TemplateListModel model)
        {
            var query = new QueryInfo();
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<TemplateEntity>().
                Where(it => it.Account.Id == Identity.Id)
                .Select(it => new object[] { it.Id, it.Name});
            if (!string.IsNullOrWhiteSpace(model.FolderId))
                query.AppendWhere("Folder.Id==@FolderId").SetParameter("FolderId", model.FolderId.Convert<long>());
            var infos = this.GetEntities<TemplateEntity>(query);
            model.DataCount = query.DataCount;
            return infos;
        }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual IList<FolderEntity> GetFolders(TemplateListModel model)
        {
            var query = new QueryInfo();
            query.Query<FolderEntity>().
                Where(it => it.Account.Id == Identity.Id && it.Type == FolderType.Template)
                .Select(it => new object[] { it.Id, it.Name });
            return this.GetEntities<FolderEntity>(query);
        }

 

        /// <summary>
        /// 保存
        /// </summary>
        [ValidateInput(false)]
        public virtual void Upload()
        {
            var info = new TemplateEntity
            {
                Name = Request["templatename"],
                Detail = Request["editorvalue"],
                Folder = new FolderEntity { Id = 0 },
                SaveType = SaveType.Add,
                Account = new AccountEntity { Id = Identity.Id }

            };
            Ioc.Resolve<IApplicationService, TemplateEntity>().Save(info);
            Response.Write(string.Format("<script type='text/javascript'>document.domain='{0}';{1};</script>", ConfigurationManager.GetSetting<string>("Domain"),
                                      string.Format(Request["function"], string.Format("{0}/Template/Detail?id={1}", ConfigurationManager.GetSetting<string>("PresentationWebsiteEditorUrl"), info.Id), GetErrorMessage(info))));
        }
      

       
        /// <summary>
        /// 得到错误信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string GetErrorMessage(TemplateEntity info)
        {
            if (info == null || info.Errors == null || info.Errors.Count == 0) return "";
            var sb = new StringBuilder();
            foreach (var errorEntity in info.Errors)
            {
                sb.AppendFormat(@"{0}\r\n", errorEntity.Message);
            }
            return sb.ToString();
        }
        #region 移动
        /// <summary>
        /// 保存
        /// </summary>
        [DataFilter(EntityType = typeof(TemplateEntity))]
        public virtual void Move(string id)
        {
            var info = new TemplateEntity { Id = id.Convert<long>(), Folder = new FolderEntity { Id = Request["folderid"].Convert<long>() }, SaveType = SaveType.Modify };
            info.SetProperty(it => it.Folder.Id);
            Ioc.Resolve<IApplicationService, TemplateEntity>().Save<TemplateEntity>(info);
            Response.Write(GetErrorMessage(info));
        }
        #endregion

        #region 删除
 
        [DataFilter(EntityType = typeof(TemplateEntity))]

        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            var mess = "";
            if (id != null)
            {
                var infos = new List<TemplateEntity>();
                foreach (var i in id)
                {
                    var info = new TemplateEntity
                    {
                        Id = i.Convert<long>(),
                        SaveType = SaveType.Remove
                    };
                    infos.Add(info);
                }
                rev = this.SaveEntities(infos);
                mess = rev ? "" : infos?.FirstOrDefault()?.Errors?.FirstOrDefault()?.Message;

            }
            result.Add("Status", rev);
            result.Add("Message", mess);
            return this.Jsonp(result);
        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        [DataFilter(EntityType = typeof(TemplateEntity))]
        public virtual void Detail(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            var info = Ioc.Resolve<IApplicationService, TemplateEntity>().GetEntity<TemplateEntity>(id.Convert<long>());
            if (info != null)
                Response.Write(string.Format("<html><head></head><body><script type='text/javascript'>document.domain='{0}';</script>{1}</body></html>", ConfigurationManager.GetSetting<string>("Domain"), info.Detail));
        }
    }
}

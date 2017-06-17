using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
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
    public class FlashController : BaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index(string folderId, int? page)
        {
            var model = new FlashListModel
            {
                FolderId = folderId,
                PageIndex = page.HasValue ? page.Value : 0
            };
            model.Folders = GetFolders(model);
            model.Flashs = GetFlashs(model);
            return View("~/Views/Editor/Flash/Index.cshtml", model);
        }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual IList<FlashEntity> GetFlashs(FlashListModel model)
        {
            var query = new QueryInfo();
            query.SetPageIndex(model.PageIndex).SetPageSize(model.PageSize).Query<FlashEntity>().
                Where(it => it.Account.Id == Identity.Id)
                .Select(it => new object[] { it.Id, it.Name, it.FileName });
            if (!string.IsNullOrWhiteSpace(model.FolderId))
                query.AppendWhere("Folder.Id==@FolderId").SetParameter("FolderId", model.FolderId.Convert<long>());
            var infos = this.GetEntities<FlashEntity>(query);
            model.DataCount = query.DataCount;
            return infos;
        }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual IList<FolderEntity> GetFolders(FlashListModel model)
        {
            var query = new QueryInfo();
            query.Query<FolderEntity>().
                Where(it => it.Account.Id == Identity.Id && it.Type == FolderType.Flash)
                .Select(it => new object[] { it.Id, it.Name });
            return this.GetEntities<FolderEntity>(query);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Add()
        {
            var model = new FlashListModel();
            var info = Save(Server.UrlDecode(Request["Path"]), Request["FolderId"].Convert<long>());
            if (info == null)
                return View("~/Views/Editor/Flash/Index.cshtml", model);
            var rev = info.HandleResult == true;
            model.ErrorMessage = rev ? "" : info?.Errors?.FirstOrDefault()?.Message;
            if (rev)
                return RedirectToAction("Index", new RouteValueDictionary
                {
                    {"Path", Request["Path"]}
                    ,
                    {"FolderId", Request["FolderId"]}
                    ,
                    {"function", Request["function"]}
                });
            model.Folders = GetFolders(model);
            model.Flashs = GetFlashs(model);
            return View("~/Views/Editor/Flash/Index.cshtml", model);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public virtual void Upload()
        {
            var info = Save(Server.UrlDecode(Request.QueryString["Path"]), 0);
            Response.Write(string.Format("<script type='text/javascript'>document.domain='{0}';{1};</script>", ConfigurationManager.GetSetting<string>("Domain"),
                                                 string.Format(Request["function"], info.FullFileName, GetErrorMessage(info))));
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        protected virtual FlashEntity GetSaveEntity(string path, long folderId)
        {
            if (string.IsNullOrEmpty(path) || Request.Files.Count == 0) return null;
            string fileName = string.Format("{0}{1}", path, Path.GetFileName(Request.Files[0].FileName));
            var info = new FlashEntity
            {
                FileName = fileName,
                Name = Path.GetFileName(Request.Files[0].FileName),
                Folder = new FolderEntity { Id = folderId },
                SaveType = SaveType.Add,
                Account = new AccountEntity { Id = Identity.Id },
                FileByte = new byte[Request.Files[0].ContentLength]
            };
            Request.Files[0].InputStream.Read(info.FileByte, 0, info.FileByte.Length);
            return info;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        protected virtual FlashEntity Save(string path, long folderId)
        {
            var info = GetSaveEntity(path, folderId);
            if (info == null) return null;
            Ioc.Resolve<IApplicationService, FlashEntity>().Save(info);
            return info;
        }
        /// <summary>
        /// 得到错误信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string GetErrorMessage(FlashEntity info)
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
        [DataFilter(EntityType = typeof(FlashEntity))]
        public virtual void Move(string id)
        {
            var info = new FlashEntity { Id = id.Convert<long>(), Folder = new FolderEntity { Id = Request["folderid"].Convert<long>() }, SaveType = SaveType.Modify };
            info.SetProperty(it => it.Folder.Id);
            Ioc.Resolve<IApplicationService, FlashEntity>().Save<FlashEntity>(info);
            Response.Write(GetErrorMessage(info));
        }
        #endregion

        #region 删除

        [DataFilter(EntityType = typeof(FlashEntity))]

        public virtual ActionResult Remove(string[] id)
        {
            var result = new Dictionary<string, object>();
            var rev = false;
            var mess = "";
            if (id != null)
            {
                var infos = new List<FlashEntity>();
                foreach (var i in id)
                {
                    var info = new FlashEntity
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
    }
}

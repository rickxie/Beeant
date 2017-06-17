using System.Linq;
using System.Text;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Editor;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Editor.Editor
{
    public partial class FolderList : System.Web.UI.UserControl
    {
        public FolderType FolderType { get; set; }
        protected string Url { get; set; }
        public bool IsLink { get; set; }
        public virtual void LoadData()
        {
            var query = new QueryInfo();
            query.Query<FolderEntity>()
                 .Where(it => it.Account.Id == 0 && it.Type == FolderType)
                 .OrderByDescending(it=>it.Sequence)
                 .Select(it => new object[] {it.Id, it.Name});
            var infos = Ioc.Resolve<IApplicationService, FolderEntity>().GetEntities<FolderEntity>(query);
            Repeater1.DataSource = infos;
            Repeater1.DataBind();
        }


        /// <summary>
        /// 得到地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetUrl(object id)
        {
            if (!IsLink)
                return "javascript:void(0);";
            if (string.IsNullOrEmpty(Url))
            {
                var url = new StringBuilder(Request.Url.ToString().Split('?')[0]);
                var i = 0;
                foreach (string key in Request.QueryString.Keys)
                {
                    if (!key.ToLower().Equals("folderid"))
                    {
                        url.AppendFormat(i == 0 ? "?{0}={1}" : "&{0}={1}", key, Request.QueryString[key]);
                        i++;
                    }
                }
                Url = url.ToString();
            }
            return string.Format("{0}{1}", Url,
                                 Url.Contains("?")
                                     ? string.Format("&folderid={0}", id)
                                     : string.Format("?folderid={0}", id));
        }
    }
}
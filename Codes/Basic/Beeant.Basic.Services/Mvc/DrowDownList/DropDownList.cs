using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities;
using Winner.Persistence;

namespace Beeant.Basic.Services.Mvc.DrowDownList
{
    public class DropDownList
    {
        /// <summary>
        /// 视图上下文
        /// </summary>
        public HtmlHelper HtmlHelper { get; set; }
 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="htmlHelper"></param>
        public DropDownList(HtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
           
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="partialViewName"></param>
        /// <param name="model"></param>
        /// <param name="valueName"></param>
        /// <param name="textName"></param>
        public virtual MvcHtmlString EntityPartial<T>(string partialViewName, DropDownListModel model, string valueName = "Id", string textName = "Name")
        {
            var query = new QueryInfo();
            query.From<T>().Select(string.Format("{0},{1}", valueName, textName));
            var infos = Ioc.Resolve<IApplicationService,T>().GetEntities<T>(query);
            return EntityPartial(partialViewName, model, infos, valueName, textName);
        }

        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="partialViewName"></param>
        /// <param name="model"></param>
        /// <param name="query"></param>
        /// <param name="valueName"></param>
        /// <param name="textName"></param>
        public virtual MvcHtmlString EntityPartial<T>(string partialViewName, DropDownListModel model, QueryInfo query, string valueName = "Id", string textName = "Name")
        {
            var infos = Ioc.Resolve<IApplicationService, T>().GetEntities<T>(query);
            return EntityPartial(partialViewName, model, infos, valueName, textName);
        }
        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="partialViewName"></param>
        /// <param name="model"></param>
        /// <param name="infos"></param>
        /// <param name="valueName"></param>
        /// <param name="textName"></param>
        public virtual MvcHtmlString EntityPartial<T>(string partialViewName, DropDownListModel model, IList<T> infos, string valueName = "Id", string textName = "Name")
        {
            if (infos != null)
            {
                var server = Winner.Creator.Get<Winner.Base.IProperty>();
                model.Items = new BindingList<SelectListItem>();
                foreach (var info in infos)
                {
                    model.Items.Add(new SelectListItem
                    {
                        Text = server.GetValue<string>(info, textName),
                        Value = server.GetValue<string>(info, valueName)
                    });
                }
            }
            return HtmlHelper.Partial(partialViewName, model);
        }


        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="partialViewName"></param>
        /// <param name="model"></param>
        public virtual MvcHtmlString EnumPartial<T>(string partialViewName, DropDownListModel model)
        {
            var infos = EnumExtension.GetNames<T>();
            if (infos != null)
            {
                model.Items = new BindingList<SelectListItem>();
                foreach (var info in infos)
                {
                    model.Items.Add(new SelectListItem
                    {
                        Text = info.Message,
                        Value =info.Name
                    });
                }
            }
            return HtmlHelper.Partial(partialViewName, model);
        }

    }
}

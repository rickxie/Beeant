using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Beeant.Application.Services.Authority;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Authority;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Configuration;
using Dependent;

namespace Beeant.Basic.Services.WebForm.Extension
{
    public static class MenuMasterPageExtension
    {
        #region 加载菜单

        /// <summary>
        /// 加载菜单
        /// </summary>
        /// <returns></returns>
        public static string GetMenu(this MasterPage masterPage)
        {
            var identity = Ioc.Resolve<IIdentityApplicationService>().Get<IdentityEntity>();
            var infos = Ioc.Resolve<IAuthorityEngineApplicationService>().GetMenus(identity.Id);
            var builder = new StringBuilder();
            builder.AppendFormat(GetHeaderHtml(masterPage));
            builder.AppendFormat("<div id='menu' class='menu'><h1>蜂蚁窝系统</h1> <ul>");
            BuilderMenu(builder, infos);
            builder.AppendFormat("</ul></div>");
            return builder.ToString();
        }

        /// <summary>
        /// 拼接菜单
        /// </summary>
        public static void BuilderMenu(StringBuilder sb, IList<MenuEntity> infos)
        {
            if (infos == null) return;
            var exsits = new List<long>();
            foreach (
                var info in
                    infos.Where(it => it.Parent != null && it.Parent.Id != 0)
                        .Select(it => it.Parent)
                        .OrderBy(it => it.Sequence)
                        .Where(it => !exsits.Contains(it.Id)))
            {
                sb.AppendFormat("<li> <a class=\"parent\"  title=\"{0}\" href=\"javascript:void(0);\">{0}</a>",
                    info.Name);
                sb.Append("<ul style=\"display: none;\">");
                BuilderSubMenu(sb, infos.Where(it => it.Parent != null && it.Parent.Id.Equals(info.Id)).ToList());
                sb.Append("</ul></li>");
                exsits.Add(info.Id);
            }
        }

        /// <summary>
        /// 拼接子菜单
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="infos"></param>
        public static void BuilderSubMenu(StringBuilder sb, IList<MenuEntity> infos)
        {
            if (infos == null) return;
            foreach (var info in infos.OrderBy(it => it.Sequence))
            {
                var url = ConfigurationManager.GetSetting<string>(info.Subsystem.Url);
                url = string.IsNullOrWhiteSpace(url) ? info.Subsystem.Url : url;
                sb.AppendFormat(" <li><a href=\"{0}{1}\" {2}>{3}</a> </li>", url, info.Url,
                    info.IsBlank ? "target='_blank'" : "", info.Name);
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="masterPage"></param>
        public static string GetResourcesHtml(this MasterPage masterPage)
        {
            var builder=new StringBuilder();
            var url = masterPage.Page.GetUrl("PresentationAdminHomeUrl");
            builder.Append("<script type='text/javascript' src='/Scripts/Winner/Message/Winner.Message.js'></script>");
            builder.AppendFormat("<script type='text/javascript' src='{0}/Scripts/Menu.js'></script>", url);
            builder.AppendFormat("<script type='text/javascript' src='{0}/Scripts/Message.js'></script>", url);
            builder.AppendFormat("<script type='text/javascript' src='{0}/scripts/MaintenPage.js'></script>", url);
            builder.AppendFormat("<script type='text/javascript' src='{0}/scripts/Message.js'></script>", url);
            builder.AppendFormat("<script type='text/javascript' src='{0}/scripts/Serializator.js'></script>", url);
            builder.AppendFormat("<script type='text/javascript' src='/scripts/Winner/Winner.ClassBase.js'></script>");
            builder.AppendFormat("<script type='text/javascript' src='/scripts/Winner/Dialog/Winner.Dialog.js'></script>");
            builder.AppendFormat("<script type='text/javascript' src='/scripts/Plug/jquery-1.7.1.min.js'></script>");
            builder.AppendFormat("<script type='text/javascript' src='/scripts/Plug/JqueryUI/jquery-ui-1.10.3.custom.min.js'></script>");

            builder.AppendFormat("<link href='{0}/Styles/Style.css' rel='stylesheet' type='text/css' />", url);
            builder.AppendFormat("<link href='/scripts/Plug/JqueryUI/jquery-ui-1.10.3.custom.css' rel='stylesheet' type='text/css' />");
            builder.AppendFormat("<link rel='shortcut icon' href='{0}/images/favicon.ico' type='image/x-icon' />", url);
            
            return builder.ToString();
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="masterPage"></param>
        public static string GetHeaderHtml(this MasterPage masterPage)
        {
            var builder = new StringBuilder();
            var url = masterPage.Page.GetUrl("PresentationAdminHomeUrl");
            builder.AppendFormat("<div class='head'><div class='logo'><img alt='蜂蚁窝' src='{0}/Images/logo.png' /></div>", url);
            builder.AppendFormat("<div class='name'>登录用户：{0}<a href='{1}/Quit.aspx'>退出</a></div>", ((AuthorizePageBase)masterPage.Page).Identity.Name, url);
            builder.AppendFormat("</div>");
            return builder.ToString();
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="masterPage"></param>
        public static string InitlizeMenu(this MasterPage masterPage)
        {
            var builder = new StringBuilder();
            var url = masterPage.Page.GetUrl("PresentationAdminHomeUrl");
            builder.Append("<script type='text/javascript'>$(document).ready(function () {var menu = new Menu('menu'); menu.Initialize();");
            builder.AppendFormat(" document.title.value = '蜂蚁窝系统 -' + document.getElementsByTagName('title')[0].innerHTML;$('#pageTitle').html(document.getElementsByTagName('title')[0].innerHTML);SetMessage('{0}');", url);
            builder.Append("  });  </script>");
            return builder.ToString();
        }
        #endregion
    }
}

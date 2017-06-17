using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Basedata;
using Beeant.Basic.Services.WebForm.Controls;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Scm.Controls.Basedata
{
    public partial class TagRadioButtonList : DropDownListTemplateBaseControl<TagGroupEntity>
    {
       
        protected StringBuilder Inputs = new StringBuilder();
        /// <summary>
        /// 存储名称
        /// </summary>
        public new string SaveName
        {
            get { return Attributes["SaveName"]; }
            set { Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public new string BindName
        {
            get { return lblTag.Attributes["BindName"]; }
            set { lblTag.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 验证名称
        /// </summary>
        public new string ValidateName
        {
            get { return Attributes["ValidateName"]; }
            set { Attributes.Add("ValidateName", value); }
        }
     
        /// <summary>
        /// 具体控件
        /// </summary>
        public override DropDownList DropDownList
        {
            get { return DropDownList1; }
        }
        /// <summary>
        /// 加载标签
        /// </summary>
        /// <param name="value"></param>
        public virtual void LoadTags(string value)
        {
            if(string.IsNullOrEmpty(value))
                return;
            var tags = value.Split(',');
            var query = new QueryInfo();
            query.Query<TagEntity>()
                 .Where(it => tags.Contains(it.Value))
                 .Select(it => new object[] { it.Name, it.Value });
            var infos= Ioc.Resolve<IApplicationService, TagEntity>().GetEntities<TagEntity>(query);
            if (infos != null)
            {
                foreach (var info in infos)
                {
                    Inputs.Append(string.Format("<input type='radio' name='{0}ckgSelected' value='{3}' {1}><span>{2}</span>", ClientID,
                                                tags.Contains(info.Value) ? "checked" : string.Empty, info.Name, info.Value));
                }
            }
        }
        /// <summary>
        /// 下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTags();
        }
        /// <summary>
        /// 得到值
        /// </summary>
        /// <returns></returns>
        public virtual string GetSelected()
        {
            return Request.Params[ClientID + "ckgSelected"] ?? string.Empty;
        }
        /// <summary>
        /// 加载标签
        /// </summary>
        protected void LoadTags()
        {
            var infos = GetTags();
            var tags = GetSelected().Split(',').ToList();
            if (infos != null)
            {
                foreach (var info in infos)
                    Inputs.Append(string.Format("<input type='radio' name='{0}ckgSelected' value='{3}' {1}><span>{2}</span>",
                                                ClientID, tags.Contains(info.Value) ? "checked" : string.Empty,
                                                info.Name, info.Value));
            }
        }
        /// <summary>
        /// 得到Tag
        /// </summary>
        /// <returns></returns>
        protected virtual IList<TagEntity> GetTags()
        {
            if (string.IsNullOrEmpty(DropDownList.SelectedValue))
                return null;
            var query = new QueryInfo();
            query.Query<TagEntity>()
                 .Where(it => it.TagGroup.Id == DropDownList.SelectedValue.Convert<int>())
                 .Select(it => new object[] {it.Name, it.Value});
            return Ioc.Resolve<IApplicationService, TagEntity>().GetEntities<TagEntity>(query);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            CreateClientScript();
        }

        protected override void OnInit(EventArgs e)
        {
            LoadTags();
            base.OnInit(e);
        }

        public string GetSelectedValues()
        {
            return Request.Params[string.Format("{0}ckgSelected", ClientID)] ?? string.Empty;
        }
        /// <summary>
        /// 输出脚本
        /// </summary>
        protected virtual void CreateClientScript()
        {
            var script = new StringBuilder();
            var clientAjax = string.Format("$.ajax(" + Environment.NewLine +
                                           "{{" + Environment.NewLine +
                                           "    url: '{1}?TagGroupId=' + $(\"#{2}\").val()," + Environment.NewLine +
                                           "    type: 'post'," + Environment.NewLine +
                                           "    dataType: 'text'," + Environment.NewLine +
                                           "    success: function (res) " + Environment.NewLine +
                                           "        {{  " + Environment.NewLine +
                                           "            $(\"#{0}ckg\").empty();" + Environment.NewLine +
                                           "            if(res.length > 0) {{ " + Environment.NewLine +
                                           "                var json = eval(res);" + Environment.NewLine +
                                           "                $(json).each(function()" + Environment.NewLine +
                                           "                {{" + Environment.NewLine +
                                           "                    $(\"#{0}ckg\").append('" +
                                           string.Format("<input name=\"{0}ckgSelected\" type=\"radio\" value=' + this.Value + '>' + this.Text);", ClientID) + Environment.NewLine +
                                           "                }});" + Environment.NewLine +
                                           "            }}" + Environment.NewLine +
                                           "        }}," + Environment.NewLine +
                                           "error: function (request, status, errorTxt) {{ alert(errorTxt); }}" + Environment.NewLine +
                                           "}});", ClientID, "/ajax/basedata/Tag.aspx", DropDownList1.ClientID);
            script.Append(string.Format("$(\"#{0}\").change(function () {{{1}}});", DropDownList1.ClientID, clientAjax));
            Page.ExecuteScript(script.ToString());
        }

       
 
    }
}
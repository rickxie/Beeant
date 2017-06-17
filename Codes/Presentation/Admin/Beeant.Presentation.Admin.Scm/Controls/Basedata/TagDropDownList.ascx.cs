using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class TagDropDownList : DropDownListTemplateBaseControl<TagGroupEntity>
    {

        /// <summary>
        /// 存储名称
        /// </summary>
        public override string SaveName
        {
            get { return ddlTag.Attributes["SaveName"]; }
            set { ddlTag.Attributes.Add("SaveName", value); }
        }

        /// <summary>
        /// 绑定名称
        /// </summary>
        public override string BindName
        {
            get { return ddlTag.Attributes["BindName"]; }
            set { ddlTag.Attributes.Add("BindName", value); }
        }
        /// <summary>
        /// 验证名称
        /// </summary>
        public override string ValidateName
        {
            get { return ddlTag.Attributes["ValidateName"]; }
            set { ddlTag.Attributes.Add("ValidateName", value); }
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public override string SearchWhere
        {
            get { return ddlTag.Attributes["SearchWhere"]; }
            set { ddlTag.Attributes.Add("SearchWhere", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public override string SearchParamterName
        {
            get { return ddlTag.Attributes["SearchParamterName"]; }
            set { ddlTag.Attributes.Add("SearchParamterName", value); }
        }
        /// <summary>
        /// 搜索参数名称
        /// </summary>
        public override string SearchPropertyTypeName
        {
            get { return ddlTag.Attributes["SearchPropertyTypeName"]; }
            set { ddlTag.Attributes.Add("SearchPropertyTypeName", value); }
        }
        /// <summary>
        /// 不搜索的值
        /// </summary>
        public override string UnSearchValue
        {
            get { return ddlTag.Attributes["UnSearchValue"]; }
            set { ddlTag.Attributes.Add("UnSearchValue", value); }
        }
        /// <summary>
        /// 具体控件
        /// </summary>
        public override DropDownList DropDownList
        {
            get { return DropDownList1; }
        }
   
        /// <summary>
        /// 下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (DropDownList1.SelectedIndex == 0 || DropDownList1.SelectedItem.Text=="=请选择=")
            {
                ddlTag.DataBind();
            }
            else
            {
                ddlTag.DataBind(GetTags());
            }
            ddlTag.SelectedIndex = 0;
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

       

       
 
    }
}
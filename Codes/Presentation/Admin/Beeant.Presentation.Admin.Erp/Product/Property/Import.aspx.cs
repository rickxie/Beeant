using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Extension;
using Winner.Persistence;
using Winner.Persistence.Linq;
using Beeant.Basic.Services.WebForm.Pages;

namespace Beeant.Presentation.Admin.Erp.Product.Property
{
    public partial class Import : AuthorizePageBase
    {
        protected  void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadProperties();
                tvCategoryTree.LoadData();
            }
            this.ExecuteScript("$('#btnClose').click(function(){window.close();});");
        }

        protected virtual void LoadProperties()
        {
            var infos = GetProperties();
            if (infos == null)
                return;
            lblName.Text = string.Join(",", infos.Select(it => it.Name).ToArray());
        }

        protected void tvCategoryTree_SelectedNodeChanged(object sender, EventArgs e)
        {
        
            if (!string.IsNullOrEmpty(tvCategoryTree.TreeView.SelectedNode.Value))
                tvCategoryTree.LoadData();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var rev = CopyEntities();
            Message1.ShowMessage(rev ? "导入成功" : "导入失败");
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        protected virtual bool CopyEntities()
        {
            var properities = GetProperties();
            if (properities == null)
                return false;
            var categories = GetCategories();
            if (categories.Count == 0)
            {
                Message2.ShowMessage("请选择要导入的类目");
                return false;
            }
            foreach (var properity in properities)
            {
                var propertyRules = GetPropertyRules(properity.Id);
                foreach (var category in categories)
                {
                    properity.SaveType = SaveType.Add;
                    properity.Id = 0;
                    properity.Category = category;
                    Ioc.Resolve<IApplicationService, PropertyEntity>().Save(properity);
                    foreach (var propertyRule in propertyRules)
                    {
                        propertyRule.Property = properity;
                        propertyRule.SaveType=SaveType.Add;
                        propertyRule.Id = 0;
                        Ioc.Resolve<IApplicationService, PropertyRuleEntity>().Save(propertyRule);
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 得到复制的属性
        /// </summary>
        /// <returns></returns>
        protected virtual IList<PropertyEntity> GetProperties()
        {
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
                return null;
            var values = Request.QueryString["id"].Split(',');
            var ids = values.Select(value => value.Convert<long>()).ToArray();
            var query = new QueryInfo();
            query.Query<PropertyEntity>().Where(it => ids.Contains(it.Id));
            return Ioc.Resolve<IApplicationService, PropertyEntity>().GetEntities<PropertyEntity>(query);
        
        }

        /// <summary>
        /// 得到复制的属性规则
        /// </summary>
        /// <returns></returns>
        protected virtual IList<PropertyRuleEntity> GetPropertyRules(long propertyId)
        {
            var query = new QueryInfo();
            query.Query<PropertyRuleEntity>().Where(it => it.Property.Id == propertyId);
            return Ioc.Resolve<IApplicationService, PropertyRuleEntity>().GetEntities<PropertyRuleEntity>(query);
        }
        /// <summary>
        /// 得到导入的类目
        /// </summary>
        protected virtual IList<CategoryEntity> GetCategories()
        {
           return (from TreeNode checkedNode in tvCategoryTree.TreeView.CheckedNodes where checkedNode.Checked select new CategoryEntity {Id = checkedNode.Value.Convert<long>()}).ToList();
        }
    }
}
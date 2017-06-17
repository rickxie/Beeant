using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;

using Beeant.Basic.Services.WebForm.Pages;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Ajax.Product
{
    public partial class Property : AjaxPageBase<PropertyEntity>
    {
        public IList<GoodsPropertyEntity> GoodsProperties { get; set; }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
                LoadGoodsProperties();
            base.Page_Load(sender, e);
        }

        protected virtual void LoadGoodsProperties()
        {
            if(string.IsNullOrEmpty(Request["goodsid"]))return;
            var query = new QueryInfo();
            query.Query<GoodsPropertyEntity>()
            .Where(it => it.Goods.Id == Request["goodsid"].Convert<long>()).Select(it => new object[] { it, it.Property.IsSku });
            GoodsProperties = Ioc.Resolve<IApplicationService, GoodsPropertyEntity>().GetEntities<GoodsPropertyEntity>(query);
            
        }

        protected override string GetListItem(PropertyEntity info)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Id:'{0}',Name:'{1}', Type: '{2}',  Message: '{3}', CustomCount: {4},IsSku:{5},IsAllowEdit:{6}"
                , info.Id, info.Name, info.Type.ToString("F"), info.Message, info.CustomCount, info.IsSku.ToString().ToLower(), info.IsAllowEdit.ToString().ToLower());
            AppendSelect(builder, info);
            AppendValues(builder, info);
            AppendRules(builder, info);
            AppendDefaultValues(builder, info);
            return builder.ToString();
        }

        protected virtual void AppendSelect(StringBuilder builder, PropertyEntity info)
        {
            if (GoodsProperties != null)
            {
                var property = GoodsProperties.FirstOrDefault(it => it.Property != null && it.Property.Id == info.Id);
                if (property == null)
                {
                    builder.Append(",IsSelect:false");
                    return;
                }
                if (info.IsSku)
                {
                    builder.AppendFormat(",IsSelect:{0}",(property.Product!=null && property.Product.Id>0).ToString().ToLower());
                    return;
                }
            }
            builder.Append(",IsSelect:true");
        }

        /// <summary>
        /// 拼接值
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="info"></param>
        protected virtual void AppendValues(StringBuilder builder, PropertyEntity info)
        {
            builder.Append(",Values:[");
            if (info.ValueArray != null && info.ValueArray.Length > 0)
            {
                foreach (var value in info.ValueArray)
                {
                    builder.AppendFormat("'{0}',", value);
                }
                builder.Remove(builder.Length - 1, 1);
            } 
            builder.Append("]");
        }
        /// <summary>
        /// 拼接规则
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="info"></param>
        protected virtual void AppendRules(StringBuilder builder, PropertyEntity info)
        {
            var goodsProperty =GoodsProperties==null?null:
                   GoodsProperties.FirstOrDefault(it => it.Property != null && info.Id.Equals(it.Property.Id));
            builder.Append(",Rules:[");
            if (info.PropertyRules != null && info.PropertyRules.Count > 0)
            {
                if (info.PropertyRules.Count > 0)
                {
                    bool hasPattern = false;
                    foreach (var rule in info.PropertyRules)
                    {
                        if(goodsProperty!=null && (rule.Type & (int)PropertyRuleType.Modify)<=0)continue;
                        if (goodsProperty == null && (rule.Type & (int)PropertyRuleType.Add) <= 0)continue;
                        builder.Append("{Pattern:");
                        builder.AppendFormat("'{0}'", rule.GetPattern().Replace("\\", "\\\\"));
                        builder.Append(",Options:\"");
                        if (rule.IsMultiline) builder.Append("m");
                        if (!rule.IsIgnoreCase) builder.Append("i");
                        builder.Append("\",");
                        builder.AppendFormat("Message:\"{0}\"", string.IsNullOrEmpty(rule.Message) ? info.Message : rule.Message);
                        builder.AppendFormat(",IsRange:{0}", rule.Rule.IsRange.ToString().ToLower());
                        builder.Append("},");
                        hasPattern = true;
                    }
                   if(hasPattern) builder.Remove(builder.Length - 1, 1);
                }
            }
            builder.Append("]");
        }
        /// <summary>
        /// 拼接规则
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="info"></param>
        protected virtual void AppendDefaultValues(StringBuilder builder, PropertyEntity info)
        {
            builder.Append(",DefaultValues:[");
            if (GoodsProperties != null && GoodsProperties.Count>0)
            {
                var goodsProperties =
                    GoodsProperties.Where(it => it.Property != null && info.Id.Equals(it.Property.Id)).ToList();
                if (goodsProperties.Count > 0)
                {
                    foreach (var goodsProperty in goodsProperties)
                    {
                        builder.Append("{");
                        builder.AppendFormat("Id:'{0}',Value: '{1}'"
                            , goodsProperty.Id, goodsProperty.Value);
                        builder.Append("},");
                    }
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            builder.Append("]");
        }
        
        protected override void SetQuerySelect(QueryInfo query)
        {
            query.Query<PropertyEntity>()
                .Select(it => new object[] { it.Name, it.Id, it.Type, it.Value, it.Message,it.IsSku,it.IsAllowEdit,it.IsSku,
                    it.CustomCount,it.PropertyRules.Select(s =>new object[]{s.IsIgnoreCase,s.IsMultiline,s.Type,s.Paramter,s.Rule.Pattern,s.Rule.IsRange} ) });
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<PropertyEntity>().Where(it => it.Category.Id == (Request.QueryString["categoryId"].Convert<long>()) && it.Type != PropertyType.None && it.IsUsed);
            query.PageSize = 0;
        }
    }
}
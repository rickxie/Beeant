using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Beeant.Domain.Entities.Product;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Domain.Services.Product
{
    public class GoodsPropertyDomainService : RealizeDomainService<GoodsPropertyEntity>, IGoodsPropertyDomainService
    {

        #region 重写验证
   

        #region 添加验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateAdd(GoodsPropertyEntity info)
        {
            var rev = ValidateGoods(info,null) && ValidateProduct(info) && ValidateExist(info);
            return rev;
        }
        #endregion
        #region 修改验证
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected override bool ValidateModify(GoodsPropertyEntity info)
        {
            var dataEntity = Repository.Get<GoodsPropertyEntity>(info.Id);
            var rev = ValidateGoods(info, dataEntity);
            return rev;
        }
        #endregion
        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateExist(GoodsPropertyEntity info)
        {
            var query = new QueryInfo();
            query.Query<GoodsPropertyEntity>()
                 .Where(it => it.Goods.Id == info.Goods.Id && it.Product.Id == info.Product.Id && it.Value==info.Value && info.Property.Id==it.Property.Id);
            var infos = Repository.GetEntities<GoodsPropertyEntity>(query);
            var dataEntity = infos == null ? null : infos.FirstOrDefault();
            if (dataEntity != null)
            {
                info.AddError("Exist");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证类目
        /// </summary>
        /// <param name="info"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        protected virtual bool ValidateGoods(GoodsPropertyEntity info, GoodsPropertyEntity dataEntity)
        {
            if (!info.HasSaveProperty(it => it.Goods.Id))
                return true;
            if (dataEntity != null && dataEntity.Goods != null && info.Goods != null && info.Goods.Id == dataEntity.Goods.Id)
                return true;
            if (info.Goods != null && info.Goods.SaveType == SaveType.Add)
                return true;
            if (info.Goods != null && info.Goods.Id!=0)
            {
                var goods = Repository.Get<GoodsEntity>(info.Goods.Id);
                if (goods == null)
                {
                    info.AddErrorByName(typeof(GoodsEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(GoodsEntity).FullName, "NoExist");
            return false;
        }
        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidateProduct(GoodsPropertyEntity info)
        {
            if (!info.HasSaveProperty(it => it.Product.Id))
                return true;
            if (info.Product != null && info.Product.Id == 0)
                return true;
            if (info.Product != null && info.Product.SaveType == SaveType.Add)
                return true;
            if (info.Product != null && info.Product.Id != 0)
            {
                var product = Repository.Get<ProductEntity>(info.Product.Id);
                if (product == null)
                {
                    info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
                    return false;
                }
                return true;
            }
            info.AddErrorByName(typeof(ProductEntity).FullName, "NoExist");
            return false;
        }
        #endregion

        #region 验证接口

        /// <summary>
        /// 验证发布
        /// </summary>
        /// <param name="goodId"></param>
        /// <param name="saveType"></param>
        /// <param name="goodsProperties"></param>
        /// <returns></returns>
        public virtual IList<ErrorInfo> ValidateRuleType(long goodId, SaveType saveType,
                                                         IList<GoodsPropertyEntity> goodsProperties)
        {
            if (goodId == 0 || (goodsProperties != null && goodsProperties.Count(it => it.SaveType != SaveType.None) == 0)) return null;
            var errors = new List<ErrorInfo>();
            var properties = GetPropertiesByGoodId(goodId);
            var validEntities = new List<GoodsPropertyEntity>();
            if(goodsProperties!=null)
                validEntities.AddRange(goodsProperties);
            if ((saveType == SaveType.Add || saveType == SaveType.Modify) && properties != null)
            {
                validEntities.AddRange(
                    properties.Where(it => validEntities.Count(s => s.Property != null && s.Property.Id == it.Id) == 0)
                              .Select(it =>
                                      new GoodsPropertyEntity
                                          {
                                              Property = it,
                                              SaveType = saveType,
                                              Goods = new GoodsEntity {Id = goodId}
                                          }));
            }
            foreach (var goodsProperty in validEntities)
            {
                var rules = GetPropertyRules(goodsProperty.Property.Id);
                rules = rules != null ? rules.Where(it => it.Type != 0).ToList() : null;
                if (rules.Count == 0) continue;
                var ruleType = (int) GetPropertyRuleType(goodsProperty.SaveType);
                rules.Where(rule => (rule.Type & ruleType) > 0)
                     .Aggregate(true, (current, rule) => ValidateRule(errors, goodsProperty, rule) & current);
            }
            return errors;
        }



        /// <summary>
        /// 根据产品得到属性
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        protected virtual IList<PropertyEntity> GetPropertiesByGoodId(long goodsId)
        {
            var goods = Repository.Get<GoodsEntity>(goodsId);
            if (goods == null) return null;
            var query = new QueryInfo();
            query.Query<PropertyEntity>().Where(it => it.Category.Id == goods.Category.Id && it.Type!=PropertyType.None);
            return Repository.GetEntities<PropertyEntity>(query);
        }

     
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="info"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        protected virtual bool ValidateRule(IList<ErrorInfo> errors,GoodsPropertyEntity info, PropertyRuleEntity rule)
        {
            var isValidate = true;
            if (rule.Rule.IsRange)
            {
                object value = 0;
                if (!string.IsNullOrEmpty(info.Value))
                {
                    var reg = new Regex(@"[^\d]");
                    value = reg.Replace(value.ToString(), "");
                }
                var values = rule.GetPattern().Split('-');
                object startValue = values.Length > 1? values[0] : null;
                object endValue = values.Length > 2 ? values[1] : null;
                if (startValue != null)
                    isValidate = Convert.ToDouble(value) >= Convert.ToDouble(startValue);
                if (endValue != null)
                    isValidate = isValidate && Convert.ToDouble(value) <= Convert.ToDouble(endValue);
            }
            else
            {
                var option = rule.IsMultiline ? RegexOptions.Multiline : RegexOptions.Singleline;
                option = rule.IsIgnoreCase ? option & RegexOptions.IgnoreCase : option;
                if (!Regex.IsMatch(info.Value ?? "", rule.GetPattern(), option))
                {
                    isValidate = false;
                }
            }
            if (!isValidate)
            {
                var rev = new ErrorInfo { Key = rule.Property.Name, Message =string.IsNullOrEmpty(rule.Message)? rule.Property.Message:rule.Message };
                errors.Add(rev);
            }
            return isValidate;
        }
        /// <summary>
        /// 得到验证类型
        /// </summary>
        /// <param name="saveType"></param>
        /// <returns></returns>
        protected virtual PropertyRuleType GetPropertyRuleType(SaveType saveType)
        {
            IDictionary<SaveType, PropertyRuleType> temp = new Dictionary<SaveType, PropertyRuleType>
                    {
                        {SaveType.Add, PropertyRuleType.Add},
                        {SaveType.Modify, PropertyRuleType.Modify},
                        {SaveType.Remove, PropertyRuleType.Remove}
                    };
            return temp[saveType];
        }

        /// <summary>
        /// 得到规则
        /// </summary>
        /// <returns></returns>
        protected virtual IList<PropertyRuleEntity> GetPropertyRules(long propertyId)
        {
            var query = new QueryInfo();
            query.SetCacheTime(DateTime.MaxValue)
                .Query<PropertyRuleEntity>().Select(it => new object[] { it, it.Rule, it.Property,it.Type});
            var propertyRules= Repository.GetEntities<PropertyRuleEntity>(query);
            if (propertyRules == null) return null;
            return propertyRules.Where(it => it.Property.Id.Equals(propertyId) && it.Property.IsUsed).ToList();
        }
 
        #endregion
    }
}

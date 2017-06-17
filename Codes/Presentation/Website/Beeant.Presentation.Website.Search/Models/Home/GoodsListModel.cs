using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Beeant.Basic.Services.Mvc.Paging;
using Beeant.Domain.Entities.Product;


namespace Beeant.Presentation.Website.Search.Models.Home
{
    public class GoodsListModel : PagerModel
    {
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 提交Url
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public override int PageSize
        {
            get { return 25; }
            set { base.PageSize = value; }
        }

        public override int PageCount
        {
            get
            {
                var value = base.PageCount;
                return value > 100 ? 100 : value;
            }
            set { base.PageCount = value; }
        }



        #region 搜索

        /// <summary>
        /// 关键字
        /// </summary>
        public string SearchKey { get; set; }
 

        #endregion

        #region 类目属性检索

        /// <summary>
        /// 商品列表
        /// </summary>
        public IList<GoodsEntity> GoodsList { get; set; }

        /// <summary>
        /// 未搜索属性
        /// </summary>
        public IList<PropertyEntity> SearchProperties { get; set; }

        /// <summary>
        /// 已经搜索属性
        /// </summary>
        public IList<PropertyEntity> ExistSearchProperties { get; set; }

        /// <summary>
        /// 排序名称
        /// </summary>
        public string OrderbyName { get; set; }

        /// <summary>
        /// 属性的Url
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 类目编号
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 类目
        /// </summary>
        public CategoryEntity Category { get; set; }
        /// <summary>
        /// 得到属性
        /// </summary>
        /// <returns></returns>
        public virtual string GetPropertyUrl(long id,string name,string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            var infos = new List<PropertyEntity>();
            if (ExistSearchProperties != null && ExistSearchProperties.Count > 0)
            {
                if ( ExistSearchProperties.FirstOrDefault(it => it.Id == id && it.Value==value) != null )
                    return null;
                var p = SearchProperties.FirstOrDefault(it => it.Id == id);
                if (p != null && (p.SearchType == PropertySearchType.None || p.SearchType == PropertySearchType.Select))
                {
                    infos.AddRange(ExistSearchProperties.Where(it=>it.Id!=id));
                }
                else
                {
                    infos.AddRange(ExistSearchProperties);
                }
                infos.Add(new PropertyEntity {Id = id, Name = name, Value = value});
            }
            else
            {
                infos.Add(new PropertyEntity {Id = id, Name = name, Value = value});
            }
            return SerializeProperty(infos);
        }
        /// <summary>
        /// 序列化属性
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected virtual string SerializeProperty(IList<PropertyEntity> properties)
        {
            if (properties == null)
                return null;
            var array = new ArrayList();
            foreach (var property in properties)
            {
                var dis = new Dictionary<string, object>
                    {
                        {"Id", property.Id},
                        {"Name", property.Name},
                        {"Value", property.Value}
                    };
                array.Add(dis);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(array);
        }
        /// <summary>
        /// 得到搜索属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual void SetExistSearchProperties(string property)
        {
            if (string.IsNullOrEmpty(property))
                return;
            try
            {
                ExistSearchProperties = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PropertyEntity>>(property);
            }
            catch (Exception)
            {
                
               
            }
          
        }

        #endregion

   
      
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Erp.Product.Product
{
    public partial class Sku : Basic.Services.WebForm.Pages.MaintenPageBase<ProductEntity>
    {
        public long GoodsId
        {
            get { return Request.QueryString["GoodsId"].Convert<long>(); }
        }
       
   
      
        protected override  void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSkus();
            }
            LoadProperties();
            base.Page_Load(sender,e);
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<ProductEntity>().Where(it => it.Goods.Id == GoodsId);
            base.SetQueryWhere(query);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="info"></param>
        protected override void Save(ProductEntity info)
        {
            if(info.SaveType!=SaveType.Modify)
                return;
            base.Save(info);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        protected override ProductEntity FillEntity()
        {
            var info= base.FillEntity();
            info.Sku = GetSku();
            info.SetProperty(it => it.Sku);
            var goodsProperties = GetGoodsProperties();
            if (info.SkuJsons != null )
            {
                foreach (var json in info.SkuJsons)
                {
                    var goodsProperty = goodsProperties.FirstOrDefault(it => 
                        it.Property != null && it.Property.Id == json.Id
                        && it.Product!=null && it.Product.Id==RequestId);
                    if (goodsProperty != null && goodsProperty.Value==json.Value)
                        continue;
                    if (goodsProperty == null)
                    {
                        goodsProperty = new GoodsPropertyEntity
                            {
                                Value = json.Value,
                                Goods = new GoodsEntity {Id = GoodsId},
                                Product = info,
                                Property = new PropertyEntity {Id = json.Id},
                                SaveType = SaveType.Add
                            };
                    }
                    else
                    {
                        goodsProperty.Value = json.Value;
                        goodsProperty.SetProperty(it => it.Value);
                        goodsProperty.SaveType = SaveType.Modify;
                    }
                    info.GoodsProperties = info.GoodsProperties ?? new List<GoodsPropertyEntity>();
                    info.GoodsProperties.Add(goodsProperty);
                }
                foreach (var goodsProperty in goodsProperties)
                {
                    if (info.SkuJsons.Count(it => it.Id == goodsProperty.Property.Id) == 0)
                    {
                        goodsProperty.SaveType = SaveType.Remove;
                        info.GoodsProperties = info.GoodsProperties ?? new List<GoodsPropertyEntity>();
                        info.GoodsProperties.Add(goodsProperty);
                    }
                }
              

            }
            return info;
        }
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="info"></param>
        protected override void BindEntity(ProductEntity info )
        {
            if (info!=null && info.SkuJsons != null)
            {
                foreach (var json in info.SkuJsons)
                {
                    var textBox = divPropery.FindControl(string.Format("txt{0}", json.Id)) as TextBox; 
                    if(textBox==null)
                        continue;
                    textBox.Text = json.Value;
                }
            }
            base.BindEntity(info);
        }
      
        /// <summary>
        /// 得到属性
        /// </summary>
        /// <returns></returns>
        protected virtual IList<GoodsPropertyEntity> GetGoodsProperties()
        {
            var query = new QueryInfo();
            query.Query<GoodsPropertyEntity>().Where(it => it.Goods.Id == GoodsId && it.Property.IsSku && it.Product.Id==RequestId);
            var infos = Ioc.Resolve<IApplicationService, GoodsPropertyEntity>().GetEntities<GoodsPropertyEntity>(query);
            return infos;
        }
        /// <summary>
        /// 得到SKU
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSku()
        {
            var builder = new List<string>();
            foreach (var ctrl in divPropery.Controls)
            {
                var txt = ctrl as TextBox;
                if (txt == null) continue;
                var value = "{"+string.Format("Id:\"{0}\",Name:\"{1}\",Value:\"{2}\"", txt.Attributes["PropertyId"],
                                                       txt.Attributes["PropertyName"],txt.Text.Trim())+"}";
                builder.Add(value);
            }
            return string.Format("[{0}]", string.Join(",", builder.ToArray()));
        }
        /// <summary>
        /// 得到类目编号
        /// </summary>
        /// <returns></returns>
        protected virtual long GetCategoryId()
        {
            var query = new QueryInfo();
            query.Query<GoodsEntity>().Where(it => it.Id == GoodsId).Select(it => it.Category.Id);
            var infos = Ioc.Resolve<IApplicationService, GoodsEntity>().GetEntities<GoodsEntity>(query);
            var info = infos == null ? null : infos.FirstOrDefault();
            if (info != null  && info.Category != null)
                return info.Category.Id;
            return 0;
        }
        /// <summary>
        /// 得到标签
        /// </summary>
        /// <returns></returns>
        public virtual void LoadSkus()
        {
            var categoryId = GetCategoryId();
            if (categoryId == 0)
                return;
            var query = new QueryInfo();
            query.Query<PropertyEntity>().Where(it => it.Category.Id==categoryId && it.IsSku).Select(it=>new object[]{it.Id,it.Name});
            var infos = Ioc.Resolve<IApplicationService, PropertyEntity>().GetEntities<PropertyEntity>(query);
            CheckBoxList1.DataTextField = "Name";
            CheckBoxList1.DataValueField = "Id";
            CheckBoxList1.DataSource = infos;
            CheckBoxList1.DataBind();
            foreach (ListItem item in CheckBoxList1.Items)
            {
                item.Selected = true;
            }
        }
        /// <summary>
        /// 得到标签
        /// </summary>
        /// <returns></returns>
        public virtual void LoadProperties()
        {
            divPropery.Controls.Clear();
            var ids = new List<long>();
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    ids.Add(item.Value.Convert<long>());
                }
            }
            if(ids.Count==0)
                return;
            var query = new QueryInfo();
            query.Query<PropertyEntity>().Where(it =>ids.ToArray().Contains(it.Id));
            var infos= Ioc.Resolve<IApplicationService, PropertyEntity>().GetEntities<PropertyEntity>(query);
            if(infos==null)
                return;
            CreateProperty(infos);
        }
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="properties"></param>
        protected virtual void CreateProperty(IList<PropertyEntity> properties)
        {
            foreach (var property in properties)
            {
                var lt = new Literal {ID = string.Format("lit{0}", property.Id), Text = property.Name };
                var textbox = new TextBox { ID = string.Format("txt{0}", property.Id) };
                textbox.Attributes.Add("PropertyId",property.Id.ToString());
                textbox.Attributes.Add("PropertyName", property.Name);
                divPropery.Controls.Add(lt);
                divPropery.Controls.Add(textbox);
            }
        }
        /// <summary>
        /// 改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckBoxList1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadProperties();
        }
 
     
    }
}
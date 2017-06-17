using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Application.Services.Utility;
using Beeant.Domain.Entities.Order;
using Beeant.Domain.Entities.Product;
using Beeant.Presentation.Admin.Erp.Controls.Basedata;
using Beeant.Basic.Services.WebForm.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Winner.Base;
using Winner.Persistence;
using Winner.Persistence.Linq;

namespace Beeant.Presentation.Admin.Erp.Product.Goods
{

    public partial class Edit : System.Web.UI.UserControl
    {
        public bool IsPublish { get; set; }
        public SaveType SaveType { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public TagRadioButtonList TagCheckBoxList
        {
            get { return ckTag; }
        }
        /// <summary>
        /// 产品
        /// </summary>
        public string Products { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Prices { get; set; }


        public decimal DespoitRate { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string GoodsImages { get; set; }
        /// <summary>
        /// 产品列表
        /// </summary>
        public IList<ProductEntity> ProductEntities { set; get; }

        public long CategoryId
        {
            get
            {
                var values = string.IsNullOrEmpty(hfbranchId.Value) ? null : hfbranchId.Value.Split(',');
                if (values == null) return 0;
                return values[values.Length - 1].Convert<long>();
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                ckTag.LoadData();
                cbPayTypes.LoadData();
                LoadBranchId();
                ddlFreight.LoadData(0);
                ckUnusedStatus.LoadData();
                if (ckUnusedStatus.CheckBoxList.Items.Count > 1)
                {
                    foreach (ListItem item in ckUnusedStatus.CheckBoxList.Items)
                    {
                        if (item.Value == OrderStatusType.Cancel.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
            IsPublish = SaveType == SaveType.Modify || IsPostBack ||
                          !string.IsNullOrEmpty(Request.QueryString["Id"]);

            LoadProducts();
            LoadImages();
            LoadDetails();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindProductValidation();
        }



        /// <summary>
        /// 加载图片
        /// </summary>
        protected virtual void LoadImages()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Id"])) return;
            var query = new QueryInfo();
            query.Query<GoodsImageEntity>().Where(it => it.Goods.Id == Request.QueryString["Id"].Convert<long>()).OrderBy(it => it.Sequence)
                .Select(it => new object[] { it, it.Product.Sku });
            var infos = Ioc.Resolve<IApplicationService, GoodsImageEntity>().GetEntities<GoodsImageEntity>(query);
            var builder = new StringBuilder();
            builder.Append("[");
            if (infos != null && infos.Count > 0)
            {
                foreach (var info in infos)
                {
                    builder.Append("{");
                    builder.AppendFormat("Id:'{0}',Value:'{1}',Sku:'{2}'", info.Id, info.FullFileName, info.Product == null || string.IsNullOrEmpty(info.Product.Sku) ? "" : info.Product.Sku.Replace("\"", "\\\""));
                    builder.Append("},");
                }
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
            GoodsImages = builder.ToString();
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        protected virtual void LoadDetails()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Id"])) return;
            var info = GoodsDetail();
            if (info != null)
            {
                txtCount.Value = info.Goods.Count == 0 ? "1000" : info.Goods.Count.ToString();
                txtCost.Value = info.Goods.Cost == 0 ? "1000" : info.Goods.Cost.ToString();
                txtPrice.Value = info.Goods.Price == 0 ? "2000" : info.Goods.Price.ToString();
                Editor1.TextArea.Value = info.Detail;
                hfAttchment.HRef = info.DownAttachmentUrl;
                hfAttchment.InnerText = info.Attachment;
            }
            else
            {

                txtCost.Value = "1000";
                txtCount.Value = "1000";
                txtPrice.Value = "2000";

            }
        }
        /// <summary>
        /// 得到商品详情
        /// </summary>
        /// <returns></returns>
        protected virtual GoodsDetailEntity GoodsDetail()
        {
            var query = new QueryInfo();
            query.Query<GoodsDetailEntity>()
                 .Where(it => it.Goods.Id == Request.QueryString["Id"].Convert<long>() && it.Product.Id == 0);
            var infos = Ioc.Resolve<IApplicationService, GoodsDetailEntity>().GetEntities<GoodsDetailEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }

        /// <summary>
        /// 加载产品
        /// </summary>
        protected virtual void LoadProducts()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Id"])) return;
            var query = new QueryInfo();
            query.Query<ProductEntity>().Where(it => it.Goods.Id == Request.QueryString["Id"].Convert<long>())
                 .OrderBy(it => it.Id);
            ProductEntities = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query);
            var builder = new StringBuilder();
            builder.Append("[");
            var tempEntities = ProductEntities == null ? null : ProductEntities.Where(it => it.Sku != "").ToList();
            if (tempEntities != null && tempEntities.Count > 0)
            {
                foreach (var info in tempEntities)
                {
                    builder.Append("{");
                    builder.AppendFormat("Id:'{0}',Price:{1},Cost:{2},Count:{3},OrderMinCount:{4},OrderStepCount:{5},DataId:'{6}',DepositRate:{7},IsCustom:{8},IsReturn:{9},IsSales:'{10}',Sku:'{11}',OrderLimitCount:{12}",
                        info.Id, info.Price, info.Cost, info.Count, info.OrderMinCount, info.OrderStepCount, info.DataId, info.DepositRate,
                        info.IsCustom.ToString().ToLower(), info.IsReturn.ToString().ToLower(), info.IsSales.ToString().ToLower(),
                        string.IsNullOrEmpty(info.Sku) ? "" : info.Sku.Replace("\"", "\\\""),info.Promotion==null?0:info.Promotion.OrderLimitCount);
                    builder.Append("},");
                }
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("]");
            Products = builder.ToString();
            var defaultEntity = ProductEntities == null ? null : ProductEntities.FirstOrDefault(it => it.Sku == "");
            if (defaultEntity != null)
            {
                hfProductId.Value = defaultEntity.Id.ToString();
            }

        }

        /// <summary>
        /// 加载类目编号
        /// </summary>
        protected virtual void LoadBranchId()
        {
            if (string.IsNullOrEmpty(Request.QueryString["Id"])) return;
            var goods = Ioc.Resolve<IApplicationService, GoodsEntity>().GetEntity<GoodsEntity>(Request.QueryString["Id"].Convert<long>());
            if (goods == null || goods.Category == null) return;
            var builder = new StringBuilder();
            BuilderCategoryBranch(builder, goods.Category.Id);
            if (builder.Length > 0) builder.Remove(0, 1);
            hfbranchId.Value = builder.ToString();
        }

        /// <summary>
        /// 拼接类目
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="categoryId"></param>
        protected virtual void BuilderCategoryBranch(StringBuilder builder, long categoryId)
        {
            if (categoryId == 0) return;
            builder.Insert(0, string.Format(",{0}", categoryId));
            var category = Ioc.Resolve<IApplicationService, CategoryEntity>().GetEntity<CategoryEntity>(categoryId);
            if (category == null || category.Parent == null) return;
            BuilderCategoryBranch(builder, category.Parent.Id);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var builder = new StringBuilder();
            builder.AppendFormat("InitGoods({0});", IsPublish.ToString().ToLower());
            Page.ExecuteScript(builder.ToString());
        }

        #region 获取图片
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        public virtual IList<GoodsImageEntity> GetGoodsImages(GoodsEntity goods)
        {
            var infos = new List<GoodsImageEntity>();
            AddGoodsImage(infos, goods);
            return infos;
        }

        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="goods"></param>
        protected virtual void AddGoodsImage(IList<GoodsImageEntity> infos, GoodsEntity goods)
        {
            if (string.IsNullOrEmpty(hfImageValue.Value)) return;
            var arrayList = JsonConvert.DeserializeObject(hfImageValue.Value.Replace(@"\", @"\\")) as JArray;
            if (arrayList == null)
                return;
            var i = 0;
            foreach (JObject item in arrayList)
            {
                i++;
                var info = new GoodsImageEntity { Sequence = i };
                item.Properties().ToList().ForEach(a =>
                {
                    SerializeGoodsImage(info, goods, a.Name.ToString(), a.Value.ToString());
                });

                if (goods.SaveType == SaveType.Add && info.SaveType == SaveType.Remove)
                    continue;
                if (goods.SaveType == SaveType.Modify && info.SaveType == SaveType.None)
                {
                    info.SaveType = SaveType.Modify;
                    info.SetProperty(it => it.Sequence);
                }
                else if (goods.SaveType == SaveType.Modify && info.SaveType == SaveType.Remove)
                {
                    info.SaveType = SaveType.Remove;
                }
                else
                {
                    var index = info.FileName.IndexOf("Files/");
                    info.FileName = info.FileName.Substring(index, info.FileName.Length - index);
                    info.FileByte = Ioc.Resolve<IFileApplicationService>().Grab(info.FileName);
                    info.FileName = string.Format("Files/Images/Goods/{0}", Path.GetFileName(info.FileName));
                }
                infos.Add(info);
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="goods"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected virtual void SerializeGoodsImage(GoodsImageEntity info, GoodsEntity goods, string key, string value)
        {

            if (key.Equals("SaveType"))
            {
                switch (value)
                {
                    case "Add":
                        info.SaveType = SaveType.Add;
                        break;
                    case "Modify":
                        info.SaveType = SaveType.Modify;
                        break;
                    case "Remove":
                        info.SaveType = SaveType.Remove;
                        break;
                }
            }
            else
            {
                var name = key;
                switch (name)
                {
                    case "Value":
                        name = "FileName";
                        break;
                    case "Sku":
                        if (goods.Products != null)
                        {
                            var product = goods.Products.FirstOrDefault(it => it.Sku == value);
                            info.Product = product ?? new ProductEntity { Id = 0 };
                        }
                        return;
                }
                Winner.Creator.Get<IProperty>()
                      .SetValue(info, name, value);
            }

        }

        #endregion

        #region 获取商品SKU产品
        /// <summary>
        /// 得到商品属性
        /// </summary>
        /// <returns></returns>
        public virtual IList<ProductEntity> GetProducts(GoodsEntity goods)
        {
            var infos = new List<ProductEntity>();
            AddProduct(goods, infos);
            if (infos.Count == 0)
            {
                var info = new ProductEntity
                {
                    Id = hfProductId.Value.Convert<long>(),
                    Count = txtCount.Value.Convert<int>(),
                    OrderMinCount = txtOrderMinCount.Value.Convert<int>(),
                    OrderStepCount = txtOrderStepCount.Value.Convert<int>(),
                    DataId = txtDataId.Value,
                    Cost = txtCost.Value.Convert<decimal>(),
                    Price = txtPrice.Value.Convert<decimal>(),
                    Sku = "",
                    IsReturn = ckIsReturn.Checked,
                    IsCustom = ckIsCustom.Checked,
                    DepositRate = txtDespoitRate.Value.Convert<decimal>(),
                    IsSales = ckIsSales.Checked,
                    Name = goods.Name
                };
                infos.Add(info);
            }
            return infos;
        }



        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="infos"></param>
        protected virtual void AddProduct(GoodsEntity goods, IList<ProductEntity> infos)
        {
            if (string.IsNullOrEmpty(hfProductValue.Value)) return;
            var arrayList = hfProductValue.Value.Replace("\\", "\\\\").DeserializeJson<IList<Dictionary<string, object>>>();
            foreach (Dictionary<string, object> dictionary in arrayList)
            {
                var info = new ProductEntity();
                foreach (var o in dictionary)
                {
                    if (SaveType == SaveType.Add && o.Key.Equals("Id")) continue;
                    Winner.Creator.Get<IProperty>().SetValue(info, o.Key, o.Value);
                }
                switch (dictionary["SaveType"].ToString())
                {
                    case "Add":
                        info.SaveType = SaveType.Add;
                        break;
                    case "Modify":
                        info.SaveType = SaveType.Modify;
                        break;
                    case "Remove":
                        info.SaveType = SaveType.Remove;
                        break;
                }
                if (info.SaveType != SaveType.Remove)
                {
                    var skuArray = info.Sku.Replace("\\", "\\\\").DeserializeJson<IList<Dictionary<string, object>>>();
                    var builder = new StringBuilder();
                    foreach (Dictionary<string, object> sku in skuArray)
                    {
                        builder.Append(sku["Value"]);
                        builder.Append(",");
                    }
                    if (builder.Length > 0)
                        builder.Remove(builder.Length - 1, 1);
                    info.Name = string.Format("{0}/{1}", goods.Name, builder);
                }
                infos.Add(info);
            }
        }


        #endregion

        #region 获取商品属性
        /// <summary>
        /// 得到商品属性
        /// </summary>
        /// <returns></returns>
        public virtual IList<GoodsPropertyEntity> GetGoodsProperties(GoodsEntity goods)
        {
            var infos = new List<GoodsPropertyEntity>();
            var ids = new Dictionary<long,object>();
            foreach (var product in goods.Products)
            {
                if(product.SkuJsons==null)
                    continue;
                foreach (var json in product.SkuJsons)
                {
                    infos.Add(new GoodsPropertyEntity
                    {
                        Product = product,
                        Goods = goods,
                        Property = new PropertyEntity {Id = json.Id},
                        Value = json.Value
                    });
                    if (!ids.ContainsKey(json.Id))
                        ids.Add(json.Id, json.Id);
                }
            }
            AddGoodsProperty(infos, goods, ids);
            return infos;
        }


        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="goods"></param>
        /// <param name="ids"></param>
        protected virtual void AddGoodsProperty(IList<GoodsPropertyEntity> infos,GoodsEntity goods, IDictionary<long, object> ids)
        {
            if (string.IsNullOrEmpty(hfPropertyValue.Value)) return;
            var arrayList = hfPropertyValue.Value.Replace("\\", "\\\\").DeserializeJson<IList<IDictionary<string, object>>>();
            foreach (Dictionary<string, object> dictionary in arrayList)
            {
                if (ids.ContainsKey(dictionary["PropertyId"].Convert<long>()))
                    continue;
                var info = new GoodsPropertyEntity { Product = new ProductEntity{Id=0} };
                foreach (var o in dictionary)
                {
                    if (SaveType == SaveType.Add && o.Key.Equals("Id")) continue;
                    var name = o.Key.Equals("PropertyId") ? "Property.Id" : o.Key;
                    Winner.Creator.Get<IProperty>().SetValue(info, name, o.Value);
                }
                infos.Add(info);
            }
        }


        #endregion

        #region 获取详情
        /// <summary>
        /// 得到商品详情
        /// </summary>
        /// <returns></returns>
        public virtual IList<GoodsDetailEntity> GetGoodsDetails(GoodsEntity goods)
        {
            var dataEntity = GoodsDetail();
            var infos = new List<GoodsDetailEntity>();
            var info = new GoodsDetailEntity
            {
                Detail = Editor1.TextArea.Value,
                Name="Detail",
                Attachment = upAttchment.GetFileName(),
                AttachmentByte = upAttchment.GetFileByte(),
                Goods = goods,
                Product = new ProductEntity { Id = 0 }
            };
            if (dataEntity != null)
            {
                info.SaveType = SaveType.Modify;
                info.SetProperty(it => it.Detail);
                info.SetProperty(it => it.Attachment);
                info.Id = dataEntity.Id;
            }
            else
            {
                info.SaveType = SaveType.Add;
            }
            infos.Add(info);
            return infos;
        }
        #endregion

        #region 绑定验证

        /// <summary>
        /// 绑定产品验证
        /// </summary>
        protected virtual void BindProductValidation()
        {
            var validateEntities =
                ScriptValidatorExtension.Initialize<ProductEntity>(
                    new List<string> { "Count", "OrderMinCount", "OrderStepCount", "OrderLimitCount", "Price", "Cost",  "DataId", "DepositRate" }, SaveType);
            var detailValidateEntities =
                ScriptValidatorExtension.Initialize<GoodsDetailEntity>(
                    new List<string> { "Detail" }, SaveType);
            var builder = new StringBuilder();
            builder.AppendFormat("var validateEntities=[{0}];var detailValidateEntities=[{1}];", validateEntities,
                                 detailValidateEntities);
            this.ExecuteScript(builder.ToString());
        }

        #endregion
    }
}
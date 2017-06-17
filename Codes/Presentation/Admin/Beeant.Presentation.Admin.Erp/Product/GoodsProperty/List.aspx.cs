using System.Linq;
using System.Web.UI.WebControls;
using Component.Extension;
using Dependent;
using Beeant.Application.Services;
using Beeant.Domain.Entities.Product;
using Winner.Persistence;
using Winner.Persistence.Linq;


namespace Beeant.Presentation.Admin.Erp.Product.GoodsProperty
{
    public partial class List : Basic.Services.WebForm.Pages.MaintenPageBase<GoodsPropertyEntity>
    {
        public long ProductId
        {
            get { return Request.QueryString["ProductId"].Convert<long>(); }
        }
        public override bool IsShowEditContainer
        {
            get { return true; }
            set
            {
                base.IsShowEditContainer = value;
            }
        }
        protected override void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProperties();
            }
            base.Page_Load(sender, e);
        }
        protected virtual void LoadProperties()
        {
            var query = new QueryInfo();
            query.Query<ProductEntity>().Where(it => it.Id == ProductId).Select(it => it.Goods.Category.Id);
            var infos = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntities<ProductEntity>(query);
            var info = infos == null ? null : infos.FirstOrDefault();
            if (info != null && info.Goods!=null && info.Goods.Category!=null)
            {
                ddlProperty.Query=new QueryInfo();
                ddlProperty.Query.Query<PropertyEntity>().Where(it => it.Category.Id == info.Goods.Category.Id && !it.IsSku && it.IsUsed);
                ddlProperty.LoadData();
            }
        }
        protected override void SetQueryWhere(QueryInfo query)
        {
            query.Query<GoodsPropertyEntity>().Where(it => it.Product.Id == ProductId && !it.Property.IsSku);
            base.SetQueryWhere(query);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="info"></param>
        protected override void Save(GoodsPropertyEntity info)
        {
            info.Product = new ProductEntity {Id = ProductId};
            var product = Ioc.Resolve<IApplicationService, ProductEntity>().GetEntity<ProductEntity>(ProductId);
            info.Goods =product==null?null: product.Goods;
            base.Save(info);
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProperty_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadValues();
 
        }

        protected virtual void LoadValues()
        {
            ddlValue.Items.Clear();
            if (string.IsNullOrEmpty(ddlProperty.DropDownList.SelectedValue))
                return;
            var info =
                Ioc.Resolve<IApplicationService, PropertyEntity>().GetEntity<PropertyEntity>(ddlProperty.DropDownList.SelectedValue.Convert<long>());
            if (info != null && info.SearchValueArray != null)
            {
                foreach (var searchValue in info.SearchValueArray)
                {
                    ddlValue.Items.Add(new ListItem(searchValue, searchValue));
                }
            }
        }
    }
}
using System.Web.UI.WebControls;
using Beeant.Domain.Entities.Product;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities.Account;
using Component.Extension;

namespace Beeant.Presentation.Admin.Erp.Product.Inquery
{
    public partial class Add : AddPageBase<InqueryEntity>
    {
        public override Button SaveButton
        {
            get { return btnSave; }
            set { base.SaveButton = value; }
        }

        /// <summary>
        /// 填充实体
        /// </summary>
        /// <returns></returns>
        protected override InqueryEntity FillEntity()
        {
            var entity= base.FillEntity();
            if (entity == null)
                return null;
            entity.IsReply = true;
            entity.IsShow = true;
            entity.Goods = new GoodsEntity
            {
                Id = Request["goodsId"].Convert<long>()
            };
            entity.Account=new AccountEntity();
            entity.Tag = "";
            return entity;
        }
    }
}
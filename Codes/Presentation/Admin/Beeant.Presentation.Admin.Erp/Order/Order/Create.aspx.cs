using System;
using System.Collections.Generic;
using System.Linq;
using Component.Extension;
using Dependent;
using Beeant.Application.Dtos.Order;
using Beeant.Application.Services;
using Beeant.Application.Services.Order;
using Beeant.Domain.Entities.Account;
using Beeant.Domain.Entities.Agent;
using Beeant.Domain.Entities.Member;
using Beeant.Basic.Services.WebForm.Extension;
using Beeant.Basic.Services.WebForm.Pages;
using Beeant.Domain.Entities;
using Beeant.Domain.Entities.Order;
using Winner.Filter;
using Winner.Persistence;
using Winner.Persistence.Linq;
using InvoiceEntity = Beeant.Domain.Entities.Member.InvoiceEntity;

namespace Beeant.Presentation.Admin.Erp.Order.Order
{
    public partial class Create : AuthorizePageBase
    {
        

        protected void btnChangeAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
        /// <summary>
        /// 加载账户
        /// </summary>
        protected virtual void LoadAccount()
        {
            if (string.IsNullOrEmpty(cbAccount.InputHidden.Value))
                return;
            var account =
                Ioc.Resolve<IApplicationService, AccountEntity>().GetEntity<AccountEntity>(cbAccount.InputHidden.Value.Convert<long>());
            if (account != null)
            {
                this.BindEntity(account, null, ValidationType.Modify);
                var agent = GetAgent(account.Id);
                if (agent != null)
                {
                    lblAgentName.Text = agent.Name;
                }
                LoadAddresses(account.Id);
                LoadInvoices(account.Id);
                LoadSettlements(account.Id);
            }
        }
        /// <summary>
        /// 得到代理
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        protected virtual AgentEntity GetAgent(long accountId)
        {
            var query = new QueryInfo();
            query.Query<AgentEntity>()
                 .Where(it => it.IsUsed && it.Account.Id == accountId)
                 .Select(it => new object[] { it, it.Name });
            var infos = Ioc.Resolve<IApplicationService, AgentEntity>().GetEntities<AgentEntity>(query);
            return infos == null ? null : infos.FirstOrDefault();
        }
        /// <summary>
        /// 加载发票
        /// </summary>
        /// <param name="accountId"></param>
        protected virtual void LoadInvoices(long accountId)
        {
            var query = new QueryInfo();
            query.Query<InvoiceEntity>().Where(it => it.Account.Id == accountId).OrderByDescending(it=>it.Id).Select(it => 
                new object[] { it.Id, it.Title, it.Content,  it.Type, it.GeneralType, it.Recipient, it.Mobile, it.Address });
            var infos = Ioc.Resolve<IApplicationService, InvoiceEntity>().GetEntities<InvoiceEntity>(query);
            gvInvoice.DataSource = infos;
            gvInvoice.DataBind();
        }
        /// <summary>
        /// 加载地址
        /// </summary>
        /// <param name="accountId"></param>
        protected virtual void LoadAddresses(long accountId)
        {
            var query = new QueryInfo();
            query.Query<AddressEntity>()
                 .Where(it => it.Account.Id == accountId).OrderByDescending(it=>it.Id)
                 .Select(
                     it =>
                     new object[]
                         {
                             it.Id, it.Province, it.City, it.County, it.Company,
                             it.Recipient, it.Mobile, it.Address, it.Tag, it.Email, it.Telephone, it.IsDefault
                         });
            var infos = Ioc.Resolve<IApplicationService, AddressEntity>().GetEntities<AddressEntity>(query);
            gvAddress.DataSource = infos;
            gvAddress.DataBind();
        }

        /// <summary>
        /// 加载产品
        /// </summary>
        /// <param name="accountId"></param>
        protected virtual void LoadSettlements(long accountId)
        {
            var dto = GetSettlementDto(accountId);
            dto = Ioc.Resolve<IOrderApplicationService>().Create(dto);
            if (dto != null)
            {
                if (dto.Products != null)
                {
                    gvProduct.DataSource = dto.Products;
                }
                LoadSummary(dto);
            }
            gvProduct.DataBind();
    
        }
        /// <summary>
        /// 下单
        /// </summary>
        /// <returns></returns>
        protected virtual SettlementDto GetSettlementDto(long accountId)
        {
            var dto = new SettlementDto
            {
                ChannelType = ChannelType.Admin,
                OrderType=OrderType.Standard,
                AddressId = Request.Form["Address"].Convert<long>(),
                InvoiceId = Request.Form["Invoice"].Convert<long>(),
                AccountId = accountId,
                Products =
                    string.IsNullOrEmpty(hfProducts.Value)
                        ? null
                        : Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderProductDto>>(hfProducts.Value)
            };
            return dto;
        }

        /// <summary>
        /// 加载汇总
        /// </summary>
        protected virtual void LoadSummary(SettlementDto dto)
        {
            var kindCount = dto.Products == null ? 0 : dto.Products.Count;
            var totalCount = dto.Products == null ? 0 : dto.Products.Sum(it => it.Count);
            lblKindCount.Text = kindCount.ToString();
            lblTotalCount.Text = totalCount.ToString();
            lblFeightPrice.Text = dto.Orders==null?"": dto.Orders.Sum(it=>it.FreightPrice).ToString();
            lblFactPrice.Text = dto.Orders == null ? "" : dto.Orders.Sum(it => it.FactPrice).ToString();
            lblProductPrice.Text = dto.Orders == null ? "" : dto.Orders.Sum(it => it.ProductPrice).ToString();
        }
    
 
        /// <summary>
        /// 呈现
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ExecuteScript("Init();");
        }

        /// <summary>
        /// 保存订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var dto = GetSettlementDto(cbAccount.InputHidden.Value.Convert<long>());
            dto.IsGenerate = true;
            Ioc.Resolve<IOrderApplicationService>().Create(dto);
            Message1.ShowMessage(dto.Errors);
        }
    }
}
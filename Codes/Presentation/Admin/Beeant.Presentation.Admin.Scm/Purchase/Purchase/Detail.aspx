<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.Purchase.Detail" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Wms" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc2" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>采购单详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
 <a href='add.aspx?id=<%=RequestId%>'  name="Add">新增</a>
   <a href='update.aspx?id=<%=RequestId%>'  name="Edit">编辑</a>
   <a href='handle.aspx?id=<%=RequestId%>'  name="Handle">处理</a>
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
     
         <tr>
               <td class="font">采购单编号</td>
            <td class="text" >
                  <asp:Label ID="lblId" runat="server" Text=""  BindName="Id"></asp:Label>
             </td>
             <td class="font">跟单人</td>
            <td class="text">
                <asp:Label ID="lblFollowRealName" runat="server" Text=""  BindName="Follow.RealName"></asp:Label>
            </td>
         </tr>
 
         <tr>
            <td class="font">采购日期</td>
               <td class="text"  >
                <asp:Label ID="lblPurchaseDate" runat="server" BindName="PurchaseDate"></asp:Label>
             </td>
             <td class="font">交货日期</td>
               <td class="text"  >
                <asp:Label ID="lblDeliveryDate" runat="server" BindName="DeliveryDate"></asp:Label>
             </td>
             
        </tr>
             <tr>
            <td class="font">金额</td>
               <td class="text"  >
                <asp:Label ID="lblItemAmount" runat="server" BindName="ItemAmount"></asp:Label>
             </td>
             <td class="font">实付金额</td>
               <td class="text"  >
                <asp:Label ID="lblPayAmount" runat="server" BindName="PayAmount"></asp:Label>
             </td>
        </tr>
                  <tr>
            <td class="font">运费</td>
               <td class="text"  >
                <asp:Label ID="lblExpressAmount" runat="server" BindName="ExpressAmount"></asp:Label>
             </td>
            
        </tr>
         <tr>
            <td class="font">账户</td>
            <td class="text">
                
                <a href="<%=this.GetUrl("PresentationAdminErpUrl") %>/Account/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" BindName="Account.Id"> <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label></a>
               
            </td>
              <td class="font">订单</td>
            <td class="text">

                <span style='<%=OrderId.ToString()=="0" ? "display:none" :" " %>'>
                <a href="<%=this.GetUrl("PresentationAdminErpUrl") %>/Order/Order/Detail.aspx?Id=" id="hfOrderId" runat="server" BindName="Order.Id"><asp:Label ID="lblOrderId" runat="server" Text=""  BindName="Order.Id"></asp:Label></a>
                </span>
            </td>
        </tr>
         <tr>
         
         <td class="font">
                 仓库</td>
             <td class="text">
                 <asp:Label ID="selStoreId" runat="server" BindName="Storehouse.Name" Text=""></asp:Label>
             </td>
             <td class="font">
                 采购类型</td>
             <td class="text" colspan="3">
                 <asp:Label ID="lblType" runat="server" BindName="TypeName"></asp:Label>
             </td>
         <tr>
             <td class="font">
                 状态</td>
             <td class="text">
                 <asp:Label ID="lblStatusName" runat="server" BindName="StatusName" Text=""></asp:Label>
             </td>
             <td class="font">
                 状态更新时间</td>
             <td class="text">
                 <asp:Label ID="lblStatusTime" runat="server" BindName="StatusTime" Text=""></asp:Label>
             </td>
         </tr>
         <tr>
             <td class="font">
                 级别</td>
             <td class="text">
                 <asp:Label ID="lblLevel" runat="server" BindName="Level" Text=""></asp:Label>
             </td>
             <td class="font">
                 所属人</td>
             <td class="text">
                 <asp:Label ID="lblUserRealName" runat="server" BindName="User.RealName" Text=""></asp:Label>
             </td>
         </tr>
         <tr>
             <td class="font">
                 提交人</td>
             <td class="text">
                 <asp:Label ID="lblSubmitRealName" runat="server" BindName="Submit.RealName" 
                     Text=""></asp:Label>
             </td>
             <td class="font">
                 提交时间</td>
             <td class="text">
                 <asp:Label ID="lblInsertTime" runat="server" BindName="InsertTime" Text=""></asp:Label>
             </td>
         </tr>
          <tr>
              
             <td class="font">
                 开票金额</td>
             <td class="text" colspan="3">
                 <asp:Label ID="lblInvoiceAmount" runat="server" BindName="InvoiceAmount" Text=""></asp:Label>
             </td>
         </tr>
         <tr>
             <td class="font">
                 备注</td>
             <td class="mtext" colspan="3">
                 <asp:Label ID="lblRemark" runat="server" BindName="Remark" Text=""></asp:Label>
             </td>
         </tr>
         <tr>
             <td class="center" colspan="4">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
   
    <div class="subtitle" onclick="SetEntityBody('divHistory')">流程详情记录(<span class="count"><%=pgHistory.DataCount%></span>)</div>
       <div id="divHistory" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="text"><asp:TextBox ID="txtBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="text"><asp:TextBox ID="txtEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<==@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
              <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td >
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>

      <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
        <asp:TemplateField HeaderText="步骤"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                第<%#pgHistory.DataCount-pgHistory.PageIndex*pgHistory.PageSize-Index%>步
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="级别"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("LevelName")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="转发人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ToUser.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="当前操作人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("HandleUser.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="业务信息"  ItemStyle-CssClass="center">
            <ItemTemplate>
            <a href="javascript:void(0);" onclick="SetEntityBody('divHistory<%#Index%>')">业务信息</a> 
            <div id='divHistory<%#Index++%>' style="display: none;">
                <%#Eval("WebDataEntity")%>
            </div>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>


     <uc1:Pager ID="pgHistory" runat="server" PageSize="10"  SelectExp="Id,StatusName,DataEntity,ToUser.RealName,HandleUser.RealName,LevelName,Remark,InsertTime" FromExp="HistoryEntity" OrderByExp="UpdateTime desc" WhereExp="DataId==@Id && FlowId==@FlowId" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>

        <div class="subtitle" onclick="SetEntityBody('divPurchaseItem')">采购明细信息(<span class="count"><%=pgPurchaseItem.DataCount%></span>)</div>
       <div id="divPurchaseItem" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
           <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="text"><asp:TextBox ID="txtPurchaseItemBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtPurchaseItemBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="text"><asp:TextBox ID="txtPurchaseItemEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<==@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
              <cc1:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtPurchaseItemEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            
            <td >
                <asp:Button ID="Button3" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
      <asp:GridView ID="gvPurchaseItem" runat="server" AutoGenerateColumns="False" CssClass="table"  onrowdatabound="GridView1_RowDataBound" >
       <Columns>
         
         <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <input id="hfSelect" runat="server" type="hidden" value='<%#Eval("Product.Id") %>' IsWarning='<%#Eval("Product.Cost").Convert<decimal>()<Eval("Price").Convert<decimal>() %>'/>
              <a href='/Product/Product/Detail.aspx?id=<%#Eval("Product.Id") %>'> <%#Eval("Name")%> </a> 
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="单价"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                实际：<%#Eval("Price")%>/当前：<%#Eval("Product.CurrentCost")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="数量"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Count")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
         
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
              <asp:TemplateField HeaderText="类目"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Product.Goods.Category.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
     <asp:TemplateField HeaderText="面价"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Product.Price")%> 
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="毛利率"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "Product.CostRate", "{0:N2}%")%>
            </ItemTemplate>
        </asp:TemplateField>
         
        

          <asp:TemplateField HeaderText="报退" ItemStyle-CssClass="center time">
                                           <HeaderTemplate>
                                                 <a id="btnReturn" href='javascript:void(0)'>报退</a>
                                                   <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
                                            </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="<%#Eval("Product.Id")%>" value='<%#Eval("Product.Id")%>' name="product" type="checkbox" style='width: 50px;<%#Eval("Product.Id").Convert<long>()>0?"":"display:none" %>' SubCheckName="selectall"   />
                                        </ItemTemplate>
          </asp:TemplateField>
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgPurchaseItem" runat="server" PageSize="10"  
     SelectExp="Name,Price,Count,User.RealName,Amount,Remark,Product.Id,Product.Goods.Category.Name,Product.Price,Product.Goods.Category.Rate,Product.IsSales" 
     FromExp="Beeant.Domain.Entities.Purchase.PurchaseItemEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />

        </ContentTemplate>
        </asp:UpdatePanel>
        <script type="text/javascript" src="/scripts/Winner/CheckBox/Winner.CheckBox.js"></script>
                    <script type="text/javascript">
                        function Return() {
                            var checkbox = new Winner.CheckBox('<%=gvPurchaseItem.ClientID %>', { StyleFile: null });
                            checkbox.Initialize();
                            $("#btnReturn").click(function () {
                                debugger;
                                var url = '/Purchase/Purchase/Add.aspx?OriginalPurchaseId=<%=RequestId %>';
                                var productIds = [];
                                $("#<%=gvPurchaseItem.ClientID %>").find("input[name='product']").each(function (index, sender) {
                                    if (sender.style.display == "none")
                                        return;
                                    if ($(sender).attr("checked")) {
                                        var productid = parseInt($(sender).val());
                                        productIds.push(productid);
                                    }
                                });
                                if (productIds.length > 0) {
                                    url += "&productids=" + productIds.join(",");
                                    window.location.href = url;
                                    return;
                                }
                                alert("请选择报退产品");
                            });
                        }
                    </script>
      </div>

  <div class="subtitle" onclick="SetEntityBody('divPay')">付款核销信息(<span class="count"><%=pgPay.DataCount%></span>)</div>
       <div id="divPay" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
           <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="text"><asp:TextBox ID="txtPayBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtPayBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="text"><asp:TextBox ID="txtPayEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<==@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
              <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtPayEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            
            <td >
                <asp:Button ID="Button2" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
      <asp:GridView ID="gvPay" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
           <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsStatusName")%>  
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="付款单据"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
               <span style='<%#Eval("Payout.Id").ToString()=="0" ? "display:none":"" %>' >
               <a href='/Finance/Payout/Detail.aspx?Id=<%#Eval("Payout.Id") %>'> <%#Eval("Payout.Id")%></a>
               </span>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
  
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgPay" runat="server" PageSize="10"  
     SelectExp="Amount,Remark,User.RealName,Payout.Id,InsertTime,UpdateTime,IsStatus" 
     FromExp="Beeant.Domain.Entities.Purchase.PayEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />

        </ContentTemplate>
        </asp:UpdatePanel>
      </div>
 
 
  <div class="subtitle" onclick="SetEntityBody('divInvoice')">付款发票信息(<span class="count"><%=pgInvoice.DataCount%></span>)</div>
       <div id="divInvoice" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
      
      <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
           <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsStatusName")%>  
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="进项发票"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
               <a href='/Finance/Invoicein/Detail.aspx?Id=<%#Eval("Invoicein.Id") %>'> <%#Eval("Invoicein.Id")%></a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
  
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgInvoice" runat="server" PageSize="10"  
     SelectExp="Amount,Remark,User.RealName,Invoicein.Id,InsertTime,UpdateTime,IsStatus" 
     FromExp="Beeant.Domain.Entities.Purchase.InvoiceEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />

        </ContentTemplate>
        </asp:UpdatePanel>
      </div>
 
       
           <div class="subtitle" onclick="SetEntityBody('divAttachment')">采购附件信息(<span class="count"><%=pgAttachment.DataCount%></span>)</div>
       <div id="divAttachment" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
           <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="text"><asp:TextBox ID="txtAttachmentBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender15" runat="server" TargetControlID="txtAttachmentBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="text"><asp:TextBox ID="txtAttachmentEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<==@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
              <cc1:CalendarExtender ID="CalendarExtender16" runat="server" TargetControlID="txtAttachmentEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            
            <td >
                <asp:Button ID="Button7" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
      <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
        <asp:TemplateField HeaderText="操作人真实姓名"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="附件名称(标题)"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="附件"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                  <a href="<%#Eval("DownFileName") %>" target="_blank"> 下载</a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
         
  
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgAttachment" runat="server" PageSize="10"  
     SelectExp="User.RealName,Name,FileName,InsertTime,UpdateTime" 
     FromExp="Beeant.Domain.Entities.Purchase.AttachmentEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />

        </ContentTemplate>
        </asp:UpdatePanel>
      </div>


       <div class="subtitle" onclick="SetEntityBody('divStock')">进出单列表(<span class="count"><%=pgStock.DataCount%></span>)</div>
       <div id="divStock" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="mtext"><asp:TextBox ID="txtStockBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtStockBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="mtext"><asp:TextBox ID="txtStockEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtStockEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">类型</td>
            <td class="mtext">
                <uc2:GeneralDropDownList ID="ddlStockType" runat="server" SearchWhere="Type==@Type" SearchPropertyTypeName="Type" SearchParamterName="Type" ObjectName="Beeant.Domain.Entities.Wms.StockType" IsEnum="True" />
            </td>
            <td >
                <asp:Button ID="Button1" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
   
           <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
               <a href='/Wms/Stock/Detail.aspx?id=<%#Eval("Id") %>' target="_blank"><%#Eval("Id")%></a> 
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="类型"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="时间"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("UpdateTime")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="相关订单"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <span style='<%#Eval("Order")!=null && Eval("Order.Id").ToString()=="0" ? "display:none":"" %>' >
               <a href='<%=this.GetUrl("PresentationAdminErpUrl") %>/Order/Order/Detail.aspx?id=<%#Eval("Order.Id") %>' target="_blank">  <%#Eval("Order.Id")%></a>
               </span>
            </ItemTemplate>
        </asp:TemplateField>
                
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgStock" runat="server" PageSize="10"  
     SelectExp="Id,User.RealName,Type,UpdateTime,Order.Id,Status"
      FromExp="Beeant.Domain.Entities.Wms.StockEntity,Beeant.Domain.Entities"
      OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>   
     
     
      <div class="subtitle" onclick="SetEntityBody('divExpress')">订单快递信息(<span class="count"><%=pgExpress.DataCount%></span>)</div>
       <div id="divExpress" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
      
      <asp:GridView ID="gvExpress" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
          <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="left Sequence xlstext">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="快递公司"  ItemStyle-CssClass="left Sequence xlstext">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="快递单号"  ItemStyle-CssClass="left Sequence xlstext">
            <ItemTemplate>
                <%#Eval("Number")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="接收人"  ItemStyle-CssClass="left Sequence xlstext">
            <ItemTemplate>
                <%#Eval("Recipient")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="手机号码"  ItemStyle-CssClass="left Sequence xlstext">
            <ItemTemplate>
                <%#Eval("Mobile")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="邮政编码"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Postcode")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="地址"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Address")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgExpress" runat="server" PageSize="10"  
     SelectExp="Id,Name,Number,Recipient,Mobile,Postcode,Address,Remark,User.RealName,InsertTime,UpdateTime,Amount" 
     FromExp="Beeant.Domain.Entities.Purchase.ExpressEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />

        </ContentTemplate>
        </asp:UpdatePanel>
      </div>

     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
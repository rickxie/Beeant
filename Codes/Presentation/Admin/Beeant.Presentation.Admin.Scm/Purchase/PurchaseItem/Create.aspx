<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.PurchaseItem.Create" MasterPageFile="~/Main.Master" ValidateRequest="false" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Wms" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
 
 <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>采购单明细录入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<div class="edit">
<table class="tb">
           <tr>
                <td colspan="4" class="center">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
                    <input id="btnClose" type="button" value="关闭" class="btn" />
                </td>
            </tr>
        </table>
            <uc2:Message ID="Message1" runat="server" />

     
    </div>
         <div id="divProduct" runat="server">

      <div   class="search" >
               <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
    <tr>
         <td class="font">状态</td>
              <td class="text"   >
                <uc10:GeneralDropDownList ID="ddlSearchStatus" runat="server" ObjectName="Beeant.Domain.Entities.Product.ProductStatus" IsEnum="True" SearchPropertyTypeName="Status" SearchWhere="Status==@Status" SearchParamterName="Status"  />
             </td>
              <td class="font">品牌</td>
            <td class="text">
                  <asp:TextBox ID="txtBrand" runat="server"  SearchWhere="Brand==@Brand" SearchParamterName="Brand"  SearchPropertyTypeName="Brand"  CssClass="seinput"></asp:TextBox>
            </td>
               <td class="font">是否直送</td>
            <td class="text">
               <asp:DropDownList ID="ddlIsDirectDelivery" runat="server"  SearchWhere="IsDirectDelivery==@IsDirectDelivery" SearchParamterName="IsDirectDelivery"  SearchPropertyTypeName="IsDirectDelivery">
                     <asp:ListItem  Value="True"  Text="是" ></asp:ListItem>  
                     <asp:ListItem  Value="False" Text="否"></asp:ListItem>
                  </asp:DropDownList>
            </td>
               <td class="font">是否上架</td>
            <td class="text">
                   <asp:DropDownList ID="ddlIsOnlineSales" runat="server"  SearchWhere="SalesStatus==@SalesStatus" SearchParamterName="SalesStatus"  SearchPropertyTypeName="SalesStatus">
                     <asp:ListItem  Value="Normal"  Text="是" ></asp:ListItem>  
                     <asp:ListItem  Value="UnSales" Text="否"></asp:ListItem>
                  </asp:DropDownList>
            </td>
    </tr>
         <tr>
            <td class="font">处理开始时间</td>
            <td class="text"><asp:TextBox ID="txtBeginStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime>==@BeginStatusTime" SearchParamterName="BeginStatusTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBeginStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">处理新结束时间</td>
            <td  class="text" ><asp:TextBox ID="txtEndStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime<==@EndStatusTime" SearchParamterName="EndStatusTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtEndStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">
                            编号 
                        </td>
                        <td class="text" >
                                <asp:TextBox ID="txtId" runat="server" CssClass="seinput" SearchParamterName="Id" SearchWhere="Id==@Id" SearchPropertyName="Id"></asp:TextBox>
                        </td>
                        <td colspan="2">
                             <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                        </td>
        </tr>
     </table>
        </div>
        <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
       <Columns>
           <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove, Price"  />
           </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <a href='/Product/Product/Detail.aspx?id=<%#Eval("Id") %>' target="_blank"><%#Eval("Name")%></a> <input id="hfName" runat="server" type="hidden" class="input" value='<%#Eval("Name")%>' />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类目"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Goods.Category.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
     <asp:TemplateField HeaderText="面价/毛利率"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Price")%> / <%# DataBinder.Eval(Container.DataItem, "PriceRate", "{0:N2}%")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="售价/毛利率"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("CurrentCost")%> / <%# DataBinder.Eval(Container.DataItem, "CostRate", "{0:N2}%")%>
            </ItemTemplate>
        </asp:TemplateField>
         
        
        <asp:TemplateField HeaderText="平均进价"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%# Eval("IsDirectDelivery").Convert<bool>() ? "0" : Eval("AverageCost") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="毛利率警戒线"  ItemStyle-CssClass="left status" ItemStyle-Width="100" >
            <ItemTemplate>               
                <%# DataBinder.Eval(Container.DataItem, "Category.Rate", "{0:N2}%")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="品牌"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Brand")%>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="最近采购价"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("LastCost")%>
            </ItemTemplate>
        </asp:TemplateField> 
             <asp:TemplateField HeaderText="最低采购价"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("MinCost")%>
            </ItemTemplate>
        </asp:TemplateField> 
             <asp:TemplateField HeaderText="最近采购时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("LastPurchaseTime", "{0:yyyy-MM-dd}")%>
            </ItemTemplate>
        </asp:TemplateField> 
             <asp:TemplateField HeaderText="产品状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="库存"  ItemStyle-CssClass="left">
            <ItemTemplate>
                  <%#GetInventoryCount(Eval("Inventories").Convert<InventoryEntity[]>())%>          
            </ItemTemplate>
        </asp:TemplateField> 
            <asp:TemplateField HeaderText="库存预警"  ItemStyle-CssClass="left">
            <ItemTemplate>
                  <%#GetInventoryWarningCount(Eval("Inventories").Convert<InventoryEntity[]>())%>   
            </ItemTemplate>
        </asp:TemplateField> 
            <asp:TemplateField HeaderText="在途库存"  ItemStyle-CssClass="left">
            <ItemTemplate>
                    <%#GetInventoryTransitCount(Eval("Inventories").Convert<InventoryEntity[]>())%>       
            </ItemTemplate>
        </asp:TemplateField> 
         <asp:TemplateField HeaderText="占用库存"  ItemStyle-CssClass="left">
            <ItemTemplate>
                    <%#GetInventoryLockCount(Eval("Inventories").Convert<InventoryEntity[]>())%>       
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="近7天销量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Get7CostSumCount(Eval("Id").Convert<long>()) %>
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="近30天销量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Get30CostSumCount(Eval("Id").Convert<long>()) %>
            </ItemTemplate>
        </asp:TemplateField> 
                <asp:TemplateField HeaderText="进价"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%#Eval("Cost") %>' id="txtCost" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="采购数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='1' id="txtCount" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
                              
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgProduct" runat="server" PageSize="20" FromExp="Beeant.Domain.Entities.Product.ProductEntity,Beeant.Domain.Entities"  
     SelectExp="Id,Name,Goods.Category.Name,Price,Category.Rate,CurrentCost,IsDirectDelivery,SalesStatus,AverageCost,Brand,LastCost,MinCost,LastPurchaseTime,Status,Cost" OrderByExp="Id desc"   />  
     </div>

    
       </ContentTemplate>
</asp:UpdatePanel>
 
 </asp:Content>
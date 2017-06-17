<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Storehouse.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>仓库详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">名称</td>
            <td class="text"  >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
              <td class="font">排序</td>
            <td class="text"  >
                <asp:Label ID="lblSort" runat="server"  BindName="Sequence"></asp:Label>
             </td>
        </tr>
      
        <tr>
            <td class="font">是否启用</td>
               <td class="text"  colspan="3" >
                <asp:Label ID="lblIsUsed" runat="server" BindName="IsUsedName"></asp:Label>
             </td>
            
        </tr>
           
         <tr>
            <td class="font">备注</td>
            <td class="mtext" colspan="3"  >
                <asp:Label ID="lblRemark" runat="server"  BindName="Remark"></asp:Label>
             </td>
        </tr>
        
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
     
 <div class="subtitle" onclick="SetEntityBody('divInventory')">库存清单列表(<span class="count"><%=pgInventory.DataCount%></span>)</div>
       <div id="divInventory" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="mtext"><asp:TextBox ID="txtInventoryBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtInventoryBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="mtext"><asp:TextBox ID="txtInventoryEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtInventoryEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>

             <td class="font">商品编号</td>
            <td class="mtext">
            <asp:TextBox ID="txtProductId" runat="server" CssClass="seinput"  SearchWhere="Product.Id==@ProductId" SearchParamterName="ProductId"></asp:TextBox>
            </td>
            <td >
                <asp:Button ID="Button1" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
   
         <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
         <asp:TemplateField HeaderText="商品编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
            <%#Eval("Product.Id")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="仓库"  ItemStyle-CssClass="left status">
            <ItemTemplate>
               <a href='/Wms/Storehouse/Detail.aspx?id=<%#Eval("Storehouse.Id") %>' target="_blank">  <%#Eval("Storehouse.Name")%></a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="实际库存"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Count")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="锁定库存"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("LockCount")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="可用库存"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("EnableCount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="在途库存"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("TransitCount")%>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="警戒库存"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("WarningCount")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="发货周期"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Recycle")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="打包数量"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("PackCount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="录入时间"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("InsertTime")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>
     
         <uc1:Pager ID="pgInventory" runat="server" PageSize="10"  
         SelectExp="Product.Id,Storehouse.Id,Storehouse.Name,Count,LockCount,TransitCount,WarningCount,Recycle,Product.PackCount,Product.PackUnit,Product.Goods.Unit,InsertTime"
          FromExp="Beeant.Domain.Entities.Wms.InventoryEntity,Beeant.Domain.Entities"
          OrderByExp="UpdateTime desc" WhereExp="Storehouse.Id==@Id"/>
        </ContentTemplate>
        </asp:UpdatePanel>
         </div>   
         

 <div class="subtitle" onclick="SetEntityBody('divStockItem')">进出单列表(<span class="count"><%=pgStockItem.DataCount%></span>)</div>
       <div id="divStockItem" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="mtext"><asp:TextBox ID="txtStockBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStockBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="mtext"><asp:TextBox ID="txtStockEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtStockEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            
            <td >
                <asp:Button ID="Button3" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
   
           <asp:GridView ID="gvStockItem" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
               <a href='/Wms/Stock/Detail.aspx?id=<%#Eval("Stock.Id") %>' target="_blank"><%#Eval("Stock.Id")%></a> 
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Stock.StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>

            <asp:TemplateField HeaderText="数量"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Count")%>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="录入时间"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("InsertTime")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgStockItem" runat="server" PageSize="10"  
     SelectExp="Id,Name,Count,User.RealName,Stock.Id,Stock.Status,InsertTime"
      FromExp="Beeant.Domain.Entities.Wms.StockItemEntity,Beeant.Domain.Entities"
      OrderByExp="UpdateTime desc" WhereExp="Storehouse.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>   

           <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
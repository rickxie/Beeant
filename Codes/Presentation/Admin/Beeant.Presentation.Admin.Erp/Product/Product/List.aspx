<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Product.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Wms" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>  
<%@ Register TagPrefix="uc8" TagName="SupplierComboBox" src="~/Controls/Supplier/SupplierComBox.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>产品列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
  
        <tr>
           <td class="font">
                    类目 
                </td>
                <td class="text">
                    <asp:TextBox ID="txtCategory" runat="server"  SearchWhere="Goods.Category.Name==@CategoryName" SearchParamterName="Goods.CategoryName" CssClass="seinput"></asp:TextBox>
                </td>
                <td class="font">产品名称</td>
                <td class="text">
                    <asp:TextBox ID="txtName" runat="server"  SearchWhere="Name.Contains(@Name)" SearchParamterName="Name" CssClass="seinput"></asp:TextBox>
                </td>
                <td class="font">产品编号</td>
                <td class="text">
                    <asp:TextBox ID="txtId" runat="server"  SearchWhere="Id==@Id" SearchParamterName="Id"  SearchPropertyTypeName="Id"  CssClass="seinput"></asp:TextBox>
                </td>
                  <td class="font">销售状态</td>
            <td class="text">
              <asp:DropDownList ID="ddlIsSales" runat="server"  SearchWhere="IsSales==@IsSales" SearchParamterName="IsSales"  SearchPropertyTypeName="IsSales">
                     <asp:ListItem  Value="True"  Text="是" ></asp:ListItem>  
                     <asp:ListItem  Value="False" Text="否"></asp:ListItem>
                  </asp:DropDownList>
            </td>
        </tr>
       
    
         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem Value="Id" Text="编号" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Name" Text="名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Goods.Category.Name" Text="类目" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Model" Text="型号"  ></asp:ListItem>
                     <asp:ListItem  Value="Price" Text="面价/毛利率"  Selected="True"  Enabled="False"></asp:ListItem>
                     <asp:ListItem  Value="Cost" Text="进价"  Selected="True"  Enabled="False"></asp:ListItem>
                     <asp:ListItem  Value="DepositRate" Text="定金比率"  Selected="True"></asp:ListItem>
                          <asp:ListItem  Value="IsReturn" Text="是否支持退换"  Selected="True"></asp:ListItem>      
                       <asp:ListItem  Value="IsCustom" Text="是否支持定制"  Selected="True"></asp:ListItem>       
                    <asp:ListItem  Value="IsSales" Text="销售状态"  Selected="True"></asp:ListItem>      
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem Value="Id" Text="编号" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Price" Text="面价"  ></asp:ListItem>
                     <asp:ListItem  Value="Cost" Text="进价"  ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间"  ></asp:ListItem>
                </asp:DropDownList>
            </td>
          <td class="font">
                排序方式
            </td>
            <td >
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
               
            </td>
            <td colspan="4">
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten"  >
            <table>
            <tr>
                <td>
                    <a href="Add.aspx" name="Add" target="_blank"class="btn" style='<%=Request.QueryString["Type"] == "1"?"":"display:none;"%>' >添加</a>
                </td>
                <td>
                   <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button> 
                </td>
               
                <td>
                     <asp:Button ID="btnUnales" runat="server" Text="下架" CssClass="btn mbtn" onclick="btnUnSales_Click" ConfirmBox="UnSales" ConfirmMessage="您确定要改成下架状态吗" ComfirmCheckBoxMessage="你没有选择任何行"/>
                </td>
                   <td>
                     <asp:Button ID="btnSales" runat="server" Text="下架" CssClass="btn mbtn" onclick="btnSales_Click" ConfirmBox="Sales" ConfirmMessage="您确定要改成上架状态吗" ComfirmCheckBoxMessage="你没有选择任何行"/>
                </td>
               
          </tr>
          </table>
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CssClass="table" DataKeyNames="Id,IsSales" 
                onrowdatabound="GridView1_RowDataBound" >
       <Columns>
         <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox"  CategoryId='<%#Eval("Goods.Category.Id") %>' SubCheckName="selectall" ComfirmValidate="Remove,UnSales,Sales" IsWarning='<%#IsWarning(Eval("Inventories").Convert<InventoryEntity[]>())%>'  />
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="面价" ItemStyle-CssClass="center ckbox" >
            <ItemTemplate>
            <input value='<%#Eval("Price") %>' id='txtPrice' runat='server' type='text' style='width: 60px;'  class='input'  />
           </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="底价" ItemStyle-CssClass="center ckbox" >
            <ItemTemplate>
                 <input value='<%#Eval("Cost") %>' id='txtCost' runat='server' type='text' style='width: 60px;'  class='input'  />
           </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Entity">详情</a>
                &nbsp;<a href='<%#string.Format("{0}/{1}",this.GetUrl("PresentationMvcDetailUrl"),Eval("Id").Convert<long>()) %>' target="_blank" name="Entity">预览</a>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target='_blank' name='Edit'>编辑</a>
                <a href="Add.aspx?Id=<%#Eval("Id") %>" target="_blank">复制</a>
            </ItemTemplate>
        </asp:TemplateField>
     
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>

            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类目"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Goods.Category.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
     
  
        
        <asp:TemplateField HeaderText="毛利率"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "PriceRate", "{0:N2}%")%>
            </ItemTemplate>
        </asp:TemplateField>
        
               <asp:TemplateField HeaderText="定金比率"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("DepositRate")%>
            </ItemTemplate>
        </asp:TemplateField> 
        
            
             <asp:TemplateField HeaderText="是否支持退换"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsReturnName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否支持定制"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsCustomName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="销售状态"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsSalesName")%>
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
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,IsSales,Goods.Category.Id" OrderByExp="Id desc"  />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>


 </asp:Content>
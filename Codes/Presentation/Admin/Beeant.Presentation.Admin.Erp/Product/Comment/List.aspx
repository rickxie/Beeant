<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Comment.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>  
  <%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>评价管理列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
               
        <tr>
            <td class="font">
                账户
            </td>
            <td class="text">
              <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId"  />
            </td>
             <td class="font">
                订单编号
            </td>
            <td class="text" >
                <asp:TextBox ID="txtOrderId" runat="server" CssClass="seinput" SearchWhere="Order.Id==@OrderId" SearchParamterName="OrderId" ></asp:TextBox>
            </td>
            <td class="font">
                商品编号
            </td>
            <td class="text" >
                <asp:TextBox ID="txtGoodsId" runat="server" CssClass="seinput" SearchWhere="Goods.Id==@GoodsId" SearchParamterName="GoodsId" ></asp:TextBox>
            </td>
             <td class="font">
                是否显示
            </td>
            <td class="text">
                <asp:DropDownList ID="ddlSearchIsShow" runat="server" SearchWhere="IsShow==@IsShow" SearchParamterName="IsShow" SearchPropertyTypeName="IsShow" >
                     <asp:ListItem  Value="False" Text="隐藏"></asp:ListItem>
                     <asp:ListItem  Value="True"  Text="显示" ></asp:ListItem>   
                </asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Name" Text="名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="类型" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Detail" Text="内容" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsShow" Text="是否显示" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Order.Id" Text="订单" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Product.Id" Text="商品" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="账户" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext"  colspan="2">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem  Value="Id" Text="编号" ></asp:ListItem>
                     <asp:ListItem  Value="Name" Text="名称" ></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="类型" ></asp:ListItem>
                     <asp:ListItem  Value="Detail" Text="内容" ></asp:ListItem>
                     <asp:ListItem  Value="IsShow" Text="是否显示" ></asp:ListItem>
                     <asp:ListItem  Value="Order.Id" Text="订单"></asp:ListItem>
                     <asp:ListItem  Value="Product.Id" Text="商品" ></asp:ListItem>
                     <asp:ListItem  Value="Account.Id" Text="账户"></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="font">
                排序方式
            </td>
            <td>
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td colspan="3">
                  <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
         
                <asp:DropDownList ID="ddlIsShow" runat="server" SaveName="IsShow" ComfirmDropdownListMessage="请选择是否显示项" ComfirmValidate="IsShow">
                  <asp:ListItem  Value="True" Text="显示" ></asp:ListItem>
                  <asp:ListItem  Value="False" Text="隐藏" ></asp:ListItem>
            </asp:DropDownList>
              <asp:Button ID="btnIsShow" runat="server" Text="确定" CssClass="btn" ConfirmBox="IsShow" ConfirmMessage="您确定要修改吗？" 
            ComfirmCheckBoxMessage="你没有选择任何行" onclick="btnIsShow_Click" />
         
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="IsShow"  />
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类型"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="内容"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Detail")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="是否显示"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsShowName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="订单"  ItemStyle-CssClass="left">
            <ItemTemplate>
              <a href='/Order/Order/Detail.aspx?id=<%#Eval("Order.Id")%>' target="_blank"><%#Eval("Order.Id")%></a>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="商品"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <a href='/Product/Product/Detail.aspx?id=<%#Eval("Product.Id")%>' target="_blank"><%#Eval("Product.Id")%></a>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Name")%></a>  
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id" OrderByExp="Id desc"  />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
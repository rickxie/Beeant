<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderNote.List" MasterPageFile="~/Datum.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单维护记录列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
                <uc2:DataSearch ID="DataSearch1" runat="server" />
        </tr>
        <tr>
            <td class="font">维护人</td>
            <td class="text">  <asp:TextBox ID="txtUserRealName" runat="server" CssClass="seinput" SearchWhere="User.RealName.Contains(@User.RealName) " SearchParamterName="User.RealName" ></asp:TextBox></td>
            <td class="font">订单编号</td>
            <td class="text" colspan="5">  <asp:TextBox ID="txtOrderId" runat="server" CssClass="seinput" SearchWhere="Order.Id==@OrderId " SearchParamterName="OrderId" SearchPropertyTypeName="OrderId" ></asp:TextBox></td>
        </tr>
          <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" RepeatColumns="8" >
                     <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                      <asp:ListItem  Value="Order.Id" Text="订单编号"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Account.RealName" Text="用户" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Content" Text="内容" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext" colspan="2">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem  Value="Id" Text="编号" ></asp:ListItem>
                     <asp:ListItem  Value="Order.Id" Text="订单编号" ></asp:ListItem>
                     <asp:ListItem  Value="Content" Text="内容"></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" Selected="True"></asp:ListItem>
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
            <td colspan="4" style="border: 1px solid #bbd8fa;">
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
             <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
               
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table">
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
         <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,IsAudit,IsSales"  />
           </ItemTemplate>
         </asp:TemplateField>
        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="订单编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
            <a href='/Order/Order/Detail.aspx?id=<%#Eval("Order.Id")%>' target="_blank"><%#Eval("Order.Id")%></a>  
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="用户"  ItemStyle-CssClass="left status">
            <ItemTemplate>
              <%#Eval("Account.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="内容"  ItemStyle-CssClass="left">
            <ItemTemplate>
              <%#Eval("Content")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
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
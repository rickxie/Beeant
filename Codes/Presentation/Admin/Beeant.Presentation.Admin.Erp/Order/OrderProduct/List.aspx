<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderProduct.List" MasterPageFile="~/Datum.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单明细列表</title>  
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
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext">
                <asp:CheckBoxList ID="ckSelectList" runat="server">
                     <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Selected="True" Value="FileName" Text="图片" ></asp:ListItem>
                     <asp:ListItem  Selected="True" Value="Name" Text="名称" ></asp:ListItem>
         
                     <asp:ListItem  Selected="True" Value="Price" Text="单价" ></asp:ListItem>
                     <asp:ListItem  Selected="True" Value="Count" Text="数量" ></asp:ListItem>
                     <asp:ListItem  Selected="True" Value="Amount" Text="金额" ></asp:ListItem>
                      <asp:ListItem  Selected="True" Value="IsCount" Text="是否使用库存" ></asp:ListItem>
                         <asp:ListItem  Selected="True" Value="IsReturn" Text="是否支持退货" ></asp:ListItem>
                            <asp:ListItem  Selected="True" Value="Remark" Text="备注" ></asp:ListItem>
                     <asp:ListItem  Selected="True" Value="Product.Id,Product.Goods.Id" Text="商品" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
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
                     <asp:ListItem Value="Product.Id" Text="商品" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Selected="True" Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
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

        <div class="mainten">
         <a href='Add.aspx?orderid=<%=Request.QueryString["OrderId"] %>' name="Add" target="_blank"class="btn" >添加</a>
             <a href='Create.aspx?orderid=<%=Request.QueryString["orderid"] %>' name="Add" target="_blank"class="btn mbtn" >批量添加</a>
         <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
         
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="图片"  ItemStyle-CssClass="center time">
            <ItemTemplate>
             <img src='<%#string.IsNullOrEmpty(Eval("FileName").Convert<string>()) ? "/Images/Nopic.jpg" : Eval("FullFileName").Convert<string>()%>' alt="" class="img"/>   
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="单价"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Price")%>
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

         <asp:TemplateField HeaderText="是否使用库存"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("IsCountName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="是否支持退货"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("IsReturnName")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="描述"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Description")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
        
         <asp:TemplateField HeaderText="商品"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
          <a href='/Product/Goods/Detail.aspx?Id=<%#Eval("Product.Goods.Id") %>'><%#Eval("Product.Id")%></a>
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
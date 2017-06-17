<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderProduct.Add" MasterPageFile="~/Datum.Master" ValidateRequest="false" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 
 <%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>
 
 <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单明细录入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">



 
    <uc1:Edit ID="Edit1" runat="server" UploaderSaveType="Add" />



 
    <uc2:Message ID="Message1" runat="server" />
    <a id="hfGoods" runat="server" href="/Product/Goods/List.aspx?orderid=" Visible="False">继续添加新产品</a>
   
 
 </asp:Content>
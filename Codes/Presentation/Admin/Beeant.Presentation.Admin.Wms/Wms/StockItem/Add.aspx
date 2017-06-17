<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.StockItem.Add" MasterPageFile="~/Main.Master" ValidateRequest="false" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 
 <%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>
 
 <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>出入库明细录入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">



  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

 
    <uc1:Edit ID="Edit1" runat="server" UploaderSaveType="Add" IsLoadStorehouse="True" />



 
    <uc2:Message ID="Message1" runat="server" />
    <a id="hfProduct" runat="server" href="/Product/Product/List.aspx?orderid=" Visible="False">继续添加新产品</a>
       </ContentTemplate>
</asp:UpdatePanel>
 
 
 </asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Promotion.Promotion.Update" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %> 
 <%@ Register TagPrefix="uc1" TagName="Edit" Src="~/Promotion/Promotion/Edit.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
   <title>活动编辑</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
     <uc1:Edit ID="Edit1" runat="server"  />
    <uc2:Message ID="Message1" runat="server" />

</asp:Content>

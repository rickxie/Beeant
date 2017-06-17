<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Cms.Cms.Class.Update" MasterPageFile="~/Main.Master" ValidateRequest="false"%>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 
 <%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>
 
 <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>信息类别编辑</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
 
    <uc1:Edit ID="Edit1" runat="server" />



 
    <uc2:Message ID="Message1" runat="server" />

     </ContentTemplate>
</asp:UpdatePanel>
 
 </asp:Content>
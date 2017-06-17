<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Promotion.Promotion.Add" %>
<%@ Register TagPrefix="uc1" TagName="Edit" Src="~/Promotion/Promotion/Edit.ascx" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>活动录入</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Body" runat="server">
  <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <uc1:Edit ID="Edit1" runat="server" />
    <uc2:Message ID="Message1" runat="server" />

        </ContentTemplate>
</asp:UpdatePanel>
 
 </asp:Content>
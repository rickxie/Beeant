<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Certification.Add" %>
<%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>供应商证书录入</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    
    <uc1:Edit ID="Edit1" runat="server" />
    <uc2:Message ID="Message1" runat="server" />    
    
</asp:Content>

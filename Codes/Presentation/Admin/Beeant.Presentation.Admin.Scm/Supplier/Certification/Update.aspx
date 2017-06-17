<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Certification.Update" %>
<%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Message" Src="~/Controls/Message.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>供应商证书修改</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <uc1:Edit ID="Edit1" runat="server" />
        <uc2:Message ID="Message1" runat="server" />  
</asp:Content>

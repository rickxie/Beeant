<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Brand.Add" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>品牌录入</title>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <uc1:Edit ID="Edit1" runat="server" UploaderSaveType="Add"  />
    <uc2:Message ID="Message1" runat="server" />
</asp:Content>

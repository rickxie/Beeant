<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagDropDownList.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Controls.Basedata.TagDropDownList" %>

 
<asp:DropDownList ID="DropDownList1" runat="server" DataTextField="Name" DataValueField="Id" onselectedindexchanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True">
</asp:DropDownList>
<asp:DropDownList ID="ddlTag" runat="server" DataTextField="Name" DataValueField="Value" >
</asp:DropDownList>
 

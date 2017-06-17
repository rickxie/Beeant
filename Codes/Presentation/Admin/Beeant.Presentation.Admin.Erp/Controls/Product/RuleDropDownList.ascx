<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleDropDownList.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Controls.Product.RuleDropDownList" %>
<asp:DropDownList ID="DropDownList1" runat="server" DataTextField="Name" 
    DataValueField="Id" AutoPostBack="True" onselectedindexchanged="DropDownList1_SelectedIndexChanged">
</asp:DropDownList>
 说明： <span id="spRemark" runat="server" ></span>
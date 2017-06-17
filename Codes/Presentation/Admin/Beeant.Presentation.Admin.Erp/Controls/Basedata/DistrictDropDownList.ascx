<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DistrictDropDownList.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Controls.Basedata.DistrictDropDownList" %>
<asp:DropDownList ID="ddlProvince" runat="server" DataTextField="Name" DataValueField="Name">
</asp:DropDownList>
<asp:DropDownList ID="ddlCity" runat="server" DataTextField="Name" DataValueField="Name" ViewStateMode="Disabled">
</asp:DropDownList>
<asp:DropDownList ID="ddlCounty" runat="server" DataTextField="Name" DataValueField="Name" ViewStateMode="Disabled">
</asp:DropDownList>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagCheckBoxList.ascx.cs" Inherits="Beeant.Presentation.Admin.Wms.Controls.Basedata.TagCheckBoxList" %>

 
<asp:DropDownList ID="DropDownList1" runat="server" DataTextField="Name" DataValueField="Id"  >
</asp:DropDownList>
<span id="<%=ClientID%>ckg" >
    <%=Inputs.ToString() %>
</span>
<asp:Label ID="lblTag" runat="server" Text="" Visible="False"></asp:Label>

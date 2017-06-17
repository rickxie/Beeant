<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Api.Protocol.Add" MasterPageFile="~/Main.Master" ValidateRequest="false" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 
 <%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>
 
 <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>协议录入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


 
    <uc1:Edit ID="Edit1" runat="server" />



 
    <uc2:Message ID="Message1" runat="server" />

 
 </asp:Content>
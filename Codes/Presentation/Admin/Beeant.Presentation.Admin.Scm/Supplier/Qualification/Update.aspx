<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" MasterPageFile="~/Main.Master" ValidateRequest="false" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Qualification.Update"%>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 
 <%@ Register src="Edit.ascx" tagname="Edit" tagprefix="uc1" %>
 
 <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>供应商资质编辑</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


 
    <uc1:Edit ID="Edit1" runat="server" UploaderSaveType="Modify"  />



 
    <uc2:Message ID="Message1" runat="server" />


 </asp:Content>
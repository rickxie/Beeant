<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserComboBox.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Controls.Product.UserComboBox" %>
  <input id="txtInputText" runat="server"  type="text" class="input"   BindName="User.Name"  SaveName="User.Name" />
   <input id="hfInputHidden" type="hidden"  BindName="User.Id"  SaveName="User.Id" runat="server"  />
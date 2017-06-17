<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierComBox.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Controls.Supplier.SupplierComBox" %>
  <input id="txtInputText" runat="server"  type="text" class="input"   BindName="Supplier.Name"  SaveName="Supplier.Name" />
   <input id="hfInputHidden" type="hidden"  BindName="Supplier.Id"  SaveName="Supplier.Id" runat="server"  />
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountComboBox.ascx.cs" Inherits="Beeant.Presentation.Admin.Finance.Controls.Account.AccountComboBox" %>


  <input id="txtInputText" runat="server"  type="text" class="input"   BindName="Account.Name"  SaveName="Account.Name" />
   <input id="hfInputHidden" type="hidden"  BindName="Account.Id"  SaveName="Account.Id" runat="server"  />

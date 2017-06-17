<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Home.Desktop.Password.Update" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>用户密码修改</title>  
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
     <div class="edit">
    <table class="tb">
        <tr>
            <td class="font">用户名</td>
            <td class="mtext"><asp:Label ID="lblName" BindName="Name" runat="server" Text=""></asp:Label> </td>
        </tr>
        <tr>
            <td class="font">原密码</td>
            <td class="mtext"><input id="txtOriginPassword" runat="server" type="password" class="input" autocomplete="off"     /></td>
        </tr>
         <tr>
            <td class="font">密码</td>
            <td class="mtext"><input id="txtPassword" runat="server" type="password" class="input"  SaveName="Password"   /></td>
        </tr>
         <tr>
            <td class="font">确认密码</td>
            <td class="mtext"><input id="txtSurePassword" runat="server" class="input" type="password"  ValidateName="Password"  /> </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />

        </tr>
    </table>
 
</div>
    <uc2:Message ID="Message1" runat="server" />
    <uc3:Progress ID="Progress1" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>

 </asp:Content>
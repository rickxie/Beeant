<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Passsword.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Management.User.Passsword" MasterPageFile="~/Datum.Master" %>
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
            <td class="font">真实姓名</td>
            <td class="mtext"><asp:Label ID="lblRealName" BindName="RealName" runat="server" Text=""></asp:Label></td>
        </tr>
         <tr>
            <td class="font">密码</td>
            <td class="mtext"><input id="txtPassword" runat="server" type="password" class="input"   SaveName="Password"  OrmSaveName="Password"   /></td>
        </tr>
         <tr>
            <td class="font">确认密码</td>
            <td class="mtext"><input id="txtSurePassword" runat="server" class="input" type="password"  ValidateName="Password"   /> </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>

    <uc2:Message ID="Message1" runat="server" />
    <uc3:Progress ID="Progress1" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>

 </asp:Content>
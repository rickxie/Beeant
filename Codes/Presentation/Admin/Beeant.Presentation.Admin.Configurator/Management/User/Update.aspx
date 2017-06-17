<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Management.User.Update" MasterPageFile="~/Datum.Master" %>
 


  <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
   <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>用户编辑</title>  
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
 <div class="edit">
    <table class="tb">
        <tr>
             <td class="font">用户名</td>
            <td class="text"><input id="txtAccountName" runat="server" type="text" class="input"  BindName="Account.Name" SaveName="Account.Name" ValidateName="Name"    /></td>
            <td class="font">真实姓名</td>
            <td class="text"><input id="txtAccountRealName" runat="server"  type="text" class="input"  BindName="Account.RealName" SaveName="Account.RealName" ValidateName="RealName"   />  </td>
           
        </tr>
        <tr>
            <td class="font">邮箱</td>
             <td class="text"><input id="txtAccountEmail" runat="server" type="text" class="input"  BindName="Account.Email" SaveName="Account.Email"  ValidateName="Email"   /></td>
              <td class="font">手机号码</td>
            <td class="text"><input id="txtAccountMobile" runat="server"  type="text" class="input"  BindName="Account.Mobile" SaveName="Account.Mobile"  ValidateName="Mobile" /> </td>
        </tr>
        <tr>
               <td class="font">别名</td>
            <td class="mtext"  >
               <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name" ValidateName="" ValidateName1="Name"  />
            </td>
            <td class="font">是否启用</td>
            <td class="text">
                <asp:RadioButtonList runat="server" ID="rdbtnIsUsed" RepeatDirection="Horizontal" BindName="IsUsed" SaveName="IsUsed" >
                             <asp:ListItem Selected="True" Value="true">启用</asp:ListItem>
                             <asp:ListItem Value="false">禁止</asp:ListItem>
                             </asp:RadioButtonList>
                          </td>

        </tr>
           
         
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"   /></td>
        </tr>
    </table>
 
</div>

   <uc2:Message ID="Message1" runat="server" />
   <uc3:Progress ID="Progress1" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>

 </asp:Content>
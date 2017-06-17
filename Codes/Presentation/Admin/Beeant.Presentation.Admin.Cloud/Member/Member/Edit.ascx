<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Member.Member.Edit" %>
     
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
     
<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">昵称</td>
            <td class="text" >
             <input id="txtNickname" runat="server"  type="text" class="input long"  BindName="Nickname" SaveName="Nickname"  /> 
            </td>
             <td class="font">性别</td>
            <td class="text">
                
                <asp:RadioButtonList runat="server" ID="RadioButtonList1" RepeatDirection="Horizontal" BindName="Gender" SaveName="Gender" >
                             <asp:ListItem Selected="True" Value="男">男</asp:ListItem>
                             <asp:ListItem Value="女">女</asp:ListItem>
                             </asp:RadioButtonList>
            </td>
        </tr>               
         <tr>
           
               <td class="font">固定电话</td>
            <td class="text"><input id="txtTelephone" runat="server"  type="text" class="input"   BindName="Telephone" SaveName="Telephone"   /> </td>
             <td class="font">身份证号码</td>
            <td class="text"   >
                 <input id="txtIdCardNumber" runat="server"  type="text" class="input long"   BindName="IdCardNumber" SaveName="IdCardNumber"   />
            </td>
        </tr>             
        <tr>
            <td class="font">头像</td>
            <td class="mtext">
                <uc2:Uploader ID="Uploader1" runat="server" Path="Files/Images/Member/" FileByteSaveName="FileByte" FileNameBindName="FileName"  FileNameSaveName="FileName"  FullFileNameBindName="FullFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
            <td class="font">身份证附件</td>
            <td class="mtext">
                <uc2:Uploader ID="Uploader2" runat="server" Path="Files/Images/Member/" FileByteSaveName="IdCardFileByte" FileNameBindName="IdCardFileName"  FileNameSaveName="IdCardFileName"  FullFileNameBindName="IdCardFullFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
        <tr>
            <td class="font">账户</td>
            <td class="mul">
                <uc2:AccountComboBox ID="cbAccount" runat="server" BindName="Account.Id" SaveName="Account.Id"  />
            </td>
            <td class="font">是否启用</td>
            <td class="text" >
              <asp:RadioButtonList runat="server" ID="rbIsUsed" RepeatDirection="Horizontal" BindName="IsUsed" SaveName="IsUsed" >
                             <asp:ListItem Selected="True" Value="True">启用</asp:ListItem>
                             <asp:ListItem Value="False">禁止</asp:ListItem>
                             </asp:RadioButtonList>
            </td>
            
        </tr>            
        <tr>
           <td class="font">地址</td>
           <td class="text" >
                 <input id="txtAddress" runat="server"  type="text" class="input long"   BindName="Address" SaveName="Address"   />
            </td>
            <td class="font">邮政编码</td>
            <td class="text"><input id="txtPostal" runat="server"  type="text" class="input"   BindName="Postal" SaveName="Postal"   /></td>
        </tr>
        <tr>
             <td class="font">备注</td>
             <td colspan="3"  class="mtext">
                 <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
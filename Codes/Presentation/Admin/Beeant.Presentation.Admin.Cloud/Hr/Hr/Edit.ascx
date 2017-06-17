<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Hr.Hr.Edit" %>

<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc2" %>


<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="text"   >
             <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"  /> 
               </td>
               <td class="font">账户</td>
            <td class="mul">
                <uc2:AccountComboBox ID="cbAccount" runat="server" BindName="Account.Id" SaveName="Account.Id"  />
            </td>
        </tr>
           <tr>
          <td class="font">是否启用</td>
            <td class="mtext" colspan="3">
                  <asp:CheckBox ID="ckIsUsed" runat="server"  BindName="IsUsed" SaveName="IsUsed"  /> 
            </td>
        </tr>
      
          
   
  
            
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
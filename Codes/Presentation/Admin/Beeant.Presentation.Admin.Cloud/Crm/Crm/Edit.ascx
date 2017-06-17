<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Crm.Crm.Edit" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
     
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc2" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>


<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="text" colspan="3"  >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
               </td>
             
        </tr>
  
        <tr>
            <td class="font">账户</td>
            <td class="mul">
                <uc2:AccountComboBox ID="cbAccount" runat="server" BindName="Account.Id" SaveName="Account.Id"  />
            </td>
            <td class="font">到期时间</td>
            <td class="text" >
                <asp:TextBox ID="txtExpireDate" runat="server" BindName="ExpireDate" SaveName="ExpireDate" ></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtExpireDate" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            
        </tr>
          
        <tr>
           <td class="font">员工数量</td>
            <td class="text" >
              <input id="txtStaffCount" runat="server"  type="text" class="input"  BindName="StaffCount" SaveName="StaffCount"   /> 
               </td>
             
        </tr>
  
            
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
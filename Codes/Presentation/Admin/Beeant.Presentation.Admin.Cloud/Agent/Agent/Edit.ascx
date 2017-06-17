<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Agent.Agent.Edit" %>
<%@ Register TagPrefix="uc3" TagName="UserComboBox" Src="~/Controls/User/UserComboBox.ascx" %>
     
<%--<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>--%>
     
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc2" %>
<%@ Register TagPrefix="uc5" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
     
<div class="edit">
    <table class="tb">
      
       
           <tr>
            <td class="font">名称</td>
            <td class="text"  >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
                
               </td>
                  <td class="font">是否启用</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsUsed" runat="server" BindName="IsUsed" SaveName="IsUsed" />

               </td>
            
        </tr>         
        <tr>
            <td class="font">账户</td>
            <td class="mul">
                <uc2:AccountComboBox ID="cbAccount" runat="server" BindName="Account.Id" SaveName="Account.Id"  />
            </td>
             <td class="font">折扣</td>
            <td class="text" >
             <input id="txtDiscount" runat="server"  type="text" class="input"  BindName="Discount" SaveName="Discount"  /> </td>
        </tr>  
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
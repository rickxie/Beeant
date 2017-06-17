<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Currency.Edit" %>
     
<div class="edit">
    <table class="tb">
         <tr>
            <td class="font">名称</td>
            <td class="text" >
             <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"  /> </td>
             <td class="font">代码</td>
            <td class="text" >
             <input id="txtCode" runat="server"  type="text" class="input"  BindName="Code" SaveName="Code"  /> </td>
        </tr>
      
         
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.PayType.Edit" %>
     
<div class="edit">
    <table class="tb">
         <tr>
            <td class="font">名称</td>
            <td class="text" >
             <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"  /> </td>
             <td class="font">标签</td>
            <td class="text" >
             <input id="txtTag" runat="server"  type="text" class="input"  BindName="Tag" SaveName="Tag"  /> </td>
        </tr>
        <tr>
               <td class="font">支付地址</td>
            <td class="mtext" colspan="3" >
                <input id="txtUrl" runat="server"  type="text" class="input"  BindName="Url" SaveName="Url"  /> </td>
        </tr>
         
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
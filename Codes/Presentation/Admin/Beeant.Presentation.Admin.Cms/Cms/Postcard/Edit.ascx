<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cms.Cms.Postcard.Edit" %>

<div class="edit">
    <table class="tb">
         <tr>
            <td class="font">名称</td>
            <td class="mtext">
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> </td>
     
         <td class="font">是否显示</td>
            <td class="text"> 
                <asp:CheckBox ID="ckIsShow" runat="server" BindName="IsShow" SaveName="IsShow"  Checked="True"></asp:CheckBox> </td>
         
        </tr>

          <tr>
           <td class="font">描述</td>
            <td class="mtext" colspan="3"><input id="txtDescription" runat="server"  type="text" class="input long"   BindName="Description" SaveName="Description"   /> </td>
        </tr>
    
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.PurchaseAttachment.Edit" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
<div class="edit">
    <table class="tb">
       
        <tr>
            <td class="font">附件名称</td>
            <td class="text" >
               <input id="txtName" runat="server"  type="text" class="input "  BindName="Name" SaveName="Name"  />
            </td>
             <td class="font">附件</td>
            <td class="text"  >
                <uc2:Uploader ID="Uploader2" runat="server" IsShowViewControl="False" Path="Files/Documents/PurchaseAttachment/" FileByteSaveName="FileByte" FileNameBindName="FileName"  FileNameSaveName="FileName"   />
            </td>                                                                                                                 
        </tr>

        <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>

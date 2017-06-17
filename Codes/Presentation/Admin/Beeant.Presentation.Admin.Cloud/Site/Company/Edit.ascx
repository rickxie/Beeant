<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Site.Company.Edit" %>
 
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
    
<div class="edit">
    <table class="tb">
             <tr>
            <td class="font">微信二维码</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader2" runat="server" Path="Files/Images/SiteWeixinQr/" FileByteSaveName="WeixinQrCodeFileByte" FileNameBindName="WeixinQrCodeFileName"  FileNameSaveName="WeixinQrCodeFileName"  FullFileNameBindName="WeixinQrCodeFullFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
   
         <tr>
            <td class="font">备案号</td>
            <td class="mtext" colspan="3"  >
             <input id="txtRecordNumber" runat="server"  type="text" class="input long"  BindName="RecordNumber" SaveName="RecordNumber"  /> 
               </td>
            
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
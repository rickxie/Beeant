<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Certification.Edit" %>
<%@ Register src="../../Controls/Uploader.ascx" tagname="Uploader" tagprefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Message" Src="~/Controls/Message.ascx" %>

<div class="edit">
    <table class="tb">
        
        <tr>
            <td class="font">供应商其他证书</td>
            <td class="mtext" colspan="7">                
                <uc1:Uploader ID="Uploader1" runat="server" Path="Files/Documents/Supplier/" FileByteSaveName="CertificationByte" FileNameBindName="Certification"  FileNameSaveName="Certification"  FullFileNameBindName="FullCertification" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />                
            </td>
        </tr>
        <tr>
            <td colspan="8" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  />

         </td>
        </tr>
    </table>
</div>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Album.Edit" %>
     <%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
<%@ Register src="../../Controls/Editor.ascx" tagname="Editor" tagprefix="uc3" %>
 
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
              <td class="font">路径</td>
            <td class="text" >
             <input id="txtPath" runat="server"  type="text" class="input"  BindName="Path" SaveName="Path"  /> </td>
            
            <td class="font">每页大小</td>
            <td class="text" >
             <input id="txtPageSize" runat="server"  type="text" class="input"  BindName="PageSize" SaveName="PageSize"  /> </td>
            
        </tr>
             <tr>
              <td class="font">宽度</td>
            <td class="text" >
             <input id="txtWidth" runat="server"  type="text" class="input"  BindName="Width" SaveName="Width"  /> </td>
            
            <td class="font">高度</td>
            <td class="text" >
             <input id="txtHeight" runat="server"  type="text" class="input"  BindName="Height" SaveName="Height"  /> </td>
            
        </tr>
           <tr>
            <td class="font">封面</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader1" runat="server" Path="Files/Images/BasedataAlbum/" FileByteSaveName="FrontFileByte" FileNameBindName="FrontFileName"  FileNameSaveName="FrontFileName"  FullFileNameBindName="FullFrontFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
           <tr>
            <td class="font">背面</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader2" runat="server" Path="Files/Images/BasedataAlbum/" FileByteSaveName="BackFileByte" FileNameBindName="BackFileName"  FileNameSaveName="BackFileName"  FullFileNameBindName="FullBackFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
           <tr>
            <td class="font">介绍面</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader3" runat="server" Path="Files/Images/BasedataAlbum/" FileByteSaveName="AboutFileByte" FileNameBindName="AboutFileName"  FileNameSaveName="AboutFileName"  FullFileNameBindName="FullAboutFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
             <tr>
           <td class="font">详情</td>
           <td colspan="3" class="mul text">
               

               <uc3:Editor ID="Editor1" runat="server" ImagePath="Files/Images/BasedataAlbum/" FlashPath="Files/Images/BasedataAlbum/" ValidateName="Detail" SaveName="Detail" BindName="Detail" />
               

           </td>
       </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
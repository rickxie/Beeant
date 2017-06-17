<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Tag.Edit" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
<div class="edit">
    <table class="tb">
         <tr>
            <td class="font">名称</td>
            <td class="text" >
             <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"  /> </td>
             <td class="font">标签组</td>
            <td class="text" >
                <uc1:GeneralDropDownList ID="ddlTagGroup" runat="server" BindName="TagGroup.Id" SaveName="TagGroup.Id" ObjectName="TagGroupEntity"   />
            </td>
           
        </tr>
        <tr>
            <td class="font">标签值</td>
            <td class="text" colspan="3" >
             <input id="txtValue" runat="server"  type="text" class="input long"  BindName="Value" SaveName="Value"  /> </td>
        </tr>
        
        <tr>
            <td class="font">图片</td>
            <td class="text" colspan="3">
                <uc2:Uploader ID="Uploader1" runat="server" Path="Files/Images/Tags/" FileByteSaveName="FileByte" FileNameBindName="FileName"  FileNameSaveName="FileName"  FullFileNameBindName="FullFileName" IsMultiple="True" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
         
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>

    </table>
 
</div>
 
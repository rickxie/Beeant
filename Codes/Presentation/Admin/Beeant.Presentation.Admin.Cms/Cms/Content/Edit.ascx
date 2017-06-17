<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cms.Cms.Content.Edit" %>
<%@ Register src="../../Controls/Uploader.ascx" tagname="Uploader" tagprefix="uc2" %>
<%@ Register src="../../Controls/GeneralTreeView.ascx" tagname="GeneralTreeView" tagprefix="uc7" %>    
<%@ Register TagPrefix="uc3" TagName="Editor" Src="~/Controls/Editor.ascx" %>     
     
<div class="edit">
    <table class="tb">
         <tr>
            <td class="font">标题</td>
            <td class="mtext" colspan="3">
             <input id="txtTitle" runat="server"  type="text" class="input long"  BindName="Title" SaveName="Title"  /> </td>
        </tr>
        <tr>
         <td class="font">是否显示</td>
            <td class="text"> 
                <asp:CheckBox ID="ckIsShow" runat="server" BindName="IsShow" SaveName="IsShow"  Checked="True"></asp:CheckBox> </td>
           <td class="font">排序</td>
            <td class="text"><input id="txtSequence" runat="server"  type="text" class="input"  BindName="Sequence" SaveName="Sequence" DefaultValue="1"  value="1"  /> </td>
        </tr>
        <tr>
           <td class="font">标签</td>
            <td class="mtext" colspan="3"><input id="txtTag" runat="server"  type="text" class="input long"   BindName="Tag" SaveName="Tag"   /> </td>
        </tr>
         <tr>
           <td class="font">链接地址</td>
            <td class="mtext" colspan="3"><input id="txtUrl" runat="server"  type="text" class="input long"   BindName="Url" SaveName="Url"   /> </td>
        </tr>
<%--          <tr>
        
        <td class="font">账户</td>
            <td class="mul mtext" colspan="3"  >
               <uc9:AccountComboBox ID="cbAccount" runat="server" />
            </td>
       
         
       </tr>--%>
          <tr>
           <td class="font">描述</td>
            <td class="mtext" colspan="3"><input id="txtDescription" runat="server"  type="text" class="input long"   BindName="Description" SaveName="Description"   /> </td>
        </tr>
           <tr>
           <td colspan="4" class="text">
    <table class="intb">
        <tr>
             
         <td class="infont">类别</td>
            <td class="intext">
                 <uc7:GeneralTreeView ID="tvClassTree" runat="server" EntityName="ClassEntity" OnSelectedNodeChanged="tvClassTree_SelectedNodeChanged" IsShowNone="True" />
 
            </td>
             <td class="infont">已经选择</td>
              <td class="intext">
                  <asp:Label ID="lblClassName" runat="server"  BindName="Class.Name" Text=""></asp:Label>
                 <input id="hfClassId" type="hidden" runat="server"  BindName="Class.Id"  SaveName="Class.Id"/>   
                 
               </td>
               </tr>
    </table>
             </td>
     </tr>
      <tr>
            <td class="font">图片</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader1" runat="server" Path="Files/Images/Content/" FileByteSaveName="FileByte" FileNameBindName="FileName"  FileNameSaveName="FileName"  FullFileNameBindName="FullFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
          <tr>
            <td class="font">附件</td>
            <td class="mtext" colspan="3" >
                <uc2:Uploader ID="Uploader2" runat="server" Path="Files/Documents/Content/"  IsShowViewControl="False" FileByteSaveName="AttachmentByte" FileNameBindName="AttachmentName"  FileNameSaveName="AttachmentName"  FullFileNameBindName="FullAttachmentName"  />
            </td>
        </tr>
      <tr>
           <td class="font">描述</td>
            <td class="mul" colspan="3">
                 
                <uc3:Editor ID="Editor1" runat="server" ImagePath="Files/Eidtor/Images/Content/" FlashPath="Files/Eidtor/Flashs/Content/" BindName="Detail" SaveName="Detail"/>
                 
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>

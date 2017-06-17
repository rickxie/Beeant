<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Brand.Edit" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
<%@ Register TagPrefix="uc8" TagName="TagRadioButtonList" Src="~/Controls/Basedata/TagRadioButtonList.ascx" %>
<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="mtext">
                <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"   /> 
            </td>
             <td class="font">英文名</td>
            <td class="mtext">
                <input id="txtEnglishName" runat="server"  type="text" class="input"  BindName="EnglishName" SaveName="EnglishName"   /> 
            </td>
        </tr>
        <tr>
            <td class="font">标签</td>
            <td class="text">
                <uc8:TagRadioButtonList ID="ckTag" runat="server" SaveName="Tag" BindName="Tag" />
            </td>
        </tr>
         <tr>
            <td class="font">首字母</td>
            <td class="mtext">
                 <input id="txtInitial" runat="server"  type="text" class="input" BindName="Initial" SaveName="Initial"  />
            </td>
             <td class="font">状态</td>
            <td class="mtext">
                <asp:RadioButtonList runat="server" ID="rbIsUsed" RepeatDirection="Horizontal" BindName="IsUsed" SaveName="IsUsed" >
                    <asp:ListItem Selected="True" Value="True">启用</asp:ListItem>
                    <asp:ListItem Value="False">禁止</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
          <tr>
            <td class="font">图片</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader1" runat="server" Path="Files/Images/Brand/" FileByteSaveName="FileByte" FileNameBindName="FileName"  FileNameSaveName="FileName"  FullFileNameBindName="FullFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
        <tr>
            <td colspan="4" class="center">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
                <input id="btnClose" type="button" value="关闭" class="btn"  />
            </td>
        </tr>
    </table>
</div>
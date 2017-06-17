<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cms.Cms.Class.Edit" %>
     <%@ Register src="~/Controls/GeneralTreeView.ascx" tagname="GeneralTreeView" tagprefix="uc4" %>
 
<div class="edit">
<table class="tb">
        
        <tr>
            <td class="font">名称</td>
            <td class="text" >
              <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"   /> 
            </td>
             <td class="font">排序</td>
            <td class="text"  >
                 <input id="txtSequence" runat="server"  type="text" class="input" DefaultValue="1" value="1"  BindName="Sequence" SaveName="Sequence"  />
            </td>
        </tr>
         <tr>
            <td class="font">是否启用</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsUsed" runat="server" Checked="True"  BindName="IsUsed" SaveName="IsUsed"/>
                
            </td>
        <td class="font">是否公开</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsPublic" runat="server" Checked="True"  BindName="IsPublic" SaveName="IsPublic"/>
                
            </td>
        </tr>
        <tr>
            
             <td class="font">是否允许发布</td>
            <td class="mtext" colspan="3" >
                <asp:CheckBox ID="ckIsPublish" runat="server" Checked="True"  BindName="IsPublish" SaveName="IsPublish"/>
                
            </td>
        </tr>
         <tr>
           <td class="font">标签</td>
            <td class="mtext" colspan="3">
                <input id="txtTag" runat="server"  type="text" class="input long"  BindName="Tag" SaveName="Tag"  />
            </td>
        
       </tr>

       <tr>
       <td colspan="4" class="text">
    <table class="intb">
        <tr>
         <td class="infont">父级类目</td>
            <td class="intext">
                 <uc4:GeneralTreeView ID="tvClassTree" runat="server" EntityName="ClassEntity" OnSelectedNodeChanged="tvClassTree_SelectedNodeChanged" IsShowNone="True" />
 
            </td>
             <td class="infont">已经选择</td>
              <td class="intext">
                  <asp:Label ID="lblParentName" runat="server"  BindName="Parent.Name" Text=""></asp:Label>
                 <input id="hfParentId" type="hidden" runat="server"  BindName="Parent.Id"  SaveName="Parent.Id"/>   
                 
               </td>
               </tr>
    </table>
             </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
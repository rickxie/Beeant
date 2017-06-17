<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Category.Edit" %>
<%@ Register src="~/Controls/GeneralTreeView.ascx" tagname="generalTreeView" tagprefix="uc1" %>

<div class="edit">
    <table class="tb">
        
        <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3">
              <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"   /> 
            </td>
           
        </tr>

         <tr>
           <td class="font">拼音</td>
            <td class="mtext" colspan="3">
                <input id="txtPinyin" runat="server"  type="text" class="input long"  BindName="Pinyin" SaveName="Pinyin"  />
            </td>
       </tr>
         <tr>
           <td class="font">首字母</td>
            <td class="mtext" colspan="3" >
                <input id="txtInitial" runat="server"  type="text" class="input long"  BindName="Initial" SaveName="Initial"  />
            </td>      
       </tr>
        <tr>
            <td class="font">链接地址</td>
            <td class="mtext" colspan="3" >
                <input id="txtUrl" runat="server" type="text" class="input long" BindName="Url" SaveName="Url"/>
            </td>        
        </tr>
       <tr>       
        <td class="font">是否允许发布</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsPublish" runat="server"  BindName="IsPublish" SaveName="IsPublish"  />
            </td>
              <td class="font">是否显示</td>
            <td class="text"> 
                <asp:CheckBox ID="ckIsShow" runat="server" BindName="IsShow" SaveName="IsShow" > </asp:CheckBox> </td>
       </tr>
        <tr>
             <td class="font">图片数量</td>
            <td class="text"  >
                 <input id="txtImageCount" runat="server"  type="text" class="input" DefaultValue="10" value="10"  BindName="ImageCount" SaveName="ImageCount"  />
            </td>
             <td class="font">排序</td>
            <td class="text"  >
                 <input id="txtSequence" runat="server"  type="text" class="input" DefaultValue="1" value="1"  BindName="Sequence" SaveName="Sequence"  />
            </td>
       </tr>
             
       <tr>
       <td colspan="4" class="text">
    <table class="intb">
        <tr>
         <td class="infont">父级类目</td>
            <td class="intext">
                 <uc1:generalTreeView ID="tvCategoryTree" runat="server" OnSelectedNodeChanged="tvCategoryTree_SelectedNodeChanged" EntityName="CategoryEntity" IsShowNone="True" />
            </td>
             <td class="infont">已经选择</td>
              <td class="intext">
                  <asp:Label ID="lblParentName" runat="server"  BindName="Name" Text=""></asp:Label>
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
 
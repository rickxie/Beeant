<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.District.Edit" %>
<%@ Register TagPrefix="uc1" TagName="GeneralTreeView" Src="~/Controls/GeneralTreeView.ascx" %>
<div class="edit">
<table class="tb">
        
        <tr>
            <td class="font">名称</td>
            <td class="text" >
              <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"   /> 
            </td>
           <td class="font">拼音</td>
            <td class="text" >
              <input id="txtPinyin" runat="server"  type="text" class="input"  BindName="Pinyin" SaveName="Pinyin"   /> 
            </td>
        </tr>
         <tr>
               <td class="font">排序</td>
            <td class="text"  >
                 <input id="txtSequence" runat="server"  type="text" class="input" DefaultValue="1" value="1"  BindName="Sequence" SaveName="Sequence"  />
            </td>
            <td class="font">是否启用</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsUsed" runat="server" Checked="True"  BindName="IsUsed" SaveName="IsUsed"/>
                
            </td>
           
        </tr>
       
       <tr>
       <td colspan="4" class="text">
    <table class="intb">
        <tr>
         <td class="infont">父级类目</td>
            <td class="intext">
                 <uc1:GeneralTreeView ID="tvDistrictTree" runat="server" OnSelectedNodeChanged="tvDistrictTree_SelectedNodeChanged" IsShowNone="True" EntityName="DistrictEntity" />
 
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
 
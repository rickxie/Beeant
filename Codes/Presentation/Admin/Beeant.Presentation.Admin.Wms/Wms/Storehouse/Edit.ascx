<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Storehouse.Edit" %>
     <%@ Register src="../../Controls/GeneralTreeView.ascx" tagname="GeneralTreeView" tagprefix="uc4" %>
<div class="edit">
<table class="tb">
        
        <tr>
            <td class="font">名称</td>
            <td class="text" >
              <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"   /> 
            </td>
             <td class="font">排序</td>
            <td class="text"  >
                 <input id="txtSequence" runat="server"  type="text" class="input" DefaultValue="1" value="1"  BindName="Sequence" SaveName="Sequence"  />
            </td>
        </tr>
        
           <tr>

            <td class="font">是否启用</td>
            <td class="text" colspan="3" >
                <asp:CheckBox ID="ckIsUsed" runat="server" Checked="True"  BindName="IsUsed" SaveName="IsUsed"/>
            </td>
           
       </tr>
      
       <tr>
            <td class="font">备注</td>
            <td class="text" colspan="3">
                <input id="txtRemark" runat="server"  type="text" class="input long"  BindName="Remark" SaveName="Remark"  />
            </td>
       </tr>
    
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
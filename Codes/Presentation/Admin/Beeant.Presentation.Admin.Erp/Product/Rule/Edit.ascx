<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Rule.Edit" %>
<div class="edit">
    <table class="tb">
        
        <tr>
            <td class="font">名称</td>
            <td class="text" >
              <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"   /> 
            </td>
            <td class="font">排序</td>
            <td class="text" >
                 <input id="txtSequence" runat="server"  type="text" class="input" DefaultValue="1" value="1"  BindName="Sequence" SaveName="Sequence"  />
            </td>
        </tr>
      
         <tr>
           <td class="font">表达式</td>
            <td class="mtext" colspan="3" >
                   <input id="txtPattern" runat="server"  type="text" class="input long"  BindName="Pattern" SaveName="Pattern"  />
            </td>
            
       </tr>
       <tr>
           <td class="font">是否范围验证</td>
            <td class="mtext" colspan="3" >
                <asp:CheckBox ID="ckIsRange" runat="server"  BindName="IsRange" SaveName="IsRange"  />
            </td>
            
       </tr>
       <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                   <input id="txtRemark" runat="server"  type="text" class="input long"  BindName="Remark" SaveName="Remark"  />
            </td>
       </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
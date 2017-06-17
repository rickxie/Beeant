<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.PropertyRule.Edit" %>
<%@ Register src="../../Controls/Product/RuleDropDownList.ascx" tagname="RuleDropDownList" tagprefix="uc1" %>
<%@ Register src="../../Controls/GeneralCheckBoxList.ascx" tagname="PropertyRuleTypeCheckBoxList" tagprefix="uc2" %>
<div class="edit">
    <table class="tb">
        
        <tr>
            <td class="font">当前属性</td>
            <td class="text">
                <asp:Label ID="lblPropertyName" runat="server" Text="" BindName="Property.Name"></asp:Label>
            </td>
           <td class="font">规则</td>
            <td class="text" >
                <uc1:RuleDropDownList ID="ddlRule" runat="server" SaveName="Rule.Id" BindName="Rule.Id" />
               
            </td>
        </tr>
        <tr>
           <td class="font">是否多行验证</td>
            <td class="text" >
                 <asp:CheckBox ID="ckIsMultiline" runat="server"  BindName="IsMultiline" SaveName="IsMultiline"  />
            </td>
             <td class="font">是否区分大小写</td>
            <td class="text" >
                         <asp:CheckBox ID="ckIsIgnoreCase" runat="server"  BindName="IsIgnoreCase" SaveName="IsIgnoreCase"  />
                
            </td>
       </tr>
       <tr>
           <td class="font">验证类型</td>
            <td class="mtext ckmul" colspan="3" >
                <uc2:PropertyRuleTypeCheckBoxList ID="ckPropertyRuleType" runat="server" ValidateName="Type" BindName="TypeValue" SaveName="TypeValue" IsEnum="True" ObjectName="Beeant.Domain.Entities.Product.PropertyRuleType"/>
            </td>
       </tr>
        <tr>
         <td class="font">错误提示</td>
            <td class="mtext" colspan="3"  >
                 <input id="txtMessage" runat="server"  type="text" class="input long"    BindName="Message" SaveName="Message"  />
            </td>
     </tr>
        <tr>
           <td>参数(换行表示分割)</td>
            <td class="mtext" colspan="3" >
                <textarea id="txtParamter" runat="server"  type="text" class="input long"   BindName="Paramter" SaveName="Paramter" ></textarea>
            </td>
       </tr>

         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
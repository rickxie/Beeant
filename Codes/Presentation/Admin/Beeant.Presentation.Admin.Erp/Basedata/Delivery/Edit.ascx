<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Delivery.Edit" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>

     

<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="mtext"  >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
            </td>
               <td class="font">城市</td>
             <td class="text">
               <uc1:GeneralDropDownList ID="ddlCity" runat="server" DataValueField="Name" BindName="City" SaveName="City" ObjectName="CityEntity" />
             </td>

        </tr>
 
        <tr>
           <td class="font">限量</td>
            <td class="text" >
                 <input id="txtLimitCount" runat="server"  type="text" class="input"   BindName="LimitCount" SaveName="LimitCount"   />
            </td>
            <td class="font">是否启用</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsUsed" runat="server"   BindName="IsUsed" SaveName="IsUsed"  Checked="True" />
            </td>
        </tr>
          
   
        <tr>
           <td class="font">备注</td>
           <td class="text"  colspan="3" >
                 <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
                 
            </td>
        </tr>

         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>


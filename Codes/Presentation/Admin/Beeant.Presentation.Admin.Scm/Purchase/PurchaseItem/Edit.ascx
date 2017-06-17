<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.PurchaseItem.Edit" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>
     
<div class="edit">
    <table class="tb">
    

      <tr>
               <td class="font">名称</td>
            <td class="text" >
              <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"   /> 
            </td>
              <td class="font">产品编号</td>
            <td class="text">
              <input id="txtProductId" runat="server"  type="text" class="input"  BindName="Product.Id" SaveName="Product.Id"    /> 
            </td>
       </tr>
        <tr>
             <td class="font">单价</td>
            <td class="text" >
                 <input id="txtPrice" runat="server"  type="text" class="input"  BindName="Price" SaveName="Price"    /> 
            </td>
             <td class="font">数量</td>
            <td class="text" >
                 <input id="txtCount" runat="server"  type="text" class="input"  BindName="Count" SaveName="Count"    /> 
            </td>
       </tr>
               <tr>
              <td class="font">类型</td>
            <td class="text">
               
                <uc1:GeneralDropDownList ID="ddlType" runat="server" ObjectName="Beeant.Domain.Entities.Purchase.PurchaseItemType" IsEnum="True" SaveName="Type" BindName="Type"/>
               
            </td>
           <td class="font">是否开票</td>
            <td class="text">
                <asp:CheckBox ID="ckIpOpen" runat="server"  BindName="IpOpen" SaveName="IpOpen" />
            </td>
        
       </tr>
        <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3">
                <input id="txtRemark" runat="server"  type="text" class="input long"  BindName="Remark" SaveName="Remark"  />
            </td>
        
       </tr>

         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
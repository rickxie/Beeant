<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderItem.Edit" %>

<div class="edit">
    <table class="tb">
              <tr>
            
            <td class="font">
                名称</td>
            <td class="text" colspan="3">
                <input id="txtName" runat="server"  type="text" class="input long"   BindName="Name" SaveName="Name"   />
                
            </td>
           
            
        </tr>
           <tr>
             <td class="font">
                单价</td>
            <td class="text">
                <input id="txtPrice" runat="server"  type="text" class="input"   BindName="Price" SaveName="Price"   />
            </td>
            <td class="font">
                数量</td>
            <td class="text">
                <input id="txtCount" runat="server"  type="text" class="input"   BindName="Count" SaveName="Count"   />
                
            </td>
           
            
        </tr>
    <tr>
         <td class="font">
                成本</td>
            <td class="text">
                <input id="txtCostAmount" runat="server"  type="text" class="input"   BindName="CostAmount" SaveName="CostAmount"   />
            </td>
             <td class="font">开票金额</td>
            <td class="text"  >
              <input id="txtInvoiceAmount" runat="server"  type="text" class="input"   BindName="InvoiceAmount" SaveName="InvoiceAmount"   />
             </td>
          
        </tr>
        <tr>
                  <td class="font">
                备注</td>
            <td class="text" colspan="3">
                <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
                
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
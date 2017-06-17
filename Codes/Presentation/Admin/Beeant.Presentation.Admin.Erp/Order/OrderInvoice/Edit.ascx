<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderInvoice.Edit" %>

<div class="edit">
    <table class="tb">
              <tr>
            
            <td class="font">
                名称</td>
            <td class="text" >
                <input id="txtName" runat="server"  type="text" class="input long"   BindName="Name" SaveName="Name"   />
                
            </td>
            <td class="font">
                发票号码</td>
            <td class="text" >
                <input id="txtNumber" runat="server"  type="text" class="input long"   BindName="Number" SaveName="Number"   />
                
            </td>
            
        </tr>
           <tr>
             <td class="font">
                金额</td>
            <td class="text">
                <input id="txtAmount" runat="server"  type="text" class="input"   BindName="Amount" SaveName="Amount"   />
            </td>
            <td class="font">
                备注</td>
            <td class="text">
                <input id="txtRemark" runat="server"  type="text" class="input"   BindName="Remark" SaveName="Remark"   />
                
            </td>
           
            
        </tr>
   
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
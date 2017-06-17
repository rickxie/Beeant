<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.PurchaseExpress.Edit" %>
     
<div class="edit">
    <table class="tb">
            <tr>
            
            <td class="font">
                运费</td>
            <td class="text" colspan="3">
                <input id="txtAmount" runat="server"  type="text" class="input"   BindName="Amount" SaveName="Amount"   />
                
            </td>
         
            
        </tr>
    <tr>
             <td class="font">快递公司</td>
            <td class="text"  >
              <input id="txtName" runat="server"  type="text" class="input long"   BindName="Name" SaveName="Name"   />
             </td>
              <td class="font">快递号</td>
            <td class="text"  >
              <input id="txtNumber" runat="server"  type="text" class="input long"   BindName="Number" SaveName="Number"   />
             </td>
        </tr>
         <tr>
            
            <td class="font">
                接收人</td>
            <td class="text">
                <input id="txtRecipient" runat="server"  type="text" class="input"   BindName="Recipient" SaveName="Recipient"   />
                
            </td>
            <td class="font">
                手机号码</td>
            <td class="text">
                <input id="txtMobile" runat="server"  type="text" class="input"   BindName="Mobile" SaveName="Mobile"   />
            </td>
            
        </tr>
         <tr>
               <td class="font">邮政编码</td>
            <td class="mtext" colspan="3"  >
              <input id="txtPostcode" runat="server"  type="text" class="input"   BindName="Postcode" SaveName="Postcode"   />
             </td>
        
           
        </tr>
        <tr>
             <td class="font">地址</td>
            <td class="mtext" colspan="3" >
              <input id="txtAddress" runat="server"  type="text" class="input long"   BindName="Address" SaveName="Address"   />
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
 
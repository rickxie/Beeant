<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderExpress.Edit" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
     
<div class="edit">
    <table class="tb">
           <tr>
            
            <td class="font">
                运费</td>
            <td class="text">
                <input id="txtAmount" runat="server"  type="text" class="input"   BindName="Amount" SaveName="Amount"   />
                
            </td>
            <td class="font">
                成本</td>
            <td class="text">
                <input id="txtCost" runat="server"  type="text" class="input"   BindName="Cost" SaveName="Cost"   />
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
            
            <td class="font">
                邮箱</td>
            <td class="text">
                <input id="txtEmail" runat="server"  type="text" class="input"   BindName="Email" SaveName="Email"   />
                
            </td>
            <td class="font">
                固定电话</td>
            <td class="text">
                <input id="txtTelephone" runat="server"  type="text" class="input"   BindName="Telephone" SaveName="Telephone"   />
            </td>
            
        </tr>
            <tr>
            
            <td class="font">
                公司名称</td>
            <td class="mtext" colspan="3">
                <input id="txtCompany" runat="server"  type="text" class="input"   BindName="Company" SaveName="Company"   />
                
            </td>
        </tr>
         <tr>
               <td class="font">邮政编码</td>
            <td class="text"  >
              <input id="txtPostcode" runat="server"  type="text" class="input"   BindName="Postcode" SaveName="Postcode"   />
             </td>
           <td class="font">是否确认</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsConfirmation" runat="server" BindName="IsConfirmation" SaveName="IsConfirmation"  />
           
             </td>
           
        </tr>
          <tr>
            <td class="font">确认时间</td>
            <td class="mtext" colspan="3" >
                <asp:TextBox ID="txtConfirmationTime" runat="server"  CssClass="input" BindName="ConfirmationTime" SaveName="ConfirmationTime" > </asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtConfirmationTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
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
 
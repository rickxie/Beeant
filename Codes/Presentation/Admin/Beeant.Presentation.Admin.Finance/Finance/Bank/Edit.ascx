<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Bank.Edit" %>
<%@ Register TagPrefix="uc1" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
     
<div class="edit">
    <table class="tb">
         <tr>
         <td class="font">开户人</td>
            <td class="text">
                <input id="txtHolder" runat="server"  type="text" class="input"  BindName="Holder" SaveName="Holder"  />
            </td>
            <td class="font">账户</td>
            <td class="mul" >
                
                 <uc1:AccountComboBox ID="cbAccount" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="font">银行名称</td>
            <td class="mtext" colspan="3">
              <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"   /> 
            </td>
           
        </tr>

         <tr>
           <td class="font">银行账户</td>
            <td class="mtext" colspan="3">
                <input id="txtNumber" runat="server"  type="text" class="input long"  BindName="Number" SaveName="Number"  />
            </td>
        
       </tr>
      <tr>
           <td class="font">联系人</td>
            <td class="mtext" colspan="3">
                <input id="txtLinkman" runat="server"  type="text" class="input long"  BindName="Linkman" SaveName="Linkman"  />
            </td>
       </tr>
       <tr>
           <td class="font">联系电话</td>
            <td class="mtext" colspan="3">
                <input id="txtTelephone" runat="server"  type="text" class="input long"  BindName="Telephone" SaveName="Telephone"  />
            </td>
       </tr>
       <tr>
           <td class="font">联系邮箱</td>
            <td class="mtext" colspan="3">
                <input id="txtEmail" runat="server"  type="text" class="input long"  BindName="Email" SaveName="Email"  />
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
 
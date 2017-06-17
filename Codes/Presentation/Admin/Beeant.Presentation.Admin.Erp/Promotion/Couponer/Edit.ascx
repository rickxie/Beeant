<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Promotion.Couponer.Edit" %>
     
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc2" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
 
     
 
<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3" >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
            </td>
            
        </tr>
 
         <tr>
          <td class="font">账户</td>
            <td class="mul">
                <uc2:AccountComboBox ID="cbAccount" runat="server" BindName="Account.Id" SaveName="Account.Id"  />
            </td>
           <td class="font">面值</td>
            <td class="text"  >
               <input id="txtAmount" runat="server"  type="text" class="input"   BindName="Amount" SaveName="Amount"   />
            </td>
             
        </tr>
        <tr>
          <td class="font">截止日期</td>
            <td class="text">
                <asp:TextBox ID="txtEndDate" runat="server" BindName="EndDate" SaveName="EndDate" CssClass="input"  ></asp:TextBox>
                 <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
           <td class="font">数量</td>
            <td class="text"  >
               <input id="txtCount" runat="server"  type="text" class="input"   BindName="Count" SaveName="Count"   />
            </td>
             
        </tr>
          <tr>
          <td class="font">领取截止日期</td>
            <td class="text">
                <asp:TextBox ID="txtCollectEndDate" runat="server" BindName="CollectEndDate" SaveName="CollectEndDate" CssClass="input"  ></asp:TextBox>
                 <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCollectEndDate" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
           <td class="font">领取数量</td>
            <td class="text"  >
               <input id="txtCollectCount" runat="server"  type="text" class="input"  value="1"  BindName="CollectCount" SaveName="CollectCount"   />
            </td>
             
        </tr>
           <tr>
          <td class="font">是否需要密码</td>
            <td class="mul">
                <asp:CheckBox ID="ckIsCode" runat="server"  BindName="IsCode" SaveName="IsCode"  />
            </td>
           <td class="font">是否启用</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsUsed" runat="server" BindName="IsUsed" SaveName="IsUsed"  />
            </td>
             
        </tr>
         <tr>
            <td class="font">是否显示</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsShow" runat="server" BindName="IsShow" SaveName="IsShow"  />
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
 
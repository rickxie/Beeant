<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.Purchase.Update"
    MasterPageFile="~/Main.Master" ValidateRequest="false" %>

<%@ Register Src="/Controls/Message.ascx" TagName="Message" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc3" TagName="UserComboBox" Src="~/Controls/User/UserComboBox.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GeneralZTreeView_1" Src="~/Controls/GeneralZTreeView.ascx" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<%@ Register TagPrefix="uc4" TagName="StorehouseComboBox" Src="~/Controls/Wms/StorehouseComboBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>采购单编辑</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <input id="hfStatusControl" type="hidden" runat="server" />
    <div class="edit">
        <a href='add.aspx?id=<%=RequestId%>' name="Add">新增</a> <a href='Detail.aspx?id=<%=RequestId%>'
            name="Entity">详情</a> <a href='handle.aspx?id=<%=RequestId%>' name="Edit">处理</a>
        <table class="tb">
            <tr>
                <td class="font">
                    账户
                </td>
                <td class="mul mtext">
                    <uc8:AccountComboBox ID="AccountComboBox1" runat="server" />
                </td>
                <td class="font">
                    订单编号
                </td>
                <td class="text">
                    <input id="txtOrderId" runat="server" type="text" class="input" bindname="Order.Id"
                        savename="Order.Id" />
                </td>
            </tr>
            <tr>
                  <td class="font">采购类型</td>
                <td class="text"  >
                <uc10:GeneralDropDownList ID="ddlType" runat="server" SaveName="Type" BindName="Type" ObjectName="Beeant.Domain.Entities.Purchase.PurchaseType" IsEnum="True" />
                </td>
                <td class="font">
                    跟单人
                </td>
                <td class="mul mtext">
                    <uc3:UserComboBox ID="ckUser" runat="server" IsValidateHidden="False" HiddenValidateName=""
                        HiddenSaveName="Follow.Id" HiddenBindName="Follow.Id" TextBindName="Follow.RealName"
                        TextSaveName="Follow.RealName" />
                </td>
            </tr>
            <tr>
                <td class="font">
                    交货日期
                </td>
                <td class="text">
                    <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="input" BindName="DeliveryDate"
                        SaveName="DeliveryDate"> </asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDeliveryDate"
                        Format="yyyy-MM-dd">
                    </cc1:CalendarExtender>
                </td>
                <td class="font">
                    采购日期
                </td>
                <td class="text">
                    <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="input" BindName="PurchaseDate"
                        SaveName="PurchaseDate"> </asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPurchaseDate"
                        Format="yyyy-MM-dd">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="font">
                    消息
                </td>
                <td class="mtext">
                    <asp:CheckBoxList ID="ckMessageType" runat="server">
                    </asp:CheckBoxList>
                </td>
                <td class="font">
                    仓库
                </td>
                <td class="text">
                 
                    
                    <uc4:StorehouseComboBox ID="StorehouseComboBox1" runat="server" IsUsed="True" IsWarehouse="True" />
                </td>
            </tr>
            <tr>
                <td class="font">
                    备注
                </td>
                <td class="mtext" colspan="3">
                    <input id="txtRemark" runat="server" type="text" class="input long" bindname="Remark"
                        savename="Remark" />
                </td>
            </tr>
            <tr>
                <td class="font">
                    流程备注
                </td>
                <td class="mtext" colspan="3">
                    <input id="txtHistoryRemark" runat="server" type="text" class="input long" />
                </td>
            </tr>
            <tr>
                <td class="center" colspan="4">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="保存" />
                    <input id="btnClose" type="button" value="关闭" class="btn" />
                </td>
            </tr>
        </table>
          <uc2:message id="Message1" runat="server" />
    </div>

  

             
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

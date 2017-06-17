<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Contract.Edit" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>  
<%@ Register src="../../Controls/Uploader.ascx" tagname="Uploader" tagprefix="uc4" %>
   

<div class="edit">
    <table class="tb">
         <tr>
             
              <td class="font">结算方式</td>
            <td class="text" >
                <input id="txtSettlementType" type="text" runat="server" SaveName="SettlementType" BindName="SettlementType" />
               
              </td>
                <td class="font">支付方式</td>
            <td class="text"   >
                <uc1:GeneralDropDownList ID="ddlPaymentType"  runat="server" SaveName="PaymentType" BindName="PaymentType" 
                     ObjectName="Beeant.Domain.Entities.Supplier.ContractPaymentType" IsEnum="True" ValidateName="PaymentTypeName" />
              </td>
        </tr>
         
        <tr>
           <td class="font">配送方式</td>
            <td class="text" >
                <uc1:GeneralDropDownList ID="ddlDispatchType"  runat="server" SaveName="DispatchType" BindName="DispatchType" 
                     ObjectName="Beeant.Domain.Entities.Supplier.ContractDispatchType" IsEnum="True" ValidateName="DispatchTypeName" />
            </td>
            <td class="font">发票类型</td>
            <td class="text" >
                <uc1:GeneralDropDownList ID="ddlBillType"  runat="server" SaveName="BillType" BindName="BillType" 
                     ObjectName="Beeant.Domain.Entities.Supplier.ContractBillType" IsEnum="True" ValidateName="BillTypeName" />
            </td>
        </tr>
        <tr>
              <td class="font">合同开始日期</td>
            <td class="text">
            <asp:TextBox ID="txtStartDate" runat="server"  CssClass="input" BindName="StartDate" SaveName="StartDate" > </asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
              <td class="font">合同结束日期</td>
                <td class="text">
                    <asp:TextBox ID="txtEndDate" runat="server"  CssClass="input" BindName="EndDate" SaveName="EndDate" > </asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
                </td>
        </tr>
        
        

          <tr>
            <td class="font">返利条件说明</td>
            <td class="text" colspan="3">
                <input id="txtRebate" runat="server"  type="text" class="input long"   BindName="Rebate" SaveName="Rebate"/></td>
        </tr>
      
        <tr>
           <td class="font">合同附件</td>
           <td class="text"  colspan="3" >
                 <uc4:Uploader ID="Uploader1" runat="server" Path="Files/Documents/Supplier/" IsShowViewControl="False" FileByteSaveName="AttachmentByte" FileNameBindName="Attachment"  FileNameSaveName="Attachment" FullFileNameBindName="FullAttachment"  />
            </td>
        </tr>

         <tr>
            <td colspan="4" class="center">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  />
             <input id="hidSupplierId" type="hidden" runat="server" BindName="Supplier.Id" SaveName="Supplier.Id"/>
            </td>
        </tr>
        
    </table>
 
</div>
<script type="text/javascript" src="/Scripts/winner/ComboBox/Winner.ComboBox.js"></script>
 <script type="text/javascript">
     $(document).ready(function () {
         var settleCombobox = new Winner.ComboBox("<%=txtSettlementType.ClientID %>", "");
         settleCombobox.Initialize();
         settleCombobox.GetEntities = function () {
             return [
                 { Text: "货到且票到7天", Value: "货到且票到7天" }
             , { Text: "货到且票到15天", Value: "货到且票到15天" }
             , { Text: "货到且票到30天", Value: "货到且票到30天" }
             , { Text: "货到且票到60天", Value: "货到且票到60天" }
             , { Text: "货到且票到90天", Value: "货到且票到90天" }
             , { Text: "月结", Value: "月结" }
             , { Text: "双月结", Value: "双月结" }
             , { Text: "双月结15天", Value: "双月结15天" }
             , { Text: "月结15天", Value: "月结15天" }
             , { Text: "月结30天", Value: "月结30天" }
             , { Text: "月结45天", Value: "月结45天" }
             , { Text: "月结60天", Value: "月结60天" }
             , { Text: "月结90天", Value: "月结90天" }
             , { Text: "月结120天", Value: "月结120天" }
             ];
         };
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
     });
 </script>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Contract.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>    
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>合同详情详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">供应商名称</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="lblName" runat="server"  BindName="Supplier.Name"></asp:Label>
             </td>
            
        </tr>
         <tr>
            <td class="font">结算方式</td>
            <td class="text" >
                <asp:Label ID="lblSettlementType" runat="server" BindName="SettlementType"></asp:Label>
            </td>
              <td class="font">支付方式</td>
            <td class="text" >
                <asp:Label ID="lblPaymentTypeName" runat="server" BindName="PaymentTypeName"></asp:Label>
             </td>
        </tr> 
       
        <tr>
          <td class="font">配送方式</td>
            <td class="text" >
                <asp:Label ID="lblDispatchTypeName" runat="server" BindName="DispatchTypeName"></asp:Label>
            </td>
             <td class="font">发票类型</td>
            <td class="text" >
                <asp:Label ID="lblBillTypeName" runat="server" BindName="BillTypeName"></asp:Label>
            </td>
        </tr>
         <tr>
             <td class="font">合同起始日期</td>
            <td class="text" >
                <asp:Label ID="lblStartDate" runat="server" BindName="StartDate"></asp:Label>
            </td>
           <td class="font">合同结束日期</td>
            <td class="text" >
                <asp:Label ID="lblEndDate" runat="server" BindName="EndDate"></asp:Label>
            </td>
               
        </tr>
        <tr>
            <td class="font">返利条件说明</td>
            <td class="text" colspan="3">
                 <asp:Label ID="lblRebate" runat="server" BindName="Rebate"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="font">合同附件</td>
            <td class="text" colspan="3">
                <a href="" runat="server" id="hrefAttachment" BindName="FullAttachment">附件下载</a>
            </td>
        </tr>
        <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
    
     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
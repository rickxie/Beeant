<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Delivery.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>配送站详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">名称</td>
            <td class="text"  >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
               <td class="font">城市</td>
            <td class="text" >
                <asp:Label ID="lblCity" runat="server" BindName="City"></asp:Label>
             </td>
        </tr>
      <tr>
            <td class="font">限量</td>
            <td class="text"  >
                <asp:Label ID="lblLimitCount" runat="server"  BindName="LimitCount"></asp:Label>
             </td>
               <td class="font">是否启用</td>
            <td class="text" >
                <asp:Label ID="lblIsUsed" runat="server" BindName="IsUsed"></asp:Label>
             </td>
        </tr>
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="lblRemark" runat="server" BindName="Remark""></asp:Label>
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
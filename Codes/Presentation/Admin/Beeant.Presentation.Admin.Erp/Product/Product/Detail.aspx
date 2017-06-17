<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Product.Detail" MasterPageFile="~/Datum.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>供应链产品信息</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
            
        </tr>


           <tr>
            <td class="font">毛利率警戒线</td>
            <td class="text" >
                <asp:Label ID="Label5" runat="server" BindName="Goods.Category.Rate"></asp:Label>
            </td>
           <td class="font">毛利率</td>
            <td class="text" >
                <asp:Label ID="lblPrice" runat="server" BindName="Price"></asp:Label>/<asp:Label ID="Label3" runat="server" BindName="PriceRate"></asp:Label>%
            </td>
        </tr>
      
        
      

        <tr>
           
            <td class="font">类目</td>
            <td class="text" >
                <asp:Label ID="lblCatagoryName" runat="server"  BindName="Goods.Category.Name"></asp:Label>
             </td>
        </tr>
        
        <tr>
            <td class="font">销售状态</td>
            <td class="text" >
                <asp:Label ID="lblIsSalesName" runat="server" BindName="IsSalesName"></asp:Label>
            </td>
                 <td class="font">销售库存</td>
            <td class="text"  >
                <asp:Label ID="Label6" runat="server" BindName="Count"></asp:Label>
            </td>
            
        </tr>
          <tr>
            <td class="font">是否支持退换</td>
            <td class="text" >
                <asp:Label ID="lblIsReturn" runat="server" BindName="IsReturnName"></asp:Label>
            </td>
              <td class="font">是否支持定制</td>
            <td class="text"  >
                <asp:Label ID="lblIsCustomName" runat="server" BindName="IsCustomName"></asp:Label>
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
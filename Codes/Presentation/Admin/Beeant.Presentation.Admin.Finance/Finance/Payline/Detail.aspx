<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payline.Detail" MasterPageFile="~/Main.Master"%>

<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>支付详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">付款金额</td>
            <td class="text" >
                <asp:Label ID="lblAmount" runat="server"  BindName="Amount"></asp:Label>
             </td>
            <td class="font">支付平台</td>
            <td class="text" >
                <asp:Label ID="lblTypeName" runat="server" BindName="TypeName"></asp:Label>
             </td>
        </tr>
         <tr>
            <td class="font">外部编号</td>
            <td class="text" >
                <asp:Label ID="lblDataId" runat="server" BindName="DataId"></asp:Label>
             </td>
              <td class="font">是否有效</td>
            <td class="text" >
                <asp:Label ID="lblIsStatusName" runat="server" BindName="IsStatusName"></asp:Label>
             </td>
        </tr>
             
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
  <div class="subtitle" onclick="SetEntityBody('divpgPaylineItem')">订单信息(<span class="count"><%=pgPaylineItem.DataCount%></span>)
        </div>
       <div id="divpgPaylineItem" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
 
      <asp:GridView ID="gvOrder" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
             <asp:TemplateField HeaderText="订单编号"  ItemStyle-CssClass="center xlstext">
            <ItemTemplate>
                 <a href='/Order/Order/Detail.aspx?id=<%#Eval("Order.Id")%>' target="_blank"><%#Eval("Order.Id")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="right xlsfloat">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
           
  
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgPaylineItem" runat="server" PageSize="10"  
     SelectExp="Id,Order.Id,Amount" 
     FromExp="Beeant.Domain.Entities.Finance.PaylineItemEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Payline.Id==@Id" />

        </ContentTemplate>
        </asp:UpdatePanel>
      </div>
                          
       <uc3:Progress ID="Progress1" runat="server" />
         
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
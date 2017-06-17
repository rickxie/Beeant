<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Promotion.Promotion.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>活动详情</title>  
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
            
              <td class="font">开始日期</td>
              <td class="text">
                 <asp:Label ID="lblStartDate" runat="server" Text=""  BindName="StartDate" Format="yyyy-MM-dd"></asp:Label>
                 
            </td>
            <td class="font">截止日期</td>
            <td class="text" >
                 <asp:Label ID="lblEndDate" runat="server" Text=""  BindName="EndDate" Format="yyyy-MM-dd"></asp:Label>
                 
            </td> 
        </tr>
         <tr>
          
             <td class="font">开始时间</td>
            <td class="text" >
                 <asp:Label ID="lblStartTime" runat="server" BindName="StartTime" DateFormat="HH-mm-ss"></asp:Label>

            </td>
             <td class="font">结束时间</td>
            <td class="text"  >
                <asp:Label ID="lblEndTime" runat="server" Text="" BindName="EndTime" ></asp:Label>
                </td>
                </tr>
         <tr>
             <td class="font">月份</td>
               <td class="text"  >
                <asp:Label ID="lblMonths" runat="server" BindName="Months"></asp:Label>
             </td>
           <td class="font">周期</td>
               <td class="text"  >
                <asp:Label ID="Weeks" runat="server" BindName="Weeks"></asp:Label>
             </td>
       
        </tr>
 
         <tr>
            <td class="font">支付方式</td>
            <td class="text" colspan="3"  >
                <asp:Label ID="lblPayType" runat="server"  BindName="PayType"></asp:Label>
             </td>
             </tr>
                 <tr>
            <td class="font">活动价格</td>
            <td class="text"  >
                <asp:Label ID="lblPrice" runat="server"  BindName="Price"></asp:Label>
             </td>
         <td class="font">限购</td>
            <td class="text"  >
                <asp:Label ID="lblOrderLimitCount" runat="server"  BindName="OrderLimitCount"></asp:Label>
             </td>
             </tr>
         <tr>
            <td class="font">备注</td>
            <td class="text" colspan="3"  >
                <asp:Label ID="lblRemark" runat="server"  BindName="Remark"></asp:Label>
             </td>
             </tr>
       
            
     
     </table>
          <div class="subtitle" onclick="SetEntityBody('divQualification')">活动明细(<span class="count"><%=suQualification.DataCount%></span>)
        </div>
       <div id="divQualification" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     
           <asp:GridView ID="gvQualification" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
           <asp:TemplateField HeaderText="活动编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="产品ID"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Promotion.Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="产品名称"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Promotion.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="活动面价" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Price")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="活动底价" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Cost")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否启用"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("IsUsedName")%>
            </ItemTemplate>
        </asp:TemplateField>
            
        </Columns>
     </asp:GridView>

       <uc1:Pager ID="suQualification" runat="server" PageSize="10"  
                     SelectExp="Id,Product.Id,Price,Cost,Product.Name"
                     FromExp="PromotionItemEntity"
                     OrderByExp="UpdateTime desc" WhereExp="Promotion.Id==@id" />
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>

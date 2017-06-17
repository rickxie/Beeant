<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Promotion.Couponer.Detail" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Basedata" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>优惠券模板详情</title>  
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
               <td class="font">面值</td>
            <td class="text" >
                <asp:Label ID="lblAmount" runat="server" BindName="Amount"></asp:Label>
             </td>
        </tr>
  
       
        <tr>
          <td class="font">截止日期</td>
            <td class="text" >
                <asp:Label ID="lblEndDate" runat="server" BindName="EndDate"></asp:Label>
            </td>
            <td class="font">数量</td>
            <td class="text" >
                <asp:Label ID="lblCount" runat="server" BindName="Count"></asp:Label>
            </td>
        </tr>
             <tr>
            <td class="font">领取截止日期</td>
            <td class="text" >
             <asp:Label ID="lblCollectEndDate" runat="server" BindName="CollectEndDate"></asp:Label>
            </td>
            <td class="font">数量</td>
            <td class="text" >
                <asp:Label ID="lblCollectCount" runat="server" BindName="CollectCount"></asp:Label>
            </td>
        </tr>
           <tr>
            <td class="font">是否需要密码</td>
            <td class="text" >
             <asp:Label ID="lblIsCodeName" runat="server" BindName="IsCodeName"></asp:Label>
            </td>
            <td class="font">是否启用</td>
            <td class="text" >
                <asp:Label ID="lblIsUsedName" runat="server" BindName="IsUsedName"></asp:Label>
            </td>
        </tr>
            <td class="font">是否显示</td>
            <td class="text" >
                <asp:Label ID="lblIsShowName" runat="server" BindName="IsShowName"></asp:Label>
            </td>
           <tr>
           
              <td class="font">账户信息</td>
             <td class="text" > <a href="/Finance/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" BindName="Account.Id"> <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label></a></td>
         </tr>
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="lblRemark" runat="server" BindName="Remark"></asp:Label>
            </td>
            
        </tr>
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
     <div class="subtitle" onclick="SetEntityBody('divCoupon')">优惠券信息(<span class="count"><%=pgCoupon.DataCount%></span>)</div>
       <div id="divCoupon" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
 
   
           <asp:GridView ID="gvCoupon" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>

         
        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="面值"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="截止日期"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("EndDate","{0:yyyy-MM-dd}")%>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="领取时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("CollectTime")%>   
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="使用时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("UsedTime")%>   
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否使用"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsUsedName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="密码"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Code")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="所属账户"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Id")%></a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="使用订单"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <a href='/Order/Order/Detail.aspx?id=<%#Eval("Order.Id") %>' target="_blank"><%#Eval("Order.Id")%></a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgCoupon" runat="server" PageSize="10"  
     SelectExp="Id,Name,Amount,EndDate,CollectTime,UsedTime,IsUsed,Code,Account.Id,Order.Id,Remark,InsertTime,UpdateTime" 
     FromExp="Beeant.Domain.Entities.Member.CouponEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Couponer.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
         

     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Account.Account.Detail" MasterPageFile="~/Datum.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>账户详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">用户名</td>
            <td class="text" >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
             <td class="font">真实姓名</td>
            <td class="text" >
                <asp:Label ID="lblRealName" runat="server" BindName="RealName"></asp:Label>
             </td>
        </tr>
         <tr>
            <td class="font">手机号码</td>
            <td class="text" >
                <asp:Label ID="lblMobile" runat="server" BindName="Mobile"></asp:Label>
             </td>
              <td class="font">电子邮箱</td>
            <td class="text" >
                <asp:Label ID="lblEmail" runat="server" BindName="Email"></asp:Label>
             </td>
        </tr>
      
        
         <tr>
                <td class="font">余额</td>
            <td class="text" >
                 <asp:Label ID="lblBalance" runat="server" BindName="Balance"></asp:Label>
            </td>
              <td class="font">是否启用</td>
            <td class="text" >
                <asp:Label ID="lbIsUsedName" runat="server" BindName="IsUsedName"></asp:Label>                  
            </td>
        </tr>
         
      
         
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
 <div class="subtitle" onclick="SetEntityBody('divAccountItem')">流水账(<span class="count"><%=pgAccountItem.DataCount%></span>)</div>
       <div id="divAccountItem" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="mtext"><asp:TextBox ID="txtBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="mtext"><asp:TextBox ID="txtEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
    
            <td >
                <asp:Button ID="Button1" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
   
           <asp:GridView ID="gvAccountItem" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>

         <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>

           <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="相关单据"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("DataId")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgAccountItem" runat="server" PageSize="10"  SelectExp="Id,Status,Amount,DataId,Remark,InsertTime" 

     FromExp="Beeant.Domain.Entities.Account.AccountItemEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Account.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
         
    

       <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
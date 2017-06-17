<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Property.Detail" MasterPageFile="~/Datum.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>属性详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3"  >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
         
             
        </tr>
         <tr>
              <td class="font">类型</td>
               <td class="text"  >
                <asp:Label ID="lblType" runat="server" BindName="TypeName"></asp:Label>
             </td>
           
             <td class="font">搜索类型</td>
               <td class="text"  >
                <asp:Label ID="lblSearchTypeName" runat="server" BindName="SearchTypeName"></asp:Label>
             </td>
        </tr>
          <tr>
              <td class="font">排序</td>
               <td class="text"  >
                <asp:Label ID="lblSort" runat="server" BindName="Sequence"></asp:Label>
             </td>
             <td class="font">自定义属性个数</td>
               <td class="text"  >
                <asp:Label ID="lblCustomCount" runat="server" BindName="CustomCount"></asp:Label>
             </td>
        </tr>
        <tr>
              <td class="font">是否SKU</td>
               <td class="text"  >
                <asp:Label ID="lblIsSku" runat="server" BindName="IsSkuName"></asp:Label>
             </td>
             <td class="font">是否启用</td>
               <td class="text"  >
                <asp:Label ID="lblIsUsed" runat="server" BindName="IsUsedName"></asp:Label>
             </td>
        </tr>
        <tr>
           <td class="font">类目</td>
            <td class="text"    >
                <asp:Label ID="lblCatagoryName" runat="server"  BindName="Category.Name"></asp:Label>
             </td>
              <td class="font">标签</td>
            <td class="text">
                <asp:Label ID="lblTag" runat="server"  BindName="Tag"></asp:Label>
             </td>
            </tr>
          <tr>
              <td class="font">错误提示</td>
               <td class="mtext" colspan="3"  >
                <asp:Label ID="lblMessage" runat="server" BindName="Message"></asp:Label>
             </td>
             
        </tr>
         <tr>
              <td class="font">值</td>
               <td class="mtext" colspan="3"  >
                <asp:Label ID="lblValue" runat="server" BindName="Value"></asp:Label>
             </td>
             
        </tr>
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     

  <div class="subtitle" onclick="SetEntityBody('divPropertyRule')">规则列表(<span class="count"><%=pgPropertyRule.DataCount%></span>)</div>
       <div id="divPropertyRule" style="display: none;" >
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
   
           <asp:GridView ID="gvPropertyRule" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
         <asp:TemplateField HeaderText="规则"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Rule.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="参数"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Paramter")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="验证类型"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否多行验证"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsMultilineName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否区分大小写"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsIgnoreCaseName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgPropertyRule" runat="server" PageSize="10"  
     SelectExp="Id,Rule.Name,Paramter,Type,IsMultiline,IsIgnoreCase,InsertTime"
      FromExp="PropertyRuleEntity"
      OrderByExp="UpdateTime desc" WhereExp="Property.Id==@Id" />
     
      
          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
         
      <uc3:Progress ID="Progress1" runat="server" />
                </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
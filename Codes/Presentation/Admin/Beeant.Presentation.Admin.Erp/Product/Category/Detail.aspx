<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Category.Detail" MasterPageFile="~/Datum.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>类目详情</title>  
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
              <td class="font">拼音</td>
               <td class="mtext" colspan="3" >
                <asp:Label ID="lblPinyin" runat="server" BindName="Pinyin"></asp:Label>
             </td>
        </tr>
         <tr>
              <td class="font">链接地址</td>
               <td class="mtext" colspan="3">
                <asp:Label ID="lblUrl" runat="server" BindName="Url"></asp:Label>
             </td>
        </tr>
       <tr>
              <td class="font">首字母</td>
               <td class="text" >
                <asp:Label ID="lblInitial" runat="server" BindName="Initial"></asp:Label>
             </td>
               <td class="font">是否允许发布</td>
               <td class="text"  >
                <asp:Label ID="lblIsPublish" runat="server" BindName="IsPublishName"></asp:Label>
             </td>
           </tr>
            <tr>
              <td class="font">排序</td>
               <td class="text"  >
                <asp:Label ID="lblSort" runat="server" BindName="Sequence"></asp:Label>
             </td>
              <td class="font">父类</td>
               <td class="text"  >
                <asp:Label ID="lblParentName" runat="server" BindName="Parent.Name"></asp:Label>
             </td>
        </tr>
          <tr>
              <td class="font">是否展示</td>
               <td class="text"  >
                <asp:Label ID="lblIsShow" runat="server" BindName="IsShowName"></asp:Label>
             </td>
              <td class="font">图片数量</td>
               <td class="text"  >
                <asp:Label ID="lblImageCount" runat="server" BindName="ImageCount"></asp:Label>
             </td>
        </tr>
          <tr>
             <td class="font">录入时间</td>
               <td class="text"  >
                <asp:Label ID="lblInsertTime" runat="server" BindName="InsertTime"></asp:Label>
             </td>
             <td class="font">编辑时间</td>
               <td class="text"  >
                <asp:Label ID="lblUpdateTime" runat="server" BindName="UpdateTime"></asp:Label>
             </td>
        </tr>
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
 
 
   <div class="subtitle" onclick="SetEntityBody('divProperty')">属性列表(<span class="count"><%=pgProperty.DataCount%></span>)</div>
       <div id="divProperty" style="display: none;" >
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
            <td class="font">类型</td>
            <td class="mtext">
                <uc2:GeneralDropDownList ID="ddlPropertyType" runat="server" SearchWhere="Type==@Type" SearchPropertyTypeName="Type" SearchParamterName="Type" ObjectName="Beeant.Domain.Entities.Product.PropertyType" IsEnum="True"  />
            </td>
            <td >
                <asp:Button ID="Button1" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
   
           <asp:GridView ID="gvProperty" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
       
         <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                  <a href='/Product/Property/Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Property"> <%#Eval("Name")%></a>
            </ItemTemplate>
        </asp:TemplateField>
      
         <asp:TemplateField HeaderText="类型"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
            
        
    
         <asp:TemplateField HeaderText="搜索类型"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("SearchTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="标签"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Tag")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="错误提示"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Message")%>
            </ItemTemplate>
        </asp:TemplateField>

        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgProperty" runat="server" PageSize="10"  
     SelectExp="Id,Name,Type,SearchType,Tag,Message"
      FromExp="Beeant.Domain.Entities.Product.PropertyEntity,Beeant.Domain.Entities"
      OrderByExp="UpdateTime desc" WhereExp="Category.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
           <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
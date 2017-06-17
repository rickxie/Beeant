<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Cms.Cms.Class.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>信息类目详情</title>  
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
              <td class="font">排序</td>
            <td class="text"  >
                <asp:Label ID="lblSort" runat="server"  BindName="Sequence"></asp:Label>
             </td>
        </tr>
         <tr>
              <td class="font">标签</td>
            <td class="mtext"  colspan="3" >
                <asp:Label ID="lblTag" runat="server"  BindName="Tag"></asp:Label>
             </td>
        </tr>
        <tr>
            <td class="font">父类</td>
               <td class="text"   >
                <asp:Label ID="lblParentName" runat="server" BindName="Parent.Name"></asp:Label>
             </td>
              <td class="font">是否启用</td>
               <td class="text"  >
                <asp:Label ID="lblIsUsed" runat="server" BindName="IsUsedName"></asp:Label>
             </td>
        </tr>
          <tr>
            <td class="font">是否公开</td>
               <td class="text"   >
                <asp:Label ID="lblIsPublic" runat="server" BindName="IsPublicName"></asp:Label>
             </td>
              <td class="font">是否允许发布</td>
               <td class="text"  >
                <asp:Label ID="lblIsPublish" runat="server" BindName="IsPublishName"></asp:Label>
             </td>
        </tr>
         <tr>
            <td class="font">备注</td>
            <td class="mtext" colspan="3"  >
                <asp:Label ID="lblRemark" runat="server"  BindName="Remark"></asp:Label>
             </td>
        </tr>
        
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
     
 <div class="subtitle" onclick="SetEntityBody('divContent')">信息内容列表(<span class="count"><%=pgContent.DataCount%></span>)</div>
       <div id="divContent" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="mtext"><asp:TextBox ID="txtContentBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtContentBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="mtext"><asp:TextBox ID="txtContentEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtContentEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>

             <td class="font">标题</td>
            <td class="mtext">
            <asp:TextBox ID="txtTitle" runat="server" CssClass="seinput"  SearchWhere="Title.Contains(@Title)" SearchParamterName="Title"></asp:TextBox>
            </td>
            <td >
                <asp:Button ID="Button1" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
   
           <asp:GridView ID="gvContent" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
           <a href='/Cms/Content/Detail.aspx?id=<%#Eval("Id") %>' target="_blank"><%#Eval("Id")%></a> 
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="标题"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Title")%>
            </ItemTemplate>
        </asp:TemplateField>

      <asp:TemplateField HeaderText="排序"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Sequence")%>
            </ItemTemplate>
        </asp:TemplateField>
           
      <asp:TemplateField HeaderText="是否显示"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsShowName")%>
            </ItemTemplate>
        </asp:TemplateField>
      <asp:TemplateField HeaderText="操作用户"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="描述"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Description")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgContent" runat="server" PageSize="10"  
     SelectExp="Id,Title,Sequence,IsShow,User.RealName,Description,InsertTime"
      FromExp="Beeant.Domain.Entities.Cms.ContentEntity,Beeant.Domain.Entities"
      OrderByExp="UpdateTime desc" WhereExp="Class.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>   
         
 
          
           <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Home.Desktop.Notice.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>公告查询</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
                <uc2:DataSearch ID="DataSearch1" runat="server" />
            
        
        </tr>
   
     </table>
        </div>

        <div class="mainten">
             <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
      <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
          <asp:TemplateField HeaderText="标题"  ItemStyle-CssClass="left">
            <ItemTemplate>
               <a href='Detail.aspx?id=<%#Eval("Id")%>' target="_blank"><%#Eval("Title")%></a> 
            </ItemTemplate>
        </asp:TemplateField>

     
        <asp:TemplateField HeaderText="发布人"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Title,User.RealName,InsertTime" FromExp="ContentEntity" OrderByExp="UpdateTime desc" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
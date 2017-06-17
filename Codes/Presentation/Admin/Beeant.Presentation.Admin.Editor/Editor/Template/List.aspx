<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Editor.Editor.Template.List" MasterPageFile="~/None.Master" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
 
  <%@ Register src="../FolderList.ascx" tagname="FolderList" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>编辑器模板选择</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

 <div class="fullbody">

       <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
  <div id="finder" class="finder">
    


     <div class="mainten">
                <table>
                    <tr>
                        <td><a href="/Editor/Folder/List.aspx" target="_blank">目录管理</a></td>
                     
                      
                        <td>
                            <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="button"></asp:Button>
                            <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
                        </td>
                    </tr>
                </table>
               </div>
     
      <div class="content">
           <div class="left">
             <ul class="sidebar" id="sidebar">
            <li><a href='<%=litFolder.GetUrl(0)%>'>所有</a></li>    
              <uc5:FolderList ID="litFolder" runat="server" FolderType="Template" IsLink="True"/>
     
        </ul>
      </div>
      <div class="right">
          <div class="images">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <div class="element long">
                      <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
                     <div class="out" Finder="Element" Url='/Editor/Template/Detail.aspx?id=<%#Eval("Id") %>'>
                         <iframe frameborder="0" src='/Editor/Template/Detail.aspx?id=<%#Eval("Id") %>' width="300" height="200"></iframe>
                        <div class="font"><%#Eval("Name") %></div>
                    </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <uc1:Pager ID="Pager1" runat="server" PageSize="6"   SelectExp="Id,Name,InsertTime" FromExp="TemplateEntity" OrderByExp="UpdateTime desc" />
        <uc3:Progress ID="Progress1" runat="server" />
     </div>
     </div>
<div Finder="RightMenu" class="rightmenu" >
    <div Finder="Select" class="selectbtn"><a href="javascript:void(0);">选择</a> </div>
    <div Finder="Browse" class="browsebtn"><a href="javascript:void(0);">浏览</a> </div>
     <div ><a href="javascript:void(0);"  class="browsebtn" style="float: left;" id="hfMoveSwitcher">移动</a>
        <ul id="hfMoveContainer" style="float: left;background: #ffe4e1;width: 130px;padding-left: 20px;display: none;">
            <uc5:FolderList ID="litMoveFolder" runat="server" FolderType="Template" IsLink="False"/>
        </ul>
         
     </div>
</div>


</div>
 
 
             </ContentTemplate>
 </asp:UpdatePanel>
 </div>
 </asp:Content>
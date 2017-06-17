<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Editor.Editor.Flash.List" MasterPageFile="~/None.Master" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc4" TagName="Message" Src="~/Controls/Message.ascx" %>

   <%@ Register src="../FolderList.ascx" tagname="FolderList" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>编辑器flash选择</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

 <div class="fullbody">
  
 
          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
  <div id="finder" class="finder">
      <div class="top">
          
                      <input id="Button1" type="button" value="上传" Finder="UploadSwitch" class="button" />
          <div Finder="UploaderContent" class="uploadercontent" style="height: 250px;">
          <table>
              <tr>
                  <td> <uc2:Uploader ID="Uploader1" runat="server" IsShowViewControl="False" Path="" FileByteSaveName="FileByte" FileNameBindName="Name"  FileNameSaveName="FileName"  FullFileNameBindName="FullFileName"  /></td>
                  <td>  <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button" /></td>
              </tr>
             <tr>
                   <td style="text-align: center;"><asp:Button ID="Button2" runat="server" Text="保存" CssClass="button"  /></td></td>
               </tr>
               <tr>
                   <td style="text-align: center;">  <uc4:Message ID="Message1" runat="server" />
                       
                   </td>
               </tr>
          </table>
           
                          
           </div> 
               
      </div>


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
            <li><a href='<%=litFolder.GetUrl(0)%>'>未分组</a></li>    
              <uc5:FolderList ID="litFolder" runat="server" FolderType="Flash" IsLink="True"/>
 
        </ul>
      </div>
      <div class="right">
          <div class="images">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <div class="element long" >
                      <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
                     <div class="out" Finder="Element" Url='<%#Eval("FullFileName") %>'>
                         <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="300" height="200">
  <param name="movie" value='<%#Eval("FullFileName") %>' />
  <param name="quality" value="high" />
  <embed src='<%#Eval("FullFileName") %>' quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" width="300" height="200"></embed>
</object>
                    
                        <div class="font"><%#Eval("Name") %></div>
                    </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <uc1:Pager ID="Pager1" runat="server" PageSize="4"   SelectExp="Id,FileName,Name,InsertTime" FromExp="FlashEntity" OrderByExp="UpdateTime desc" />
        <uc3:Progress ID="Progress1" runat="server" />
     </div>
     </div>
<div Finder="RightMenu" class="rightmenu" >
    <div Finder="Select" class="selectbtn"><a href="javascript:void(0);">选择</a> </div>
    <div Finder="Browse" class="browsebtn"><a href="javascript:void(0);">浏览</a> </div>
     <div ><a href="javascript:void(0);"  class="browsebtn" style="float: left;" id="hfMoveSwitcher">移动</a>
        <ul id="hfMoveContainer" style="float: left;background: #ffe4e1;width: 130px;padding-left: 20px;display: none;">
            <uc5:FolderList ID="litMoveFolder" runat="server" FolderType="Flash" IsLink="False"/>
        </ul>
         
     </div>
</div>


 
             </ContentTemplate>
          <Triggers >
              
              <asp:PostBackTrigger ControlID="btnSave" />
              
          </Triggers>
 </asp:UpdatePanel>
  
  </div>
 </asp:Content>
 
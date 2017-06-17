<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Editor.Editor.Image.List" MasterPageFile="~/None.Master"  %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc4" TagName="Message" Src="~/Controls/Message.ascx" %>

 
 <%@ Register src="../FolderList.ascx" tagname="FolderList" tagprefix="uc5" %> <%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>编辑器图片选择</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


 <div class="fullbody">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
      <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
               
        <tr>
           
            <td class="font">
                名称
            </td>
            <td class="text" colspan="7" >
                <asp:TextBox ID="txtName" runat="server" CssClass="seinput" SearchWhere="Name.Contains(@Name)" SearchParamterName="Name" ></asp:TextBox>
            </td>
           
        </tr>
         <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext"  colspan="2">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem  Value="Id" Text="编号" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="font">
                排序方式
            </td>
            <td>
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td colspan="3">
                  <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
       </table>
     </div>
  <div id="finder" class="finder">
      <div class="top">
          
                      <input id="Button1" type="button" value="上传" Finder="UploadSwitch" class="button" />
          <div Finder="UploaderContent" class="uploadercontent" style="min-height: 250px;">
           <table>
               <tr>
                   <td> <uc2:Uploader ID="Uploader1" runat="server" Path="" FileByteSaveName="FileByte" FileNameBindName="Name"  FileNameSaveName="FileName"  FullFileNameBindName="FullFileName" IsMultiple="True" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
                   
               </tr>
               <tr>
                   <td style="text-align: center;"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button"  /></td></td>
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
              <uc5:FolderList ID="litFolder" runat="server" FolderType="Image" IsLink="True"/>
        </ul>
      </div>
      <div class="right">
          <div class="images">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <div class="element short">
                      <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
                     <div class="out" Finder="Element" Url='<%#Eval("FullFileName") %>'>
                         
                        <img src='<%#Eval("FullFileName") %>' alt='<%#Eval("Name") %>' />
                        <div class="font"><%#Eval("Name") %></div>
                    </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <uc1:Pager ID="Pager1" runat="server" PageSize="70"   SelectExp="Id,FileName,Name,InsertTime" FromExp="ImageEntity" OrderByExp="UpdateTime desc" />
        <uc3:Progress ID="Progress1" runat="server" />
     </div>
     </div>

    <div Finder="RightMenu" class="rightmenu" >
    <div Finder="Select"><a href="javascript:void(0);" class="selectbtn">选择</a> </div>
    <div Finder="Browse"><a href="javascript:void(0);"  class="browsebtn">浏览</a> </div>
    <div ><a href="javascript:void(0);"  class="browsebtn" style="float: left;" id="hfMoveSwitcher">移动</a>
        <ul id="hfMoveContainer" style="float: left;background: #ffe4e1;width: 130px;padding-left: 20px;display: none;">
            <uc5:FolderList ID="litMoveFolder" runat="server" FolderType="Image" IsLink="False"/>
        </ul>
         
     </div>
</div>

</div>
             </ContentTemplate>
          <Triggers >
              
              <asp:PostBackTrigger ControlID="btnSave" />
              
          </Triggers>
 </asp:UpdatePanel>
  </div>
 
 </asp:Content>

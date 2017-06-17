<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Home.Desktop.Notice.Detail" MasterPageFile="~/Main.Master" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>公告详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
     <table class="tb">
         <tr>
             <td class="font">标题</td>
             <td class="mtext" colspan="3"><asp:Label ID="lblTitle" runat="server" BindName="Title"></asp:Label></td>
         </tr>
    
         <tr>
            
              <td class="font">发布人</td>
             <td class="mtext" colspan="3"><asp:Label ID="lblUserRealName" runat="server"  BindName="User.RealName"></asp:Label></td>
         </tr>
         <tr>
             <td class="font">内容</td>
             <td  class="text" colspan="3"><asp:Label ID="lblDetail" runat="server"  BindName="Detail"></asp:Label></td>
              
         </tr>
          <tr>
             <td class="font">发布时间</td>
             <td  class="text"><asp:Label ID="lblInsertTime" runat="server"  BindName="InsertTime"></asp:Label></td>
             <td class="font">附件</td>
             <td  class="text"><a id="hfAttachmentName" href="" runat="server" target="_blank" BindName="DownAttachmentName">
                                   <asp:Label ID="lblAttachmentName" runat="server"  BindName="AttachmentName"></asp:Label>
                               </a></td>
         </tr>
          <tr>
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
 </div>
    

 </asp:Content>
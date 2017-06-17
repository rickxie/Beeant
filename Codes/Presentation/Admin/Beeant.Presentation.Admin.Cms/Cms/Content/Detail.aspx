<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Cms.Cms.Content.Detail" MasterPageFile="~/Main.Master" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>信息内容详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
     <table class="tb">
         <tr>
             <td class="font">标题</td>
             <td class="mtext" colspan="3"><asp:Label ID="lblTitle" runat="server" BindName="Title"></asp:Label></td>
         </tr>
      <tr>
             <td class="font">类目</td>
             <td class="text" ><asp:Label ID="lblClassName" runat="server" BindName="Class.Name"></asp:Label></td>
                <td class="font">排序</td>
             <td class="text" ><asp:Label ID="lblSequence" runat="server" BindName="Sequence"></asp:Label></td>
         </tr>
          <tr>
             <td class="font">是否显示</td>
             <td class="text" ><asp:Label ID="lblIsShowName" runat="server" BindName="IsShowName"></asp:Label></td>
                <td class="font">标签</td>
             <td class="text" ><asp:Label ID="lblTag" runat="server" BindName="Tag"></asp:Label></td>
         </tr>
         <tr>
              <td class="font">发布人</td>
             <td class="text"><asp:Label ID="lblUserRealName" runat="server"  BindName="User.RealName"></asp:Label></td>
              <td class="font">发布时间</td>
             <td  class="text"><asp:Label ID="lblInsertTime" runat="server"  BindName="InsertTime"></asp:Label></td>
         </tr>
         <tr >
            
            <td class="font" >附件</td>
             <td  class="text" colspan="3">
                 <a id="hfAttachmentName" href="" runat="server" target="_blank" BindName="DownAttachmentName">
                                   <asp:Label ID="lblAttachmentName" runat="server"  BindName="AttachmentName"></asp:Label>
                               </a>
             </td>
         </tr>
         <tr>
<%--              <td class="font">账户</td>
               <td class="text"  >
                 <a href="/Finance/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" BindName="Account.Id">
                      <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label>
                 </a>
             </td>--%>
              <td class="font">图片</td>
             <td  class="text" colspan="3">
                 <img id="imgFullFileName" runat="server" src="" BindName="FullFileName" class="img"/>
             </td>
         </tr>
         <tr>
             <td class="font">连接地址</td>
             <td  class="mtext" colspan="3"><a id="hfUrl" href="" runat="server" target="_blank" BindName="Url">
                                   <asp:Label ID="lblUrl" runat="server"  BindName="Url"></asp:Label>
                               </a></td>
           
         </tr>
         <tr>
             <td class="font">描述</td>
             <td  class="text" colspan="3"><asp:Label ID="lblDescription" runat="server"  BindName="Description"></asp:Label></td>
              
         </tr>
         <tr>
             <td class="font">内容</td>
             <td  class="text" colspan="3"><asp:Label ID="lblDetail" runat="server"  BindName="Detail"></asp:Label></td>
              
         </tr>
          
          <tr>
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
 </div>
    

 </asp:Content>
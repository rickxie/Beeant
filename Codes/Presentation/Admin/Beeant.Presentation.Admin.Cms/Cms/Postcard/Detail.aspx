<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Cms.Cms.Postcard.Detail" MasterPageFile="~/Main.Master" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>明信片详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
     <table class="tb">
         <tr>
             <td class="font">名称</td>
             <td class="text" ><asp:Label ID="lblTitle" runat="server" BindName="Title"></asp:Label></td>
                <td class="font">是否显示</td>
             <td class="text" ><asp:Label ID="lblIsShowName" runat="server" BindName="IsShowName"></asp:Label></td>
         </tr>
  

         <tr>

              <td class="font">图片</td>
             <td  class="text" colspan="3">
                 <img id="imgFullFileName" runat="server" src="" BindName="FullFileName" class="img"/>
             </td>
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
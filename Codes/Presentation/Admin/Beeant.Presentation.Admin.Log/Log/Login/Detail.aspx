<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Log.Log.Login.Detail" MasterPageFile="~/Main.Master" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>登入日志详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
     <table class="tb">
         <tr>
             <td class="font">地址</td>
             <td class="mtext" colspan="3"><asp:Label ID="lblAddress" runat="server" BindName="Address"></asp:Label></td>
         </tr>
         <tr>
             <td class="font">Ip</td>
             <td class="mtext" colspan="3"><asp:Label ID="lblIp" runat="server"  BindName="Ip"></asp:Label></td>
         </tr>
                 <tr>
             <td class="font">设备</td>
             <td class="mtext" colspan="3"><asp:Label ID="lblDevice" runat="server"  BindName="Device"></asp:Label></td>
         </tr>
    
                 <tr>
             <td class="font">城市</td>
             <td class="mtext" colspan="3"><asp:Label ID="lblCity" runat="server"  BindName="City"></asp:Label></td>
         </tr>
        <tr>
             <td class="font">操作人</td>
             <td class="text"><asp:Label ID="lblAccountName" runat="server"  BindName="Account.Name"></asp:Label></td>
              <td class="font">出错时间</td>
             <td class="text"><asp:Label ID="lblInsertTime" runat="server"  BindName="InsertTime"></asp:Label></td>
         </tr>
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
 </div>
    

 </asp:Content>
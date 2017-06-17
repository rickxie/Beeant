<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Agent.Agent.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="../../Controls/Progress.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>代理商详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">名称</td>
            <td class="text" >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
              <td class="font">是否启用</td>
            <td class="text" >
                <asp:Label ID="lblIsUsedName" runat="server"  BindName="IsUsedName"></asp:Label>
             </td>
        </tr>        
        
          <tr>
             <td class="font">加价幅度</td>
             <td class="text" ><asp:Label ID="lblIncrease" runat="server" BindName="Increase"></asp:Label></td>
              <td class="font">账户信息</td>
                     <td class="text" >  <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label>
             </td>
         </tr>
      
      
       
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
  


     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
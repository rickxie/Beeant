<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Member.Member.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>会员详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">昵称</td>
            <td class="text"  >
                <asp:Label ID="lblNickname" runat="server"  BindName="Nickname"></asp:Label>
             </td>
             <td class="font">性别</td>
            <td class="text" >
                <asp:Label ID="lblGender" runat="server" BindName="Gender"></asp:Label>
               
            </td>   
        </tr> 
         <tr>           
               <td class="font">固定电话</td>
            <td class="text">
                 <asp:Label ID="lblTelephone" runat="server" BindName="Telephone"></asp:Label>
            </td>
            <td class="font">身份证</td>
            <td class="text" >
                <asp:Label ID="lblIdCardNumber" runat="server" BindName="IdCardNumber" ></asp:Label>
             </td>
        </tr>
       
          <tr>
             <td class="font">会员录入时间</td>
             <td class="text" ><asp:Label ID="lblInsertTime" runat="server" BindName="InsertTime"></asp:Label></td>
              <td class="font">账户信息</td>
             <td class="text"> <a href="/Finance/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" BindName="Account.Id"> <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label></a></td>
         </tr>      
 
         <tr>         
          <td class="font">是否启用</td>
            <td class="mtext" >
                 <asp:Label ID="lblIsUsedName" runat="server" BindName="IsUsedName"></asp:Label>
            </td>
            <td class="font">邮政编码</td>
            <td class="mtext"><asp:Label ID="lblPostal" runat="server" BindName="Postal"></asp:Label></td>
        </tr>        
        <tr>
            
             <td class="font">地址</td>
            <td  colspan="3" >
                <asp:Label ID="lblAddress" runat="server" BindName="Address"></asp:Label>
            </td>
        </tr>
       
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="Label2" runat="server" BindName="Remark"></asp:Label>
            </td>
        </tr>
      
        <tr>
       <td class="font">头像</td>
            <td class="mtext">
              <img id="imgFileName" runat="server" BindName="FullFileName" alt="" class="img"/>
            </td>   
        <td class="font">身份证附件</td>
            <td class="mtext">
              <img id="imgIdCardName" runat="server" BindName="IdCardFullFileName" alt="" class="img"/>
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
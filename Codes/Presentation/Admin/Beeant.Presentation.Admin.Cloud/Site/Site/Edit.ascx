<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Site.Site.Edit" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="~/Controls/Uploader.ascx" %>
     
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc2" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>


<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="text"  >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
               </td>
               <td class="font">域名</td>
            <td class="text"  >
             <input id="txtDomain" runat="server"  type="text" class="input long"  BindName="Domain" SaveName="Domain"  /> 
               </td>
        </tr>
  
        <tr>
            <td class="font">账户</td>
            <td class="mul">
                <uc2:AccountComboBox ID="cbAccount" runat="server" BindName="Account.Id" SaveName="Account.Id"  />
            </td>
            <td class="font">到期时间</td>
            <td class="text" >
                <asp:TextBox ID="txtExpireDate" runat="server" BindName="ExpireDate" SaveName="ExpireDate" ></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtExpireDate" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            
        </tr>
           <tr>
            <td class="font">Logo</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader1" runat="server" Path="Files/Images/SiteLogo/" FileByteSaveName="LogoFileByte" FileNameBindName="LogoFileName"  FileNameSaveName="LogoFileName"  FullFileNameBindName="LogoFullFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
           <tr>
            <td class="font">图标</td>
            <td class="mtext" colspan="3">
                <uc2:Uploader ID="Uploader2" runat="server" Path="Files/Images/SiteFavicon/" FileByteSaveName="FaviconFileByte" FileNameBindName="FaviconFileName"  FileNameSaveName="FaviconFileName"  FullFileNameBindName="FaviconFullFileName" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
        <tr>
           <td class="font">是否显示作者</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsShowAuthor" runat="server" Checked="True"   BindName="IsShowAuthor" SaveName="IsShowAuthor"/>
               </td>
               <td class="font">是否启用密码</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsPassword" runat="server" Checked="True"  BindName="IsPassword" SaveName="IsPassword"/>
               </td>
        </tr>
           <tr>
           <td class="font">手机端风格</td>
            <td class="text" colspan="3" >
                <asp:RadioButtonList ID="rdMobileStyle" runat="server" RepeatDirection="Horizontal" BindName="MobileStyle" SaveName="MobileStyle">
                    <asp:ListItem Value="" Selected="True">默认</asp:ListItem>
                    <asp:ListItem Value="en">英文</asp:ListItem>
                    <asp:ListItem Value="Window" >窗体版</asp:ListItem>
                    <asp:ListItem Value="Brand" >品牌版</asp:ListItem>
                    <asp:ListItem Value="Starry" >星空版</asp:ListItem>
                </asp:RadioButtonList>
               </td>
        </tr>
         <tr>
           <td class="font">电脑端风格</td>
            <td class="text" colspan="3" >
                <asp:RadioButtonList ID="rdWebsiteStyle" runat="server" RepeatDirection="Horizontal"  BindName="WebsiteStyle" SaveName="WebsiteStyle">
                    <asp:ListItem Value="" Selected="True">默认</asp:ListItem>
                    <asp:ListItem Value="en">英文</asp:ListItem>
                </asp:RadioButtonList>
               </td>
        </tr>
        <tr>
           <td class="font">是否开通细节图</td>
            <td class="text" colspan="3" >
                   <asp:CheckBox ID="ckIsOpenImages" runat="server" Checked="True" BindName="IsOpenImages" SaveName="IsOpenImages"/>
               </td>
        </tr>
        <tr>
          <td class="font">是否开通手机端</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsOpenMobile" runat="server" Checked="True" BindName="IsOpenMobile" SaveName="IsOpenMobile"/>
               </td>
               <td class="font">是否开通电脑端</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsOpenWebsite" runat="server" Checked="True" BindName="IsOpenWebsite" SaveName="IsOpenWebsite"/>
               </td>
        </tr>
 
        <tr>
          <td class="font">是否开通手消息</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsOpenMessage" runat="server" Checked="True" BindName="IsOpenMessage" SaveName="IsOpenMessage"/>
               </td>
               <td class="font">是否开通一件分享</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsOpenMultiShare" runat="server" Checked="True" BindName="IsOpenMultiShare" SaveName="IsOpenMultiShare"/>
               </td>
        </tr>
          <tr>
          <td class="font">是否开通订阅用户</td>
            <td class="text">
                <asp:CheckBox ID="ckIsOpenSubscribeUser" runat="server" Checked="True" BindName="IsOpenSubscribeUser" SaveName="IsOpenSubscribeUser" />
               </td>
               <td class="font">是否开通目录本</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsPrint" runat="server" Checked="True" BindName="IsPrint" SaveName="IsPrint"/>
               </td> 
        </tr>
          <tr>
          <td class="font">微信Token</td>
            <td class="text" >
                <input id="txtWechatToken" runat="server"  type="text" class="input long"  BindName="WechatToken" SaveName="WechatToken" /> 
               </td>
               <td class="font">微信AppId</td>
            <td class="text" >
                 <input id="txtWechatAppId" runat="server"  type="text" class="input long"  BindName="WechatAppId" SaveName="WechatAppId" /> 
               </td>
        </tr>
         <tr>
          <td class="font">微信Secret</td>
            <td class="text" colspan="3" >
                <input id="txtWechatSecret" runat="server"  type="text" class="input long"  BindName="WechatSecret" SaveName="WechatSecret" /> 
               </td>
            
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
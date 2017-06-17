<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Account.Account.Update" MasterPageFile="~/Datum.Master" ValidateRequest="false"%>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>账户编辑</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        

        <div class="edit">
    <table class="tb">
       <tr>
            <td class="font">用户名</td>
            <td class="text">
             <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"  /> </td>
            <td class="font">真实姓名</td>
            <td class="text" >
             <input id="txtRealName" runat="server"  type="text" class="input"  BindName="RealName" SaveName="RealName"  />
             </td>
        </tr>
         
         <tr>
            <td class="font">手机号码</td>
            <td class="text" >
             <input id="txtMobile" runat="server"  type="text" class="input"  BindName="Mobile" SaveName="Mobile"  />
             </td>
              <td class="font">电子邮箱</td>
            <td class="text" >
             <input id="txtEmail" runat="server"  type="text" class="input"  BindName="Email" SaveName="Email"  />
             </td>
        </tr>
      
        <tr>
      
        <td class="font">是否启用</td>
            <td class="mtext" colspan="3">
                  <asp:CheckBox ID="ckIsUsed" runat="server"  BindName="IsUsed" SaveName="IsUsed"  /> 
            </td>
        </tr>
   
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
    <uc2:Message ID="Message1" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
 

    

 
 </asp:Content>
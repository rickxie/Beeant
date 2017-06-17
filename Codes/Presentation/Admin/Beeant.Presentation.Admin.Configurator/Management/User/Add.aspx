<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Management.User.Add" MasterPageFile="~/Datum.Master" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>用户录入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
     <div class="edit">
    <table class="tb">
            <tr>
             <td class="font">用户名</td>
            <td class="text"><input id="txtAccountName" runat="server" type="text" class="input"  BindName="Account.Name" SaveName="Account.Name" ValidateName="Name"    /></td>
            <td class="font">真实姓名</td>
            <td class="text"><input id="txtAccountRealName" runat="server"  type="text" class="input"  BindName="Account.RealName" SaveName="Account.RealName"  ValidateName="RealName"     />  </td>
           
        </tr>
        <tr>
            <td class="font">邮箱</td>
             <td class="text"><input id="txtAccountEmail" runat="server" type="text" class="input"  BindName="Account.Email" SaveName="Account.Email"  ValidateName="Email"   /></td>
              <td class="font">手机号码</td>
            <td class="text"><input id="txtAccountMobile" runat="server"  type="text" class="input"  BindName="Account.Mobile" SaveName="Account.Mobile"  ValidateName="Mobile"  /> </td>
        </tr>
         <tr>
            <td class="font">密码</td>
            <td class="text"><input id="txtPassword" runat="server" class="input" type="password"  BindName="Account.Password" SaveName="Account.Password"  ValidateName="Password"  /></td>
            <td class="font">确认密码</td>
            <td class="text"><input id="txtSurePassword" runat="server" class="input" type="password"  /> </td>
        </tr>
        
      <tr>
               <td class="font">别名</td>
            <td class="mtext"  >
               <input id="Text1" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name" ValidateName="" ValidateName1="Name" />
            </td>
            <td class="font">是否启用</td>
            <td class="text">
                <asp:RadioButtonList runat="server" ID="rdbtnIsUsed" RepeatDirection="Horizontal" BindName="IsUsed" SaveName="IsUsed" >
                             <asp:ListItem Selected="True" Value="true">启用</asp:ListItem>
                             <asp:ListItem Value="false">禁止</asp:ListItem>
                             </asp:RadioButtonList>
                          </td>

        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
            <input id="btnClose" type="button" value="关闭" class="btn" /></td>
        </tr>
    </table>
 
</div>
    
     <div class="list">
          <asp:GridView ID="gvRole" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckRoleSelectAll" type="checkbox" AllCheckName="roleselectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="roleselectall" ComfirmValidate="Add"/>
           </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="角色"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
       
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
     
    
      <div class="list">
          <asp:GridView ID="gvOnwer" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckTeamSelectAll" type="checkbox" AllCheckName="teamselectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="teamselectall" ComfirmValidate="Add"/>
           </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="组织"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
       
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>

     <div class="list">
          <asp:GridView ID="gvGroup" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckGroupSelectAll" type="checkbox" AllCheckName="groupselectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="groupselectall" ComfirmValidate="Add"/>
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="工作组"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        
           
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
    
    <div class="list">
          <asp:GridView ID="gvAuditor" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckAuditorSelectAll" type="checkbox" AllCheckName="auditorselectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="auditorselectall" ComfirmValidate="Add"/>
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="工作组"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        
           
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
   
    <uc2:Message ID="Message1" runat="server" />
    <uc3:Progress ID="Progress1" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>

 </asp:Content>
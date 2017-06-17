<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Management.User.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

   <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>用户列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
                <uc2:DataSearch ID="DataSearch1" runat="server" />
    
        </tr>
        <tr>
            <td class="font">
                用户名
            </td>
            <td class="mtext" >
                <asp:TextBox ID="txtName" runat="server" SearchWhere="Name.Contains(@Name)" SearchParamterName="Name" CssClass="seinput"></asp:TextBox></td>
           <td class="font">
                真实姓名
            </td>
            <td class="mtext" colspan="5" >
                <asp:TextBox ID="txtRealName" runat="server" SearchWhere="RealName.Contains(@RealName)" SearchParamterName="RealName" CssClass="seinput"></asp:TextBox></td>
          
        </tr>
     </table>
        </div>

        <div class="mainten">
         <a href="Add.aspx" target="_blank"class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
               <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank">编辑</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="授权角色" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Authority/RoleAccount/Add.aspx?accountid=<%#Eval("Account.Id") %>' target="_blank">授权角色</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="回收角色" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Authority/RoleAccount/Delete.aspx?accountid=<%#Eval("Account.Id") %>' target="_blank">回收角色</a>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="授权组织" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Authority/OwnerAccount/Add.aspx?accountid=<%#Eval("Account.Id") %>' target="_blank">授权组织</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="回收组织" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Authority/OwnerAccount/Delete.aspx?accountid=<%#Eval("Account.Id") %>' target="_blank">回收组织</a>
            </ItemTemplate>
        </asp:TemplateField>
              <asp:TemplateField HeaderText="授权组" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Workflow/GroupAccount/Add.aspxaccountid=<%#Eval("Account.Id") %>' target="_blank">授权组</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="回收组" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Workflow/GroupAccount/Delete.aspx?accountid=<%#Eval("Account.Id") %>' target="_blank">回收组</a>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="授权审核组" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Workflow/AuditorAccount/Add.aspx?accountid=<%#Eval("Account.Id") %>' target="_blank">授权审核组</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="回收审核组" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Workflow/AuditorAccount/Delete.aspx?accountid=<%#Eval("Account.Id") %>' target="_blank">回收审核组</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="密码修改" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='Passsword.aspx?id=<%#Eval("Account.Id") %>' target="_blank">密码修改</a>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="别名"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Account.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="真实姓名"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Account.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="邮箱"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Account.Email")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="手机号码"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Account.Mobile")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("IsUsedName")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,Account.Name,Account.RealName,Account.Email,Account.Mobile,IsUsed,Account.Id,InsertTime" FromExp="UserEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Authority.RoleAccount.Delete" MasterPageFile="~/Datum.Master" %>

<%@ Import Namespace="Beeant.Presentation.Admin.Configurator.Authority.RoleAccount" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title><%=this.GetAccountName()%>回收角色</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>


        <div class="mainten">

        <asp:Button ID="btnRemove" runat="server" Text="回收" onclick="Remove_Click" ConfirmBox="Remove" ConfirmMessage="您确定要回收吗" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"/>
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="角色"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Role.Name")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Role.Name,InsertTime" FromExp="UserRoleEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
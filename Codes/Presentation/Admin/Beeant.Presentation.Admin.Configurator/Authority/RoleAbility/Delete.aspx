<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Authority.RoleAbility.Delete" MasterPageFile="~/Datum.Master" %>
<%@ Import Namespace="Beeant.Presentation.Admin.Configurator.Authority.RoleAbility" %>

<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

 <%@ Register src="/Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc4" %> 

  <%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %> 
   <%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title><%=this.GetRoleName()%>回收功能</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
       <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
            <td class="font">菜单类型</td>
            <td class="text">
                  <uc2:DataSearch ID="DataSearch1" runat="server" />
         
            </td>
      
        </tr>
        <tr> <td  class="font">菜单类型</td><td class="text"> <uc4:GeneralDropDownList ID="ddlSubsystem" runat="server" ObjectName="SubsystemEntity" SearchWhere="Ability.Menu.Subsystem.Id=@SubsystemId " SearchParamterName="SubsystemId"/></td>
        <td  class="font">菜单名称</td> <td colspan="7" class="text"><input type="text" id="txtMenu" runat="server" CssClass="seinput" SearchWhere="Ability.Menu.Name.Contains(@MenuName) " SearchParamterName="MenuName" /><td>
        </tr>
     </table>
        </div>

           <div class="mainten">
                <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"  ConfirmBox="Remove" ConfirmMessage="您确定要删除吗" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>
                <asp:DropDownList ID="ddlIsForbid" runat="server" SaveName="IsForbid" ComfirmDropdownListMessage="请选择操作项" ComfirmValidate="IsForbid">
                  <asp:ListItem  Value="False" Text="允许" ></asp:ListItem>
                  <asp:ListItem  Value="True" Text="禁止" ></asp:ListItem>
            </asp:DropDownList>
              <asp:Button ID="btnIsForbid" runat="server" Text="确定" CssClass="btn" ConfirmBox="IsForbid" ConfirmMessage="您确定要修改吗？"  ComfirmCheckBoxMessage="你没有选择任何行" onclick="btnIsForbid_Click" />
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
               <input value='<%#Eval("ID") %>'  id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,IsForbid" />
           </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="角色名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Role.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="功能名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Ability.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="菜单名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Ability.Menu.Name")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Role.Name,Ability.Name,Ability.Menu.Name,InsertTime" FromExp="RoleAbilityEntity" />
                
        </div>

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
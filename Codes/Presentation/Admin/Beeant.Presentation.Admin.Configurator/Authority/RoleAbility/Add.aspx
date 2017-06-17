<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Authority.RoleAbility.Add" MasterPageFile="~/Datum.Master" %>
<%@ Import Namespace="Beeant.Presentation.Admin.Configurator.Authority.RoleAbility" %>

<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

 <%@ Register src="/Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc4" %>
  <%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %> 
   <%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title><%=this.GetRoleName()%>授权功能</title>  
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
        <tr> <td  class="font">菜单类型</td><td class="text"> <uc4:GeneralDropDownList ID="ddlSubsystem" runat="server" ObjectName="SubsystemEntity" SearchWhere="Menu.Subsystem.Id==@SubsystemId " SearchParamterName="SubsystemId"/></td>
        <td  class="font">菜单名称</td> <td colspan="7" class="text"><input type="text" id="txtMenu" runat="server" CssClass="seinput" SearchWhere="Menu.Name.Contains(@MenuName) " SearchParamterName="MenuName" /><td>
        </tr>
     </table>
        </div>

           <div class="mainten">
                <asp:Button ID="btnAdd" runat="server" Text="授权" onclick="btnAdd_Click" CssClass="btn" ConfirmBox="Add" ConfirmMessage="您确定要授权吗"  ComfirmCheckBoxMessage="你没有选择任何行" ></asp:Button>
               
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
               <input value='<%#Eval("ID") %>'  id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Add" />
           </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="菜单名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Menu.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="验证类型"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("IsVerifyName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("Remark")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,Menu.Name,IsVerify,Remark,InsertTime" FromExp="AbilityEntity" />
                
        </div>
 

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
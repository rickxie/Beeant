<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Authority.Resource.List" MasterPageFile="~/Datum.Master" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
  <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc4" %>
  <%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc5" %>
  <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title><%=GetMenuName()%>资源列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="Edit" class="edit">
              <input type="button" id="Hide" class="btn" value="隐藏"/>
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="text" ><input id="txtName" runat="server" type="text" class="input"  BindName="Name" SaveName="Name"  /></td>
              <td class="font">功能</td>
            <td class="text" >
                <uc5:GeneralDropDownList ID="ddlAbility" runat="server" BindName="Ability.Id" SaveName="Ability.Id" ObjectName="AbilityEntity"/>
            </td>
        </tr>
        <tr>
            <td class="font">是否验证参数</td>
            <td class="text">
                <asp:CheckBox ID="ckIsValidateParamter" runat="server" BindName="IsValidateParamter" SaveName="IsValidateParamter"  />
            </td>
            <td class="font">是否正则验证</td>
            <td class="text">
                <asp:CheckBox ID="ckIsRegexValidate" runat="server" BindName="IsRegexValidate" SaveName="IsRegexValidate"  />
              
            </td>
        </tr>
          <tr>
            <td class="font">Url</td>
            <td class="mtext" colspan="3"><input id="txtUrl" runat="server" class="input long" type="text"  BindName="Url" SaveName="Url"  /></td>
        </tr>
         <tr>
            <td class="font">控件</td>
            <td class="text"  ><input id="txtControls" runat="server" class="input" type="text"  BindName="Controls" SaveName="Controls"  /></td>
           <td class="font">是否排除</td>
            <td class="text">
                <asp:CheckBox ID="ckIsExcude" runat="server" BindName="IsExcude" SaveName="IsExcude"  />
              
            </td>
        </tr>
         <tr>
         <td class="font">备注</td>
            <td  colspan="3" class="mtext">
                <input id="txtRemark" runat="server"  type="text"  BindName="Remark" SaveName="Remark" class="input long"/>
            </td>
        </tr>
      
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   /></td>
        </tr>
    </table>
   <uc4:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
        <tr>
            <td class="font">功能</td>
            <td class="mtext" colspan="7">
                   <uc5:GeneralDropDownList ID="ddlAbilitySearch" runat="server" ObjectName="AbilityEntity" SearchWhere="Ability.Id==@AbilityId" SearchParamterName="AbilityId" />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
         <a href='javascript:void(0);' id="Add" class="btn" >添加</a>
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
               <input value='<%#Eval("ID") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"/>
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
               <asp:LinkButton runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="资源"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Url")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="控件"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Controls")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="功能"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Ability.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="是否验证参数"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("IsValidateParamterName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否正则验证"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("IsRegexValidateName")%>
            </ItemTemplate>
        </asp:TemplateField>

                <asp:TemplateField HeaderText="描述"  ItemStyle-CssClass="left name">
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,Url,Controls,Remark,Ability.Name,IsValidateParamter,IsRegexValidate,InsertTime" FromExp="ResourceEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
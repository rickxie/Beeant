<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Workflow.Condition.List" MasterPageFile="~/Datum.Master" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
      <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc4" %>
   <%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc5" %>
   <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title><%=NodeName%>条件编辑</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
<div class="edit" id="Edit">
       <input type="button" id="Hide" class="btn" value="隐藏"/>
    <table class="tb">
        <tr>
            <td class="font">节点</td>
            <td class="mtext" colspan="3">
                <uc5:GeneralDropDownList ID="ddlNode" ObjectName="NodeEntity" runat="server" BindName="Node.Id" SaveName="Node.Id" />
            </td>
          
        </tr>
        <tr>
            <td class="font">比较表达式</td>
            <td class="mtext" colspan="3">
               <input id="txtInspectExp" runat="server" type="text" class="input long"  BindName="InspectExp" SaveName="InspectExp"  />
            </td>
             
        </tr>
           <tr>
            <td class="font">查询表达式</td>
            <td class="mtext" colspan="3">
               <input id="txtSelectExp" runat="server" type="text" class="input long"  BindName="SelectExp" SaveName="SelectExp"  />
            </td>
             
        </tr>
        <tr>
            <td class="font">参数</td>
            <td class="mtext" colspan="3">
               <input id="txtArgument" runat="server" type="text" class="input long"  BindName="Argument" SaveName="Argument"  />
            </td>
             
        </tr>
        

         <tr>
            <td class="font">备注</td>
            <td class="mtext" colspan="3">
               <input id="txtRemark" runat="server" type="text" class="input long"  BindName="Remark" SaveName="Remark"  />
            </td>
             
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   /></td>
        </tr>
    </table>
      <uc4:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>

        <div id="divSearch" class="search" runat="server" enableviewstate="false">
           <table class="tb">
        <tr>
                <uc2:DataSearch ID="DataSearch1" runat="server" />
        
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
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall" />
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
        <asp:TemplateField HeaderText="节点"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Node.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="比较表达式"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("InspectExp")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="查询表达式"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("SelectExp")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="参数表达式"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("Argument")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Node.Id,Node.Name,Argument,InspectExp,SelectExp,Remark,InsertTime" FromExp="ConditionEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
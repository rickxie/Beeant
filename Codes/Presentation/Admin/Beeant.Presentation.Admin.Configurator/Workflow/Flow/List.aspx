<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Workflow.Flow.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
        <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc4" %>
   <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>工作流列表</title>  
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
               <td class="font">排序</td>
            <td class="text"><input id="txtSequence" runat="server" type="text" class="input"  BindName="Sequence" SaveName="Sequence"  /></td>
        </tr>
        <tr>
         <td class="font">处理类</td>
            <td class="mtext" colspan="3" ><input id="txtClassName" runat="server" class="input long"  type="text"  BindName="ClassName" SaveName="ClassName"  /> </td>
        </tr>
 
           <tr>
          <tr>
          <td class="font">默认处理地址</td>
            <td class="mtext" colspan="3" ><input id="txtDefaultUrl" runat="server" class="input long"  type="text"  BindName="DefaultUrl" SaveName="DefaultUrl"  /> </td>
        </tr>
        <tr>
        <td class="font">邮箱处理地址</td>
            <td class="mtext" colspan="3" ><input id="txtEmailUrl" runat="server" class="input long"  type="text"  BindName="EmailUrl" SaveName="EmailUrl"  /> </td>
        </tr>
        <tr>
        <td class="font">手机处理地址</td>
            <td class="mtext" colspan="3" ><input id="txtMobileUrl" runat="server" class="input long"  type="text"  BindName="MobileUrl" SaveName="MobileUrl"  /> </td>
        </tr>
         <tr>
         <td class="font">备注</td>
            <td class="mtext" colspan="3" ><input id="txtRemark" runat="server" class="input long"  type="text"  BindName="Remark" SaveName="Remark"  /> </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"  /></td>
        </tr>
    </table>
    <uc4:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>

        <div id="divSearch" class="search" runat="server" >
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
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" name="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove" />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                  <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
           
          
          
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="处理类" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ClassName")%>
            </ItemTemplate>
        </asp:TemplateField>
 
          <asp:TemplateField HeaderText="默认处理地址" ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("DefaultUrl")%>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="邮箱处理地址" ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("EmailUrl")%>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="手机处理地址" ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("MobileUrl")%>
            </ItemTemplate>
        </asp:TemplateField>
            
            <asp:TemplateField HeaderText="排序" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Sequence")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   Select="Id,Name,ClassName,DefaultUrl,EmailUrl,MobileUrl,Sequence,InsertTime" From="FlowEntity" />
                        
     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
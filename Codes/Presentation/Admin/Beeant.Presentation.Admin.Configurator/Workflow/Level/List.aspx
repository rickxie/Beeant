<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Workflow.Level.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="uc5" TagName="Message" Src="~/Controls/Message.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>级别列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
<div id="Edit" class="edit">
      <input type="button" id="Hide" class="btn" value="隐藏"/>
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="text" colspan="3"><input id="txtName" runat="server" type="text" class="input"  BindName="Name" SaveName="Name"  /></td>
            
        </tr>
         <tr>
            <td class="font">颜色</td>
            <td class="text" ><input id="txtColor" runat="server" type="text" class="input"  BindName="Color" SaveName="Color"  /></td>
             <td class="font">排序</td>
            <td class="text" ><input id="txtSequence" runat="server" type="text" class="input"  BindName="Sequence" SaveName="Sequence" DefaultValue="1" value="1"  /></td>
        </tr>
 
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   /></td>
        </tr>
    </table>
     <uc5:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>
 

        <div class="mainten">
                <a href='javascript:void(0);' id="Add" class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn" ></asp:Button>
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>

        <div class="list">
           
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
              <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall" ComfirmValidate="Remove"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove" />
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

          <asp:TemplateField HeaderText="颜色" ItemStyle-CssClass="left status">
            <ItemTemplate>
             <span style='color:#<%#Eval("Color")%>'><%#Eval("Color")%></span>   
            </ItemTemplate>
        </asp:TemplateField>
                  <asp:TemplateField HeaderText="排序" ItemStyle-CssClass="left status">
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,Color,Sequence,InsertTime" FromExp="LevelEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
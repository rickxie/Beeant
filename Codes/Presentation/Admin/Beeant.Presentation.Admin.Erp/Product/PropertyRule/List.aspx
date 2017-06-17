<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.PropertyRule.List" MasterPageFile="~/Datum.Master" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title><%=GetPropertyName()%>规则</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
     
         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="3" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Rule.Name" Text="规则名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Paramter" Text="参数"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="验证类型"  Selected="True"></asp:ListItem>
                      <asp:ListItem  Value="Message" Text="错误提示"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsMultiline" Text="是否多行验证"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsIgnoreCase" Text="是否忽略大小写"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem Value="Id" Text="编号" ></asp:ListItem>
                     <asp:ListItem  Value="Rule.Name" Text="规则名称" ></asp:ListItem>
                     <asp:ListItem  Value="Paramter" Text="参数" ></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="验证类型" ></asp:ListItem>
                     <asp:ListItem  Value="IsMultiline" Text="是否多行验证"  ></asp:ListItem>
                     <asp:ListItem  Value="IsIgnoreCase" Text="是否忽略大小写" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" Selected="True" ></asp:ListItem>
                </asp:DropDownList>
            </td>
           <td class="font">
                排序方式
            </td>
            <td >
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
               
            </td>
            <td colspan="4">
                 <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
          <a href="Add.aspx?propertyId=<%=Request.QueryString["propertyId"] %>" name="Add" class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" onclick="Remove_Click" ConfirmBox="Remove" CssClass="btn" ConfirmMessage="您确定要删除吗" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>
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
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>'  name="Edit">编辑</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="规则"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Rule.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="参数"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Paramter")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="验证类型"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="错误提示"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Message")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否多行验证"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsMultilineName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否忽略大小写"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsIgnoreCaseName")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id" OrderByExp="Id desc"  />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
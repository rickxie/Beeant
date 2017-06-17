<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Crm.Crm.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>客户系统列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
               
        <tr>
            <td class="font">
                名称
            </td>
            <td class="text">
                <asp:TextBox ID="txtName" runat="server" CssClass="seinput" SearchWhere="Name.Contains(@Name) " SearchParamterName="Name" ></asp:TextBox>
            </td>
            <td class="font">
                编号
            </td>
            <td class="text">
               
         <asp:TextBox ID="txtId" runat="server" CssClass="seinput" SearchWhere="Id==@Id " SearchParamterName="Id"  SearchPropertyName="Id"></asp:TextBox>
               
            </td>
            <td class="font">
                账户
            </td>
            <td class="text"  >
                <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId"  />
            </td>
        
             <td class="font">
                状态
            </td>
            <td class="text"  >
                 <asp:DropDownList ID="ddlIsUsed" runat="server" SearchWhere="IsUsed==@IsUsed" SearchParamterName="IsUsed" SearchPropertyTypeName="IsUsed">
                    <asp:ListItem Text="启用" Value="true" ></asp:ListItem>
                    <asp:ListItem Text="禁止" Value="false" ></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            
      </tr>
         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                      <asp:ListItem  Value="Name" Text="公司名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="所属账户" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="ExpireDate" Text="到期时间" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext"  colspan="2">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem  Value="Id" Text="编号" ></asp:ListItem>
                     <asp:ListItem  Value="ExpireDate" Text="到期时间" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="font">
                排序方式
            </td>
            <td>
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td colspan="3">
                  <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
         <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,Status"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
            </ItemTemplate>
        </asp:TemplateField>
                    <asp:TemplateField HeaderText="公司信息" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='/Crm/Company/Save.aspx?siteid=<%#Eval("Id") %>' target="_blank" name="Company">公司信息</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
 
          <asp:TemplateField HeaderText="所属账户"  ItemStyle-CssClass="left">
            <ItemTemplate>
              <%#Eval("Account.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        
        <asp:TemplateField HeaderText="到期时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ExpireDate","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
          
 
        <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Setting" OrderByExp="Id desc"  />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 

     

     

 </asp:Content>
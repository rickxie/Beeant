<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Property.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>属性列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
        <tr>
                <td class="font">
                    类型 
                </td>
                <td class="text">
                   <uc1:GeneralDropDownList ID="ddlType"  runat="server" SaveName="Type" BindName="Type" 
                            SearchWhere="Type==@Type" SearchParamterName="Type"  SearchPropertyTypeName="Type"
                                 ObjectName="Beeant.Domain.Entities.Product.PropertyType" IsEnum="True" />
                   
                </td>
            <td class="font">
                    是否SKU 
                </td>
                <td class="text">
                    <asp:DropDownList ID="ddlIsSku" runat="server" SearchWhere="IsSku==@IsSku" SearchPropertyTypeName="IsSku" SearchParamterName="IsSku">
                    <asp:ListItem Text="启用" Value="true"></asp:ListItem>
                     <asp:ListItem Text="禁止" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                 <td class="font">
                    是否启用 
                </td>
                <td class="text">
                    <asp:DropDownList ID="ddlIsUsed" runat="server" SearchWhere="IsUsed==@IsUsed" SearchPropertyTypeName="IsUsed" SearchParamterName="IsUsed">
                    <asp:ListItem Text="启用" Value="true"></asp:ListItem>
                     <asp:ListItem Text="禁止" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                  
                </td>
           <td class="font">
                    类目 
                </td>
                <td class="text">
                    <asp:TextBox ID="txtCategory" runat="server"  SearchWhere="Category.Name.Contains(@CategoryName)" SearchParamterName="CategoryName" CssClass="seinput"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td class="font">
                    标签 
                </td>
                <td class="mtext" colspan="7">
                    <asp:TextBox ID="txtTag" runat="server"  SearchWhere="Tag.Contains(@Tag)" SearchParamterName="Tag" CssClass="seinput"></asp:TextBox>
                </td>
        </tr>
         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Name" Text="名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Category.Name" Text="类目" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="类型"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsSku" Text="是否SKU" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="SearchType" Text="搜索类型"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Tag" Text="标签"  Selected="True"></asp:ListItem>
                      <asp:ListItem  Value="Message" Text="错误提示"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="CustomCount" Text="自定义属性个数"  Selected="True"></asp:ListItem>
                      <asp:ListItem  Value="Value" Text="值"  ></asp:ListItem>
                     <asp:ListItem  Value="Sequence" Text="排序" ></asp:ListItem>
                      <asp:ListItem  Value="IsUsed" Text="是否启用" Selected="True" ></asp:ListItem>
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
                   <asp:ListItem  Value="Name" Text="名称" ></asp:ListItem>
                      <asp:ListItem  Value="Category.Name" Text="类目"></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="类型" ></asp:ListItem>
                      <asp:ListItem  Value="IsOrder" Text="是否下单选项" ></asp:ListItem>
                     <asp:ListItem  Value="SearchType" Text="搜索类型"  ></asp:ListItem>
                      <asp:ListItem  Value="Tag" Text="标签"></asp:ListItem>
                      <asp:ListItem  Value="Message" Text="错误提示"  ></asp:ListItem>
                     <asp:ListItem  Value="CustomCount" Text="自定义属性个数" ></asp:ListItem>
                      <asp:ListItem  Value="Value" Text="值"  ></asp:ListItem>
                     <asp:ListItem  Value="Sequence" Text="排序" ></asp:ListItem>
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
         <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
          <a href="javascript:void(0);" name="Import" class="btn mbtn" id="btnImport">导入类目</a>
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
         <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">详情</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="绑定规则" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../PropertyRule/list.aspx?propertyId=<%#Eval("Id") %>' target="_blank" name="PropertyRule">绑定规则</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类目"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Category.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类型"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="是否SKU"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsSkuName")%>
            </ItemTemplate>
               </asp:TemplateField>
          
         <asp:TemplateField HeaderText="搜索类型"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("SearchTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="标签"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Tag")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="错误提示"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Message")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="自定义属性个数"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("CustomCount")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="值"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Value")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="排序"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Sequence")%>
            </ItemTemplate>
        </asp:TemplateField>
               <asp:TemplateField HeaderText="是否启用"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("IsUsedName")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id" OrderByExp="Id desc"  />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>



     

     

 </asp:Content>
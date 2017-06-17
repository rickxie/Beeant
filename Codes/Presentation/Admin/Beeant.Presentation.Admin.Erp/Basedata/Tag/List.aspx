<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Tag.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="../../Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>标签列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
        <tr>
                <td class="font">
                    标签组 
                </td>
                <td class="mtext" colspan="7">
                    <uc4:GeneralDropDownList ID="ddlTagGroup" runat="server" SearchWhere="TagGroup.Id==@TagGroupId" SearchParamterName="TagGroupId" ObjectName="TagGroupEntity" />
                </td>
          </tr>
                <tr>
                    <td class="font">
                        显示内容 
                    </td>
                    <td class="mtext" colspan="7">
                        <asp:CheckBoxList ID="ckSelectList" runat="server">
                            <asp:ListItem Selected="True" Text="编号" Value="Id"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="名称" Value="Name"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="标签组" Value="TagGroup.Name"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="标签值" Value="Value"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="录入时间" Value="InsertTime"></asp:ListItem>
                            <asp:ListItem Text="编辑时间" Value="UpdateTime"></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="font">
                        排序 
                    </td>
                    <td class="mtext">
                        <asp:DropDownList ID="ddlOrderbyList" runat="server">
                            <asp:ListItem Text="编号" Value="Id"></asp:ListItem>
                            <asp:ListItem Text="名称" Value="Name"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="录入时间" Value="InsertTime"></asp:ListItem>
                            <asp:ListItem Text="编辑时间" Value="UpdateTime"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="font">
                        排序方式 
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdOrderbyType" runat="server" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="升序" Value="asc"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="降序" Value="desc"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td colspan="4">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn" Text="搜索" />
                        <asp:Button ID="btnSavePersonalization" runat="server" CssClass="btn" 
                            Text="保存" />
                        <asp:Button ID="btnClearPersonalization" runat="server" CssClass="btn" 
                            Text="清除" />
                    </td>
                </tr>
            
      
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
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
         <asp:TemplateField HeaderText="标签组"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TagGroup.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="标签值"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Value")%>
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
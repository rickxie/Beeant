<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Shelf.List" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>货架列表</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
            <tr> 
                <td class="font">名称</td>
                <td class="text" colspan="7">
                    <asp:TextBox ID="txtName" runat="server"  SearchWhere="Name.Contains(@Name)" SearchParamterName="Name" CssClass="seinput"></asp:TextBox>
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
                            <asp:ListItem  Value="Product.Id,Product.Name" Text="是否启用"  Selected="True"></asp:ListItem>
                            <asp:ListItem  Value="Count" Text="数量"  Selected="True"></asp:ListItem>
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
                         <asp:ListItem Value="Id" Text="编号"></asp:ListItem>
                         <asp:ListItem  Value="InsertTime" Text="录入时间"></asp:ListItem>
                         <asp:ListItem  Value="UpdateTime" Text="编辑时间"></asp:ListItem>
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
                <td colspan="2">
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
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="Id" >
                <Columns>
                    <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
                    <asp:TemplateField ItemStyle-CssClass="center ckbox">
                        <HeaderTemplate>
                            <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
                        </HeaderTemplate>
                        <ItemTemplate>
                           <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,Open,Close"  />
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
                        <ItemTemplate>
                            <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                          <asp:TemplateField HeaderText="移动" ItemStyle-CssClass="center operate">
              <ItemTemplate>
                  <a href='javascript:void(0);' name="Note" Note="note" NoteUrl='/Wms/Shift/Add.aspx?ShelfId=<%#Eval("Id") %>' >移动</a>
             
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
      
                    <asp:TemplateField HeaderText="产品"  ItemStyle-CssClass="left">
                        <ItemTemplate>
                            <%#Eval("Product.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderText="数量"  ItemStyle-CssClass="left">
                        <ItemTemplate>
                            <%#Eval("Count")%>
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
                   <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
     </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>

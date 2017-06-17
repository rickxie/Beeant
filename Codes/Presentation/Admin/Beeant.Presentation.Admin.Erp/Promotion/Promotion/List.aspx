<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Promotion.Promotion.List" %>
<%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>

<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>活动列表</title>  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <tr>
                       <uc2:DataSearch ID="DataSearch1" runat="server" />

               </tr>
               <tr>
                   <td class="font">活动编号</td>
                   <td colspan="3" class="text">
                       <asp:TextBox ID="txtId" runat="server"  SearchWhere="Id==@Id" SearchParamterName="Id" CssClass="seinput"></asp:TextBox>
                   </td>
                   <td class="font">活动名称</td>
                   <td colspan="3" class="text">
                       <asp:TextBox ID="txtName" runat="server"  SearchWhere="Name.Contains(@Name)" SearchParamterName="Name" CssClass="seinput"></asp:TextBox>
                   </td>
               </tr>
               <tr class="font">
                   <td class="font">显示内容</td>
                   <td colspan="7" class="mtext"> 
                     <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Name" Text="名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="StartDate" Text="开始日期" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="EndDate" Text="结束日期" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="StartTime" Text="开始时间" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="EndTime" Text="结束时间" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Months" Text="月份"  ></asp:ListItem>
                     <asp:ListItem  Value="Weeks" Text="周期" ></asp:ListItem>
                      <asp:ListItem  Value="IsUsed" Text="是否启用" ></asp:ListItem>
                     <asp:ListItem  Value="Remark" Text="备注"  ></asp:ListItem>
                  
                </asp:CheckBoxList>
            </td>
               </tr>
                               <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem Value="Id" Text="编号" Selected="True"></asp:ListItem>
                    <asp:ListItem  Value="Name" Text="名称"></asp:ListItem>
                     <asp:ListItem  Value="StartDate" Text="开始日期"  ></asp:ListItem>
                     <asp:ListItem  Value="EndDate" Text="结束日期" ></asp:ListItem>
                     <asp:ListItem  Value="StartTime" Text="开始时间"  ></asp:ListItem>
                       <asp:ListItem  Value="EndTime" Text="结束时间"></asp:ListItem>
                     <asp:ListItem  Value="Months" Text="月份"  ></asp:ListItem>
                     <asp:ListItem  Value="Weeks" Text="周期" ></asp:ListItem>
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
            <table>
            <tr>
                 <td><a href="Add.aspx?Type=<%=Request.QueryString["Type"]%>" name="Add" target="_blank" class="btn">添加</a></td>
                <td><asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button></td>
             
            </tr>
            </table>
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,IsSales"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Entity">详情</a>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="添加商品" ItemStyle-CssClass="center operate">
            <ItemTemplate>
           <a href='/Promotion/PromotionItem/Add.aspx?PromotionId=<%=Request.QueryString["Id"]%>' target='_blank' name='List'>添加商品</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                  <a href='Update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
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
        
         <asp:TemplateField HeaderText="开始日期"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                   <%#Eval("StartDate", "{0:yyyy-MM-dd }")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="结束日期" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("EndDate", "{0:yyyy-MM-dd }")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="开始时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                   <%#Eval("StartTime", "{0:HH:mm:ss}")%>
            </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="结束时间"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                   <%#Eval("EndTime", "{0:HH:mm:ss}")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="月份" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Months")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="周期" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Weeks")%>
            </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="是否启用"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsUsedName")%>
            </ItemTemplate>
        </asp:TemplateField>
                <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>

        <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id" OrderByExp="Id asc"  />
        <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>


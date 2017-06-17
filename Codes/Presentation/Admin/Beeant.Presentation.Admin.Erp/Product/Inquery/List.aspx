<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Inquery.List" MasterPageFile="~/Main.Master" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>商品询问列表</title>  
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
                <td class="mtext" colspan="7">
                    <asp:CheckBoxList ID="ckSelectList" runat="server">
                        <asp:ListItem Text="编号" Value="Id" Selected="True" ></asp:ListItem>
                        <asp:ListItem Selected="True" Text="商品编号" Value="Goods.Id"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="商品名称" Value="Goods.Name"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="问题" Value="Question"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="是否回答" Value="IsReply"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="是否显示" Value="IsShow"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="回答" Value="Answer"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="回答时间" Value="AnswerTime"></asp:ListItem>
                       
                        <asp:ListItem  Value="Account.Id,Account.Name" Text="账户"  Selected="True"></asp:ListItem>
                        <asp:ListItem Text="录入时间" Value="InsertTime"></asp:ListItem>
                        <asp:ListItem Text="编辑时间" Value="UpdateTime"></asp:ListItem>
                    </asp:CheckBoxList>
                </td>
        </tr>
        <tr>
                <td class="font">
                    是否回答 
                </td>
                <td class="text" >
                    <asp:DropDownList ID="ddlIsSku" runat="server" SearchWhere="IsReply==@IsReply" SearchPropertyTypeName="IsReply" SearchParamterName="IsReply">
                    <asp:ListItem Text="是" Value="true"></asp:ListItem>
                     <asp:ListItem Text="否" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="font">
                    是否显示
                </td>
                <td class="text" colspan="4">
                    <asp:DropDownList ID="ddlIsShow" runat="server" SearchWhere="IsShow==@IsShow" SearchPropertyTypeName="IsShow" SearchParamterName="IsShow">
                    <asp:ListItem Text="是" Value="true"></asp:ListItem>
                     <asp:ListItem Text="否" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                </td>
        </tr>
         <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext" >
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                    <asp:ListItem Text="编号" Value="Id"></asp:ListItem>
                    <asp:ListItem Text="录入时间" Value="InsertTime"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="编辑时间" Value="UpdateTime"></asp:ListItem>
                </asp:DropDownList>
            </td>
            
             <td class="font">
                 排序方式 
             </td>
             <td >
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
     </table>
        </div>
             <div class="mainten">
            <table>
            <tr>
              
                <td><asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button></td>
                <td>
                    <asp:Button ID="btnUnShow" runat="server" Text="下架" CssClass="btn" OnClick="btnUnShow_Click"   ConfirmBox="UnShow" ConfirmMessage="您保存本次修改吗？" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button> 
                </td>
                  <td>
                    <asp:Button ID="btnShow" runat="server" Text="上架" CssClass="btn" OnClick="btnShow_Click"   ConfirmBox="Show" ConfirmMessage="您保存本次修改吗？" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button> 
                </td>
            </tr>
            </table>
        </div>
        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall"  ComfirmValidate="Remove,UnShow,Show"  />
           </ItemTemplate>
        </asp:TemplateField>
  
         <asp:TemplateField HeaderText="回复" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">回复</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="商品编号"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
               <a href='/Product/Goods/Detail.aspx?id= <%#Eval("Goods.Id")%>'> <%#Eval("Goods.Id")%></a>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="商品名称"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
               <%#Eval("Goods.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        
         <asp:TemplateField HeaderText="问题"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Question")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否回答"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("IsReplyName")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="是否显示"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("IsShowName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="回答"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Answer")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="回答时间"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("AnswerTime")%>
            </ItemTemplate>
        </asp:TemplateField>
 
             <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left Sequence xlstext">
            <ItemTemplate>
              <a href='/Account/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Name")%></a>  
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id"  OrderByExp="Id desc" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
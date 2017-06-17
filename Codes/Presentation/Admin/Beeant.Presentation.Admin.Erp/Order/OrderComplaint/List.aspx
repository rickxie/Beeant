<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderComplaint.List" MasterPageFile="~/Datum.Master" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单投诉列表</title>  
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
                        <asp:ListItem Selected="True" Text="订单编号" Value="Order.Id"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="问题" Value="Question"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="是否回答" Value="IsReply"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="回答" Value="Answer"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="回答时间" Value="AnswerTime"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="满意程度" Value="Type"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="操作人" Value="User.RealName"></asp:ListItem>
                        <asp:ListItem Text="录入时间" Value="InsertTime"></asp:ListItem>
                        <asp:ListItem Text="编辑时间" Value="UpdateTime"></asp:ListItem>
                    </asp:CheckBoxList>
                </td>
        </tr>
        <tr>
            <td class="font">是否回答</td>
            <td class="text">
               <asp:DropDownList ID="ddlIsReply" runat="server"  SearchWhere="IsReply==@IsReply" SearchParamterName="IsReply"  SearchPropertyTypeName="IsReply">
                     <asp:ListItem  Value="True"  Text="是" ></asp:ListItem>  
                     <asp:ListItem  Value="False" Text="否"></asp:ListItem>
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall"  ComfirmValidate="Remove"  />
           </ItemTemplate>
        </asp:TemplateField>
  
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">回复</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="订单"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
               <a href='/Order/Order/Detail.aspx?id= <%#Eval("Order.Id")%>'> <%#Eval("Order.Id")%></a>
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
            <asp:TemplateField HeaderText="满意程度"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Order.Id"  OrderByExp="Id desc" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
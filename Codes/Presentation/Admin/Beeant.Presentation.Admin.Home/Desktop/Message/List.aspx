<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Home.Desktop.Message.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>任务消息</title>  
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
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext">
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem Value="Id" Text="编号" ></asp:ListItem>
                     <asp:ListItem  Value="StatusName" Text="流程状态" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Flow.Name" Text="工作流" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="LevelName" Text="级别"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="DataId" Text="单据编号"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="StatusName" Text="流程状态"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="HandleUser.RealName" Text="操作人" Selected="True"></asp:ListItem>
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
                     <asp:ListItem  Value="Flow.Name" Text="工作流" ></asp:ListItem>
                     <asp:ListItem  Value="LevelName" Text="级别"  ></asp:ListItem>
                     <asp:ListItem  Value="HandleUser.RealName" Text="操作人" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
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
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>
        
        <div class="mainten">
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>


        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CssClass="table" OnRowCommand="GridView1_RowCommand"  >
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
          <asp:TemplateField HeaderText="阅读状态" ItemStyle-CssClass="center status">
            <ItemTemplate>
             <img src='<%#Convert.ToBoolean(Eval("IsRead")) ? "/Images/icon_message_yes.png" : "/Images/icon_message_no.png"%>' alt="">  
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center ">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="标题"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <asp:LinkButton ID="lkbtnTitle" runat="server" CommandName="ReadMessage" CommandArgument='<%#Eval("Id")%>'><%#Eval("Title")%></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="单据详情"  ItemStyle-CssClass="left ">
            <ItemTemplate>
              <a href='<%#Eval("Flow.DetailUrl") %>?id=<%#Eval("DataId") %>&flowid=<%#Eval("Flow.Id") %>&statusvalue=<%#Eval("StatusValue") %>' target="_blank">单据详情</a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="工作流"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Flow.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="单据编号"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("DataId")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="流程状态"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="级别"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("LevelName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("HandleUser.RealName")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Flow.Id,StatusValue,DataId,Title,Flow.DetailUrl,IsRead" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
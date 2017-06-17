<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Sys.Task.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
      <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc4" %>     
<%@ Register TagPrefix="uc5" src="/Controls/GeneralCheckBoxList.ascx" tagname="GeneralCheckBoxList" %>
<%@ Register src="/Controls/MonthsCheckBoxList.ascx" tagname="MonthsCheckBoxList" tagprefix="uc6" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>系统服务列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="Edit" class="edit">
             <input type="button" id="Hide" class="btn" value="隐藏"/>
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td  class="text"><input id="txtName" runat="server" type="text" class="input"  BindName="Name" SaveName="Name"  /></td>
              <td class="font">是否启动</td>
            <td  class="text"> <asp:CheckBox ID="ckIsStart" runat="server"  BindName="IsStart" SaveName="IsStart" /></td>
        </tr>
        <tr>
            <td class="font">执行时间</td>
            <td  class="text">
                <asp:TextBox ID="txtBeginTime" runat="server" CssClass="input" BindName="BeginTime" SaveName="BeginTime" ></asp:TextBox>
                 <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
         
            </td>
              <td class="font">执行周期(秒)</td>
            <td  class="text"> <input id="txtRecycle" runat="server" type="text" class="input"  BindName="Recycle" SaveName="Recycle" DefaultValue="86400" value="86400"  /></td>
          </tr>
    
        <tr>
            <td class="font">
                类名</td>
            <td class="text" colspan="3">
                <input id="txtClassName" runat="server" class="input long"  type="text"  BindName="ClassName" SaveName="ClassName"  />
            </td>
        </tr>
        <tr>
            <td class="font">
                参数</td>
            <td class="text" colspan="3">
                <input id="txtParamters" runat="server" class="input long"  type="text"  BindName="Args" SaveName="Args"  />
            </td>
        </tr>
        <tr>
            <td class="font">
                执行日期</td>
            <td class="mtext" colspan="3">
                <uc6:MonthsCheckBoxList ID="ckMonths" runat="server" BindName="Months" SaveName="Months" />
            </td>
        </tr>
        <tr>
            <td class="font">
                执行星期</td>
            <td class="mtext" colspan="3">
                <uc5:GeneralCheckBoxList ID="ckWeeks" runat="server" BindName="BindWeeks" SaveName="BindWeeks" IsEnum="True" ObjectName="System.DayOfWeek" />
            </td>
        </tr>
        <tr>
            <td class="font">
                备注</td>
            <td class="text" colspan="3">
                <input id="txtRemark" runat="server" class="input long"  type="text"  BindName="Remark" SaveName="Remark"  />
            </td>
        </tr>
        <tr>
            <td class="center" colspan="4">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="保存" />
            </td>
        </tr>
    </table>
     <uc4:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
                <uc2:DataSearch ID="DataSearch1" runat="server" />
 
        </tr>
     </table>
        </div>

        <div class="mainten">
       <a href='javascript:void(0);' id="Add" class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
        <asp:Button ID="btnExecute" runat="server" Text="立即执行" CssClass="btn mbtn" ConfirmBox="Execute" 
                ConfirmMessage="您确定要立即执行吗" ComfirmCheckBoxMessage="你没有选择任何行" 
                onclick="btnExecute_Click"></asp:Button>
                 <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="ClassName,Args" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall"  ComfirmValidate="Remove,Execute" />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否启动"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsStartName")%>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="开始执行时间"  ItemStyle-CssClass="left time">
            <ItemTemplate>
                <%#Eval("BeginTime", "{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="执行周期(秒)"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Recycle")%>
            </ItemTemplate>
        </asp:TemplateField>
          
        <asp:TemplateField HeaderText="日期" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Months")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="参数" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Args")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="执行星期" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("WeekName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,IsStart,BeginTime,Recycle,ClassName,Args,Weeks,Months,Remark,InsertTime" FromExp="TaskEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
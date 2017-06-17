<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Inventory.Inventory" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GeneralZTreeView_1" Src="~/Controls/GeneralZTreeView.ascx" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc10" %>
<%@ Register TagPrefix="uc5" src="/Controls/GeneralCheckBoxList.ascx" tagname="GeneralCheckBoxList" %>
<%@ Register src="/Controls/MonthsCheckBoxList.ascx" tagname="MonthsCheckBoxList" tagprefix="uc6" %>
<%@ Register TagPrefix="uc4" TagName="Message" Src="~/Controls/Message.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>库存预警表</title>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
   <div id="Edit" class="edit">

    <input type="button" id="Hide" class="btn" value="隐藏"/>
    <table class="tb">
        <tr>
            <td class="font">仓库:</td>
            <td class="mtext" colspan="3">
                <uc10:GeneralDropDownList ID="ddlStorehouse" runat="server" ObjectName="StorehouseEntity" SaveName="Storehouse.Id" BindName="Storehouse.Id" />
            </td>
        </tr>
        <tr>
         <td class="font">库存预警</td>
         <td class="text" ><input id="txtWarningCount" runat="server" class="input"  type="text"  BindName="WarningCount" SaveName="WarningCount"  /> </td>
           <td class="font">发货周期</td>
         <td class="text" ><input id="txtRecycle" runat="server" class="input"  type="text"  BindName="Recycle" SaveName="Recycle"  /> </td>
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
                限定城市</td>
            <td class="mtext" colspan="3">
                         <uc5:GeneralCheckBoxList ID="ckCities" runat="server"  BindName="Cities" SaveName="Cities"  ObjectName="Beeant.Domain.Entities.Basedata.CityEntity" />
            </td>
        </tr>
           <tr>
         <td class="font">类型</td>
         <td class="text" > <uc10:GeneralDropDownList ID="ddlType" runat="server" ObjectName="Beeant.Domain.Entities.Wms.InvertoryType" IsEnum="True" SaveName="Type" BindName="Type" /> </td>
           <td class="font">开始执行时间</td>
         <td class="text" >
                 <asp:TextBox ID="txtBeginTime" runat="server" CssClass="input" BindName="BeginTime" SaveName="BeginTime" ></asp:TextBox>
                 <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
         </td>
        </tr>
         
        <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"  />
     
            </td>
        </tr>

    </table>
         <uc4:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>

        <div id="divSearch" class="search" runat="server" >
        <table class="tb">
            <uc2:DataSearch ID="DataSearch1" runat="server" />
        </table>
        </div>


        <div class="mainten">
          <a href='javascript:void(0);' id="Add" class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove" />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                  <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="仓库"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Storehouse.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="库存预警" ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("WarningCount")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="发货周期" ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Recycle")%>
            </ItemTemplate>
        </asp:TemplateField>
         
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime", "{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>

     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Storehouse.Id,Storehouse.Name,WarningCount,Recycle,InsertTime" FromExp="InventoryEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
     

</asp:Content>


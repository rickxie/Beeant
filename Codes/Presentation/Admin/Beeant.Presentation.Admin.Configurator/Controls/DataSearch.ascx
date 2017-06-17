<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataSearch.ascx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Controls.DataSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<td class="font">录入开始日期</td>
 <td class="text"><asp:TextBox ID="txtBeginInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox></td>
<td class="font">录入截止日期</td>
<td class="text"><asp:TextBox ID="txtEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox></td>
 <td class="font">编辑开始日期</td>
<td class="text"><asp:TextBox ID="txtBeginUpdateTime" runat="server" CssClass="seinput"  SearchWhere="UpdateTime>=@BeginUpdateTime"  SearchParamterName="BeginUpdateTime"></asp:TextBox></td>
 <td class="font">编辑截止日期</td>
<td class="text"> <asp:TextBox ID="txtEndUpdateTime" runat="server" CssClass="seinput"  SearchWhere="UpdateTime<=@EndUpdateTime"  SearchParamterName="EndUpdateTime"></asp:TextBox>
<cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
<cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
<cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBeginUpdateTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
<cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtEndUpdateTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
</td>

<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BindPurchase.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payout.BindPurchase" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc2" Namespace="Beeant.Presentation.Admin.Finance.Controls" Assembly="Beeant.Presentation.Admin.Finance" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc5" TagName="LevelCheckBoxList" Src="~/Controls/Workflow/LevelCheckBoxList.ascx" %>
<%@ Register TagPrefix="uc4" TagName="StatusCheckBoxList" Src="~/Controls/Workflow/StatusCheckBoxList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>绑定核销</title>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divSearch" class="search" runat="server" >
                <table class="tb">
                    <uc2:DataSearch ID="DataSearch1" runat="server" />
       
         <tr>
            <td class="font">处理开始时间</td>
            <td class="text"><asp:TextBox ID="txtBeginStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime>==@BeginStatusTime" SearchParamterName="BeginStatusTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">处理新结束时间</td>
            <td  class="text" ><asp:TextBox ID="txtEndStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime<==@EndStatusTime" SearchParamterName="EndStatusTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">
                            编号 
                        </td>
                        <td class="text" colspan="3">
                                <asp:TextBox ID="txtId" runat="server" CssClass="seinput" 
                                SearchParamterName="Id" 
                                SearchWhere="Id==@Id"></asp:TextBox>
                        </td>
        </tr>
                    <tr>
                        <td class="font">
                            显示内容  
                        </td>
                        <td colspan="7" class="mtext">
                            <asp:CheckBoxList ID="ckSelectList" runat="server">
                                <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                                <asp:ListItem Value="Follow.RealName" Text="跟单人" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="TotalAmount" Text="应付金额" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="PayAmount" Text="实付金额" Selected="True"></asp:ListItem>
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
                           <asp:ListItem Value="Follow.RealName" Text="跟单人"></asp:ListItem>
                            <asp:ListItem Value="TotalAmount" Text="应付金额" ></asp:ListItem>
                            <asp:ListItem Value="PayAmount" Text="实付金额" ></asp:ListItem>
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
                        <td colspan="4">
                            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                            <asp:Button ID="btnExcel" runat="server" Text="导出Excel" CssClass="lmbtn btn"   />
                            <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                            <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="mainten">
                <asp:Button ID="btnModifyAmount" runat="server" Text="绑定" CssClass="btn" OnClick="btnModifyAmount_Click"   ConfirmBox="Amount" ConfirmMessage="您保存本次修改吗？" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>    
                <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn" ConfirmBox="Remove"></asp:Button>    
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
                                <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove, Amount"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center xlstext">
                            <ItemTemplate>
                                <%#Eval("Id")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="跟单人"  ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                               <%#Eval("Follow.RealName")%>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="应付金额"  ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                <%#Eval("TotalAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="实付金额"  ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                <%#Eval("PayAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="本次付款"  ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                 <input value='<%#Convert.ToDecimal(Eval("Amount")) - Convert.ToDecimal(Eval("PaidAmount")) %>' id="txtPayout" runat="server" type="text" class="input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="center xlsfloat">
                            <ItemTemplate>
                                 <input id="txtRemark" runat="server" type="text" class="input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time xlsdatetime">
                            <ItemTemplate>
                                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time xlsdatetime">
                            <ItemTemplate>
                                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
                            </ItemTemplate> 
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,TotalAmount,PayAmount" OrderByExp="Id desc"   />
            <uc3:Progress ID="Progress1" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel"/>
        </Triggers>
    </asp:UpdatePanel>    
      <input id="hfAccountId" runat="server" type="hidden"  />

</asp:Content>

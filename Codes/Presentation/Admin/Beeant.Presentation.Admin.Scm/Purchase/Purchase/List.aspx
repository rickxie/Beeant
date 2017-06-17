<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.Purchase.List"
    MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register Src="/Controls/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<%@ Register Src="/Controls/DataSearch.ascx" TagName="DataSearch" TagPrefix="uc2" %>
<%@ Register Src="/Controls/Progress.ascx" TagName="Progress" TagPrefix="uc3" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register src="../../Controls/Wms/StorehouseComboBox.ascx" tagname="StorehouseComboBox" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>采购单列表</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divSearch" class="search" runat="server">
                <table class="tb">
                    <uc2:datasearch id="DataSearch1" runat="server" />
                   
                    <tr>
                        <td class="font">
                            采购单编号
                        </td>
                        <td class="text" colspan="3">
                            <asp:TextBox ID="txtId" runat="server" CssClass="seinput" SearchParamterName="Id"
                                SearchWhere="Id==@Id "></asp:TextBox>
                        </td>
                        <td class="font">
                            处理开始时间
                        </td>
                        <td class="text">
                            <asp:TextBox ID="txtBeginStatusTime" runat="server" CssClass="seinput" SearchWhere="StatusTime>==@BeginStatusTime"
                                SearchParamterName="BeginStatusTime"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginStatusTime"
                                Format="yyyy-MM-dd">
                            </cc1:CalendarExtender>
                        </td>
                        <td class="font">
                            处理新结束时间
                        </td>
                        <td class="text">
                            <asp:TextBox ID="txtEndStatusTime" runat="server" CssClass="seinput" SearchWhere="StatusTime<==@EndStatusTime"
                                SearchParamterName="EndStatusTime"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndStatusTime"
                                Format="yyyy-MM-dd">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            供应商
                        </td>
                        <td class="mtext" colspan="3">
                          <uc8:SupplierAccountComboBox ID="SupplierAccountComboBox1" runat="server"  Status=""
                        HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId" IsValidateHidden="False" />
                          
                            <td class="font">
                                仓库名称
                            </td>
                            <td class="text" colspan="3">
                              <uc4:StorehouseComboBox ID="StorehouseComboBox1" runat="server" HiddenSearchParamterName="StorehouseId" HiddenSearchWhere="Storehouse.Id==@StorehouseId" IsValidateHidden="False"  />
                            </td>
                    </tr>
                    <tr>
                        <td class="font">
                            显示内容
                        </td>
                        <td colspan="7" class="mtext">
                            <asp:CheckBoxList ID="ckSelectList" runat="server">
                                <asp:ListItem Value="Id" Text="编号" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Order.Id" Text="订单编号" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Account.Id,Account.Name" Text="账户" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="ItemAmount" Text="金额" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="PayAmount" Text="实付金额" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="InvoiceAmount" Text="开票金额" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Follow.RealName" Text="跟单人" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Storehouse.Id,Storehouse.Name" Text="仓库" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Type" Text="采购类型" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="PurchaseDate" Text="采购日期" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="DeliveryDate" Text="交货日期" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Remark" Text="备注"></asp:ListItem>
                                <asp:ListItem Value="User.RealName" Text="所属人" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Status" Text="状态" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="StatusTime" Text="处理时间" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Level" Text="处理等级" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Submit.RealName" Text="提交人" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="OriginalPurchase.Id" Text="原始采购单" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="InsertTime" Text="录入时间"></asp:ListItem>
                                <asp:ListItem Value="UpdateTime" Text="编辑时间"></asp:ListItem>
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
                                <asp:ListItem Value="Order.Id" Text="订单编号"></asp:ListItem>
                                <asp:ListItem Value="Account.Id" Text="账户"></asp:ListItem>
                                <asp:ListItem Value="ItemAmount" Text="金额"></asp:ListItem>
                                <asp:ListItem Value="ExpressAmount" Text="运费"></asp:ListItem>
                                <asp:ListItem Value="PayAmount" Text="实付金额"></asp:ListItem>
                               <asp:ListItem Value="InvoiceAmount" Text="开票金额" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Follow.RealName" Text="跟单人"></asp:ListItem>
                                <asp:ListItem Value="Storehouse.Id" Text="仓库"></asp:ListItem>
                                <asp:ListItem Value="PurchaseDate" Text="采购日期"></asp:ListItem>
                                <asp:ListItem Value="DeliveryDate" Text="交货日期"></asp:ListItem>
                                <asp:ListItem Value="Remark" Text="备注"></asp:ListItem>
                                <asp:ListItem Value="User.RealName" Text="所属人"></asp:ListItem>
                                <asp:ListItem Value="Status" Text="状态"></asp:ListItem>
                                <asp:ListItem Value="StatusTime" Text="处理时间"></asp:ListItem>
                                <asp:ListItem Value="Level" Text="处理等级"></asp:ListItem>
                                <asp:ListItem Value="Submit.RealName" Text="提交人姓名"></asp:ListItem>
                                <asp:ListItem Value="InsertTime" Text="录入时间"></asp:ListItem>
                                <asp:ListItem Value="UpdateTime" Text="编辑时间"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="font">
                            排序方式
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="asc" Text="升序"></asp:ListItem>
                                <asp:ListItem Value="desc" Text="降序" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td colspan="4">
                            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn" />
                            <asp:Button ID="btnExcel" runat="server" Text="导出Excel" CssClass="lmbtn btn" />
                            <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn" />
                            <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="mainten">
                <a href="Add.aspx" name="Add" target="_blank" class="btn">添加</a>
                <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
            </div>
            <div class="list">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table">
                    <Columns>
                        <asp:BoundField HeaderText="序号" ItemStyle-CssClass="sequence" />
                        <asp:TemplateField ItemStyle-CssClass="center ckbox">
                            <HeaderTemplate>
                                <input id="ckSelectAll" type="checkbox" allcheckname="selectall" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" subcheckname="selectall"
                                    comfirmvalidate="Remove" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Entity">详情</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="处理" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <a href='handle.aspx?id=<%#Eval("Id") %>' target="_blank" name="Handle">处理</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="采购单明细" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <a href='../PurchaseItem/list.aspx?Purchaseid=<%#Eval("Id") %>' target="_blank" name="PurchaseItem"
                                   >
                                    采购单明细</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="入库" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <a target="_blank" href='/Wms/Stock/CreateByPurchase.aspx?Purchaseid=<%#Eval("Id") %>'>
                                    入库</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="快递" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <a href='../Express/list.aspx?PurchaseId=<%#Eval("Id") %>' target="_blank" name="Express">
                                    快递</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="附件" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <a href='../Attachment/list.aspx?PurchaseId=<%#Eval("Id") %>' target="_blank" name="Attachment">
                                    附件</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="编号" ItemStyle-CssClass="center xlstext">
                            <ItemTemplate>
                                <%#Eval("Id")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="订单编号" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <span style='<%#Eval("Order.Id").ToString()=="0" ? "display:none": "" %>'><a href='<%=this.GetUrl("PresentationAdminErpUrl") %>/Order/Order/Detail.aspx?id=<%#Eval("Order.Id") %>'
                                    target="_blank">
                                    <%#Eval("Order.Id")%></a> </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="账户" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank">
                                    <%#Eval("Account.Name")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="金额" ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                <%#Eval("ItemAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                             <asp:TemplateField HeaderText="运费" ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                <%#Eval("ExpressAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="实付金额" ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                <%#Eval("PayAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="开票金额" ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                <%#Eval("InvoiceAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="跟单人" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("Follow.RealName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="采购类型" ItemStyle-CssClass="left Sequence xlstext">
                            <ItemTemplate>
                                <%#Eval("TypeName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="仓库" ItemStyle-CssClass="left Sequence ">
                            <ItemTemplate>
                                <a href='/Wms/Storehouse/Detail.aspx?id=<%#Eval("Storehouse.Id") %>' target="_blank">
                                    <%#Eval("Storehouse.Name")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="采购日期" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("PurchaseDate","{0:yyyy-MM-dd}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="交货日期" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("DeliveryDate", "{0:yyyy-MM-dd}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="center time">
                            <ItemTemplate>
                                <%#Eval("Remark")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="所属人" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("User.RealName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("StatusName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="处理时间" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("StatusTime")%>
                            </ItemTemplate>
                        </asp:TemplateField>
   
                        <asp:TemplateField HeaderText="提交人" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("Submit.RealName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                             <asp:TemplateField HeaderText="原始采购单" ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                <%#Eval("OriginalPurchase.Id")%>
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
            <uc1:Pager ID="Pager1" runat="server" PageSize="10" SelectExp="Id,Storehouse.Id,Storehouse.Name"
                OrderByExp="Id desc" />
            <uc3:progress id="Progress1" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
    </asp:UpdatePanel>
 
     
</asp:Content>
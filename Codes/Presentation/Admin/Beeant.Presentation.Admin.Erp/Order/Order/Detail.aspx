<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.Order.Detail"
    MasterPageFile="~/Datum.Master" %>

<%@ Import Namespace="Component.Extension" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>订单详情</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="info">
        <a href='add.aspx?id=<%=RequestId%>' name="Add">新增</a> <a href='update.aspx?id=<%=RequestId%>'
            name="Edit">编辑</a> 
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="tb">
                    <tr>
                        <td class="font">
                            订单编号
                        </td>
                        <td class="text">
                            <asp:Label ID="lblId" runat="server" Text="" BindName="Id"></asp:Label>
                        </td>
                        <td class="font">
                            下单日期
                        </td>
                        <td class="text">
                            <asp:Label ID="lblOrderDate" runat="server" Text="" BindName="OrderDate"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            订单金额
                        </td>
                        <td class="text">
                            <asp:Label ID="txtTotalAmount" runat="server" Text="" BindName="TotalAmount"></asp:Label>
                        </td>
                        <td class="font">
                            应付金额
                        </td>
                        <td class="text">
                            <asp:Label ID="lblTotalPayAmount" runat="server" Text="" BindName="TotalPayAmount"></asp:Label>
                        </td>
                    </tr>
                      <tr>
                        <td class="font">
                            可开票金额
                        </td>
                        <td class="text">
                            <asp:Label ID="lblTotalInvoiceAmount" runat="server" Text="" BindName="TotalInvoiceAmount"></asp:Label>
                        </td>
                        <td class="font">
                            已付金额
                        </td>
                        <td class="text">
                            <asp:Label ID="lblPayAmount" runat="server" Text="" BindName="PayAmount"></asp:Label>
                        </td>
                    </tr>
                      <tr>
                        <td class="font">
                            订单成本
                        </td>
                        <td class="text">
                            <asp:Label ID="lblCostAmount" runat="server" Text="" BindName="CostAmount"></asp:Label>
                        </td>
                        <td class="font">
                            已开票金额
                        </td>
                        <td class="text">
                            <asp:Label ID="lblInvoiceAmount" runat="server" Text="" BindName="InvoiceAmount"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                            <td class="font">
                            定金
                        </td>
                        <td class="mtext">
                            <asp:Label ID="lblDeposit" runat="server" Text="" BindName="Deposit"> </asp:Label>
                        </td>
                        <td class="font">
                            支付方式限制
                        </td>
                        <td class="text">
                            <asp:Label ID="lblPayTypes" runat="server" Text="" BindName="PayTypes"></asp:Label>
                        </td>
                     
                    </tr>
                    <tr>
                           <td class="font">
                            账户
                        </td>
                        <td class="text">
                            <a href="/Account/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" bindname="Account.Id">
                                <asp:Label ID="lblAccountName" runat="server" Text="" BindName="Account.Name"></asp:Label>
                            
                            </a>
                        </td>
                        <td class="font">
                            终端来源
                        </td>
                        <td class="text">
                            <asp:Label ID="lblChannelType" runat="server" Text="" BindName="ChannelTypeName"></asp:Label>
                        </td>
                     
                    </tr>
                    <tr>
                      <td class="font">
                            结算类型
                        </td>
                        <td class="text">
                            <asp:Label ID="lblSettleType" runat="server" Text="" BindName="SettleTypeName"></asp:Label>
                        </td>
                        <td class="font">
                            类型
                        </td>
                        <td class="text">
                            <asp:Label ID="lblTypeName" runat="server" Text="" BindName="TypeName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            状态
                        </td>
                        <td class="text">
                            <asp:Label ID="lblStatusName" runat="server" Text="" BindName="StatusName"></asp:Label>
                        </td>
                        <td class="font">
                            提交时间
                        </td>
                        <td class="text">
                            <asp:Label ID="lblInsertTime" runat="server" Text="" BindName="InsertTime"></asp:Label>
                        </td>
                    </tr>
           
                    <tr>
                        <td class="font">
                            备注
                        </td>
                        <td class="mtext" colspan="3">
                            <asp:Label ID="lblRemark" runat="server" Text="" BindName="Remark"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4" class="center">
                             <asp:Button ID="btnPass" runat="server" Text="通过" CssClass="btn"   />
                            <asp:Button ID="btnReject" runat="server" Text="通过" CssClass="btn"   />
                            <input id="btnClose" type="button" value="关闭" class="btn" />
                        </td>
                    </tr>
                </table>
   
                <div class="subtitle" onclick="SetEntityBody('divOrderProduct')">
                    订单产品信息(<span class="count"><%=pgOrderProduct.DataCount%></span>)</div>
                <div id="divOrderProduct" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="search">
                                <table class="tb">
                                    <tr>
                                        <td class="font">
                                            开始日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtOrderProductBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime"
                                                SearchParamterName="BeginInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtOrderProductBeginInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="font">
                                            截止日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtOrderProductEndInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime<==@EndInsertTime"
                                                SearchParamterName="EndInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtOrderProductEndInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button1" runat="server" Text="搜索" CssClass="btn" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="gvOrderProduct" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="图片" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <img src='<%#string.IsNullOrEmpty(Eval("FileName").Convert<string>()) ? "/Images/Nopic.jpg" : Eval("FullFileName").Convert<string>()%>'
                                                alt="" class="img" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="名称" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Name")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="单价" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Price")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="数量" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Count")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="金额" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Amount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                       <asp:TemplateField HeaderText="金额" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("CostAmount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="活动编号" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Promotion.Id")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="是否开票" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("IsInvoiceName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="是否占用库存" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("IsCountName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="是否点评" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("IsAppraisementName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否支持退换" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("IsReturnName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="描述" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("Description")%>'  ToolTip='<%#Eval("Description")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Remark")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                  
                                </Columns>
                            </asp:GridView>
                            <uc1:Pager ID="pgOrderProduct" runat="server" PageSize="10" SelectExp="Id,Name,FileName,Price,Count,Amount,CostAmount,Promotion.Id,IsInvoice,IsCount,IsAppraisement,Description,IsReturn,IsReturn,Remark"
                                FromExp="Beeant.Domain.Entities.Order.OrderProductEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
               
                </div>
                <div class="subtitle" onclick="SetEntityBody('divOrderItem')">
                    订单明细信息(<span class="count"><%=pgOrderItem.DataCount%></span>)</div>
                <div id="divOrderItem" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="search">
                                <table class="tb">
                                    <tr>
                                        <td class="font">
                                            开始日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtOrderItemBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime"
                                                SearchParamterName="BeginInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender13" runat="server" TargetControlID="txtOrderItemBeginInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="font">
                                            截止日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtOrderItemEndInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime<==@EndInsertTime"
                                                SearchParamterName="EndInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender14" runat="server" TargetControlID="txtOrderItemEndInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button6" runat="server" Text="搜索" CssClass="btn" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="gvOrderItem" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="名称" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                         <%#Eval("Name")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="单价" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Price")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="数量" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Count")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="金额" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Amount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="成本" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("CostAmount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="开票金额" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("InvoiceAmount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="left Sequence">
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
                            <uc1:Pager ID="pgOrderItem" runat="server" PageSize="10" SelectExp="Id,Name,Price,Count,Amount,CostAmount,InvoiceAmount,Remark,InsertTime,UpdateTime"
                                FromExp="Beeant.Domain.Entities.Order.OrderItemEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="subtitle" onclick="SetEntityBody('divOrderPay')">
                    订单支付信息(<span class="count"><%=pgOrderPay.DataCount%></span>)</div>
                <div id="divOrderPay" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="search">
                                <table class="tb">
                                    <tr>
                                        <td class="font">
                                            开始日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtPayBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime"
                                                SearchParamterName="BeginInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtPayBeginInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="font">
                                            截止日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtPayEndInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime<==@EndInsertTime"
                                                SearchParamterName="EndInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtPayEndInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button2" runat="server" Text="搜索" CssClass="btn" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="gvOrderPay" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                       <asp:TemplateField HeaderText="支付方式" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Name")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="金额" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Amount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                    <asp:TemplateField HeaderText="支付流水号" ItemStyle-CssClass="left">
                                        <ItemTemplate>
                                            <%#Eval("Number")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="left Sequence">
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
                            <uc1:Pager ID="pgOrderPay" runat="server" PageSize="10" SelectExp="Name,Amount,Number,Remark,InsertTime"
                                FromExp="Beeant.Domain.Entities.Order.OrderPayEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="subtitle" onclick="SetEntityBody('divOrderInvoice')">
                    订单发票信息(<span class="count"><%=pgOrderInvoice.DataCount%></span>)</div>
                <div id="divOrderInvoice" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvOrderInvoice" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="金额" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Amount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="发票编号" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Number")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="left Sequence">
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
                            <uc1:Pager ID="pgOrderInvoice" runat="server" PageSize="10" SelectExp="Amount,Number,Remark,InsertTime"
                                FromExp="Beeant.Domain.Entities.Order.OrderInvoiceEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
          
 
                <div class="subtitle" onclick="SetEntityBody('divOrderAttachment')">
                    订单附件信息(<span class="count"><%=pgOrderAttachment.DataCount%></span>)</div>
                <div id="divOrderAttachment" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="search">
                                <table class="tb">
                                    <tr>
                                        <td class="font">
                                            开始日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtAttachmentBeginInsertTime" runat="server" CssClass="seinput"
                                                SearchWhere="InsertTime>==@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender15" runat="server" TargetControlID="txtAttachmentBeginInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="font">
                                            截止日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtAttachmentEndInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime<==@EndInsertTime"
                                                SearchParamterName="EndInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender16" runat="server" TargetControlID="txtAttachmentEndInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button7" runat="server" Text="搜索" CssClass="btn" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
              
                                    <asp:TemplateField HeaderText="附件名称(标题)" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <%#Eval("Name")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="附件" ItemStyle-CssClass="left status">
                                        <ItemTemplate>
                                            <a href="<%#Eval("DownFileName") %>" target="_blank">下载</a>
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
                            <uc1:Pager ID="pgOrderAttachment" runat="server" PageSize="10" SelectExp="Name,FileName,InsertTime,UpdateTime"
                                FromExp="Beeant.Domain.Entities.Order.OrderAttachmentEntity,Beeant.Domain.Entities"
                                OrderByExp="Id desc" WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="subtitle" onclick="SetEntityBody('divOrderExpress')">
                    订单快递信息(<span class="count"><%=pgOrderExpress.DataCount%></span>)</div>
                <div id="divOrderExpress" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvOrderExpress" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="编号" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <%#Eval("Id")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                          <asp:TemplateField HeaderText="运费金额" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Amount")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                          <asp:TemplateField HeaderText="成本" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Cost")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="快递公司" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Name")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="快递单号" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Number")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="接收人" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Recipient")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="手机号码" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Mobile")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="邮政编码" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Postcode")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="地址" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Address")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 
                                    <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="left Sequence">
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
                            <uc1:Pager ID="pgOrderExpress" runat="server" PageSize="10" SelectExp="Id,Name,Number,Recipient,Mobile,Postcode,Address,Amount,Cost,Remark,InsertTime"
                                FromExp="Beeant.Domain.Entities.Order.OrderExpressEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="subtitle" onclick="SetEntityBody('divOrderComplaint')">
                    订单投诉信息(<span class="count"><%=pgOrderComplaint.DataCount%></span>)</div>
                <div id="divOrderComplaint" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvOrderComplaint" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="编号" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <%#Eval("Id")%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="问题" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Question")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否回答" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("IsReplyName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="回答" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Answer")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="回答时间" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("AnswerTime")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="满意程度" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("TypeName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                              
                                    <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:Pager ID="pgOrderComplaint" runat="server" PageSize="10" SelectExp="Id,Question,IsReply,Answer,AnswerTime,Type,InsertTime"
                                FromExp="Beeant.Domain.Entities.Order.OrderComplaintEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="subtitle" onclick="SetEntityBody('divOrderNote')">
                    订单维护信息(<span class="count"><%=pgOrderNote.DataCount%></span>)</div>
                <div id="divOrderNote" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="search">
                                <table class="tb">
                                    <tr>
                                        <td class="font">
                                            开始日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtNoteBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime"
                                                SearchParamterName="BeginInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtNoteBeginInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="font">
                                            截止日期
                                        </td>
                                        <td class="text">
                                            <asp:TextBox ID="txtNoteEndInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime<==@EndInsertTime"
                                                SearchParamterName="EndInsertTime"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtNoteEndInsertTime"
                                                Format="yyyy-MM-dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button3" runat="server" Text="搜索" CssClass="btn" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="gvNode" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="内容" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Content")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="操作人" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Account.RealName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:Pager ID="pgOrderNote" runat="server" PageSize="10" SelectExp="Content,Account.RealName,InsertTime"
                                FromExp="Beeant.Domain.Entities.Order.OrderNoteEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Order.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
   
                <uc3:Progress ID="Progress1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

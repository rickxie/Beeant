<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.Order.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Order" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<%@ Register src="../../Controls/GeneralCheckBoxList.ascx" tagname="GeneralCheckBoxList" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
         <tr>
            <td class="font">
                    
                    状态 
                </td>
                <td class="mtext" colspan="7">
                    <uc4:GeneralCheckBoxList ID="ckStatus" runat="server" SearchWhere="Status==@Status" SearchParamterName="Status" ObjectName="Beeant.Domain.Entities.Order.OrderStatusType" IsEnum="True" />
                   
                </td>
             
               
       
        </tr>
         <tr>
            <td class="font">处理开始时间</td>
            <td class="text"><asp:TextBox ID="txtBeginStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime>==@BeginStatusTime" SearchParamterName="BeginStatusTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">处理新结束时间</td>
            <td  class="text"><asp:TextBox ID="txtEndStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime<==@EndStatusTime" SearchParamterName="EndStatusTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">
                    账户 
                </td>
                <td class="text">
                  <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId"  />
                 
                </td>
                <td class="font">
                    订单编号 
                </td>
                <td class="text">
                       <asp:TextBox ID="txtId" runat="server" CssClass="seinput" 
                        SearchParamterName="Id" 
                        SearchWhere="Id==@Id "></asp:TextBox>
                </td>
        </tr>

        <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext">
                <asp:CheckBoxList ID="ckSelectList" runat="server">
                     <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="TotalAmount" Text="订单金额" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="TotalPayAmount" Text="应付金额" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="PayAmount" Text="已付金额" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="TotalInvoiceAmount" Text="可开票金额" Selected="True"></asp:ListItem> 
                     <asp:ListItem  Value="InvoiceAmount" Text="已开票金额 " Selected="True"></asp:ListItem>
                      <asp:ListItem  Value="Deposit" Text="定金" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="CostAmount" Text="订单成本 " Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="账户"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="ChannelType" Text="来源终端" Selected="True"  ></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="订单类型" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="OrderDate" Text="下单日期" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="SettleType" Text="结算类型" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Remark" Text="备注" ></asp:ListItem>
                     <asp:ListItem  Value="Status" Text="状态" Selected="True" ></asp:ListItem>
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
                     <asp:ListItem  Value="ItemAmount" Text="金额" ></asp:ListItem>
                     <asp:ListItem  Value="PayAmount" Text="核销金额" ></asp:ListItem>
                     <asp:ListItem  Value="OrderDate" Text="下单日期"  ></asp:ListItem>
                     <asp:ListItem  Value="DeliveryDate" Text="交货日期" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间"></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间"  ></asp:ListItem>
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
                <asp:Button ID="btnExcel" runat="server" Text="导出Excel" CssClass="lmbtn btn" ExcelName="订单列表" />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
         <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
         <a href="Create.aspx" name="Create" target="_blank"class="btn" >下单</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
         
           
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
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

       
         <asp:TemplateField HeaderText="维护记录" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='javascript:void(0);' name="Note" Note="note" NoteUrl='/Order/OrderNote/Mainten.aspx?OrderId=<%#Eval("Id") %>' >维护记录</a>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center xlstext">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="订单金额"  ItemStyle-CssClass="right xlsfloat">
            <ItemTemplate>
                <%#Eval("TotalAmount")%>
            </ItemTemplate>
        </asp:TemplateField>
        
            <asp:TemplateField HeaderText="应付金额"  ItemStyle-CssClass="right xlsfloat">
            <ItemTemplate>
                <%#Eval("TotalPayAmount")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="已付金额"  ItemStyle-CssClass="right Sequence xlsfloat">
            <ItemTemplate>
                <%#Eval("PayAmount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="可开票金额" ItemStyle-CssClass="right xlsfloat">
             <ItemTemplate>
                <%#Eval("TotalInvoiceAmount")%>
            </ItemTemplate>
       </asp:TemplateField>
              <asp:TemplateField HeaderText="已开票金额" ItemStyle-CssClass="right xlsfloat">
             <ItemTemplate>
                <%#Eval("InvoiceAmount")%>
            </ItemTemplate>
       </asp:TemplateField>
          <asp:TemplateField HeaderText="定金"  ItemStyle-CssClass="right Sequence xlsfloat" >
            <ItemTemplate>
                <%#Eval("Deposit")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="订单成本"  ItemStyle-CssClass="right Sequence xlsfloat" >
            <ItemTemplate>
                <%#Eval("CostAmount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="来源终端"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("ChannelTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="订单类型"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="下单日期"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("OrderDate","{0:yyyy-MM-dd}")%>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left Sequence xlstext">
            <ItemTemplate>
              <a href='/Account/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Name")%></a>  
            </ItemTemplate>
        </asp:TemplateField>
    
        <asp:TemplateField HeaderText="结算类型"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("SettleTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>

       
   
         <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("StatusName")%>
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
             <asp:TemplateField HeaderText="商品明细" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../OrderProduct/list.aspx?orderid=<%#Eval("Id") %>' target="_blank" name="OrderProduct"  >商品明细</a>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="价格明显" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../OrderItem/list.aspx?orderid=<%#Eval("Id") %>' target="_blank" name="OrderItem"  >价格明显</a>
            </ItemTemplate>
        </asp:TemplateField>
              <asp:TemplateField HeaderText="支付" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../OrderPay/list.aspx?orderid=<%#Eval("Id") %>' target="_blank" name="OrderPay"  >支付</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="发票" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../OrderInvoice/list.aspx?orderid=<%#Eval("Id") %>' target="_blank" name="OrderInvoice" style='<%#(Eval("Status").Convert<OrderStatusType>()==OrderStatusType.Cancel||Eval("Status").Convert<OrderStatusType>()==OrderStatusType.Finish)?"display:none":""%>' >发票</a>
            </ItemTemplate>
        </asp:TemplateField> 
         <asp:TemplateField HeaderText="物流" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../OrderExpress/list.aspx?orderid=<%#Eval("Id") %>' target="_blank" name="OrderExpress" >物流</a>
            </ItemTemplate>
        </asp:TemplateField>

              <asp:TemplateField HeaderText="附件" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../OrderAttachment/list.aspx?orderid=<%#Eval("Id") %>' target="_blank" name="OrderAttachment"  >附件</a>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Type" OrderByExp="Id desc"   />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
         <Triggers>
         <asp:PostBackTrigger ControlID="btnExcel"/>
     </Triggers>
 </asp:UpdatePanel>

     

     
           

     

     
           <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
     

 </asp:Content>
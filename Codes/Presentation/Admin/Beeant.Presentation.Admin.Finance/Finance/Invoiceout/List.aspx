<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Invoiceout.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>销项发票列表</title>  
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
                <td class="mtext" colspan="5">
                    <asp:CheckBoxList ID="ckStatusList" runat="server">
                    </asp:CheckBoxList>
                </td>
                <td class="font">
                    级别 
                </td>
                <td class="text">
                      <asp:CheckBoxList ID="ckLevelList" runat="server"  SearchParamterName="Level" SearchPropertyTypeName="Level" SearchWhere="Level==@Level">
                    </asp:CheckBoxList>

                </td>
               
       
        </tr>
         <tr>
            <td class="font">处理开始时间</td>
            <td class="text"><asp:TextBox ID="txtBeginStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime>==@BeginStatusTime" SearchParamterName="BeginStatusTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">处理新结束时间</td>
            <td class="mtext"><asp:TextBox ID="txtEndStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime<==@EndStatusTime" SearchParamterName="EndStatusTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
               <td class="font">
                    账户 
                </td>
                <td class="text">
                    <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId"  />
                 
                </td>
                <td class="font">
                    抬头 
                </td>
                <td class="text">
                       <asp:TextBox ID="txtTitle" runat="server" CssClass="seinput" 
                        SearchParamterName="Title" 
                        SearchWhere="Title.Contains(@Title) "></asp:TextBox>
                </td>
            
        </tr>
        <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext">
                <asp:CheckBoxList ID="ckSelectList" runat="server">
                     <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="InvoiceNumber" Text="发票号码" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Amount" Text="金额" Selected="True"></asp:ListItem>
                     <asp:ListItem Remote="Account" RemoteName="Name"  Value="Account.Id,Account.Name" Text="账户"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Title" Text="抬头" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="ChannelType" Text="渠道" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Recipient" Text="接收人" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Mobile" Text="手机号码" ></asp:ListItem>
                     <asp:ListItem  Value="Postcode" Text="邮政编码" ></asp:ListItem>
                     <asp:ListItem  Value="ExpressName" Text="快递公司" ></asp:ListItem>
                     <asp:ListItem  Value="ExpressNumber" Text="快递单号" ></asp:ListItem>
                     <asp:ListItem  Value="Address" Text="地址"  ></asp:ListItem>
                     <asp:ListItem  Value="IsFlush" Text="是否为冲帐" ></asp:ListItem>
                     <asp:ListItem  Value="Remark" Text="备注" ></asp:ListItem>
                     <asp:ListItem  Value="User.RealName" Text="所属人" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Status" Text="状态" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="StatusTime" Text="处理时间" Selected="True" ></asp:ListItem>  
                     <asp:ListItem  Value="Level" Text="处理等级" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Submit.RealName" Text="提交人" Selected="True"></asp:ListItem>
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
                     <asp:ListItem  Value="InvoiceNumber" Text="发票号码" ></asp:ListItem>
                     <asp:ListItem  Value="Amount" Text="金额" ></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="账户" ></asp:ListItem>
                     <asp:ListItem  Value="Title" Text="抬头" ></asp:ListItem>
                     <asp:ListItem  Value="Recipient" Text="接收人"  ></asp:ListItem>
                      <asp:ListItem  Value="Mobile" Text="手机号码" ></asp:ListItem>
                      <asp:ListItem  Value="Postcode" Text="邮政编码" ></asp:ListItem>
                     <asp:ListItem  Value="Address" Text="地址"  ></asp:ListItem>
                     <asp:ListItem  Value="IsFlush" Text="是否为冲帐" ></asp:ListItem>
                     <asp:ListItem  Value="Remark" Text="备注"  ></asp:ListItem>
                     <asp:ListItem  Value="User.RealName" Text="所属人"></asp:ListItem>
                     <asp:ListItem  Value="Status" Text="状态" ></asp:ListItem>
                     <asp:ListItem  Value="StatusTime" Text="处理时间" ></asp:ListItem>  
                     <asp:ListItem  Value="Level" Text="处理等级" ></asp:ListItem>
                     <asp:ListItem  Value="Submit.RealName" Text="提交人姓名"></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间"  Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="font">
                排序方式
            </td>
            <td>
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
            </td>
             <td colspan="4">
                  <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                 <asp:Button ID="btnExcel" runat="server" Text="导出Excel" CssClass="lmbtn btn" ExcelName="销项发票"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
         <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
    
        </div>

        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="IsFlush">
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
          <asp:TemplateField HeaderText="处理" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='handle.aspx?id=<%#Eval("Id") %>' target="_blank" name="Handle">处理</a>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="绑定核销" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='BindOrder.aspx?id=<%#Eval("Id") %>' target="_blank" name="Invoice" style='<%#GetWorkflowPropertyVisible(Eval("Status").Convert<int>(),"Invoices",Eval("User.Id").Convert<long>()) %>' >绑定核销</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编辑核销" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='EditBindOrder.aspx?id=<%#Eval("Id") %>' target="_blank" name="Invoice" style='<%#GetWorkflowPropertyVisible(Eval("Status").Convert<int>(),"Invoices",Eval("User.Id").Convert<long>()) %>'>编辑核销</a>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="发票号码"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("InvoiceNumber")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="left Sequence xlsfloat">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
               <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank">
                    <%#Eval("Account.Name")%>
               </a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="抬头"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Title")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="渠道"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("ChannelTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="接收人"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Recipient")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="手机号码"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Mobile")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="邮政编码"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Postcode")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="快递公司"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("ExpressName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="快递单号"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("ExpressNumber")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="地址"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Address")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否为冲帐"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("IsFlushName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="所属人"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="处理时间"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("StatusTime")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="处理等级"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
               <span style='color:#<%#GetLevelColumnColor(Eval("Level"))%>;'> <%#Eval("Level")%></span>    
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="提交人"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Submit.RealName")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,IsFlush" OrderByExp="Id desc"   />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
         <Triggers>
         <asp:PostBackTrigger ControlID="btnExcel"/>
     </Triggers>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
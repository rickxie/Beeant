<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payout.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>付款列表</title>  
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
                    名称 
                </td>
                <td class="text">
                    <asp:TextBox ID="txtName" runat="server" CssClass="seinput" 
                        SearchParamterName="Name" 
                        SearchWhere="Name.Contains(@Name) "></asp:TextBox>
                 
                </td>
        </tr>
        <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext">
                <asp:CheckBoxList ID="ckSelectList" runat="server">
                     <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                      <asp:ListItem  Value="Name" Text="名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Amount" Text="金额" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="账户"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Currency" Text="货币" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="PayTime" Text="申请付款日期" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="SourceAmount" Text="原币种金额" Selected="True" ></asp:ListItem>
                      <asp:ListItem  Value="PayType" Text="收付方式" ></asp:ListItem>
                      <asp:ListItem  Value="Amountinwords" Text="大写金额" ></asp:ListItem>
                     <asp:ListItem  Value="OriginalNumber" Text="原始凭证编号"  ></asp:ListItem>
                      <asp:ListItem  Value="IsFlush" Text="是否为冲帐" ></asp:ListItem>
                      <asp:ListItem  Value="BankNumber" Text="银行账号" ></asp:ListItem>
                       <asp:ListItem  Value="BankName" Text="开户银行" ></asp:ListItem>
                        <asp:ListItem  Value="BankHolder" Text="银行开户人" ></asp:ListItem>
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
                     <asp:ListItem  Value="Amount" Text="金额" ></asp:ListItem>
                     <asp:ListItem  Value="Account.Id" Text="账户" ></asp:ListItem>
                     <asp:ListItem  Value="Currency" Text="货币" ></asp:ListItem>
                     <asp:ListItem  Value="PayTime" Text="到账日期"  ></asp:ListItem>
                     <asp:ListItem  Value="SourceAmount" Text="原币种金额" ></asp:ListItem>
                     <asp:ListItem  Value="PayType" Text="收付方式" ></asp:ListItem>
                     <asp:ListItem  Value="Amountinwords" Text="大写金额" ></asp:ListItem>
                     <asp:ListItem  Value="OriginalNumber" Text="原始凭证编号" ></asp:ListItem>
                     <asp:ListItem  Value="IsFlush" Text="是否为冲帐" ></asp:ListItem>
                      <asp:ListItem  Value="BankNumber" Text="银行账号" ></asp:ListItem>
                       <asp:ListItem  Value="BankName" Text="银行名称" ></asp:ListItem>
                        <asp:ListItem  Value="BankHolder" Text="银行开户人" ></asp:ListItem>
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
            <td  colspan="4">
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
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
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="IsFlush" >
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
        <asp:TemplateField HeaderText="打印" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='PrintDetail.aspx?id=<%#Eval("Id") %>' target="_blank" name="PrintEntity">打印</a>
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
                <a href='BindPurchase.aspx?id=<%#Eval("Id") %>' target="_blank"  name="Paid" >绑定核销</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑核销" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='EditBindPurchase.aspx?id=<%#Eval("Id") %>' target="_blank" name="Paid" >编辑核销</a>
            </ItemTemplate> 
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                 <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Name")%></a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="货币"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Currency")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="申请付款日期"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("PayTime")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="原币种金额"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("SourceAmount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="收付方式"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("PayType")%>
                
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="大写金额"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Amountinwords")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="原始凭证编号"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("OriginalNumber")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否为冲帐"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("IsFlushName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="银行账号"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("BankNumber")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="开户银行"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("BankName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="银行开户人"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("BankHolder")%>
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
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
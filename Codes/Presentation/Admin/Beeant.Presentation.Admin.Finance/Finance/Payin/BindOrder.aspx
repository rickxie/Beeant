<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BindOrder.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payin.BindOrder" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register src="../../Controls/Workflow/StatusCheckBoxList.ascx" tagname="StatusCheckBoxList" tagprefix="uc4" %>
<%@ Register src="../../Controls/Workflow/LevelCheckBoxList.ascx" tagname="LevelCheckBoxList" tagprefix="uc5" %>
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
                     <asp:ListItem Value="Id" Text="订单编号" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="OrderDate" Text="下单日期" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Follow.RealName" Text="跟单人" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Status" Text="状态" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="StatusTime" Text="状态更新时间" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Level" Text="级别"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="User.RealName" Text="所属人"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Submit.RealName" Text="提交人"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="提交时间"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="DeliveryDate" Text="交货时间" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="TotalAmount" Text="应收金额"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="PayAmount" Text="已付金额"  Selected="True"></asp:ListItem>     
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem Value="Id" Text="订单编号" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="OrderDate" Text="下单日期" ></asp:ListItem>
                     <asp:ListItem  Value="Follow.RealName" Text="跟单人" ></asp:ListItem>
                     <asp:ListItem  Value="Status" Text="状态"  ></asp:ListItem>
                     <asp:ListItem  Value="StatusTime" Text="状态更新时间" ></asp:ListItem>
                     <asp:ListItem  Value="Level" Text="级别" ></asp:ListItem>
                     <asp:ListItem  Value="User.RealName" Text="所属人" ></asp:ListItem>
                     <asp:ListItem  Value="Submit.RealName" Text="提交人" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="提交时间" ></asp:ListItem>
                     <asp:ListItem  Value="DeliveryDate" Text="交货时间"></asp:ListItem>
                     <asp:ListItem  Value="TotalAmount" Text="应收金额" ></asp:ListItem>
                     <asp:ListItem  Value="PayAmount" Text="已付金额" ></asp:ListItem>                      
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
         
         <asp:Button ID="btnBindOrder" runat="server" Text="绑定" CssClass="btn mbtn" onclick="btnBindOrder_Click"   ConfirmBox="Price" ConfirmMessage="您确定要绑定订单吗" ComfirmCheckBoxMessage="你没有选择任何行"/>
        
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove, Price"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="订单编号"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>       
        <asp:TemplateField HeaderText="下单日期"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("OrderDate", "{0:yyyy-MM-dd}")%>
            </ItemTemplate>
        </asp:TemplateField>        
        <asp:TemplateField HeaderText="跟单人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Follow.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="状态更新时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusTime", "{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="级别"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Level")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="所属人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="提交人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Submit.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="提交时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("InsertTime", "{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>   
        <asp:TemplateField HeaderText="交货时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("DeliveryDate", "{0:yyyy-MM-dd}")%>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="应收金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TotalAmount")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="已付金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("PayAmount")%>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="本次应收"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%# Convert.ToDouble(Eval("Amount").ToString())-Convert.ToDouble(Eval("ReceivedAmount").ToString()) %>' id="txtPrice" runat="server" type="text" class="input" />                
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input id="txtRemark" runat="server" type="text" class="input" />                
            </ItemTemplate>
        </asp:TemplateField>                          
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"  SelectExp="Id,TotalAmount,PayAmount" OrderByExp="Id desc"   />     
   <uc3:Progress ID="Progress1" runat="server" />
         <input id="hfAccountId" runat="server" type="hidden"  />

     </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>

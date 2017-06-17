<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payline.List" MasterPageFile="~/Main.Master" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>支付列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
      
         <tr>
             <td class="font">
                    充值方式 
                </td>
                <td class="text">
             
                       <uc6:GeneralDropDownList ID="ddlPaylineType" runat="server" SearchPropertyTypeName="Type"   SearchParamterName="Type" SearchWhere="Type==@Type" IsEnum="True" ObjectName="Beeant.Domain.Entities.Finance.PaylineType" />
                </td>
             <td class="font">
                    流水号 
                </td>
                <td class="text">
                       <asp:TextBox ID="txtDataId" runat="server" CssClass="seinput" 
                        SearchParamterName="DataId" SearchWhere="DataId==@DataId"></asp:TextBox>
                </td>
             <td class="font">
                    账户 
                </td>
                <td class="text">
                  <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId"  />
                 
                </td>
                <td class="font">
                    编号 
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
                     <asp:ListItem  Value="Amount" Text="金额" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="类型" Selected="True"></asp:ListItem>
                     <asp:ListItem Value="Account.Id,Account.Name" Text="账户"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsStatus" Text="是否有效" Selected="True"  ></asp:ListItem>
                     <asp:ListItem  Value="DataId" Text="流水号" Selected="True"  ></asp:ListItem>
                     <asp:ListItem  Value="Remark" Text="备注" Selected="True"  ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" Selected="True" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem Value="Id" Text="编号"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Amount" Text="金额"></asp:ListItem>
                     <asp:ListItem  Value="Type" Text="类型" ></asp:ListItem>
                     <asp:ListItem  Value="Account.Id" Text="账户" ></asp:ListItem>
                     <asp:ListItem  Value="DataId" Text="流水号"   ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
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
                <asp:Button ID="btnExcel" runat="server" Text="导出Excel" CssClass="lmbtn btn" ExcelName="充值列表"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>



        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
           
         <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
          <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center xlstext">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
            
        </asp:TemplateField>
                <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Entity">详情</a>
            </ItemTemplate>
            <EditItemTemplate>
                <input type="text" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="right xlsfloat">
            <ItemTemplate>
                <%#Eval("Amount")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="类型"  ItemStyle-CssClass="right Sequence xlsfloat">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
              <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Name")%></a>  
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="是否有效"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("IsStatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="流水号"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("DataId")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Remark")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id" OrderByExp="Id desc"   />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
         <Triggers>
         <asp:PostBackTrigger ControlID="btnExcel"/>
     </Triggers>
 </asp:UpdatePanel>

     
     

 </asp:Content>
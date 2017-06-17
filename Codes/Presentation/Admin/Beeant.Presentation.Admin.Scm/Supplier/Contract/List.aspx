
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Contract.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
  <%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
 <title>合同列表</title>  
    <style type="text/css">
        .style1
        {
            width: 180px;
        }
        .style2
        {
            width: 80px;
        }
    </style>
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
     
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
              <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
                    <tr>
                       
                       
                        <td class="font">
                            支付方式
                        </td>
                        <td class="text">
                            <uc1:GeneralDropDownList ID="ddlPaymentType"  runat="server" SaveName="PaymentType" BindName="PaymentType" 
                            SearchWhere="PaymentType==@PaymentType" SearchParamterName="PaymentType"  SearchPropertyTypeName="PaymentType"
                                 ObjectName="Beeant.Domain.Entities.Supplier.ContractPaymentType" IsEnum="True" ValidateName="PaymentTypeName" />
                        </td>
                         <td class="font">
                            配送方式
                        </td>
                        <td class="text">
                            <uc1:GeneralDropDownList ID="ddlDispatchType"  runat="server" SaveName="DispatchType" BindName="DispatchType" 
                            SearchWhere="DispatchType==@DispatchType" SearchParamterName="DispatchType"  SearchPropertyTypeName="DispatchType"
                                 ObjectName="Beeant.Domain.Entities.Supplier.ContractDispatchType" IsEnum="True" />
                        </td>
                        <td class="font">
                            发票类型
                        </td>
                        <td class="text" colspan="3">
                            <uc1:GeneralDropDownList ID="ddlBillType"  runat="server" SaveName="BillType" BindName="BillType" 
                            SearchWhere="BillType==@BillType" SearchParamterName="BillType"  SearchPropertyTypeName="BillType"
                                 ObjectName="Beeant.Domain.Entities.Supplier.ContractBillType" IsEnum="True" />
                        </td>

                    </tr>
                    <tr>
                        <td class="font" style="width: 80px">
                            显示内容
                        </td>
                        <td class="mtext" colspan="7"> 
                            <asp:CheckBoxList ID="ckSelectList" runat="server" >
                                 <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                                 <asp:ListItem  Value="Supplier.Name" Text="供应商名称" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="SettlementType" Text="结算方式" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="PaymentType" Text="支付方式" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="DispatchType" Text="配送方式" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="BillType" Text="发票类型" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="StartDate" Text="合同起始日期" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="EndDate" Text="合同结束日期" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="Rebate" Text="返利条件说明" Selected="True"></asp:ListItem>
                                 <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                                 <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="font" style="width: 80px">
                            排序
                        </td>
                        <td class="mtext"  colspan="2">
                            <asp:DropDownList ID="ddlOrderbyList" runat="server">
                                 <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                                 <asp:ListItem  Value="Supplier.Id" Text="供应商名称" ></asp:ListItem>
                                 <asp:ListItem  Value="SettlementType" Text="结算方式" ></asp:ListItem>
                                 <asp:ListItem  Value="PaymentType" Text="支付方式" ></asp:ListItem>
                                 <asp:ListItem  Value="DispatchType" Text="配送方式" ></asp:ListItem>
                                 <asp:ListItem  Value="BillType" Text="发票类型"></asp:ListItem>
                                 <asp:ListItem  Value="StartDate" Text="合同起始日期" ></asp:ListItem>
                                 <asp:ListItem  Value="EndDate" Text="合同结束日期" ></asp:ListItem>
                                 <asp:ListItem  Value="Rebate" Text="返利条件说明" ></asp:ListItem>
                                 <asp:ListItem  Value="Attachment" Text="合同附件" ></asp:ListItem>
                                 <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                                 <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="font">
                            排序方式
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                            <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="style1" colspan="2">
                            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                            <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                            <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
                        </td>
                    </tr>
              </table>
        </div>

        <div class="mainten">
            <a href="Add.aspx?SupplierId=<%=Request.Params["SupplierId"] %>" name="Add" target="_blank"class="btn" >添加</a>
            <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
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
                    <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,Status"  />
                </ItemTemplate>
             </asp:TemplateField>
              <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
                 <ItemTemplate>
                     <a href='Update.aspx?id=<%#Eval("Id") %>&SupplierId=<%=Request.Params["SupplierId"] %>' target="_blank" name="Edit">编辑</a>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
                 <ItemTemplate>
                     <a href='Detail.aspx?id=<%#Eval("Id") %>&SupplierId=<%=SupplierId %>' target="_blank" name="Entity">详情</a>
                 </ItemTemplate>
             </asp:TemplateField>

             <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
                 <ItemTemplate>
                     <%#Eval("Id")%>
                 </ItemTemplate>
             </asp:TemplateField>
              <asp:TemplateField HeaderText="供应商名称"  ItemStyle-CssClass="left ">
                 <ItemTemplate>
                     <%#Eval("Supplier.Name")%>
                 </ItemTemplate>
             </asp:TemplateField>
                 <asp:TemplateField HeaderText="结算方式"  ItemStyle-CssClass="left ">
                 <ItemTemplate>
                     <%#Eval("SettlementType")%>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="支付方式"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("PaymentTypeName")%>
                 </ItemTemplate>
             </asp:TemplateField>
            
             <asp:TemplateField HeaderText="配送方式"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("DispatchTypeName")%>
                 </ItemTemplate>
             </asp:TemplateField>
              <asp:TemplateField HeaderText="发票类型"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("BillTypeName")%>
                 </ItemTemplate>
             </asp:TemplateField>

             <asp:TemplateField HeaderText="合同起始日期"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("StartDate")%>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="合同结束日期"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("EndDate")%>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="返利条件说明"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("Rebate")%>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="录入时间"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("InsertTime")%>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="编辑时间"  ItemStyle-CssClass="left">
                 <ItemTemplate>
                     <%#Eval("UpdateTime")%>
                 </ItemTemplate>
             </asp:TemplateField>
           </Columns>
     </asp:GridView>
     
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id"  OrderByExp="Id desc" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
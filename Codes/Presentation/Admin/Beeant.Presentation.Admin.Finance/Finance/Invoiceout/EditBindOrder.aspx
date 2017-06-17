<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EditBindOrder.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Invoiceout.EditBindOrder" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>编辑核销</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div id="divSearch" class="search" runat="server" >
                <table class="tb">
                     <uc2:DataSearch ID="DataSearch1" runat="server" />
                    <tr>
                        <td class="font">
                            订单编号 
                        </td>
                        <td class="text" colspan="7">
                                <asp:TextBox ID="txtOrderId" runat="server" CssClass="seinput" 
                                SearchParamterName="OrderId" 
                                SearchWhere="Order.Id==@OrderId"></asp:TextBox>
                        </td>
                    </tr>
                  <tr>
                    <td class="font">
                        显示内容
                    </td>
                    <td colspan="7" class="mtext">
                        <asp:CheckBoxList ID="ckSelectList" runat="server">
                             <asp:ListItem  Value="Order.Id" Text="订单编号" Selected="True"></asp:ListItem>
                              <asp:ListItem  Value="Order.OpenAmount" Text="应开金额" Selected="True"></asp:ListItem>
                             <asp:ListItem  Value="Order.InvoiceAmount" Text="已开金额" Selected="True"></asp:ListItem>
                             <asp:ListItem  Value="Amount" Text="本次收款" Selected="True" ></asp:ListItem>
                             <asp:ListItem  Value="IsStatus" Text="状态" Selected="True" ></asp:ListItem>
                             <asp:ListItem  Value="Remark" Text="备注"  Selected="True"></asp:ListItem>
                              <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                              <asp:ListItem  Value="UpdateTime" Text="更新时间" ></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
            
                    </tr>
                    <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                            <asp:ListItem  Value="Order.Id" Text="订单编号" ></asp:ListItem>
                            <asp:ListItem  Value="Order.OpenAmount" Text="应开金额" ></asp:ListItem>
                             <asp:ListItem  Value="Order.InvoiceAmount" Text="已开金额" ></asp:ListItem>
                             <asp:ListItem  Value="Amount" Text="本次收款" ></asp:ListItem>
                             <asp:ListItem  Value="IsStatus" Text="状态"  ></asp:ListItem>
                             <asp:ListItem  Value="Remark" Text="备注"  ></asp:ListItem>
                              <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                              <asp:ListItem  Value="UpdateTime" Text="更新时间" ></asp:ListItem>
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
                <asp:Button ID="btnModifyAmount" runat="server" Text="修改" CssClass="btn" OnClick="btnModifyAmount_Click"   ConfirmBox="Amount" ConfirmMessage="您确定要修改金额吗" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>    
                <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>    
            </div>
            <div class="list">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table">
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
                        <asp:TemplateField HeaderText="订单编号" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                 <%#Eval("Order.Id")!=null&&Eval("Order.Id").ToString()!="0" ? String.Format("<a href='/Order/Order/Detail.aspx?id={0}' target='_blank'>{0}</a>",Eval("Order.Id")):""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="应付款" ItemStyle-CssClass="center operate">
                              <ItemTemplate>
                                  <%#Eval("Order.OpenAmount") %>
                              </ItemTemplate>
                          </asp:TemplateField>
                        <asp:TemplateField HeaderText="已开金额" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <%#Eval("Order.InvoiceAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="本次收款"  ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <input value='<%#Eval("Amount")%>' id="txtAmount" runat="server" type="text" class="input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                 <%#Eval("IsStatusName")%>  
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                 <input value='<%#Eval("Remark") %>' id="txtRemark" runat="server" type="text" class="input" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="录入时间"  ItemStyle-CssClass="left Sequence">
                            <ItemTemplate>
                                 <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%> 
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="更新时间"  ItemStyle-CssClass="left Sequence">
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
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sku.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Product.Sku"
    MasterPageFile="~/Datum.Master" %>
<%@ Register TagPrefix="uc2" TagName="Message" Src="~/Controls/Message.ascx" %>
<%@ Register TagPrefix="uc3" Namespace="Beeant.Presentation.Admin.Erp.Controls" Assembly="Beeant.Presentation.Admin.Erp" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>产品SKU属性更新</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
               <div id="Edit" class="edit">
                <input type="button" id="Hide" class="btn" value="隐藏" />
               <table class="tb">
                      <tr>
                        <td class="font">
                            属性
                        </td>
                        <td colspan="3" class="mtext">
                          
                            <asp:CheckBoxList ID="CheckBoxList1" runat="server" 
                                RepeatDirection="Horizontal" AutoPostBack="True" 
                                onselectedindexchanged="CheckBoxList1_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            属性值
                        </td>
                        <td colspan="3" class="mtext">
                           <div id="divPropery" runat="server"></div>
                        </td>
                    </tr>
                 
                    <tr>
                        <td colspan="2" class="center">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" 
                                 />
                        </td>
                    </tr>
                </table>
                    <uc2:Message ID="Message1" runat="server" />
                <input id="IdControl" type="hidden" runat="server" />

            </div>
         
 
    <div class="list">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table">
                    <Columns>
                        <asp:BoundField HeaderText="序号" ItemStyle-CssClass="sequence" />
                         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
               <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
                     <asp:TemplateField HeaderText="编号" ItemStyle-CssClass="left name">
                            <ItemTemplate>
                                <%#Eval("Id")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="名称" ItemStyle-CssClass="left name">
                            <ItemTemplate>
                                <%#Eval("Name")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="Sku" ItemStyle-CssClass="left name">
                            <ItemTemplate>
                                <%#Eval("Sku")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
                            <ItemTemplate>
                                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <uc1:Pager ID="Pager1" runat="server" PageSize="10" SelectExp="Id,Name,Sku,InsertTime"
                FromExp="ProductEntity" />
            <uc3:Progress ID="Progress1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

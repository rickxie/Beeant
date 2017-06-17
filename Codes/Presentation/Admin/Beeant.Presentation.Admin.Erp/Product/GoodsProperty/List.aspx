<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.GoodsProperty.List"
    MasterPageFile="~/Datum.Master" %>

<%@ Register Src="/Controls/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<%@ Register Src="/Controls/Progress.ascx" TagName="Progress" TagPrefix="uc3" %>
<%@ Register Src="/Controls/Message.ascx" TagName="Message" TagPrefix="uc4" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>产品属性列表</title>
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
                            <uc5:GeneralDropDownList ID="ddlProperty" runat="server" AutoPostBack="True" SaveName="Property.Id"
                                onselectedindexchanged="ddlProperty_SelectedIndexChanged"  ObjectName="Beeant.Domain.Entities.Product.PropertyEntity" />
                            <asp:DropDownList ID="ddlValue" runat="server" SaveName="Value" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                 
                    <tr>
                        <td colspan="2" class="center">
                            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
                        </td>
                    </tr>
                </table>
                <uc4:Message ID="Message1" runat="server" />
                <input id="IdControl" type="hidden" runat="server" />
            </div>
            
            <div class="mainten">
                <a href='javascript:void(0);' id="Add" class="btn">添加</a>
                <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn" />
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
                
                        <asp:TemplateField HeaderText="名称" ItemStyle-CssClass="left name">
                            <ItemTemplate>
                                <%#Eval("Property.Name")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="来源" ItemStyle-CssClass="left">
                            <ItemTemplate>
                                <%#Eval("Value")%>
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
            <uc1:Pager ID="Pager1" runat="server" PageSize="10" SelectExp="Id,Property.Name,Value,InsertTime"
                FromExp="GoodsPropertyEntity" />
            <uc3:Progress ID="Progress1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

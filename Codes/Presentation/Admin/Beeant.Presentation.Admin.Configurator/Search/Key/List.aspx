<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Search.Key.List"
    MasterPageFile="~/Main.Master" %>

<%@ Register Src="/Controls/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<%@ Register Src="/Controls/DataSearch.ascx" TagName="DataSearch" TagPrefix="uc2" %>
<%@ Register Src="/Controls/Progress.ascx" TagName="Progress" TagPrefix="uc3" %>
<%@ Register Src="/Controls/Message.ascx" TagName="Message" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>关键词列表</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Edit" class="edit">
                <input type="button" id="Hide" class="btn" value="隐藏" />
                <table class="tb">
                    <tr>
                        <td class="font">
                            名称
                        </td>
                        <td colspan="3" class="mtext">
                            <input id="txtName" runat="server" type="text" class="input" bindname="Name" savename="Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            来源
                        </td>
                        <td colspan="3" class="mtext">
                            <input id="txtSource" runat="server" type="text" class="input" bindname="Source"
                                savename="Source" />
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
            <div id="divSearch" class="search" runat="server">
                <table class="tb">
                    <tr>
                        <uc2:DataSearch ID="DataSearch1" runat="server" />
                    </tr>
                    <tr>
                        <td class="font">
                            名称
                        </td>
                        <td class="text">
                            <asp:TextBox ID="txtSearchName" runat="server" SearchWhere="Name.Contains(@Name)"
                                SearchParamterName="Name"></asp:TextBox>
                        </td>
                        <td class="font">
                            索引库
                        </td>
                        <td class="mtext" colspan="5">
                            <asp:TextBox ID="txtSearchSource" runat="server" SearchWhere="Source==@Source" SearchParamterName="Source"></asp:TextBox>
                        </td>
                    </tr>
                </table>
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
                        <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="名称" ItemStyle-CssClass="left name">
                            <ItemTemplate>
                                <%#Eval("Name")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="来源" ItemStyle-CssClass="left">
                            <ItemTemplate>
                                <%#Eval("Source")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IP" ItemStyle-CssClass="left">
                            <ItemTemplate>
                                <%#Eval("Ip")%>
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
            <uc1:Pager ID="Pager1" runat="server" PageSize="10" SelectExp="Id,Name,Source,Ip,InsertTime"
                FromExp="KeyEntity" />
            <uc3:Progress ID="Progress1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

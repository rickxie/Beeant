<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Member.Invoice.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>审核详情</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<div class="info">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="tb">
                    <tr>
                        <td class="font">
                            账户名称
                        </td>
                        <td class="mtext">
                            <a href="/Finance/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" BindName="Account.Id"> <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label></a>
                        </td>

                        <td class="font">
                            发票类型
                        </td>
                        <td class="text">
                            <asp:Label ID="lblTypeName" runat="server" BindName="TypeName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            发票类型名称
                        </td>
                        <td class="mtext" colspan="3">
                            <asp:Label ID="lblGeneralTypeName" runat="server" BindName="GeneralTypeName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            发票抬头
                        </td>
                        <td class="text">
                            <asp:Label ID="lblTitle" runat="server"  BindName="Title"></asp:Label>
                        </td>
                        <td class="font">
                            状态名称
                        </td>
                        <td class="text">
                             <asp:Label ID="lblStatusName" runat="server" BindName="StatusName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="font">
                            发票内容
                        </td>
                        <td class="text">
                            <asp:Label ID="lblContent" runat="server" BindName="Content"></asp:Label>
                        </td>
                        <td class="text">
                            <asp:Label ID="LabelName" runat="server" BindName="Name"></asp:Label>
                        </td>
                    </tr>

                     <tr>
             <td colspan="4" class="center">
                <input id="btnClose" type="button" value="关闭" class="btn" />
             </td>
           </tr>
                </table>
                
                <uc3:progress id="Progress1" runat="server" />
             </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>

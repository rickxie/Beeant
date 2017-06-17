<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Certification.Detail" %>

<%@ Register src="../../Controls/Progress.ascx" tagname="Progress" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>供应商证书详情</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="info">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <table class="tb">
            <tr><td class="font">供应商</td><td class="mtext"><asp:Label runat="server" ID="lblSupplierName" BindName="Supplier.Name"></asp:Label></td></tr>
            <tr>
                <td class="font">供应商其他证书</td>
                <td class="mtext" colspan="3">
                    <img ID="Image1" runat="server" BindName="FullCertification" />                          
                </td>
            </tr>
            <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
        </table>
        <uc1:Progress ID="Progress1" runat="server" />
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    
</asp:Content>

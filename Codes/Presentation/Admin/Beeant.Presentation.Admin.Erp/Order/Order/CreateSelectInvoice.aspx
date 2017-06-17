<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateSelectInvoice.aspx.cs"
    Inherits="Beeant.Presentation.Admin.Erp.Order.Order.CreateSelectInvoice" ValidateRequest="false" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>

<%@ Register Src="/Controls/Message.ascx" TagName="Message" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc5" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <link href="<%=Page.GetUrl("PresentationAdminHomeUrl")%>/Styles/Style.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="/scripts/Winner/Winner.ClassBase.js"></script>
      <script type="text/javascript" src="/scripts/plug/jquery-1.7.1.min.js"></script>
           <script type="text/javascript" src="/Scripts/winner/validator/winner.validator.js"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <input type="hidden" name="h_rtype" id="h_rtype" value="0" />
    <div class="main" style="padding: 0; margin: 0; position: relative; top: 0;">
        <div class="body" style="padding: 0; margin: 0;">
            <div class="edit">
                    <table class="tb">
                        <tr>
                            <td class="font">
                                发票抬头
                            </td>
                            <td class="mtext" colspan="3">
                                <input id="txtTitle" runat="server" type="text" value="无" class="input long" bindname="Title"
                                    savename="Title" />
                            </td>
                        </tr>
                        <tr>
                            <td class="font">
                                发票内容
                            </td>
                            <td class="mtext" colspan="3">
                                <input id="txtContent" runat="server" type="text" value="无" class="input long" bindname="Content"
                                    savename="Content" />
                            </td>
                        </tr>
                        <tr>
                            <td class="font">
                                接收人
                            </td>
                            <td class="text">
                                <input id="txtRecipient" runat="server" type="text" class="input" bindname="Recipient"
                                    savename="Recipient" />
                            </td>
                            <td class="font">
                                手机号码
                            </td>
                            <td class="text">
                                <input id="txtMobile" runat="server" type="text" class="input" bindname="Mobile"
                                    savename="Mobile" />
                            </td>
                        </tr>
                        <tr>
                            <td class="font">
                                发票类型
                            </td>
                            <td class="text">
                                <uc5:GeneralDropDownList ID="ddlType" runat="server" SaveName="Type"
                                    BindName="Type" ObjectName="Beeant.Domain.Entities.Finance.InvoiceType"
                                    IsEnum="True" />
                            </td>
                            <td class="font">
                                开票类型
                            </td>
                            <td class="text" >
                                <uc5:GeneralDropDownList ID="ddlGeneralType" runat="server" SaveName="GeneralType"
                                    BindName="GeneralType" ObjectName="Beeant.Domain.Entities.Finance.InvoiceGeneralType"
                                    IsEnum="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="font">
                                地址
                            </td>
                            <td class="mtext" colspan="3">
                                <input id="txtAddress" runat="server" type="text" class="input long" bindname="Address"
                                    savename="Address" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="center">
                                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
                            </td>
                        </tr>
                    </table>
          
            </div>
           
            <uc2:message id="Message1" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>

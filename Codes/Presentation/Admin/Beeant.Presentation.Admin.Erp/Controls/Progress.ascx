<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Progress.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Controls.Progress" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1">
                    <ProgressTemplate>
                          
         <div class="process">
                     <div class="loading"> <img src='<%=Page.GetUrl("PresentationAdminHomeUrl")%>/Images/process.gif' alt="" />
                        <img src='<%=Page.GetUrl("PresentationAdminHomeUrl")%>/Images/stop.gif'  alt="" onclick="if (Sys.
    Admins.PageRequestManager.getInstance().get_isInAsyncPostBack()) { Sys.
        Admins.PageRequestManager.getInstance().abortPostBack(); }" />
        </div>
                <div class="shelter"></div>                 
</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
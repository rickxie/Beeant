<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Property.Import" MasterPageFile="~/Datum.Master" ValidateRequest="false" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %> 
 <%@ Register TagPrefix="uc4" TagName="GeneralTreeView" Src="~/Controls/GeneralTreeView.ascx" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>属性导入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
 
  
<div class="edit">
    <table class="tb">
        
        <tr>
            <td class="font">当前导入属性</td>
            <td class="mtext"  colspan="3" >
                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
          
            </td>
           
        </tr>
 
       <tr>
       <td class="font">类目</td>
            <td class="mtext" colspan="3">
                 <uc4:GeneralTreeView ID="tvCategoryTree" runat="server" EntityName="CategoryEntity" OnSelectedNodeChanged="tvCategoryTree_SelectedNodeChanged" IsShowNone="True" TreeView-ShowCheckBoxes="All"  />
 
            </td>
           
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" 
                    CssClass="btn" onclick="btnSave_Click"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 



 
    <uc2:Message ID="Message1" runat="server" />
    <uc2:Message ID="Message2" runat="server" />
        </ContentTemplate>
</asp:UpdatePanel>
 
 </asp:Content>
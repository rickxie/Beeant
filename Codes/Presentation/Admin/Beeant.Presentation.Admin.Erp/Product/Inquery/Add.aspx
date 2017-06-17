<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Inquery.Add" MasterPageFile="~/Datum.Master" ValidateRequest="false" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>商品询问录入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
 
<div class="edit">
    <table class="tb">
    

      <tr>
               <td class="font">问题</td>
            <td class="mtext" colspan="3">
                 <input id="txtQuestion" type="text" runat="server" class="input long" BindName="Question" SaveName="Question"/>
            </td>
          
       </tr>
        <tr>
            <td class="font">提问时间</td>
            <td class="text" colspan="3" >
                <asp:TextBox ID="txtAnswerTime" runat="server" CssClass="input" BindName="AnswerTime" SaveName="AnswerTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtAnswerTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>

            </td>
            
       </tr>
   
        <tr>
           <td class="font">回答</td>
            <td class="mtext" colspan="3">
                <textarea id="txtAnswer" runat="server"  type="text" class="input long"  BindName="Answer" SaveName="Answer"></textarea>
            </td>
        
       </tr>

         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>



 
    <uc2:Message ID="Message1" runat="server" />

        </ContentTemplate>
</asp:UpdatePanel>
 
 </asp:Content>
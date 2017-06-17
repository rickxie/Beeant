<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderComplaint.Edit" %>
     
<div class="edit">
    <table class="tb">
    

      <tr>
               <td class="font">问题</td>
            <td class="mtext" colspan="3" >
                  <asp:Label ID="lblQuestion" runat="server" Text=""  BindName="Question"></asp:Label>
            </td>
            
       </tr>
        <tr>
             <td class="font">提问时间</td>
            <td class="text" >
                   <asp:Label ID="lblInsertTime" runat="server" Text=""  BindName="InsertTime"></asp:Label>
            </td>
             <td class="font">订单</td>
            <td class="text" >
                <a href="/Order/Order/Detail.aspx?Id=" id="hfOrderId" runat="server" bindname="Order.Id">
                   <asp:Label ID="lblOrderId" runat="server" Text="" BindName="Order.Id"></asp:Label>
                 </a>
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
 
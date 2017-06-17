<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.Order.Update" MasterPageFile="~/Datum.Master" ValidateRequest="false"%>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc8" %>
 

 

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc3" TagName="UserComboBox" Src="~/Controls/User/UserComboBox.ascx" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>

 
 
 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单编辑</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


    




    


  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
     <input id="hfStatusControl" type="hidden" runat="server" />
 <div class="edit">
     <a href='add.aspx?id=<%=RequestId%>'  name="Add">新增</a>
     <a href='Detail.aspx?id=<%=RequestId%>'  name="Entity">详情</a>
   <a href='handle.aspx?id=<%=RequestId%>'  name="Edit">处理</a>
      <table class="tb">
        
          <tr>
            <td class="font">账户</td>
            <td class="mtext"  >
                <uc8:AccountComboBox ID="AccountComboBox1" runat="server" />
            </td>
              <td class="font">下单日期</td>
            <td class="text">
                <asp:TextBox ID="txtOrderDate" runat="server"  CssClass="input" BindName="OrderDate" SaveName="OrderDate" > </asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOrderDate" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
             </td>
        </tr>   
      
         <tr>
           
                <td class="font">订单类型</td>
            <td class="text"  >
             <uc10:GeneralDropDownList ID="ddlType" runat="server" SaveName="Type" BindName="Type" ObjectName="Beeant.Domain.Entities.Order.OrderType" IsEnum="True" />
               
            </td>
            <td class="font">定金</td>
            <td class="mtext">
               
            <asp:TextBox ID="txtDeposit" CssClass="input" runat="server" BindName="Deposit" SaveName="Deposit" > </asp:TextBox>
               
            </td>
        </tr>
      
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                 <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
            </td>
        </tr>
         <tr>
           <td class="font">消息备注</td>
            <td class="mtext" colspan="3" >
                 <input id="txtHistoryRemark" runat="server"  type="text" class="input long"  />
            </td>
        </tr>
    
         <tr>
            <td colspan="4" class="center">
                 <asp:Button ID="btnPass" runat="server" Text="通过" CssClass="btn"   />
                 <asp:Button ID="btnSubmit" runat="server" Text="报审" CssClass="btn"   />
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
                <asp:Button ID="btnReject" runat="server" Text="拒绝" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  />
            </td>
        </tr>
    </table>
 
</div>

    <uc2:Message ID="Message1" runat="server" />
        
    </ContentTemplate>
</asp:UpdatePanel>
 


 



 </asp:Content>
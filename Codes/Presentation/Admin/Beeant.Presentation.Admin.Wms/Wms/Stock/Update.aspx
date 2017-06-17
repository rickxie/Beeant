<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Stock.Update" MasterPageFile="~/Main.Master" ValidateRequest="false"%>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
 
 <%@ Register TagPrefix="uc10" TagName="StockTypeDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>进出库编辑</title>  
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
      
        
        <tr>
            
            <td class="font">
                类型</td>
            <td class="text" colspan="3">
                <uc10:StockTypeDownList ID="ddlType" runat="server" BindName="Type" ObjectName="Beeant.Domain.Entities.Wms.StockType" IsEnum="True"
                    SaveName="Type" />
            </td>
           
        </tr>
        <tr>
            <td class="font">
                订单编号</td>
            <td class="text">
                <input id="txtOrderId" runat="server"  type="text" class="input"  BindName="Order.Id" SaveName="Order.Id"    />
            </td>
            <td class="font">
                采购单编号</td>
            <td class="text">
                <input id="txtPurchaseId" runat="server"  type="text" class="input"  BindName="Purchase.Id" SaveName="Purchase.Id"    />
            </td>
        </tr>
  

        <tr>
            <td class="font">
                消息</td>
            <td class="mtext" colspan="3">
                <asp:CheckBoxList ID="ckMessageType" runat="server">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="font">
                备注</td>
            <td class="mtext" colspan="3">
                <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
            </td>
        </tr>
        <tr>
            <td class="font">
                流程备注</td>
            <td class="mtext" colspan="3">
                <input id="txtHistoryRemark" runat="server"  type="text" class="input long"  />
            </td>
        </tr>
        <tr>
            <td class="center" colspan="4">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="保存" />
                <input id="btnClose" type="button" value="关闭" class="btn"  />
            </td>
        </tr>
    </table>
 
</div>


 
    <uc2:Message ID="Message1" runat="server" />

     </ContentTemplate>
</asp:UpdatePanel>
 
 </asp:Content>
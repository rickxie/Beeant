<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Stock.Add" MasterPageFile="~/Main.Master" ValidateRequest="false" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
          <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>进出库录入</title>  
   <script language="javascript" src="../../Scripts/Nancy/NancyGrid/NancyGrid.js"></script>
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
 
     <input id="hfIdControl" type="hidden" runat="server" />
 <div class="edit">
    <%=string.IsNullOrEmpty(hfIdControl.Value)?null:string.Format("<a href='Detail.aspx?id={0}'  name='Entity'>详情</a> <a href='update.aspx?id={0}'  name='Edit'>编辑</a> <a href='handle.aspx?id={0}'  name='Handle'>处理</a>",hfIdControl.Value)%>
    <table class="tb">
     <tr>
         
        
          

            <td class="font">状态
            </td >
            <td class="text">
                <asp:DropDownList ID="ddlStatus" runat="server" BindName="Status" SaveName="Status" ValidateName="Status" ></asp:DropDownList>
            </td>

             <td class="font">处理人</td >
            <td class="text">
                <uc5:UserComboBox ID="cbUser" runat="server" />
            </td>
        </tr>
  
        <tr>
       
                <td class="font">类型</td>
            <td class="text" >
                <uc10:GeneralDropDownList ID="ddlType" runat="server" SaveName="Type" BindName="Type" ObjectName="Beeant.Domain.Entities.Wms.StockType" IsEnum="True" />
              
                  
            </td>
          <td class="font">级别</td>
            <td class="text" >
               
         <asp:DropDownList ID="ddlLevel" runat="server" BindName="Level" SaveName="Level" ></asp:DropDownList>
               
            </td>
       </tr>
        <tr>
          <td class="font">订单编号</td>
            <td class="text" >
                 <input id="txtOrderId" runat="server"  type="text" class="input"  BindName="Order.Id" SaveName="Order.Id"    /> 
            </td>  
           <td class="font">采购单编号</td>
            <td class="text" >
                  <input id="txtPurchaseId" runat="server"  type="text" class="input"  BindName="Purchase.Id" SaveName="Purchase.Id"    /> 
            </td>
       </tr>
  
 
 
    <tr>
           
             <td class="font">消息</td>
            <td class="mtext" colspan="3">
               
                 <asp:CheckBoxList ID="ckMessageType" runat="server" ></asp:CheckBoxList>
               
            </td>
        </tr>
        <tr>
            <td class="font">备注</td>
            <td class="mtext" colspan="3">
             <input id="txtRemark" runat="server"  type="text" class="input long"  BindName="Remark" SaveName="Remark"  /> </td>
        </tr>
           <tr>
           <td class="font">流程备注</td>
            <td class="mtext" colspan="3" >
                 <input id="txtHistoryRemark" runat="server"  type="text" class="input long"  />
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
         <td colspan="4" >
                   <a href="javascript:void(0);" id="btnAdd" Note="note" NoteUrl='SelectProduct.aspx?SerializeValueId=<%=hfProducts.ClientID %>&SerializeContainerId=<%=gvProduct.ClientID %>'>添加商品</a>
               </td>
    </table>
        <uc2:Message ID="Message1" runat="server" />



      <div id="divProduct" >

       <input  type="hidden" id="hfProducts" runat="server"  />
        <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
            <EmptyDataTemplate>
                <tr>
				<th scope="col" >名称</th><th  >仓库</th><th >数量</th><th >备注</th>
			</tr>
            </EmptyDataTemplate>
       <Columns>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                   <input value='<%#Eval("Id") %>' id="hfId" runat="server" type="hidden"   />
                <a href='/Product/Product/Detail.aspx?id=<%#Eval("Id") %>' target="_blank"><%#Eval("Name")%></a> 
                <input id="hfName" runat="server" type="hidden" class="input" value='<%#Eval("Name")%>' />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="仓库"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Storehouse.Name")%>
               <input value='<%#Eval("Storehouse.Id") %>' id="hfStorehouseId" runat="server" type="hidden"   />
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField HeaderText="数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='1' id="txtCount" runat="server" type="text" class="input" style="width: 80px;" />                         
            </ItemTemplate>
        </asp:TemplateField> 
             <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='1' id="txtRemark" runat="server" type="text" class="input" style="width: 80px;"  />    
                                        
            </ItemTemplate>
        </asp:TemplateField>                   
        </Columns>
     </asp:GridView>
   
     </div>
     
     
        <table style="display:none;" >
        <tbody id="divTemplate">
         <tr class="out">
				<td class="left">
                <a href="/Product/Product/Detail.aspx?id=@Id" target="_blank">@Name</a> 
            </td><td class="left">
                @StorehouseName
            </td>
            <td class="left status">
                 <input value='@Count' id="txtCount" runat="server" type="text" class="input" style="width: 80px;" SerializeName="Count" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id" />     
            </td><td class="left status">
               <input value='1'  runat="server" type="text" class="input" style="width: 80px;" SerializeName="Remark" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id"   />    
                <a href="javascript:void(0);" SerializeRemove="true" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id">删除</a> 
            </td>
           
			</tr>
            </tbody>
   </table>

 
    
     <script type="text/javascript" src="/Scripts/Serializator.js"></script>
    <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
    <script type="text/javascript">
     var registerFunc = function() {
         $(document).ready(function() {
             window.Serial = new Serializator({ Serializes: [{ Id: '<%=gvProduct.ClientID %>', Html: $("#divTemplate").html(), ValueId: "<%=hfProducts.ClientID %>" }] });
             $(window.Serial.Note.Container).css("height", "800px");
             window.Serial.Initialize();
             setTimeout(function() {
                 $("#<%=gvProduct.ClientID %>").css("table-layout", "auto");
             }, 1000);
           
         });
     }

    </script>

    </div>        
    </ContentTemplate>
</asp:UpdatePanel>
 </asp:Content>
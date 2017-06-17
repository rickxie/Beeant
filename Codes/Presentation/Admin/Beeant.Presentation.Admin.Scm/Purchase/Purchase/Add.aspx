<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.Purchase.Add"
    MasterPageFile="~/Main.Master" ValidateRequest="false" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Wms" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>

<%@ Register Src="/Controls/Message.ascx" TagName="Message" TagPrefix="uc2" %>
<%@ Register Src="../../Controls/User/UserComboBox.ascx" TagName="UserComboBox"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc8" TagName="SupplierAccountComboBox" Src="~/Controls/Account/SupplierAccountComboBox.ascx" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<%@ Register src="../../Controls/Wms/StorehouseComboBox.ascx" tagname="StorehouseComboBox" tagprefix="uc4" %>
         <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>采购单录入</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <input id="hfIdControl" type="hidden" runat="server" />
    <div class="edit">
        <table class="tb">
            <tr>
                <td class="font">
                    状态
                </td>
                <td class="text">
                    <asp:DropDownList ID="ddlStatus" runat="server" BindName="Status" SaveName="Status"
                        ValidateName="Status">
                    </asp:DropDownList>
                </td>
                <td class="font">处理人</td >
            <td class="text">
                <uc5:UserComboBox ID="cbUser" runat="server" />
            </td>
            </tr>
            <tr>
                <td class="font">
                    级别
                </td>
                <td class="text">
                    <asp:DropDownList ID="ddlLevel" runat="server" BindName="Level" SaveName="Level">
                    </asp:DropDownList>
                </td>
                <td class="font">
                    跟单人
                </td>
                <td class="mul mtext">
                    <uc3:UserComboBox ID="ckUser" runat="server" IsValidateHidden="False" HiddenValidateName=""
                        HiddenSaveName="Follow.Id" HiddenBindName="Follow.Id" TextBindName="Follow.RealName"
                        TextSaveName="Follow.RealName" />
                </td>
            </tr>
            <tr>
                <td class="font">
                    供应商
                </td>
                <td class="mul mtext">
                    
                    <uc8:SupplierAccountComboBox ID="SupplierAccountComboBox1" runat="server"  HiddenValidateName="Account.Id"
                        OnChangedEvent="ChangedAccount" />
                </td>
                <td class="font">
                    订单编号
                </td>
                <td class="text">
                    <input id="txtOrderId" runat="server" type="text" class="input" bindname="Order.Id"
                        savename="Order.Id" />
                </td>
            </tr>
            <tr>
                <td class="font">
                    交货日期
                </td>
                <td class="text">
                    <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="input" BindName="DeliveryDate"
                        SaveName="DeliveryDate"> </asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDeliveryDate"
                        Format="yyyy-MM-dd">
                    </cc1:CalendarExtender>
                </td>
                <td class="font">
                    采购日期
                </td>
                <td class="text">
                    <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="input" BindName="PurchaseDate"
                        SaveName="PurchaseDate"> </asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPurchaseDate"
                        Format="yyyy-MM-dd">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="font">
                    仓库
                </td>
                <td class="text">
   
                    <uc4:StorehouseComboBox ID="StorehouseComboBox1" runat="server" IsUsed="True" IsWarehouse="True" />
                </td>
                <td class="font">采购类型</td>
                <td class="text"  >
                <uc10:GeneralDropDownList ID="ddlType" runat="server" SaveName="Type" BindName="Type" ObjectName="Beeant.Domain.Entities.Purchase.PurchaseType" IsEnum="True" />
                </td>
            </tr>
            <tr>
             <td class="font">原始采购单</td>
            <td class="mtext" colspan="3">
            <asp:TextBox ID="txtOriginalPurchase" runat="server"  CssClass="input" BindName="OriginalPurchase.Id" SaveName="OriginalPurchase.Id" > </asp:TextBox>
            </td>
         
            <tr>
                <td class="font">
                    消息
                </td>
                <td class="mtext" colspan="3">
                    <asp:CheckBoxList ID="ckMessageType" runat="server">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td class="font">
                    备注
                </td>
                <td class="mtext" colspan="3">
                    <input id="txtRemark" runat="server" type="text" class="input long" BindName="Remark"
                        SaveName="Remark" />
                </td>
            </tr>
            <tr>
                <td class="font">
                    流程备注
                </td>
                <td class="mtext" colspan="3">
                    <input id="txtHistoryRemark" runat="server" type="text" class="input long" />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="center">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
                    <input id="btnClose" type="button" value="关闭" class="btn" />
                </td>
            </tr>
                <tr>
               <td colspan="4" >
                   <a href="javascript:void(0);" id="btnAdd" Note="note" NoteUrl='SelectProduct.aspx?SerializeValueId=<%=hfProducts.ClientID %>&SerializeContainerId=<%=gvProduct.ClientID %>&StorehouseId=<%=StorehouseComboBox1.InputHidden.Value %>&StorehouseName=<%=StorehouseComboBox1.InputText.Value %>'>添加商品</a>
               </td>
        </tr>
        </table>
         <uc2:message id="Message1" runat="server" />
      
      
       <div id="divProduct" >

       <input  type="hidden" id="hfProducts" runat="server"  />
        <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
            <EmptyDataTemplate>
                <tr>
				<th scope="col" >名称</th><th scope="col" >类目</th><th scope="col" >面价</th><th >毛利率</th>
              <th >进价</th><th >采购数量</th>
			</tr>
            </EmptyDataTemplate>
       <Columns>

            <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                        <input value='<%#Eval("Id") %>' id="hfId" runat="server" type="hidden"   />
                <a href='<%=this.GetUrl("PresentationAdminErpUrl") %>/Product/Product/Detail.aspx?id=<%#Eval("Id") %>' target="_blank"><%#Eval("Name")%></a> 
                <input id="hfName" runat="server" type="hidden" class="input" value='<%#Eval("Name")%>' />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类目"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Goods.Category.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
     <asp:TemplateField HeaderText="面价"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Price")%> 
            </ItemTemplate>
        </asp:TemplateField>
   
        
        <asp:TemplateField HeaderText="毛利率"  ItemStyle-CssClass="left status">
            <ItemTemplate>
              <%# DataBinder.Eval(Container.DataItem, "PriceRate", "{0:N2}%")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="毛利率警戒线"  ItemStyle-CssClass="left status" ItemStyle-Width="100" >
            <ItemTemplate>               
                <%# DataBinder.Eval(Container.DataItem, "Goods.Category.Rate", "{0:N2}%")%>
            </ItemTemplate>
        </asp:TemplateField>
  
                <asp:TemplateField HeaderText="进价"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%#Eval("Cost") %>' id="txtCost" runat="server" type="text" class="input" style="width: 80px;"  SerializeName="Price" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id" />                
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="采购数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value="1" id="txtCount" runat="server" type="text" class="input" style="width: 80px;" SerializeName="Count" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id"  />    
                <a href="javascript:void(0);" SerializeRemove="true" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id">删除</a>                         
            </ItemTemplate>
        </asp:TemplateField> 
                              
        </Columns>
     </asp:GridView>
   
     </div>

    </div>
    <table style="display:none;" >
        <tbody id="divTemplate">
         <tr class="out">
				<td class="left">
                <a href="<%=this.GetUrl("PresentationAdminErpUrl") %>/Product/Product/Detail.aspx?id=@Id" target="_blank">@Name</a> 
            </td><td class="left">
                @CategoryName
            </td><td class="left status">
                @Price
            </td><td class="left status">
                @PriceRate
            </td>
              <td class="left">
                  @Count          
            </td>
             <td class="left">
                <input type="text" class="input" style="width: 80px;" value="@Price" SerializeName="Price" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id">                
            </td><td class="left">
                <input  type="text"  value="@Count" class="input" style="width: 80px;" SerializeName="Count" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id">  
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
             $(window.Serial.Note.Container).css("width", "1650px");
             $(window.Serial.Note.Container).css("height", "800px");
             window.Serial.Initialize();
             setTimeout(function() {
                 $("#<%=gvProduct.ClientID %>").css("table-layout", "auto");
             }, 1000);
             var storageCtrl = $("#<%=StorehouseComboBox1.InputHidden.ClientID %>");
             var storageId = storageCtrl.val();
             <%=StorehouseComboBox1.ClientID %>.Select = function() {
                 if (storageCtrl.val() == storageId)
                     return;
                 storageId = storageCtrl.val();
                 var name = $("#<%=StorehouseComboBox1.InputText.ClientID %>").val();
                 $("#btnAdd").attr("NoteUrl", "SelectProduct.aspx?SerializeValueId=<%=hfProducts.ClientID %>&SerializeContainerId=<%=gvProduct.ClientID %>&StorehouseId=" + storageId + "&StorehouseName=" + name);
             };
         });
     }

    </script>
         
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

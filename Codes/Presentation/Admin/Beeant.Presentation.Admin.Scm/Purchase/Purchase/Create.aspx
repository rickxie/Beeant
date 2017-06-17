<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.Purchase.Create" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc6" %>
<%@ Register TagPrefix="uc3" TagName="UserComboBox" Src="~/Controls/User/UserComboBox.ascx" %>
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc1" TagName="GeneralZTreeView_1" Src="~/Controls/GeneralZTreeView.ascx" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
  <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc5" %>
<html xmlns="http://www.w3.org/1999/xhtml">
      
<head id="Head1" runat="server">
       <title>生产出库单</title>
       <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="/Scripts/Winner/Winner.ClassBase.js"></script>
      <script type="text/javascript" src="/Scripts/Winner/Dialog/Winner.Dialog.js"></script>
      <script type="text/javascript" src="/scripts/jquery-1.7.1.min.js"></script>
      <script language="javascript" src="../../Scripts/Nancy/NancyGrid/NancyGrid.js"></script>
</head>
<body>

    <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <div class="main" style="top: 5px;">
           <div class="body" style="margin-left: 0;">

   <div class="info">
             
                <div>采购单信息</div>  
                <div>
            <input id="hfIdControl" type="hidden" runat="server" />
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
                    账户
                </td>
                <td class="text">
                    <uc8:AccountComboBox ID="AccountComboBox1" runat="server" OnChangedEvent="ChangedAccount" />
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
                 <td class="font">采购类型</td>
                <td class="text"  >
                <uc10:GeneralDropDownList ID="ddlType" runat="server" SaveName="Type" BindName="Type" ObjectName="Beeant.Domain.Entities.Purchase.PurchaseType" IsEnum="True" />
                </td>
                <td class="font">
                    消息
                </td>
                <td class="mtext">
                    <asp:CheckBoxList ID="ckMessageType" runat="server">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr runat="server" id="trStorehouse">
                <td class="font">
                    仓库
                </td>
                <td class="text">
                    <input id="txtStoreName" runat="server" bindname="Storehouse.Id" savename="Storehouse.Id"
                        type="text" class="input" data-bindname="Storehouse.Id" readonly="true" />
                    <input id="textselStoreId" runat="server" style="display: none" type="text" class="input"
                        savename="Storehouse.Id" bindname="Storehouse.Id" />
                </td>
                 <td class="font"></td>
                <td class="text"  >
                </td>
            </tr>
            <tr>
            <tr>
                <td class="font">
                    备注
                </td>
                <td class="mtext" colspan="3">
                    <input id="txtRemark" runat="server" type="text" class="input long" bindname="Remark"
                        savename="Remark" />
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
                </td>
            </tr>
        </table>

        </div>
                <div id="selStockHouse" style="overflow: auto; height: 450px;  ">
        <uc1:GeneralZTreeView_1 ID="ztrees1" runat="server" AsyncUrl="/Ajax/ZTree/OnlyWarehouseWmsAsyncTree.aspx"
            ObjectName="StorehouseEntity" ClickNode="SaveData1" DataParentField="Parent.Id"
            DataTextField="Name" DataValueField="Id" IsAsync="True"></uc1:GeneralZTreeView_1>
    </div>
                <div>采购单明细</div>
                <div class="list">
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="tblist" >
                <Columns>
                    <asp:TemplateField ItemStyle-CssClass="center ckbox">
                        <HeaderTemplate>
                            <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input value='<%#Eval("Id") %>' id="ckSelect" checked="True" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品名称" ItemStyle-CssClass="center operate">
                        <ItemTemplate>
                            <%#Eval("Product.Name")%>
                             <input id="txtName" type="hidden" runat="server"  value='<%#Eval("Product.Name")%>' />
                             <input id="hidProductId" type="hidden" runat="server"  value='<%#Eval("Product.Id")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="价格" ItemStyle-CssClass="center operate">
                        <ItemTemplate>
                            <input type="text" id="txtPrice" runat="server" value='<%#Eval("Price") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="采购数量"  ItemStyle-CssClass="left">
                        <ItemTemplate>
                            <input type="text" id="txtCount" runat="server" value='<%#Eval("Count") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
                        <ItemTemplate>
                            <input type="text" id="txtRemark" runat="server" value='<%#Eval("Remark") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
          <uc6:Message ID="Message1" runat="server" />       
   </div>

       </div>
 
    </div>

    </form>
              <script type="text/javascript">
                  var selectedHouseId = '';
                  var selectedHouseName = '';
                  var selStockHouse = $("#selStockHouse");
                  var txtStoreName = '<%=txtStoreName.ClientID %>';
                  var textselStoreId = '<%=textselStoreId.ClientID %>';


                  var dialog1 = new Winner.Dialog("选择仓库", "");
                  $(document).ready(function () {
                      dialog1.IsShowDialog = false;
                      dialog1.Initialize();
                      $(dialog1.Detail).append(selStockHouse);
                      $("#" + txtStoreName).click(function () {
                          showStockHouse();
                      });

                  });


                  function SaveData1(data) {
                      selectedHouseId = data.Id;
                      selectedHouseName = data.name;
                  }
                  function showStockHouse() {
                      dialog1.target = $(window.sourceCtrl);
                      dialog1.houseId = dialog1.target.prev();
                      dialog1.ShowDialog();
                      dialog1.SureFunction = function () {
                          if (selectedHouseId == '') {
                              alert("请选择一个仓库!");
                              return false;
                          } else {
                              $("#" + textselStoreId).val(selectedHouseId);
                              $("#" + txtStoreName).val(selectedHouseName);

                          }
                      };
                  }
              

           </script>            
</body>
</html>
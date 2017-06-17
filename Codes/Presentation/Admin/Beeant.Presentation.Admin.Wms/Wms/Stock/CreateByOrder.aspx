<%@ Page Title="" Language="C#"  AutoEventWireup="true"
    CodeBehind="CreateByOrder.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Stock.CreateByOrder" %>
<%@ Import Namespace="Component.Extension" %>
         <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc5" %>
 <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %> 
 <%@ Register TagPrefix="uc1" TagName="GeneralZTreeView" Src="~/Controls/GeneralZTreeView.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
      
<head id="Head1" runat="server">
       <title>生产出库单</title>
       <link href="/Scripts/Winner/Dialog/Styles/Style.css" rel="stylesheet" type="text/css" />
        <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="/Scripts/Winner/Winner.ClassBase.js"></script>
        <script type="text/javascript" src="/scripts/jquery-1.7.1.min.js"></script>
        <script type="text/javascript" src="/Scripts/Winner/Dialog/Winner.Dialog.js"></script>
</head>
<body>
     
    <form id="form1" runat="server">
   <div class="main" style="top: 5px;">
           <div class="body" style="margin-left: 0;">
    <div class="info">

            
                <input id="hfIdControl" type="hidden" runat="server" />
                <div class="edit">
                    <div>出库单信息</div>
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
                             <td class="font">级别</td>
                                <td class="mtext" colspan="3"  >
               
                             <asp:DropDownList ID="ddlLevel" runat="server" BindName="Level" SaveName="Level" ></asp:DropDownList>
               
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
                                <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
                                 <input id="btnClose" type="button" value="关闭" class="btn" style="display:none;" /></td>
                            </tr>
                    </table>

                    

                </div>
         
                    <uc2:Message ID="Message1" runat="server" />
                <br/>
                <div class="list" style="overflow:hidden;">
                    <div>出库单明细</div>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table">
                        <Columns>
                            <asp:TemplateField HeaderText="产品" ItemStyle-CssClass="left Sequence">
                                <ItemTemplate>
                                    <input type="hidden" id="hidProductEntity" runat="server" value='<%#Eval("Product.Id") %>' />
                                    <input type="hidden" id="hidName" runat="server" value='<%#Eval("Name") %>'/>
                                        <%#Eval("Name")%>
                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="仓库" ItemStyle-CssClass="left Sequence">
                                <ItemTemplate>
                                    <input type="hidden" id="hidStorehouseId" runat="server" value='<%#Eval("Storehouse.Id") %>' />
                                    <a href="javascript:void(0);" name="storehouse" onclick="javascript:showStockHouse(this);"><%#Eval("Storehouse.Name")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="数量" ItemStyle-CssClass="left Sequence">
                                <ItemTemplate>
                                    <input type="text" id="txtCount" runat="server" value='<%#Eval("Count").Convert<int>() %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="备注" ItemStyle-CssClass="left Sequence">
                                <ItemTemplate>
                                    <input type="text" id="txtRemark" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
 
    </div>

     <div id="selStockHouse" style="overflow: auto; height: 350px;">
        <uc1:GeneralZTreeView ID="ztrees" runat="server" ObjectName="StorehouseEntity" ClickNode="SaveData" DataParentField="Parent.Id" DataTextField="Name" DataValueField="Id"></uc1:GeneralZTreeView>
    </div>
    </div>
     </div>
     
      <script type="text/javascript">
          var selectedHouseId = '';
          var selectedHouseName = '';
          var selStockHouse = $("#selStockHouse");
          var dialog = new Winner.Dialog("更换出库仓库", "");
          $(document).ready(function () {
              dialog.IsShowDialog = false;
              dialog.Initialize();
              $(dialog.Detail).append(selStockHouse);
          });

          function SaveData(data) {
              selectedHouseId = data.Id;
              selectedHouseName = data.name;
          }

          function showStockHouse(sourceCtrl) {
              dialog.target = $(sourceCtrl);
              dialog.product = $(dialog.target.parent().prev().find("input")[0]);
              dialog.houseId = dialog.target.prev();
              dialog.ShowDialog();
              dialog.SureFunction = function () {
                  if (selectedHouseId == '') {
                      alert("请选择一个仓库!");
                      return false;
                  } else {
                      var oldStock = $(this.target.parent().next().find("input")[0]).val();
                      var getStock = GetStockCount(selectedHouseId, this.product.val());
                      $($(this.target).parent().next().find("input")[0]).val(Number(getStock) < Math.abs(Number(oldStock)) ? getStock : oldStock);
                      this.houseId.val(selectedHouseId);
                      this.target.html(selectedHouseName);
                  }
              };
          }

          function GetStockCount(storehouseId, productId) {
              var result = 0;
              $.ajax(
                {
                    url: "/Ajax/Wms/Inventory.aspx?StorehouseId=" + storehouseId + "&ProductId=" + productId,
                    async: false,
                    type: 'post',
                    dataType: 'text',
                    success: function (res) {
                        if (eval(res).length > 0)
                            result = eval(res)[0].Count;
                    }
                });
              return result;
          }
    </script>
    </form>
   
</body>
</html>


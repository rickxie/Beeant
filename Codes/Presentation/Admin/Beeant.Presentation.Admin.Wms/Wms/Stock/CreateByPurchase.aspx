<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CreateByPurchase.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Stock.CreateByPurchase" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc1" TagName="Pager_1" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc5" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<%@ Register TagPrefix="uc6" TagName="Message" Src="~/Controls/Message.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GeneralZTreeView" Src="~/Controls/GeneralZTreeView.ascx" %>
     <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>生成入库单</title>  
    <style type="text/css">
        .style1
        {
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<div class="info">
   <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <div>采购单信息</div>
        <input id="hfIdControl" type="hidden" runat="server" />
        <table class="tb">
            <tr>
            <td class="font">采购单编号 </td>
            <td class="text" >
                  <%=Purchase.Id %>
             </td>
             <td class="font">跟单人</td>
            <td class="text">
                <%=Purchase.Follow.RealName %>
            </td>
         </tr>
            <tr>
            <td class="font">采购日期</td>
               <td class="text"  >
                   <%=Purchase.PurchaseDate %>
             </td>
             <td class="font">交货日期</td>
               <td class="text"  >
                   <%=Purchase.DeliveryDate %>
             </td>
             
        </tr>
            <tr>
            <td class="font">金额</td>
               <td class="text"  >
                   <%=Purchase.Amount %>
             </td>
             <td class="font">实付金额</td>
               <td class="text"  >
                   <%=Purchase.PaidAmount %>
             </td>
        </tr>   
            <tr>
            <td class="font">账户</td>
            <td class="text">
                
                <a target="_blank" href="/Finance/Account/Detail.aspx?Id=<%=Purchase.Account.Id %>" id="hfAccountId" >
                        <%=Purchase.Account.Name %>
                </a>
               
            </td>
                <td class="font">订单</td>
            <td class="text">
                <a target="_blank" href="<%=this.GetWmsUrl() %>/Order/Order/Detail.aspx?Id=<%=Purchase.Order.Id %>" id="hfOrderId" >
                        <%=Purchase.Order.Id %>
                </a>
               
            </td>
        </tr>
            
            <tr>
            <td class="font">
                状态</td>
            <td class="text">
                <%=Purchase.StatusName %>
            </td>
            <td class="font">
                状态更新时间</td>
            <td class="text">
                <%=Purchase.StatusTime %>
            </td>
        </tr>
            <tr>
            <td class="font">
                级别</td>
            <td class="text">
                <%=Purchase.Level %>
            </td>
            <td class="font">
                所属人</td>
            <td class="text">
                <%=Purchase.User.RealName %>
            </td>
        </tr>
            <tr>
            <td class="font">
                提交人</td>
            <td class="text">
                <%=Purchase.Submit.Name %>
            </td>
            <td class="font">
                提交时间</td>
            <td class="text">
                <%=Purchase.InsertTime %>
            </td>
        </tr>
            <tr>
            <td class="font">
                备注</td>
            <td class="mtext" colspan="3">
                <%=Purchase.Remark %>
            </td>
        </tr>
        </table>
         <uc6:Message ID="Message1" runat="server" />
        <div class="edit">
            <div>入库单信息</div>
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
                <uc10:UserComboBox ID="cbUser" runat="server" />
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
                                 <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
                            </tr>
                    </table>
        </div>
        <div class="subtitle" onclick="SetEntityBody('divPurchaseItem')">采购明细信息(<span class="count"><%=pgPurchaseItem.DataCount%></span>)</div>
        <div id="divPurchaseItem" style="display: none;" >
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <div  class="search" >
                            <table class="tb">
                                <tr>
                                    <td class="font">开始日期</td>
                                    <td class="text"><asp:TextBox ID="txtPurchaseItemBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtPurchaseItemBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
                                    </td>
                                    <td class="font">截止日期</td>
                                    <td class="text"><asp:TextBox ID="txtPurchaseItemEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<==@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtPurchaseItemEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
                                    </td>
            
                                    <td >
                                        <asp:Button ID="Button3" runat="server" Text="搜索" CssClass="btn"  />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="gvPurchaseItem" runat="server" AutoGenerateColumns="False" CssClass="table"  >
                            <Columns>
                                <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
                                    <ItemTemplate>
                                        <%#Eval("Id")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="center time">
                                <ItemTemplate>
                                    <%#Eval("Name")%>
                                </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="单价"  ItemStyle-CssClass="center time">
                                    <ItemTemplate>
                                        <%#Eval("Price")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="数量"  ItemStyle-CssClass="left Sequence">
                                    <ItemTemplate>
                                        <%#Eval("Count")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="金额"  ItemStyle-CssClass="left Sequence">
                                    <ItemTemplate>
                                        <%#Eval("Amount")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="left Sequence">
                                    <ItemTemplate>
                                        <%#Eval("User.RealName")%>
                                    </ItemTemplate> 
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left Sequence">
                                    <ItemTemplate>
                                        <%#Eval("Remark")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="库存产品"  ItemStyle-CssClass="left Sequence">
                                    <ItemTemplate>
                                        <a href='/Product/Product/Detail.aspx?Id=<%#Eval("Product.Id") %>'><%#Eval("Product.Id")%></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
                                    <ItemTemplate>
                                        <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
                                    </ItemTemplate> 
                                </asp:TemplateField>
                        </Columns>  
                    </asp:GridView>
    <uc1:Pager_1 ID="pgPurchaseItem" runat="server" PageSize="10"  
     SelectExp="Id,Name,Price,Count,User.RealName,Amount,Remark,Product.Id,InsertTime,UpdateTime" 
     FromExp="Beeant.Domain.Entities.Purchase.PurchaseItemEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="subtitle" onclick="SetEntityBody('divStock')">进出单列表(<span class="count"><%=pgStock.DataCount%></span>)</div>
        <div id="divStock" style="display: none;" >
        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate>
                        <div  class="search" >
                            <table class="tb">
                                <tr>
                                    <td class="font">开始日期</td>
                                    <td class="mtext"><asp:TextBox ID="txtStockBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender11" runat="server" TargetControlID="txtStockBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
                                    </td>
                                    <td class="font">截止日期</td>
                                    <td class="mtext"><asp:TextBox ID="txtStockEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
                                     <cc1:CalendarExtender ID="CalendarExtender12" runat="server" TargetControlID="txtStockEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
                                    </td>
                                     <td class="font">类型</td>
                                    <td class="mtext">
                                        <uc5:GeneralDropDownList ID="ddlStockType" runat="server" SearchWhere="Type==@Type" SearchPropertyTypeName="Type" SearchParamterName="Type" ObjectName="Beeant.Domain.Entities.Wms.StockType" IsEnum="True" />
                                    </td>
                                    <td >
                                        <asp:Button ID="Button5" runat="server" Text="搜索" CssClass="btn"  />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="False" CssClass="table" >
                            <Columns> 
                                    <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="left status">
                                    <ItemTemplate>
                                        <a href='/Wms/Stock/Detail.aspx?id=<%#Eval("Id") %>' target="_blank"><%#Eval("Id")%></a> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left status">
                                    <ItemTemplate>
                                        <%#Eval("StatusName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="left status">
                                    <ItemTemplate>
                                        <%#Eval("User.RealName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="类型"  ItemStyle-CssClass="left status">
                                    <ItemTemplate>
                                        <%#Eval("TypeName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="时间"  ItemStyle-CssClass="left status">
                                    <ItemTemplate>
                                        <%#Eval("UpdateTime")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                            <asp:TemplateField HeaderText="相关采购单"  ItemStyle-CssClass="left status">
                                    <ItemTemplate>
                                        <a href='/Wms/Purchase/Detail.aspx?id=<%#Eval("Id") %>' target="_blank">  <%#Eval("Purchase.Id")%></a>
                                    </ItemTemplate>
                                </asp:TemplateField>    
                            </Columns>
                        </asp:GridView>

                        <uc1:Pager_1 ID="pgStock" runat="server" PageSize="10"  
                 SelectExp="Id,User.RealName,Type,UpdateTime,Purchase.Id,Status"
                  FromExp="Beeant.Domain.Entities.Wms.StockEntity,Beeant.Domain.Entities"
                  OrderByExp="UpdateTime desc" WhereExp="Purchase.Id==@Id" />
                  </ContentTemplate>
             </asp:UpdatePanel>
        </div>

       
         <div class="list">
            <div>入库单明细</div>
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
                                <input type="text" id="txtCount" runat="server" value='<%#Math.Abs(Eval("Count").Convert<int>()) %>' />
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

     </ContentTemplate>
     </asp:UpdatePanel>
</div>
  <div id="selStockHouse" style="overflow: auto; height: 350px;">
        <uc1:GeneralZTreeView ID="ztrees" runat="server" ObjectName="StorehouseEntity" ClickNode="SaveData" DataParentField="Parent.Id" DataTextField="Name" DataValueField="Id"></uc1:GeneralZTreeView>
    </div>

    <script type="text/javascript">
        var selectedHouseId = '';
        var selectedHouseName = '';

        var selStockHouse = $("#selStockHouse");
        var dialog = new Winner.Dialog("更换入库仓库", "");
        $(document).ready(function () {
            dialog.IsShowDialog = false;
            dialog.Initialize();
            $(dialog.Detail).append(selStockHouse);
            selStockHouse.show();
        });
        
        
        function SaveData(data) {
            selectedHouseId = data.Id;
            selectedHouseName = data.name;
        }
        function showStockHouse(sourceCtrl) {
            dialog.target = $(sourceCtrl);
            dialog.houseId = dialog.target.prev();
            dialog.ShowDialog();
            dialog.SureFunction = function () {
                if (selectedHouseId == '') {
                    alert("请选择一个仓库!");
                } else {
                    this.houseId.val(selectedHouseId);
                    this.target.html(selectedHouseName);
                }
            };
        }
    </script>
</asp:Content>

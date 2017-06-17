<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.Order.Add" MasterPageFile="~/Datum.Master" ValidateRequest="false" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Product" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc8" %>
<%@ Register src="../../Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc3" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
       <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单录入</title>  

 </asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
     <input id="hfIdControl" type="hidden" runat="server" />
 <div class="edit">
    <%=string.IsNullOrEmpty(hfIdControl.Value)?null:string.Format("<a href='Detail.aspx?id={0}'  name='Entity'>详情</a> <a href='update.aspx?id={0}'  name='Edit'>编辑</a> <a href='handle.aspx?id={0}'  name='Handle'>处理</a>",hfIdControl.Value)%>
    <table class="tb">
         
         <tr>
            <td class="font">账户</td>
            <td class="mul mtext"  >
                <uc8:AccountComboBox ID="AccountComboBox1" runat="server" />
            </td>
             <td class="font">下单日期</td>
            <td class="text" >
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
               
            <asp:TextBox ID="txtDeposit" runat="server"  CssClass="input" BindName="Deposit" SaveName="Deposit" Text="0" > </asp:TextBox>
               
            </td>
        </tr>
        <tr>
             <td class="font">原始订单</td>
            <td class="text">
            <asp:TextBox ID="txtOriginalOrder" runat="server"  CssClass="input" BindName="OriginalOrder.Id" SaveName="OriginalOrder.Id" > </asp:TextBox>
            </td>
              <td class="font">关联编号</td>
            <td class="text">
            <asp:TextBox ID="txtDataId" runat="server"  CssClass="input" BindName="DataId" SaveName="DataId" > </asp:TextBox>
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
                 <asp:Button ID="btnSubmit" runat="server" Text="报审" CssClass="btn"   />
                 <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  />
             </td>
        </tr>
         <tr>
               <td colspan="4" >
                   <a href="javascript:void(0);" id="btnAdd" Note="note" NoteUrl='SelectProduct.aspx?SerializeValueId=<%=hfProducts.ClientID %>&SerializeContainerId=<%=gvProduct.ClientID %>'>添加商品</a>
               </td>
        </tr>
    </table>
    <uc2:Message ID="Message1" runat="server" />
 
   <div id="divProduct">
         <input  type="hidden" id="hfProducts" runat="server"  />
        <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
            <EmptyDataTemplate>
                <tr>
				<th scope="col" style="width: 144px;">商品编号</th><th scope="col" style="width: 233px;">名称</th><th scope="col" style="width: 155px;">面价</th><th scope="col" style="width: 70px;">底价</th><th scope="col" style="width: 70px;">数量</th><th scope="col" style="width: 271px;">下单价</th><th scope="col" style="width: 272px;">下单数量</th><th scope="col" style="width: 424px;">备注</th>
			</tr>
            </EmptyDataTemplate>
       <Columns>
              
            <asp:TemplateField HeaderText="商品编号"  ItemStyle-CssClass="left">
                <ItemTemplate>
                    <input value='<%#Eval("IsReturn") %>' id="hfIsReturn" type="hidden" runat="server"  />
                    <input value='<%#GetDescription(Eval("Goods").Convert<GoodsEntity>(),Eval("Id").Convert<long>()) %>' id="hfDescription" type="hidden" runat="server"  />
                     <input value='<%#Eval("Name") %>' id="hfName" type="hidden" runat="server"  />
                       <input value='<%#Eval("Id") %>' id="hfId" type="hidden" runat="server"  />
                    <%#Eval("Id")%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
                <ItemTemplate>
                    <%#Eval("Name")%>              
                </ItemTemplate>
            </asp:TemplateField>
           <asp:TemplateField HeaderText="面价"  ItemStyle-CssClass="left Sequence">
                <ItemTemplate>
                  <%#Eval("Price")%>
                </ItemTemplate>
               
            </asp:TemplateField>

             <asp:TemplateField HeaderText="底价"  ItemStyle-CssClass="left status">
                <ItemTemplate>
                    <%#Eval("Cost")%>   
                </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="数量"  ItemStyle-CssClass="left status">
                <ItemTemplate>
                    <%#Eval("Count")%>   
                </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="下单价"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%#Eval("Cost") %>' id="txtCost" runat="server" type="text" class="input"  style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="下单数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%#Eval("OrderMinCount") %>' id="txtCount" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
            <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='' id="txtRemark" runat="server" type="text" class="input" />                
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
                     @Id
                </td><td class="left">
                    @Name             
                </td><td class="left Sequence">
                  @Product_Price  
                </td><td class="left status">
                    @Product_Cost   
                </td><td class="left status">
                    @Product_Count   
                </td>
                <td class="left">
                <input type="text" class="input" style="width: 80px;" SerializeName="Price" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id" value="@Price">                
            </td><td class="left">
                <input  type="text" class="input" style="width: 80px;" SerializeName="Count" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id" value="@Count">                
            </td><td class="left">
                <input type="text" class="input" SerializeName="Remark" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id" value="@Remark">   
                <a href="javascript:void(0);" SerializeRemove="true" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="@Id">删除</a>             
            </td>
			</tr>
            </tbody>
   </table>
    <script type="text/javascript" src="/Scripts/Serializator.js"></script>
    <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
    <script type="text/javascript">
        function Init() {
            window.Serial = new Serializator({ Serializes: [{ Id: '<%=gvProduct.ClientID %>', Html: $("#divTemplate").html(), ValueId: "<%=hfProducts.ClientID %>"}] });
            window.Serial.Initialize();
            setTimeout(function () {
                $("#<%=gvProduct.ClientID %>").css("table-layout", "auto");
            }, 1000);

        }
    </script>
 
  </ContentTemplate>
</asp:UpdatePanel>

 
        
 

 </asp:Content>
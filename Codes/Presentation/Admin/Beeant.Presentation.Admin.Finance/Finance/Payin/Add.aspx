<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payin.Add" MasterPageFile="~/Main.Master" ValidateRequest="false" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc8" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc5" %>
<%@ Register src="../../Controls/Finance/BankComboBox.ascx" tagname="BankComboBox" tagprefix="uc10" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
 
      <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>收款录入</title>  
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
            <td class="text"><asp:DropDownList ID="ddlStatus" runat="server" SaveName="Status" ValidateName="Status" ></asp:DropDownList></td>
               <td class="font">处理人</td >
            <td class="text">
                <uc5:UserComboBox ID="cbUser" runat="server" />
            </td>
        </tr>
         <tr>
          
             <td class="font">金额</td>
            <td class="text" >
             <input id="txtAmount" runat="server"  type="text" class="input"  BindName="Amount" SaveName="Amount"  />
             </td>
             <td class="font">原币种金额</td>
            <td class="text"><input id="txtSourceAmount" runat="server"  type="text" class="input"   BindName="SourceAmount" SaveName="SourceAmount"   /> </td>
        </tr>   
        <tr>
             <td class="font">名称</td>
            <td class="text">
                 <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  />
            </td> 
                <td class="font">级别</td>
            <td class="text" >
               
             <asp:DropDownList ID="ddlLevel" runat="server" BindName="Level" SaveName="Level" ></asp:DropDownList>
               
            </td>
        </tr>
   
         <tr>
              <td class="font">到账时间</td>
            <td class="text">
                <asp:TextBox ID="txtPayTime" runat="server" CssClass="input"  BindName="PayTime" SaveName="PayTime"></asp:TextBox>
 
            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPayTime" Format="yyyy-MM-dd">
                </cc1:CalendarExtender>
            </td>
           <td class="font">支付方式</td>
            <td class="text">
                <uc5:GeneralDropDownList ID="ddlPayType" runat="server"  BindName="PayType" SaveName="PayType" ObjectName="PayTypeEntity"  />
            </td>
            
        </tr>
         <tr>
             <td class="font">货币</td>
            <td class="text">
                
                <uc5:GeneralDropDownList ID="ddlCurrency" runat="server" BindName="Currency" SaveName="Currency" ObjectName="CurrencyEntity"/>
             </td>
               <td class="font">是否为冲帐</td>
            <td class="text" >
               
                <asp:CheckBox ID="ckIsFlush" runat="server"  BindName="IsFlush" SaveName="IsFlush"/>
               
            </td>
             
        </tr>
         <tr>
             <td class="font">账户</td>
            <td class="mul" >
              
                <uc8:AccountComboBox ID="cbAccount" runat="server" />
              
            </td>
            <td class="font">银行</td>
            <td class="mul" >                
                <uc10:BankComboBox ID="cbBank" runat="server"   TextValidateName="" TextBindName="BankNumber" TextSaveName="BankNumber" />     
            </td>           
        </tr>      
        <tr>
            <td class="font">银行开户人</td>
            <td class="text"><input id="txtBankHolder" runat="server"  type="text" class="input long"   BindName="BankHolder" SaveName="BankHolder"   /> </td>
     
            <td class="font">开户银行</td>
            <td class="text" >
             <input id="txtBankName" runat="server"  type="text" class="input long"  BindName="BankName" SaveName="BankName"  />
             </td>
        </tr>
        <tr>
           <td class="font">原始凭证编号</td>
            <td class="text" >
                 <input id="txtOriginalNumber" runat="server"  type="text" class="input long"   BindName="OriginalNumber" SaveName="OriginalNumber"   />
                 
            </td>
             <td class="font">消息</td>
            <td class="mtext" colspan="3">               
                <asp:CheckBoxList ID="ckMessageType" runat="server" ></asp:CheckBoxList>               
            </td>                       
        </tr>
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                 <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
            </td>
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
         <tr>
               <td colspan="4" >
                   <a href="javascript:void(0);" id="hfAccountId" Note="note" NoteUrl='SelectOrder.aspx?SerializeValueId=<%=hfOrders.ClientID %>&SerializeContainerId=<%=gvReceived.ClientID %>&accountId=<%=cbAccount.InputHidden.Value %>'>添加订单</a>
               </td>
        </tr>
    </table>
    
    <uc2:Message ID="Message1" runat="server" />
    <div id="divOrder" >
       <input  type="hidden" id="hfOrders" runat="server"  />
         <asp:GridView ID="gvReceived" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
                 <EmptyDataTemplate>
             <tr>
			<th scope="col" >订单编号</th><th scope="col" >状态</th><th >级别</th><th >所属人</th><th >提交人</th><th >提交时间</th><th >应收金额</th><th >实收金额</th><th >本次收款</th><th >备注</th>
		</tr>
            </EmptyDataTemplate>
       <Columns>
            
         <asp:TemplateField HeaderText="订单编号"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Id")%>
                    <input value='<%#Eval("Id")%>' id="hfId" type="hidden" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>       
    
  
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>  

        <asp:TemplateField HeaderText="级别"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Level")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="所属人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="提交人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Submit.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="提交时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("InsertTime", "{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>   

        <asp:TemplateField HeaderText="应收金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TotalAmount")%>
            </ItemTemplate>
        </asp:TemplateField>  
           <asp:TemplateField HeaderText="实收金额"  ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                <%#Eval("PayAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="本次收款"  ItemStyle-CssClass="right xlsfloat">
                            <ItemTemplate>
                                 <input value='<%#Convert.ToDecimal(Eval("TotalAmount")) - Convert.ToDecimal(Eval("PayAmount")) %>' id="txtAmount" runat="server" type="text" class="input" />
                            </ItemTemplate>
                        </asp:TemplateField>
          <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input id="txtRemark" runat="server" type="text" class="input" />                
            </ItemTemplate>
        </asp:TemplateField>                          
        </Columns>
     </asp:GridView>
    
     </div>
</div>
 <table style="display:none;" >
        <tbody id="divTemplate">
            <tr class="out">
			<td class="left ">
                @Id
            </td><td class="left">
                @StatusName
            </td><td class="left">
                @Level
            </td><td class="left">
                @UserName
            </td><td class="left">
                @SubmitName
            </td><td class="left">
                 @InsertTime
            </td><td class="left">
               @TotalAmount
            </td><td class="left">
                @PayAmount
            </td><td class="left">
                     <input type="text"  class="input" value="@Amount" name="amount" />                
            </td><td class="left">
                 <input type="text" class="input" SerializeName="Remark" SerializeValueId="<%=hfOrders.ClientID %>" SerializeId="@Id" value="@Remark">   
                 <a href="javascript:void(0);" SerializeRemove="true" SerializeValueId="<%=hfOrders.ClientID %>" SerializeId="@Id">删除</a>            
            </td>
		</tr>
            </tbody>
   </table>
       <script type="text/javascript" src="/Scripts/Serializator.js"></script>
    <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
     <script type="text/javascript">
     var registerFunc = function() {
         $(document).ready(function() {
             window.Serial = new Serializator({ Serializes: [{ Id: '<%=gvReceived.ClientID %>', Html: $("#divTemplate").html(), ValueId: "<%=hfOrders.ClientID %>" }] });
             window.Serial.Initialize();
             window.Serial.ResetFunction = function() {
                 var ctrls = $("#<%=gvReceived.ClientID %>").find("input[name='amount']");
                 var amount = 0;
                 ctrls.each(function(index, sender) {
                     var val = 0;
                     try {
                         val = parseFloat($(sender).val());
                         if (isNaN(val)) {
                             val = 0;
                         }
                     } catch(e) {
                     }
                     amount += val;
                 });
                 $("#<%=txtAmount.ClientID %>").val(amount);
                 $("#<%=txtSourceAmount.ClientID %>").val(amount);
             };
             setTimeout(function() {
                 $("#<%=gvReceived.ClientID %>").css("table-layout", "auto");
             }, 1000);
             var accountCtrl = $("#<%=cbAccount.InputHidden.ClientID %>");
             var accountid = accountCtrl.val();
             <%=cbAccount.ClientID %>.Select = function() {
                 if (accountCtrl.val() == accountid)
                     return;
                 accountid = accountCtrl.val();
                 var myDate = new Date();
                 $("#<%=txtName.ClientID %>").val($("#<%=cbAccount.InputText.ClientID %>").val() + "/" + myDate.getFullYear()+"-"+myDate.getMonth()+"-"+myDate.getDate() + "/收款单");
                 $("#hfAccountId").attr("NoteUrl", "SelectOrder.aspx?SerializeValueId=<%=hfOrders.ClientID %>&SerializeContainerId=<%=gvReceived.ClientID %>&accountid="+accountid);
             };
         });
     }

     </script>
        
    </ContentTemplate>
</asp:UpdatePanel>
 



    
 



 </asp:Content>
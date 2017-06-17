<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Invoicein.Add" MasterPageFile="~/Main.Master" ValidateRequest="false" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc8" %>
  <%@ Register src="/Controls/User/UserComboBox.ascx" tagname="UserComboBox" tagprefix="uc5" %>
<%@ Register TagPrefix="uc5" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>进项发票录入</title>  
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
                <asp:DropDownList ID="ddlStatus" runat="server" SaveName="Status" ValidateName="Status" ></asp:DropDownList>
            </td>
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
               <td class="font">账户</td>
            <td class="mul">
              
                <uc8:AccountComboBox ID="cbAccount" runat="server" />
              
            </td>
        </tr>   
        <tr>
            <td class="font">抬头</td>
            <td class="mtext" colspan="3" >
             <input id="txtTitle" runat="server"  type="text" class="input long"  BindName="Title" SaveName="Title"  />
             </td>
        </tr>
            <tr>
            <td class="font">发票号码</td>
            <td class="mtext" colspan="3" >
             <input id="txtInvoiceNumber" runat="server"  type="text" class="input long"  BindName="InvoiceNumber" SaveName="InvoiceNumber"  />
             </td>
        </tr>
              <tr>
                            <td class="font">
                                发票类型
                            </td>
                            <td class="text">
                                <uc5:GeneralDropDownList ID="ddlType" runat="server" SaveName="InvoiceType"
                                    BindName="InvoiceType" ObjectName="Beeant.Domain.Entities.Finance.InvoiceType"
                                    IsEnum="True" />
                            </td>
                            <td class="font">
                                开票类型
                            </td>
                            <td class="text" >
                                <uc5:GeneralDropDownList ID="ddlGeneralType" runat="server" SaveName="GeneralType"
                                    BindName="GeneralType" ObjectName="Beeant.Domain.Entities.Finance.InvoiceGeneralType"
                                    IsEnum="True" />
                            </td>
                        </tr>
          <tr>
               <tr>
                            <td class="font">
                                发票内容
                            </td>
                            <td class="mtext" colspan="3">
                                <input id="txtContent" runat="server" type="text" value="无" class="input long" bindname="Content"
                                    savename="Content" />
                            </td>
                        </tr>
             <td class="font">快递公司</td>
            <td class="text"  >
              <input id="txtExpressName" runat="server"  type="text" class="input long"   BindName="ExpressName" SaveName="ExpressName"   />
             </td>
              <td class="font">快递号</td>
            <td class="text"  >
              <input id="txtExpressNumber" runat="server"  type="text" class="input long"   BindName="ExpressNumber" SaveName="ExpressNumber"   />
             </td>
        </tr>
         <tr>
          
          <td class="font">是否为冲帐</td>
            <td class="text" >
               
                <asp:CheckBox ID="ckIsFlush" runat="server"  BindName="IsFlush" SaveName="IsFlush"/>
            </td>
            <td class="font">邮政编码</td>
            <td class="text" >
              <input id="txtPostcode" runat="server"  type="text" class="input"   BindName="Postcode" SaveName="Postcode"   />
             </td>
        </tr>
         <tr>
           <td class="font">接收人</td>
            <td class="text"><input id="txtRecipient" runat="server"  type="text" class="input"   BindName="Recipient" SaveName="Recipient"   /> </td>
           <td class="font">手机号码</td>
            <td class="text">
                <input id="txtMobile" runat="server"  type="text" class="input"   BindName="Mobile" SaveName="Mobile"   /> 
            </td>
        </tr>
         <tr>
            
             <td class="font">地址</td>
            <td class="mtext" colspan="3" >
              <input id="txtAddress" runat="server"  type="text" class="input long"   BindName="Address" SaveName="Address"   />
             </td>
        </tr>
         <tr>
           
             <td class="font">级别</td>
            <td class="mtext" colspan="3">
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
                   <a href="javascript:void(0);" id="btnAdd" style='<%=string.IsNullOrEmpty(cbAccount.InputHidden.Value)?"display:none;":"" %>' Note="note" NoteUrl='SelectProduct.aspx?SerializeValueId=<%=hfPurchases.ClientID %>&SerializeContainerId=<%=gvInvoice.ClientID %>'>添加采购单</a>
                    <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn" Visible="False"  />
               </td>
        </tr>
          <tr>
               <td colspan="4" >
                   <a href="javascript:void(0);" id="hfAccountId" Note="note" NoteUrl='SelectPurchase.aspx?SerializeValueId=<%=hfPurchases.ClientID %>&SerializeContainerId=<%=gvInvoice.ClientID %>&accountId=<%=cbAccount.InputHidden.Value %>'>添加采购单</a>
               </td>
        </tr>
    </table>
 
    <uc2:Message ID="Message1" runat="server" />
     
</div>
   <div id="divPurchase" runat="server">
           <input  type="hidden" id="hfPurchases" runat="server"  />
         <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
             <EmptyDataTemplate>
             <tr>
			<th scope="col" >采购单编号</th><th scope="col" >状态</th><th >级别</th><th >所属人</th><th >提交人</th><th >提交时间</th><th >应收金额</th><th >已开金额</th><th >本次开票</th><th >备注</th>
		</tr>
            </EmptyDataTemplate>
       <Columns>
    
         <asp:TemplateField HeaderText="采购单编号"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                 <input value='<%#Eval("Id")%>' id="hfId" type="hidden" runat="server" />
                <%#Eval("Id")%>
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

        <asp:TemplateField HeaderText="应开金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("OpenAmount")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="已开金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("InvoiceAmount")%>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="本次开票"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%# Convert.ToDouble(Eval("OpenAmount").ToString())-Convert.ToDouble(Eval("InvoiceAmount").ToString()) %>' id="txtAmount" runat="server" type="text" class="input" />                
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
    </ContentTemplate>
</asp:UpdatePanel>
 

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
               @OpenAmount
            </td><td class="left">
                @InvoiceAmount
            </td><td class="left">
                     <input type="text"  class="input" value="@Amount" name="amount" />                
            </td><td class="left">
                 <input type="text" class="input" SerializeName="Remark" SerializeValueId="<%=hfPurchases.ClientID %>" SerializeId="@Id" value="@Remark">   
                 <a href="javascript:void(0);" SerializeRemove="true" SerializeValueId="<%=hfPurchases.ClientID %>" SerializeId="@Id">删除</a>            
            </td>
		</tr>
            </tbody>
   </table>
       <script type="text/javascript" src="/Scripts/Serializator.js"></script>
    <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
     <script type="text/javascript">
     var registerFunc = function() {
         $(document).ready(function() {
             window.Serial = new Serializator({ Serializes: [{ Id: '<%=gvInvoice.ClientID %>', Html: $("#divTemplate").html(), ValueId: "<%=hfPurchases.ClientID %>" }] });
             window.Serial.Initialize();
             window.Serial.ResetFunction = function() {
                 var ctrls = $("#<%=gvInvoice.ClientID %>").find("input[name='amount']");
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
             };
             setTimeout(function() {
                 $("#<%=gvInvoice.ClientID %>").css("table-layout", "auto");
             }, 1000);
             var accountCtrl = $("#<%=cbAccount.InputHidden.ClientID %>");
             var accountid = accountCtrl.val();
             <%=cbAccount.ClientID %>.Select = function() {
                 if (accountCtrl.val() == accountid)
                     return;
                 accountid = accountCtrl.val();
                 $("#hfAccountId").attr("NoteUrl", "SelectPurchase.aspx?SerializeValueId=<%=hfPurchases.ClientID %>&SerializeContainerId=<%=gvInvoice.ClientID %>&accountid="+accountid);
             };
         });
     }

     </script>
 



 </asp:Content>
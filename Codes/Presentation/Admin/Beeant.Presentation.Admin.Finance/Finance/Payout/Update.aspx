<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payout.Update" MasterPageFile="~/Main.Master" ValidateRequest="false"%>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %> 
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc8" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc5" %>
 
<%@ Register src="../../Controls/Finance/BankComboBox.ascx" tagname="BankComboBox" tagprefix="uc10" %>

 
 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>付款编辑</title>  
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
             <td class="font">名称</td>
            <td class="mtext" colspan="3">
                 <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"  />
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
            
            <td class="font">账户</td>
            <td class="mul">
              
                <uc8:AccountComboBox ID="cbAccount" runat="server" />
              
            </td>
            <td class="font">银行</td>
            <td class="mul" >

                <uc10:BankComboBox ID="cbBank" runat="server"  TextBindName="BankNumber" TextSaveName="BankNumber"  />
     
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
            <td class="font">申请付款日期</td>
            <td class="text">
                <asp:TextBox ID="txtPayTime" runat="server" CssClass="input"  BindName="PayTime" SaveName="PayTime"></asp:TextBox>
 
            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPayTime" Format="yyyy-MM-dd">
                </cc1:CalendarExtender>
            </td>
           <td class="font">支付方式</td>
            <td class="text">
                <uc5:GeneralDropDownList ID="ddlPayType" runat="server"  BindName="PayType" SaveName="PayType" ObjectName="PayTypeEntity" />
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
           <td class="font">原始凭证编号</td>
            <td class="mtext" colspan="3"  >
                 <input id="txtOriginalNumber" runat="server"  type="text" class="input long"   BindName="OriginalNumber" SaveName="OriginalNumber"   />
                 
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
    </table>
 
</div>

    <uc2:Message ID="Message1" runat="server" />
        
    </ContentTemplate>
</asp:UpdatePanel>
 


 



 </asp:Content>
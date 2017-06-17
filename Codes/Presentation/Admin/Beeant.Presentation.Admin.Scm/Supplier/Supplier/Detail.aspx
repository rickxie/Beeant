<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Supplier.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>供应商详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">供应商名称</td>
            <td class="mtext" >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
             
             <td class="font">账户信息</td>
             <td class="text"> <a href="/Finance/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" BindName="Account.Id"> <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label></a></td>
            
        </tr>
         <tr>
            <td class="font">省</td>
            <td  class="mtext" >
                <asp:Label ID="lblProvince" runat="server" BindName="Province"></asp:Label>
            </td>
            <td class="font">固定电话</td>
            <td class="text">
                 <asp:Label ID="lblTelephone" runat="server" BindName="Telephone"></asp:Label>
            </td>
            
        </tr>
       <tr>
            <td class="font">市</td>
            <td  class="mtext" >
                <asp:Label ID="lblCity" runat="server" BindName="City"></asp:Label>
            </td>
             <td class="font">手机号码</td>
            <td class="text">
                 <asp:Label ID="lblMobile" runat="server" BindName="Mobile"></asp:Label>
                
            </td>
             
        </tr>
        <tr>
            <td class="font">镇</td>
            <td  class="mtext" >
                <asp:Label ID="lblCounty" runat="server" BindName="County"></asp:Label>
            </td>
            <td class="font">经营范围</td>
            <td class="text">
                <asp:Label ID="lblBusinessRange" runat="server" BindName="BusinessRange" /> 
            </td>
           
        </tr>
         <tr>
            <td class="font">联系人</td>
            <td class="text" >
                <asp:Label ID="lblLinkman" runat="server" BindName="Linkman"></asp:Label>
             </td>
              <td class="font">销售区域</td>
            <td class="text">
                <asp:Label ID="lblSalesRange" runat="server" BindName="SalesRange"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="font">传真</td>
            <td class="text" >
                 <asp:Label ID="lblFax" runat="server" BindName="Fax"></asp:Label>
            </td>
           <td class="font">邮政编码</td>
            <td class="mtext" >
                     <asp:Label ID="lblPostcode" runat="server" BindName="Postcode"></asp:Label>
            </td>
        </tr>
        
        <tr>
              <td class="font">官网主页</td>
            <td class="text">
                <asp:Label ID="lblWebUrl" runat="server" BindName="WebUrl"></asp:Label>
            </td>
             <td class="font">返修收货人</td>
            <td class="text">
                <asp:Label ID="lblReceiver" runat="server" BindName="Receiver"></asp:Label>
            </td>
        </tr>
        <tr>
              <td class="font" >经营品牌</td>
            <td class="text" >
                <asp:Label ID="lblBrand" runat="server" BindName="BusinessBrand"></asp:Label>
            </td>
               <td class="font">售后咨询电话</td>
            <td class="text" >
                <asp:Label ID="lblServiceTelephone" runat="server" BindName="ServiceTelephone"></asp:Label>
            </td>
        </tr>
         <tr>
          <td class="font">QQ</td>
            <td class="mtext" >
                 <asp:Label ID="lblQq" runat="server" BindName="Qq"></asp:Label>
            </td>
            <td class="font">返修联系方式</td>
            <td class="text" >
                <asp:Label ID="lblReceiverTelephone" runat="server" BindName="ReceiverTelephone"></asp:Label>
            </td>
        </tr>
           <tr>
          <td class="font">电子邮件</td>
            <td class="mtext" >
           
                   <asp:Label ID="lblEmail" runat="server" BindName="Email"></asp:Label>
            </td>
         <td class="font">集成商录入时间</td>
             <td class="text"><asp:Label ID="lblInsertTime" runat="server" BindName="InsertTime"></asp:Label></td>
        </tr>
        <tr>
             <td class="font">维护客服</td>
            <td class="text">
                <asp:Label ID="lblServiceRealName" runat="server" BindName="Service.RealName"></asp:Label>
            </td>
             <td class="font">地址</td>
            <td class="text"  >
                <asp:Label ID="lblAddress" runat="server" BindName="Address"></asp:Label>
            </td>
        </tr>
       
       <tr>
           <td class="font">返修地址</td>
           <td class="text"  colspan="3" >
               <asp:Label ID="lblServiceAddress" runat="server" BindName="ServiceAddress"></asp:Label>
            </td>
        </tr>
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
       <div class="subtitle" onclick="SetEntityBody('divQualification')">资质信息(<span class="count"><%=suQualification.DataCount%></span>)
        </div>
       <div id="divQualification" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     
           <asp:GridView ID="gvQualification" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
          <asp:TemplateField HeaderText="品牌授权"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("BrandAuthorizationName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="供应商"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Supplier.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="组织机构代码证"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <img src="<%#Eval("FullAgencyLicense")%>" class="img"/>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="营业执照"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <img src="<%#Eval("FullBusinessLicense")%>" class="img"/>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="银行开许可证"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <img src="<%#Eval("FullBankLicense")%>" class="img"/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="税务登记证"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <img src="<%#Eval("FullTaxLicense")%>" class="img"/>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="商标注册证"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <img src="<%#Eval("FullTrademarkLicense")%>" class="img"/>
            </ItemTemplate>
        </asp:TemplateField>
            
        </Columns>
     </asp:GridView>

       <uc1:Pager ID="suQualification" runat="server" PageSize="10"  
                     SelectExp="BrandAuthorization,Supplier.Id,AgencyLicense,BusinessLicense,BankLicense,TaxLicense,TrademarkLicense,Supplier.Name"
                     FromExp="QualificationEntity"
                     OrderByExp="UpdateTime desc" WhereExp="Supplier.Id==@Id" />
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
  
  
        <div class="subtitle" onclick="SetEntityBody('divContraction')">合同信息(<span class="count"><%=suContraction.DataCount%></span>)
        </div>
       <div id="divContraction" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>

     <asp:GridView ID="gvContraction" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
          <asp:TemplateField HeaderText="供应商"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Supplier.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="结算方式"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("SettlementType") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="支付方式"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("PaymentTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="配送方式"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%# Eval("DispatchTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="发票类型"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("BillTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="合同起始日期"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("StartDate", "{0:yyyy-MM-dd}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="合同结束日期"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("EndDate", "{0:yyyy-MM-dd}")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="返利条件说明"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Rebate")%>
            </ItemTemplate>
        </asp:TemplateField>
           
           <asp:TemplateField HeaderText="合同附件"  ItemStyle-CssClass="left status">
            <ItemTemplate>
               <a href="<%#Eval("FullAttachment") %>" target="_blank">附件下载</a>   
            </ItemTemplate> 
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
    
     <uc1:Pager ID="suContraction" runat="server" PageSize="10"  
                     SelectExp="Supplier.Id,SettlementType,PaymentType,DispatchType,BillType,StartDate,EndDate,Rebate,Attachment,Supplier.Name"
                     FromExp="ContractEntity"
                     OrderByExp="UpdateTime desc" WhereExp="Supplier.Id==@Id" />

     

    </ContentTemplate>
 </asp:UpdatePanel>
         </div>

     <div class="subtitle" onclick="SetEntityBody('divCertification')">其他证书(<span class="count"><%=suCertification.DataCount%></span>)
        </div>
       <div id="divCertification" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">

     </table>
        </div>
   
           <asp:GridView ID="gvCertification" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
          <asp:TemplateField HeaderText="供应商"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Supplier.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="供应商其他证书"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <img src="<%#Eval("FullCertification")%>" class="img"/>
                
            </ItemTemplate>
        </asp:TemplateField>
      
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="suCertification" runat="server" PageSize="10"  
     SelectExp="Supplier.Id,Certification,Supplier.Name"
      FromExp="CertificationEntity"
      OrderByExp="UpdateTime desc" WhereExp="Supplier.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>

     <uc3:Progress ID="Progress1" runat="server" />
  </ContentTemplate>
  </asp:UpdatePanel>
  </div>
 </asp:Content>
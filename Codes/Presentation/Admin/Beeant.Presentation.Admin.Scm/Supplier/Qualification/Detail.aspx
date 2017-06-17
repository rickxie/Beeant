<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Qualification.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>供应商资质</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
               <td class="font">品牌授权</td>
            <td class="text">
                <asp:Label ID="lblBrandAuthorization" runat="server" BindName="BrandAuthorizationName"></asp:Label>
            </td>      
            <td class="font">供应商</td>
            <td class="text" colspan="3">
                <asp:Label ID="lblSupplierName" runat="server" BindName="Supplier.Name"></asp:Label>
            </td>      
        </tr>
         <tr>
            <td class="font">组织机构代码证</td>
            <td  class="text"  >
                <img src="" id="imgAgencyLicense" runat="server" class="img" BindName="FullAgencyLicense"/>
            </td>
             <td class="font">营业执照</td>
            <td class="text" >
                <img src="" id="imgBusinessLicense" runat="server" class="img" BindName="FullBusinessLicense"/>
             </td>
        </tr>
       
         <tr>
               <td class="font">银行开许可证</td>
            <td class="text">
                <img src="" id="imgBankLicense" runat="server" class="img" BindName="FullBankLicense"/>
            </td>
            <td class="font">税务登记证</td>
            <td class="text" >
                <img src="" id="imgTaxLicense" runat="server" class="img" BindName="FullTaxLicense"/>
            </td>
        </tr>
        <tr>
            
            <td class="font">商标注册证</td>
            <td class="text" colspan="3">
                <img src="" id="imgTrademarkLicense" runat="server" class="img" BindName="FullTrademarkLicense" alt=""/>
            </td>
        </tr>
        <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
    
     <uc3:Progress ID="Progress2" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
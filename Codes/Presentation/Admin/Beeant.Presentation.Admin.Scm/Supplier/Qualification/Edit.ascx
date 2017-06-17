<%@ Control AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Qualification.Edit" Language="C#" %>
<%@ Register TagPrefix="uc1" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Uploader" Src="../../Controls/Uploader.ascx" %>
     
<div class="edit">
    <table class="tb">
        <tr>
            
            <td class="font">品牌授权</td>
            <td class="text" colspan="3" >
             <uc1:GeneralDropDownList ID="ddlBrandAuthorization"  runat="server" SaveName="BrandAuthorization" BindName="BrandAuthorization" 
                     ObjectName="Beeant.Domain.Entities.Supplier.QualificationType" IsEnum="True" ValidateName="BrandAuthorizationName" />
            </td>
        </tr>
         <tr>
             
              <td class="font">组织机构代码证：</td>
            <td class="text" >
             <uc2:Uploader ID="Uploader2" runat="server" Path="Files/Documents/Supplier/" FileByteSaveName="AgencyLicenseByte" FileNameBindName="AgencyLicense"  FileNameSaveName="AgencyLicense"  FullFileNameBindName="FullAgencyLicense" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
             </td>
                <td class="font">营业执照</td>
            <td class="text"   >
                <uc2:Uploader ID="Uploader1" runat="server" Path="Files/Documents/Supplier/" FileByteSaveName="BusinessLicenseByte" FileNameBindName="BusinessLicense"  FileNameSaveName="BusinessLicense"  FullFileNameBindName="FullBusinessLicense" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
        <tr>
            <td class="font">银行开户许可证</td>
            <td class="text">
                <uc2:Uploader ID="Uploader6" runat="server" Path="Files/Documents/Supplier/" FileByteSaveName="BankLicenseByte" FileNameBindName="BankLicense"  FileNameSaveName="BankLicense"  FullFileNameBindName="FullBankLicense" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
            <td class="font"> 税务许可证</td>
            <td class="text">
                <uc2:Uploader ID="Uploader4" runat="server" Path="Files/Documents/Supplier/" FileByteSaveName="TaxLicenseByte" FileNameBindName="TaxLicense"  FileNameSaveName="TaxLicense"  FullFileNameBindName="FullTaxLicense" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
            </td>
        </tr>
         <tr>
              <td class="font">商标注册证</td>
              <td class="text" colspan="3">
                  <uc2:Uploader ID="Uploader5" runat="server" Path="Files/Documents/Supplier/" FileByteSaveName="TrademarkLicenseByte" FileNameBindName="TrademarkLicense"  FileNameSaveName="TrademarkLicense"  FullFileNameBindName="FullTrademarkLicense" Accept="image/jpg,image/gif,image/png,image/bmp,image/jpeg" />
               </td>   
        </tr>

         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
                <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
        <!--存放传过来的供应商ID-->
        
    </table>
   <input id="hidSupplier" runat="server" type="hidden" BindName="Supplier.Id" SaveName="Supplier.Id" />
</div>
 
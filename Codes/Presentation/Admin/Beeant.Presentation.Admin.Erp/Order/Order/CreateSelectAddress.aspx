<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateSelectAddress.aspx.cs"
    Inherits="Beeant.Presentation.Admin.Erp.Order.Order.CreateSelectAddress" 
    ValidateRequest="false" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register Src="/Controls/Message.ascx" TagName="Message" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="DistrictDropDownList" Src="~/Controls/Basedata/DistrictDropDownList.ascx" %>
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
     
      <link href="<%=Page.GetUrl("PresentationAdminHomeUrl")%>/Styles/Style.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="/scripts/Winner/Winner.ClassBase.js"></script>
      <script type="text/javascript" src="/scripts/plug/jquery-1.7.1.min.js"></script>
           <script type="text/javascript" src="/Scripts/winner/validator/winner.validator.js"></script>

</head>
<body>
           <form id="form1" runat="server" enctype="multipart/form-data">
            <div class="main" style="padding: 0;margin: 0;position:relative; top: 0;">
               <div class="body" style="padding: 0;margin: 0;">
   <div class="edit">
        <table class="tb">
    <tr>
            
            <td class="font">
                接收人</td>
            <td class="text">
                <input id="txtRecipient" runat="server"  type="text" class="input"   BindName="Recipient" SaveName="Recipient"   />
                
            </td>
            <td class="font">
                手机号码</td>
            <td class="text">
                <input id="txtMobile" runat="server"  type="text" class="input"   BindName="Mobile" SaveName="Mobile"   />
            </td>
            
        </tr>
         <tr>
               <td class="font">邮政编码</td>
            <td class="text"  >
              <input id="txtPostcode" runat="server"  type="text" class="input"   BindName="Postcode" SaveName="Postcode"   />
             </td>
           <td class="font">地址标签</td>
            <td class="text"  >
                  <input id="txtTag" runat="server"  type="text" class="input"   BindName="Tag" SaveName="Tag"   />
           
             </td>
           
        </tr>
        <tr>
            
            <td class="font">
                邮箱</td>
            <td class="text" colspan="3">
                <input id="txtEmail" runat="server"  type="text" class="input"   BindName="Email" SaveName="Email"   />
                
            </td>
            
            
        </tr>
           
        <tr>
     
            <td class="font">区域</td>
            <td class="text" >
                <uc1:DistrictDropDownList ID="DistrictDropDownList1" runat="server" CityValidateName="City" ProvinceValidateName="Province" CountyValidateName="County" ProvinceSaveName="Province" CitySaveName="City" CountySaveName="County" />
            </td>
    
             <td class="font">地址</td>
            <td class="mtext"  >
              <input id="txtAddress" runat="server"  type="text" class="input long"   BindName="Address" SaveName="Address"   />
             </td>
        </tr>
        
        <tr>
                <td colspan="4" class="center">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
       
                </td>
            </tr>

            </table>
    </div>
            <uc2:message id="Message1" runat="server" />
            </div>
            </div>
            </form>
            
  </body>
  </html>
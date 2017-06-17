<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Supplier.Edit" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc2" %>

     
<%@ Register src="../../Controls/Basedata/DistrictDropDownList.ascx" tagname="DistrictDropDownList" tagprefix="uc1" %>
<%@ Register TagPrefix="uc3" TagName="UserComboBox" Src="~/Controls/User/UserComboBox.ascx" %>

     
<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">供应商名称</td>
            <td class="text" colspan="3" >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
                
               </td>
            
            
        </tr>
         <tr>
             
              <td class="font">联系人</td>
            <td class="text" >
             <input id="txtLinkman" runat="server"  type="text" class="input"  BindName="Linkman" SaveName="Linkman"  />
             </td>
                <td class="font">QQ</td>
            <td class="text"   >
                 <input id="txtQq" runat="server"  type="text" class="input"   BindName="Qq" SaveName="Qq"   />
            </td>
        </tr>
        <tr>
            <td class="font">传真</td>
            <td class="text"><input id="txtFax" runat="server"  type="text" class="input"   BindName="Fax" SaveName="Fax"   /> 
            <td class="font"> 邮政编码</td>
            <td class="mtext" colspan="3" >
                <input id="txtPostcode" runat="server"  type="text" class="input"   BindName="Postcode" SaveName="Postcode"   />
            </td>
        </tr>
         <tr>
              <td class="font">固定电话</td>
              <td class="text"><input id="txtTelephone" runat="server"  type="text" class="input"   BindName="Telephone" SaveName="Telephone"   /> </td>
              <td class="font">手机号码</td>
              <td class="text"><input id="Text1" runat="server"  type="text" class="input"   BindName="Mobile" SaveName="Mobile"   /> </td>   
        </tr>

        <tr>
              <td class="font">电子邮件</td>
            <td class="text" colspan="3"><input id="Text2" runat="server"  type="text" class="input"   BindName="Email" SaveName="Email" />
             
            </td>
              
          
        </tr>
        <tr>
            
              <td class="font">经营品牌</td>
            <td class="text"><input id="txtBusinessBrand" runat="server"  type="text" class="input"   BindName="BusinessBrand" SaveName="BusinessBrand"></input>
            <td class="font">经营范围</td>
              <td class="text"><input id="txtBusinessRange" runat="server"  type="text" class="input"   BindName="BusinessRange" SaveName="BusinessRange"   /> </td> 

              
        </tr>
        <tr>
            <td class="font">销售区域</td>
            <td class="text"><input id="txtSalesRange" runat="server"  type="text" class="input"   BindName="SalesRange" SaveName="SalesRange"></input> </td>
              <td class="font">售后咨询电话</td>
            <td class="text"><input id="txtServiceTelephone" runat="server"  type="text" class="input"   BindName="ServiceTelephone" SaveName="ServiceTelephone"></input> 
            </td>
        </tr>
        <tr>
              <td class="font">返修联系方式</td>
            <td class="text"><input id="txtReceiverTelephone" runat="server"  type="text" class="input"   BindName="ReceiverTelephone" SaveName="ReceiverTelephone"></input> 
            </td>
             <td class="font">返修收货人</td>
            <td class="text"><input id="txtReceiver" runat="server"  type="text" class="input"   BindName="Receiver" SaveName="Receiver"   /> </td>
        </tr>
        <tr>
            <td class="font">所属账户</td>
            <td class="mul">
                <uc2:AccountComboBox ID="cbAccount" runat="server" />
            </td>
            <td class="font">维护人</td>
            <td class="mul" >
        
                 <uc3:UserComboBox ID="cbService" runat="server" TextBindName="Service.RealName"  TextSaveName="Service.RealName" HiddenBindName="Service.Id"  HiddenSaveName="Service.Id" />
            </td>
            
        </tr>
           <tr>
              <td class="font">官网主页</td>
            <td class="text" colspan="3"><input id="txtWebUrl" runat="server"  type="text" class="input long"   BindName="WebUrl" SaveName="WebUrl" />
             
            </td>
              
          
        </tr>
        <tr>
           <td class="font">办公地址</td>
           <td class="text"  colspan="3" >
                 <input id="txtAddress" runat="server"  type="text" class="input long"   BindName="Address" SaveName="Address"   />
            </td>
        </tr>

        <tr>
           <td class="font">返修地址</td>
           <td class="text"  colspan="3" >
                 <input id="txtServiceAddress" runat="server"  type="text" class="input long"   BindName="ServiceAddress" SaveName="ServiceAddress"/>
            </td>
        </tr>
        <tr>
            <td class="font">区域</td>
            <td class="text" colspan="3">
                <uc1:DistrictDropDownList ID="DistrictDropDownList1" runat="server" CityValidateName="City" ProvinceValidateName="Province" CountyValidateName="County" />
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
        
    </table>
 
</div>

 
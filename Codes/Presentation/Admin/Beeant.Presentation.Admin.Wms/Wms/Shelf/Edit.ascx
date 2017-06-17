<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Shelf.Edit" %>
     
<%@ Register src="../../Controls/Product/ProductCombox.ascx" tagname="ProductCombox" tagprefix="uc1" %>
     
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc2" %>
     
<div class="edit">
<table class="tb">
        
        <tr>
            <td class="font">名称</td>
            <td class="text" >
              <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"   /> 
            </td>
             <td class="font">仓库</td>
            <td class="text"  >
                 <uc2:GeneralDropDownList ID="ddlStorehouse" runat="server" ObjectName="StorehouseEntity" SaveName="Storehouse.Id" BindName="Storehouse.Id" />
               
            </td>
        </tr>
        
 
      
       <tr>
            <td class="font">产品</td>
            <td class="text" colspan="3">

                <uc1:ProductCombox ID="ProductCombox1" runat="server" />
            </td>
       </tr>
    
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
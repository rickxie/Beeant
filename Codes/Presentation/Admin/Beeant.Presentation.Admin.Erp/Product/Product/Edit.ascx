<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Product.Edit" %>


<div class="edit">
    <table class="tb">
      
        <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3" >
              <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"   /> 
            </td>
           
        </tr>

           <tr>
          
             <td class="font">是否定制</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsCustom" runat="server" BindName="IsCustom" SaveName="IsCustom" />
                </td>
             <td class="font">可以退货</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsReturn" runat="server" BindName="IsReturn" SaveName="IsReturn" />
                </td>
       </tr>
  

         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=ckIsCustom.ClientID%>").bind("click", function () {
                var returnCtrl = '<%=ckIsReturn.ClientID %>';
                if ($("#<%=ckIsCustom.ClientID%>").prop("checked")) {
                    $("#" + returnCtrl).hide();
                    $("#" + returnCtrl).prop("checked", false);
                } else {
                    $("#" + returnCtrl).show();
                }
            });
        });
</script>
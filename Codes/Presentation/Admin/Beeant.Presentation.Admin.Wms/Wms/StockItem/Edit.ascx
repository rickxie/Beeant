<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.StockItem.Edit" %>
<%@ Register TagPrefix="uc7" TagName="GeneralTreeView" Src="~/Controls/GeneralTreeView.ascx" %>

<div class="edit">
    <table class="tb">
    

      <tr>
               <td class="font">名称</td>
            <td class="text" >
              <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"   /> 
            </td>
              <td class="font">商品编号</td>
            <td class="text">
              <input id="txtProductId" runat="server"  type="text" class="input"  BindName="Product.Id" SaveName="Product.Id"   /> 
            </td>
       </tr>
        <tr>
        
             <td class="font">数量</td>
            <td class="mtext" colspan="3" >
                 <input id="txtCount" runat="server"  type="text" class="input"  BindName="Count" SaveName="Count"    /> 
            </td>
       </tr>

        <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3">
                <input id="txtRemark" runat="server"  type="text" class="input long"  BindName="Remark" SaveName="Remark"  />
            </td>
        
       </tr>
       <tr>
           <td colspan="4" class="text">
               <table class="intb">
                    <tr>
                        <td class="infont">仓库</td>
                        <td class="intext">   
  
                
                             <uc7:GeneralTreeView ID="tvStorehouseTree" runat="server" OnSelectedNodeChanged="tvStorehouseTree_SelectedNodeChanged" EntityName="StorehouseEntity" IsShowNone="True" />
                        </td>
                        <td class="infont">已经选择</td>
                        <td class="intext">
                            <asp:Label ID="lblStorehouseName" runat="server"  BindName="Storehouse.Name" Text=""></asp:Label>
                            <input id="hfStorehouseId" type="hidden" runat="server"  BindName="Storehouse.Id"  SaveName="Storehouse.Id"/>   
                 
                        
  
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>

</div>
 
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Property.Edit" %>
      <%@ Register src="/Controls/GeneralTreeView.ascx" tagname="GeneralTreeView" tagprefix="uc4" %>
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>

<div class="edit">
    <table class="tb">
        
        <tr>
            <td class="font">名称</td>
            <td class="mtext"  colspan="3" >
              <input id="txtName" runat="server"  type="text" class="input"  BindName="Name" SaveName="Name"   /> 
            </td>
           
        </tr>

         <tr>
          
           <td class="font">搜索类型</td>
            <td class="text" >
               
                 <uc1:GeneralDropDownList ID="ddlSearchType" runat="server"  BindName="SearchType" SaveName="SearchType" ObjectName="Beeant.Domain.Entities.Product.PropertySearchType" IsEnum="True" />
            </td>
             <td class="font">自定义属性数量</td>
            <td class="text" >
                 <input id="txtCustomCount" runat="server"  type="text" class="input"  BindName="CustomCount" SaveName="CustomCount" value="0" DefaultValue="0"   /> 
            </td>
       </tr>
          
        <tr>
             <td class="font">排序</td>
            <td class="text" >
                 <input id="txtSequence" runat="server"  type="text" class="input" DefaultValue="1" value="1"  BindName="Sequence" SaveName="Sequence"  />
            </td>
               <td class="font">类型</td>
            <td class="text" >
            
                <uc1:GeneralDropDownList ID="ddlType" runat="server" SaveName="Type" BindName="Type" ObjectName="Beeant.Domain.Entities.Product.PropertyType" IsEnum="True" />
            
            </td>
     </tr>
        <tr>
           <td class="font">是否启用</td>
            <td class="text" >
               <asp:CheckBox ID="ckIsUsed" runat="server"  BindName="IsUsed" SaveName="IsUsed"  />
            </td>
        <td class="font">是否SKU</td>
            <td class="text" >
                 <asp:CheckBox ID="ckIsSku" runat="server"  BindName="IsSku" SaveName="IsSku"  />
              
            </td>
       </tr>
       <tr>
             <td class="font">是否允许编辑</td>
            <td class="text" >
                 <asp:CheckBox ID="ckIsAllowEdit" runat="server"  BindName="IsAllowEdit" SaveName="IsAllowEdit"  />
              
            </td>
         <td class="font">标签</td>
            <td class="text"   >
                 <input id="txtTag" runat="server"  type="text" class="input long"    BindName="Tag" SaveName="Tag"  />
            </td>
     </tr>
     <tr>
         <td class="font">错误提示</td>
            <td class="mtext" colspan="3"  >
                 <input id="txtMessage" runat="server"  type="text" class="input long"    BindName="Message" SaveName="Message"  />
            </td>
     </tr>
             
      
       <tr>
           <td>值(,表示分割)</td>
            <td class="mtext" colspan="3" >
                <input id="txtValue" runat="server"  type="text" class="input long"   BindName="Value" SaveName="Value" ></input>
            </td>
       </tr>
         <tr>
           <td>搜索值(,表示分割)</td>
            <td class="mtext" colspan="3" >
                <input id="txtSearchValue" runat="server"  type="text" class="input long"   BindName="SearchValue" SaveName="SearchValue" ></input>
            </td>
       </tr>
       <tr>
       <td colspan="4" class="text">
    <table class="intb">
        <tr>
         <td class="infont">类目</td>
            <td class="intext">
                 <uc4:GeneralTreeView ID="tvCategoryTree" runat="server" OnSelectedNodeChanged="tvCategoryTree_SelectedNodeChanged" EntityName="CategoryEntity" IsShowNone="True" />
 
            </td>
             <td class="infont">已经选择 </td>
              <td class="intext">
                  <asp:Label ID="lblCategoryName" runat="server"  BindName="Category.Name" Text=""></asp:Label>
                 <input id="hfCategoryId" type="hidden" runat="server"  BindName="Category.Id"  SaveName="Category.Id"/>   
                 
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
 <script type="text/javascript">
     $("#<%=txtValue.ClientID %>").bind("keyup", function () {
         $("#<%=txtSearchValue.ClientID %>").val($("#<%=txtValue.ClientID %>").val());
     });
 </script>
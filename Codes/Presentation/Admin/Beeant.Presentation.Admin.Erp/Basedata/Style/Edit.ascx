<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Style.Edit" %>
 
     
 
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>
<%@ Register src="../../Controls/Editor.ascx" tagname="Editor" tagprefix="uc2" %>
 
     
 
<div class="edit">
    <table class="tb">
         <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3" >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
            </td>
            
        </tr>
          <tr>
            <td class="font">路径</td>
            <td class="mtext" >
             <input id="txtPath" runat="server"  type="text" class="input long"  BindName="Path" SaveName="Path"  /> 
            </td>
             <td class="font">排序</td>
            <td class="mtext" >
             <input id="txtSequence" runat="server"  type="text" class="input"  BindName="Sequence" SaveName="Sequence" value="1"  /> 
            </td>
        </tr>
         <tr>
          <td class="font">类型</td>
            <td class="text">
                <uc1:GeneralDropDownList ID="ddlStyleType" runat="server" BindName="Type" SaveName="Type" ObjectName="Beeant.Domain.Entities.Basedata.StyleType" IsEnum="True" />
            </td>
           <td class="font">是否显示</td>
            <td class="text">
                <asp:CheckBox ID="ckIsShow" runat="server" BindName="IsShow" SaveName="IsShow"  />
            </td>
             
        </tr>
        
        <tr>
           <td class="font">备注</td>
           <td class="text"  colspan="3" >
                 <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
                 
            </td>
        </tr>
        <tr>
            <td class="font">详情</td>
           <td class="text"  colspan="3" >
                
               <uc2:Editor ID="Editor1" runat="server" BindName="Detail" SaveName="Detail" ImagePath="Files/Eidtor/Images/Style/" FlashPath="Files/Eidtor/Flashs/Style/"/>
                
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
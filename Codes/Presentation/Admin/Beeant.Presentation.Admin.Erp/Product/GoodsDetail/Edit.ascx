<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.GoodsDetail.Edit" %>
<%@ Register TagPrefix="uc3" TagName="Editor" Src="~/Controls/Editor.ascx" %>

<div class="edit"  >
    
    <table class="tb"> 
             <tr>
            <td class="font">说明</td>
            <td class="mtext" colspan="3" >
                <textarea id="txtDescription" runat="server"  type="text" class="input long"  BindName="Description" SaveName="Description" ></textarea>
           
            </td>
          
        </tr>
        
       <tr>
           <td class="font">详情</td>
           <td colspan="3" class="mul mtext">
               

               <uc3:Editor ID="Editor1" runat="server" ImagePath="Files/Eidtor/Images/Goods/" FlashPath="Files/Eidtor/Flashs/Goods/" SaveName="Detail" BindName="Detail" />
               

           </td>
       </tr>
         <tr>
            <td colspan="4" class="center">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  />
            </td>
        </tr>
    </table>
 
</div>
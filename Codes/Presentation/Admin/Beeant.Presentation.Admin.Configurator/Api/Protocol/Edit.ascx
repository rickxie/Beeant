<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Api.Protocol.Edit" %>
<%@ Register TagPrefix="uc3" TagName="Editor" Src="/Controls/Editor.ascx" %>

<div class="edit">
    <table class="tb">
       <tr>
            <td class="font">名称</td>
            <td colspan="3" ><input id="txtName" runat="server" type="text" class="input long"  BindName="Name" SaveName="Name"  /></td>
         </tr>
         <tr>
            <td class="font">昵称</td>
            <td  class="text">
                <input id="txtNickname" runat="server"  type="text" class="input"  BindName="Nickname" SaveName="Nickname" />
            </td>
               <td class="font">单秒请求数</td>
            <td class="text">
                <input id="txtSecondCount" runat="server"  type="text" class="input"  BindName="SecondCount" SaveName="SecondCount" />
            </td>
        </tr>
         <tr>
            
               <td class="font">单天请求数</td>
            <td colspan="3"  class="mtext" >
                <input id="txtDayCount" runat="server"  type="text" class="input"  BindName="DayCount" SaveName="DayCount" />
            </td>
        </tr>
          <tr>
            <td class="font">是否验证</td>
            <td class="text"><asp:CheckBox ID="ckIsVerify" runat="server" BindName="IsVerify" SaveName="IsVerify" ></asp:CheckBox> </td>
             <td class="font">是否启用</td>
            <td class="text"><asp:CheckBox ID="ckIsStart" runat="server" BindName="IsStart" SaveName="IsStart" ></asp:CheckBox> </td>
         </tr>
          <tr>
            <td class="font">是否记录日志</td>
            <td class="text" ><asp:CheckBox ID="ckIsLog" runat="server" BindName="IsLog" SaveName="IsLog" ></asp:CheckBox> </td>
          <td class="font">是否签名</td>
            <td class="text"  ><asp:CheckBox ID="ckIsSign" runat="server" BindName="IsSign" SaveName="IsSign" ></asp:CheckBox> </td>
         </tr>
        <tr>
           <td class="font">详情</td>
           <td colspan="3" class="mul text">
               

               <uc3:Editor ID="Editor1" runat="server" ImagePath="Files/Eidtor/Images/Protocol/" FlashPath="Files/Eidtor/Flashs/Protocol/" SaveName="Detail" BindName="Detail" />
               

           </td>
       </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
 
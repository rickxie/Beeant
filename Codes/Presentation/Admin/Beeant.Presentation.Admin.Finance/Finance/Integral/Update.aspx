<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Integral.Update" MasterPageFile="~/Main.Master" ValidateRequest="false"%>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc8" %>
 
 
 
 
 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>积分申请编辑</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


    




    


  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
     <input id="hfStatusControl" type="hidden" runat="server" />
 <div class="edit">
     <a href='add.aspx?id=<%=RequestId%>'  name="Add">新增</a>
     <a href='Detail.aspx?id=<%=RequestId%>'  name="Entity">详情</a>
   <a href='handle.aspx?id=<%=RequestId%>'  name="Edit">处理</a>
      <table class="tb">
        
         <tr>
             <td class="font">金额</td>
            <td class="text" >
             <input id="txtAmount" runat="server"  type="text" class="input"  BindName="Amount" SaveName="Amount"  />
             </td>
                 <td class="font">账户</td>
            <td class="mul">
              
                <uc8:AccountComboBox ID="cbAccount" runat="server" />
              
            </td>
        </tr>   
        <tr>
            <td class="font">抬头</td>
            <td class="mtext" colspan="3" >
             <input id="txtTitle" runat="server"  type="text" class="input long"  BindName="Title" SaveName="Title"  />
             </td>
        </tr>
             
     
       
           <tr>
           
             <td class="font">消息</td>
            <td class="mtext" colspan="3">
                <asp:CheckBoxList ID="ckMessageType" runat="server" ></asp:CheckBoxList>
               
            </td>
        </tr>
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                 <input id="txtRemark" runat="server"  type="text" class="input long"   BindName="Remark" SaveName="Remark"   />
            </td>
        </tr>
         <tr>
           <td class="font">流程备注</td>
            <td class="mtext" colspan="3" >
                 <input id="txtHistoryRemark" runat="server"  type="text" class="input long"  />
            </td>
        </tr>
    
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>

    <uc2:Message ID="Message1" runat="server" />
        
    </ContentTemplate>
</asp:UpdatePanel>
 


 



 </asp:Content>
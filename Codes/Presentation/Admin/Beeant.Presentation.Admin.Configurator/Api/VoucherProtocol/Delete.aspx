<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Api.VoucherProtocol.Delete" MasterPageFile="~/Datum.Master" %>


<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %> 

<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %> 
   <%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>凭证编号<%=Request.QueryString["VoucherId"]%>回收协议</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
       <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
     
                  <uc2:DataSearch ID="DataSearch1" runat="server" />
      
      
        </tr>
      <tr>
        <td class="font">昵称</td>
        <td class="text"><input type="text" id="txtProtocolNickname" runat="server" class="seinput" SearchWhere="Protocol.Nickname.Contains(@ProtocolNickname) " SearchParamterName="ProtocolNickname" /> </td>
        <td  class="font">名称</td> 
        <td colspan="5" class="text"><input type="text" id="txtProtocolName" runat="server" class="seinput" SearchWhere="Protocol.Name.Contains(@ProtocolName) " SearchParamterName="ProtocolName" /></td>
        </tr>
     </table>
        </div>

           <div class="mainten">
                <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"  ConfirmBox="Remove" ConfirmMessage="您确定要删除吗" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>
                <asp:DropDownList ID="ddlIsForbid" runat="server" SaveName="IsForbid" ComfirmDropdownListMessage="请选择操作项" ComfirmValidate="IsForbid">
                  <asp:ListItem  Value="False" Text="允许" ></asp:ListItem>
                  <asp:ListItem  Value="True" Text="禁止" ></asp:ListItem>
            </asp:DropDownList>
              <asp:Button ID="btnIsForbid" runat="server" Text="确定" CssClass="btn" ConfirmBox="IsForbid" ConfirmMessage="您确定要修改吗？"  ComfirmCheckBoxMessage="你没有选择任何行" onclick="btnIsForbid_Click" />
           <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>

     <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("ID") %>'  id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,IsForbid" />
           </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="凭证编号"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Voucher.Id")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="协议名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Protocol.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="协议昵称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Protocol.Nickname")%>
            </ItemTemplate>
        </asp:TemplateField>

     <asp:TemplateField HeaderText="是否记录日志"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("IsLogName")%>
            </ItemTemplate>
        </asp:TemplateField>
               <asp:TemplateField HeaderText="请求限制（数量/秒"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("SencondCount")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="请求限制时间(秒)"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("DayCount")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="是否禁止"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("IsForbidName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Voucher.Id,Protocol.Name,Protocol.Nickname,IsForbid,SencondCount,DayCount,IsLog,InsertTime" FromExp="VoucherProtocolEntity" />
                
        </div>

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
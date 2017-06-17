<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Api.VoucherProtocol.Add" MasterPageFile="~/Datum.Master" %>


<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %> 
   <%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>凭证编号<%=Request.QueryString["VoucherId"]%>授权协议</title>  
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
        <td class="text"><input type="text" id="txtNickname" runat="server" class="seinput" SearchWhere="Nickname.Contains(@Nickname) " SearchParamterName="Nickname" /> </td>
        <td  class="font">名称</td> 
        <td colspan="5" class="text"><input type="text" id="txtName" runat="server" class="seinput" SearchWhere="Name.Contains(@Name) " SearchParamterName="Name" /></td>
        </tr>
     </table>
        </div>

           <div class="mainten">
                <asp:Button ID="btnAdd" runat="server" Text="授权" onclick="btnAdd_Click" ConfirmBox="Add" ConfirmMessage="您确定要授权吗"  ComfirmCheckBoxMessage="你没有选择任何行" ></asp:Button>
               
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
               <input value='<%#Eval("ID") %>'  id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Add" />
           </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="是否禁止"  ItemStyle-CssClass="center ckbox">
             <HeaderTemplate>
             <input id="ckIsForbidAll" type="checkbox" AllCheckName="IsForbid"  />是否禁止
            </HeaderTemplate>
            <ItemTemplate>
                 <input   id="ckIsForbid" runat="server" type="checkbox" SubCheckName="IsForbid"  />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="是否记录日志"  ItemStyle-CssClass="center ckbox">
             <HeaderTemplate>
             <input id="ckIsLogAll" type="checkbox" AllCheckName="IsLogAll"  />是否记录日志
            </HeaderTemplate>
            <ItemTemplate>
                 <input   id="ckIsLog" runat="server" type="checkbox" SubCheckName="IsLogAll"  />
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="单秒请求数"  ItemStyle-CssClass="center ckbox">
            <ItemTemplate>
                 <input   id="txtSecondCount" runat="server" type="text" value="0"  />
            </ItemTemplate>
        </asp:TemplateField>
               <asp:TemplateField HeaderText="单天请求数"  ItemStyle-CssClass="center ckbox">
            <ItemTemplate>
                 <input   id="txtDayCount" runat="server" type="text" value="0"  />
            </ItemTemplate>
        </asp:TemplateField>
               <asp:TemplateField HeaderText="参数值"  ItemStyle-CssClass="center ckbox">
            <ItemTemplate>
                 <input   id="txtArgs" runat="server" type="text" value=""  />
            </ItemTemplate>
        </asp:TemplateField>
                   <asp:TemplateField HeaderText="是否签名"  ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckIsSignAll" type="checkbox" AllCheckName="IsSignAll"  />是否签名
            </HeaderTemplate>
            <ItemTemplate>
                 <input   id="ckIsSign" runat="server" type="checkbox" SubCheckName="IsSignAll"  />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="昵称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Nickname")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否验证"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("IsVerifyName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否启用"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("IsStartName")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="接口默认单秒请求数"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("SecondCount")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="接口默认单天请求数"  ItemStyle-CssClass="center status">
            <ItemTemplate>
                <%#Eval("DayCount")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,Nickname,IsStart,IsVerify,SecondCount,DayCount,InsertTime" FromExp="ProtocolEntity" />
                
        </div>
 

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
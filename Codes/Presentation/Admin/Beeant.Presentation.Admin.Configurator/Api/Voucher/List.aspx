<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Api.Voucher.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
        <%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc4" %>       
         <%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
   <%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>凭证列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="Edit" class="edit">
        <input type="button" id="Hide" class="btn" value="隐藏"/>
    <table class="tb">
        <tr>
            <td class="font">账号</td>
            <td class="text mul">
                 <uc8:AccountComboBox ID="AccountComboBox1" runat="server" />
            </td>
             <td class="font">验证类型</td>
            <td class="text">
                     <uc5:GeneralDropDownList ID="ddlType" runat="server" ObjectName="Beeant.Domain.Entities.Api.VoucherType" IsEnum="True" BindName="Type" SaveName="Type" />
   
            </td>
         </tr>
            <tr>
            <td class="font">凭证</td>
            <td  class="text" colspan="3" >
                <input id="txtToken" runat="server"  type="text" class="long input"  BindName="Token" SaveName="Token" />
            </td>
           

        </tr>
         <tr>
          <td class="font">是否记录日志</td>
            <td class="text"  ><asp:CheckBox ID="ckIsLog" runat="server" BindName="IsLog" SaveName="IsLog" ></asp:CheckBox> </td>
            <td class="font">是否签名</td>
            <td class="text"  ><asp:CheckBox ID="ckIsSign" runat="server" BindName="IsSign" SaveName="IsSign" ></asp:CheckBox> </td>

        </tr>
           <tr>
            <td class="font">Ip白名单</td>
            <td  class="text" colspan="3">
                <input id="txtIps" runat="server"  type="text" class="long input"  BindName="Ips" SaveName="Ips" />
            </td>
        </tr>
         <tr>
            <td class="font">回调地址</td>
            <td  class="text" colspan="3">
                <input id="txtUrl" runat="server"  type="text" class="long input"  BindName="Url" SaveName="Url" />
            </td>
        </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   /></td>
        </tr>
    </table>
    <uc4:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>

        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
                <uc2:DataSearch ID="DataSearch1" runat="server" />
        </tr>
            <tr>
                 
             <td class="font">
                    账户 
                </td>
                <td class="text">
                  <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId" HiddenSaveName=""  />
                 
                </td>
                 <td class="font">
                    验证类型 
                </td>
                <td class="text">
                     <uc5:GeneralDropDownList ID="ddlSearchType" runat="server" ObjectName="Beeant.Domain.Entities.Api.VoucherType" IsEnum="True" SearchWhere="Type==@Type" SearchParamterName="Type" SearchPropertyTypeName="Type" />
                  
                </td>
           <td class="font">
                    编号 
                </td>
                <td class="text">
                    <asp:TextBox ID="txtId" runat="server"  SearchWhere="Id==@Name" SearchParamterName="Id" SearchPropertyTypeName="Id" CssClass="seinput"></asp:TextBox>
                </td>
            <td class="font">
                    凭据 
                </td>
                <td class="text">
                    <asp:TextBox ID="txtSearchToken" runat="server"  SearchWhere="Token==@Token" SearchParamterName="Token" CssClass="seinput"></asp:TextBox>
                </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
          <a href='javascript:void(0);' id="Add" class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"/>
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
               <asp:LinkButton runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="授权协议" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Api/VoucherProtocol/add.aspx?VoucherId=<%#Eval("Id") %>' target="_blank">授权协议</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="回收协议" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Api/VoucherProtocol/delete.aspx?VoucherId=<%#Eval("Id") %>' target="_blank">回收协议</a>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Account.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="凭据"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Token")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="验证类型"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
                 <asp:TemplateField HeaderText="是否记录日志"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsLogName")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="回调地址"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Url")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Token,Type,Account.Id,Account.Name,IsLog,Url,InsertTime" FromExp="VoucherEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>
 

 </asp:Content>
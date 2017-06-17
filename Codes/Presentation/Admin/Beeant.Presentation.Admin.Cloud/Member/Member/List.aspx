<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Member.Member.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>  <%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>会员列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
               
        <tr>
            <td class="font">
                昵称
            </td>
            <td class="text">
                <asp:TextBox ID="txtNickname" runat="server" CssClass="seinput" SearchWhere="Nickname.Contains(@Nickname) " SearchParamterName="Nickname" ></asp:TextBox>
            </td>
            <td class="font">
                身份证号码
            </td>
            <td class="text">
                <asp:TextBox ID="txtIdCardNumber" runat="server" CssClass="seinput" SearchWhere="IdCardNumber.Contains(@IdCardNumber) " SearchParamterName="IdCardNumber" ></asp:TextBox>
            </td>
           
            <td class="font">
                固定电话
            </td>
            <td class="text">
                <asp:TextBox ID="txtTelephone" runat="server" CssClass="seinput" SearchWhere="Telephone.Contains(@Telephone) " SearchParamterName="Telephone" ></asp:TextBox>
            </td>
             <td class="font">
                用户状态
            </td>
            <td class="text"  >
                 <asp:DropDownList ID="ddlIsUsed" runat="server" SearchWhere="IsUsed==@IsUsed" SearchParamterName="IsUsed" SearchPropertyTypeName="IsUsed">
                    <asp:ListItem Text="启用" Value="true" ></asp:ListItem>
                    <asp:ListItem Text="禁止" Value="false" ></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
            <tr>
             <td class="font">
                账户
            </td>
            <td  colspan="7"  >
                <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId"  />
            </td>
      </tr>
         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Nickname" Text="昵称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="所属账户" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IdCardNumber" Text="身份证" Selected="True" ></asp:ListItem>                    
                     <asp:ListItem  Value="Telephone" Text="固定电话" Selected="True"></asp:ListItem>                    
                     <asp:ListItem  Value="Gender" Text="性别" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Postal" Text="邮政编码" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Address" Text="地址" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsUsed" Text="是否启用" ></asp:ListItem>
                     <asp:ListItem  Value="Remark" Text="备注" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext"  colspan="2">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem  Value="Id" Text="编号" ></asp:ListItem>
                     <asp:ListItem  Value="Nickname" Text="昵称"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id" Text="所属账户" ></asp:ListItem>
                     <asp:ListItem  Value="IdCardNumber" Text="身份证" ></asp:ListItem>                     
                     <asp:ListItem  Value="Telephone" Text="固定电话" ></asp:ListItem>
                     <asp:ListItem  Value="Postal" Text="邮编" ></asp:ListItem>
                     <asp:ListItem  Value="Gender" Text="性别" ></asp:ListItem>
                     <asp:ListItem  Value="IsUsed" Text="是否启用" ></asp:ListItem>
                     <asp:ListItem  Value="Remark" Text="备注" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="font">
                排序方式
            </td>
            <td>
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td colspan="3">
                  <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
         <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,Status"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Entity">详情</a>
            </ItemTemplate>
        </asp:TemplateField>
  
        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="昵称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Nickname")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="所属账户"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                 <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Name")%></a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="身份证"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IdCardNumber")%>
            </ItemTemplate>
        </asp:TemplateField>        
        <asp:TemplateField HeaderText="地址"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Address")%>
            </ItemTemplate>
        </asp:TemplateField>        
         <asp:TemplateField HeaderText="固定电话"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Telephone")%>
            </ItemTemplate>
        </asp:TemplateField>        
        <asp:TemplateField HeaderText="性别"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Gender")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="邮政编码"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Postal")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="是否启用"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsUsedName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编辑时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("UpdateTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id" OrderByExp="Id desc"  />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
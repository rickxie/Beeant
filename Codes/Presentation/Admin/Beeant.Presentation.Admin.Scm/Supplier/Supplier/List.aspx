<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Supplier.Supplier.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Supplier" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
<%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>  
<%@ Register TagPrefix="uc8" TagName="AccountComboBox" Src="~/Controls/Account/AccountComboBox.ascx" %> 
<%@ Register TagPrefix="us6" TagName="UserCombBox" Src="~/Controls/User/UserComboBox.ascx" %>  
<%@ Register src="../../Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>供应商列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
               
        <tr>
            <td class="font">
                供应商名称
            </td>
            <td class="text">
                <asp:TextBox ID="txtName" runat="server" CssClass="seinput" SearchWhere="Name.Contains(@Name) " SearchParamterName="Name" ></asp:TextBox>
            </td>
            <td class="font">
                供应商状态
            </td>
            <td class="text">
                 <uc1:GeneralDropDownList ID="ddlStatus"  runat="server" SaveName="Status" BindName="Status" 
                            SearchWhere="Status==@Status" SearchParamterName="Status"  SearchPropertyTypeName="Status"
                                ObjectName="Beeant.Domain.Entities.Supplier.SupplierStatusType" IsEnum="True" />
            </td>
            <td class="font">
                联系人
            </td>
            <td class="text">
                <asp:TextBox ID="txtLinkman" runat="server" CssClass="seinput" SearchWhere="Linkman.Contains(@Linkman) " SearchParamterName="Linkman" ></asp:TextBox>
            </td>
            <td class="font">
                账户
            </td>
            <td class="mtext">
                <uc8:AccountComboBox ID="cbAccount" runat="server" HiddenSearchParamterName="AccountId" HiddenSearchWhere="Account.Id==@AccountId" />
            </td>
        </tr>
          <tr>
           
              <td class="font">
                电话号码
            </td>
            <td class="text" >
                 <asp:TextBox ID="txtMobile" runat="server" CssClass="seinput" SearchWhere="Mobile.Contains(@Mobile) " SearchParamterName="Mobile" ></asp:TextBox>
            </td>
             <td class="font">
                固定电话
            </td>
            <td class="text" >
                 <asp:TextBox ID="txtTelephone" runat="server" CssClass="seinput" SearchWhere="Telephone.Contains(@Telephone) " SearchParamterName="Telephone" ></asp:TextBox>
            </td>
             <td class="font">
                QQ
            </td>
            <td class="text" colspan="3" >
                 <asp:TextBox ID="txtQQ" runat="server" CssClass="seinput" SearchWhere="QQ.Contains(@QQ) " SearchParamterName="QQ" ></asp:TextBox>
            </td>
          </tr>
         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem  Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                      <asp:ListItem  Value="Name" Text="供应商名称" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="所属账户"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Linkman" Text="联系人" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Province" Text="省" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="City" Text="市" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="County" Text="镇" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Email" Text="电子邮件" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Status" Text="供应商状态" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="User.RealName" Text="维护人" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="Address" Text="办公地址" ></asp:ListItem>
                     <asp:ListItem  Value="WebUrl" Text="官网主页" ></asp:ListItem>
                     <asp:ListItem  Value="BusinessRange" Text="经营范围" ></asp:ListItem>
                     <asp:ListItem  Value="BusinessBrand" Text="经营品牌" ></asp:ListItem>
                     <asp:ListItem  Value="SalesRange" Text="销售区域" ></asp:ListItem>
                     <asp:ListItem  Value="ServiceTelephone" Text="售后咨询电话" ></asp:ListItem>
                     <asp:ListItem  Value="ServiceAddress" Text="返修地址" ></asp:ListItem>
                     <asp:ListItem  Value="Receiver" Text="返修收货人" ></asp:ListItem>
                     <asp:ListItem  Value="ReceiverTelephone" Text="返修联系方式" ></asp:ListItem>
                     <asp:ListItem  Value="Mobile" Text="电话号码" ></asp:ListItem>
                     <asp:ListItem  Value="Telephone" Text="固定电话" ></asp:ListItem>
                     <asp:ListItem  Value="Fax" Text="传真" ></asp:ListItem>
                     <asp:ListItem  Value="Qq" Text="QQ" ></asp:ListItem>
                     <asp:ListItem  Value="Postcode" Text="邮政编码" ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext"  colspan="3">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem  Value="Id" Text="编号"></asp:ListItem>
                     <asp:ListItem  Value="Name" Text="供应商名称"></asp:ListItem>
                     <asp:ListItem  Value="Account.Id,Account.Name" Text="所属账户"></asp:ListItem>
                     <asp:ListItem  Value="Linkman" Text="联系人"></asp:ListItem>
                     <asp:ListItem  Value="Province" Text="省"></asp:ListItem>
                     <asp:ListItem  Value="City" Text="市"></asp:ListItem>
                     <asp:ListItem  Value="County" Text="镇"></asp:ListItem>
                    <asp:ListItem  Value="Email" Text="电子邮件"></asp:ListItem>
                     <asp:ListItem  Value="Address" Text="办公地址" ></asp:ListItem>
                     <asp:ListItem  Value="WebUrl" Text="官网主页" ></asp:ListItem>
                     <asp:ListItem  Value="BusinessRange" Text="经营范围" ></asp:ListItem>
                     <asp:ListItem  Value="BusinessBrand" Text="经营品牌" ></asp:ListItem>
                     <asp:ListItem  Value="SalesRange" Text="销售区域" ></asp:ListItem>
                     <asp:ListItem  Value="ServiceTelephone" Text="售后咨询电话" ></asp:ListItem>
                     <asp:ListItem  Value="ServiceAddress" Text="返修地址" ></asp:ListItem>
                     <asp:ListItem  Value="Receiver" Text="返修收货人" ></asp:ListItem>
                     <asp:ListItem  Value="ReceiverTelephone" Text="返修联系方式" ></asp:ListItem>
                     <asp:ListItem  Value="Mobile" Text="电话号码" ></asp:ListItem>
                     <asp:ListItem  Value="Telephone" Text="固定电话" ></asp:ListItem>
                     <asp:ListItem  Value="Fax" Text="传真" ></asp:ListItem>
                     <asp:ListItem  Value="Qq" Text="QQ" ></asp:ListItem>
                     <asp:ListItem  Value="Postcode" Text="邮政编码" ></asp:ListItem>
                     <asp:ListItem  Value="Status" Text="供应商状态" ></asp:ListItem>
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
            <td class="mtext" colspan="2">
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                <asp:Button ID="btnSavePersonalization" runat="server" Text="保存" CssClass="btn"  />
                <asp:Button ID="btnClearPersonalization" runat="server" Text="清除" CssClass="btn"  />
            </td>
        </tr>
     </table>
        </div>

        <div class="mainten">
            <table>
                <tr>
                    <td>
                        <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
                    </td>
                    <td >
                        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
                        <asp:Button ID="btnSubmit" runat="server" Text="送审" CssClass="btn" OnClick="btnSubmit_Click" ConfirmBox="Submit" ConfirmMessage="您确定要送审吗" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>
                        <asp:Button ID="btnEffective" runat="server" Text="设置为有效"  CssClass="btn mbtn" OnClick="btnEffective_Click" ConfirmBox="Effective" ConfirmMessage="您确定要设置为有效吗" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button>
                        <asp:Button ID="btnInvalid" runat="server" Text="设置为无效"  CssClass="btn mbtn" onclick="btnInvalid_Click" ConfirmBox="Invalid" ConfirmMessage="您确定要改成为无效吗" ComfirmCheckBoxMessage="你没有选择任何行"/>
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div class="list">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CssClass="table" DataKeyNames="Id,ServiceId" >
            <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
                    <input value='<%#Eval("Id") %>'  id='ckSelect' runat='server' type='checkbox' SubCheckName='selectall' ComfirmValidate='Remove,Submit,Auditting,Invalid' />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
             <%#GetDisplayUserAndStatus((SupplierStatusType)Eval("Status").Convert<int>()) ? string.Format("<a href='update.aspx?id={0}' target='_blank' name='Edit'>编辑</a>", Eval("Id")) : ""%> 
            </ItemTemplate>
        </asp:TemplateField>
     
         <asp:TemplateField HeaderText="资质信息" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                  <%#GetDisplayUserAndStatus((SupplierStatusType)Eval("Status").Convert<int>()) ? string.Format("<a href='/Supplier/Qualification/List.aspx?SupplierId={0}' target='_blank' name='Edit'>资质信息</a>", Eval("Id")) : ""%> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="合同信息" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                 <%#GetDisplayUserAndStatus((SupplierStatusType)Eval("Status").Convert<int>()) ? string.Format("<a href='/Supplier/Contract/List.aspx?SupplierId={0}' target='_blank' name='Edit'>合同</a>", Eval("Id")) : ""%> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="其他证书" ItemStyle-CssClass="center operate">
            <ItemTemplate>
               <%#GetDisplayUserAndStatus((SupplierStatusType)Eval("Status").Convert<int>()) ? string.Format("<a href='/Supplier/Certification/List.aspx?SupplierId={0}' target='_blank' name='Edit'>证书</a>", Eval("Id")) : ""%> 
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
         <asp:TemplateField HeaderText="供应商名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="所属账户"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <a href="/Finance/Account/Detail.aspx?Id=<%#Eval("Account.Id")%>">  <%#Eval("Account.Name")%></a>
              
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="联系人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Linkman")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="省"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Province")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="市"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("City")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="镇"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("County")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="电子邮件"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Email")%>
            </ItemTemplate>
        </asp:TemplateField>  
           <asp:TemplateField HeaderText="供应商状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="维护人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="办公地址"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Address")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="官网主页"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("WebUrl")%>
            </ItemTemplate>
        </asp:TemplateField> 
                      
        <asp:TemplateField HeaderText="经营品牌"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("BusinessBrand")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="经营范围"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("BusinessRange")%>
            </ItemTemplate>
        </asp:TemplateField>    
        <asp:TemplateField HeaderText="销售区域"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("SalesRange")%>
            </ItemTemplate>
        </asp:TemplateField>    
         <asp:TemplateField HeaderText="售后咨询电话"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ServiceTelephone")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="返修地址"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ServiceAddress")%>
            </ItemTemplate>
        </asp:TemplateField>       
        <asp:TemplateField HeaderText="返修收货人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Receiver")%>
            </ItemTemplate>
        </asp:TemplateField>        
        <asp:TemplateField HeaderText="返修联系方式"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ReceiverTelephone")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="电话号码"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Mobile")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="固定电话"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Telephone")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="传真"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Fax")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="QQ"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Qq")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="邮政编码"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Postcode")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Service.Id"  OrderByExp="Id desc" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
     

 </asp:UpdatePanel>

     

     

     <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>

 </asp:Content>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Cloud.Member.Invoice.List" MasterPageFile="~/Main.Master" %>
  <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
 <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>审核列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
               <tr>
                <td  class="font">账户编号</td> 
                <td class="text" colspan="3">
                    <asp:TextBox ID="txtId" runat="server" CssClass="seinput" SearchWhere="Id==@Id" SearchParamterName="Id" ></asp:TextBox>
                </td>
                <td  class="font">注册电话</td> 
                <td class="text" colspan="3">
                    <asp:TextBox ID="txtCompanyPhone" runat="server" CssClass="seinput" SearchWhere="CompanyPhone==@CompanyPhone" SearchParamterName="CompanyPhone" ></asp:TextBox>
                </td>
             
               <tr>
                    <td class="font">
                        显示内容
                    </td>
                    <td colspan="7" class="mtext"> 
                        <asp:CheckBoxList ID="ckSelectList" runat="server" >
                             <asp:ListItem Selected="True"  Value="Account.Id" Text="账户名称"></asp:ListItem>
                             <asp:ListItem Selected="True"  Value="Type" Text="发票类型" ></asp:ListItem>
                             <asp:ListItem Selected="True"  Value="GeneralType" Text="发票类型名称" ></asp:ListItem>
                             <asp:ListItem Selected="True"  Value="Title" Text="发票抬头" ></asp:ListItem>
                             <asp:ListItem Selected="True"  Value="Status" Text="状态名称" ></asp:ListItem>
                             <asp:ListItem Selected="True"  Value="Content" Text="发票类容" ></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
               <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext" colspan="2" >
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                             <asp:ListItem Value="Account.Id" Text="账户名称"></asp:ListItem>
                             <asp:ListItem Value="Type" Text="发票类型" ></asp:ListItem>
                             <asp:ListItem Value="GeneralType" Text="发票类型名称" ></asp:ListItem>
                             <asp:ListItem Value="Title" Text="发票抬头" ></asp:ListItem>
                             <asp:ListItem Value="Status" Text="状态名称" ></asp:ListItem>
                             <asp:ListItem Value="Content" Text="发票类容" ></asp:ListItem>
                          
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
        <div class="list">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="Id">
       <Columns>
             <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
           </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">详情</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <a href='/Finance/Account/Detail.aspx?id=<%#Eval("Account.Id") %>' target="_blank"><%#Eval("Account.Name")%></a>  
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="会员名"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
                 <asp:TemplateField HeaderText="发票类型"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TypeName")%>   
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="发票一般类型"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("GeneralTypeName")%>   
            </ItemTemplate>
        </asp:TemplateField>
        
         <asp:TemplateField HeaderText="发票抬头"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Title")%>   
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="状态名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
               <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="发票类容"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Content")%>   
            </ItemTemplate>
        </asp:TemplateField>
        

         <asp:TemplateField HeaderText="发票类型名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("GeneralTypeName")%>   
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
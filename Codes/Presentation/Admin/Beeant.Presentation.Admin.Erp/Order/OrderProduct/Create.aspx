<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderProduct.Create" MasterPageFile="~/Datum.Master" ValidateRequest="false" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Product" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %> <%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %> <%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>订单明细录入</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
 <div class="edit">
<table class="tb">
           <tr>
                <td colspan="4" class="center">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn" />
                    <input id="btnClose" type="button" value="关闭" class="btn" />
                </td>
            </tr>
        </table>
            <uc2:Message ID="Message1" runat="server" />
 
    
     </div>
      <div id="divProduct" runat="server">

      <div   class="search" >
               <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
    <tr>
         
              <td class="font">名称</td>
            <td class="text">
                  <asp:TextBox ID="txtName" runat="server"  SearchWhere="Name.Contains(@Name)" SearchParamterName="Name"   CssClass="seinput"></asp:TextBox>
            </td>
               <td class="font">类目</td>
            <td class="text">
               <asp:TextBox ID="txtCategoyName" runat="server"  SearchWhere="Categoy.Name.Contains(@CategoyName)" SearchParamterName="CategoyName"   CssClass="seinput"></asp:TextBox>
            </td>
               <td class="font">是否上架</td>
            <td class="text" colspan="3">
                   <asp:DropDownList ID="ddlIsSales" runat="server"  SearchWhere="IsSales==@IsSales" SearchParamterName="IsSales"  SearchPropertyTypeName="IsSales">
                     <asp:ListItem  Value="True"  Text="是" ></asp:ListItem>  
                     <asp:ListItem  Value="False" Text="否"></asp:ListItem>
                  </asp:DropDownList>
            </td>
    </tr>
         <tr>
         
            <td class="font">
                            编号 
                        </td>
                        <td class="text" >
                                <asp:TextBox ID="txtId" runat="server" CssClass="seinput" SearchParamterName="Id" SearchWhere="Id==@Id" SearchPropertyName="Id"></asp:TextBox>
                        </td>
                        <td colspan="7">
                             <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                        </td>
        </tr>
     </table>
        </div>
        <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
       <Columns>
            <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
            <asp:TemplateField ItemStyle-CssClass="center ckbox">
                <HeaderTemplate>
                 <input id="ckSelectAll" type="checkbox" AllCheckName="selectall" checked="checked"  />
                </HeaderTemplate>
                <ItemTemplate>
                   <input value='<%#Eval("IsReturn") %>' id="hfIsReturn" type="hidden" runat="server"  />
                  <input value='' id="hfDescription" type="hidden" runat="server"  />
                      <input value='<%#Eval("Name") %>' id="hfName" type="hidden" runat="server"  />
                   <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall"  checked="True"  />
               </ItemTemplate>
            </asp:TemplateField>        
            <asp:TemplateField HeaderText="商品编号"  ItemStyle-CssClass="left">
                <ItemTemplate>
                    <%#Eval("Id")%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
                <ItemTemplate>
                    <%#Eval("Name")%>              
                </ItemTemplate>
            </asp:TemplateField>
           <asp:TemplateField HeaderText="面价"  ItemStyle-CssClass="left Sequence">
                <ItemTemplate>
                  <%#Eval("Price")%>
                </ItemTemplate>
               
            </asp:TemplateField>

             <asp:TemplateField HeaderText="底价"  ItemStyle-CssClass="left status">
                <ItemTemplate>
                    <%#Eval("Cost")%>   
                </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="数量"  ItemStyle-CssClass="left status">
                <ItemTemplate>
                    <%#Eval("Count")%>   
                </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="下单价"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%#Eval("Cost") %>' id="txtCost" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="下单数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%#Eval("OrderMinCount") %>' id="txtCount" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
                              
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="pgProduct" runat="server" PageSize="20" FromExp="Beeant.Domain.Entities.Product.ProductEntity,Beeant.Domain.Entities"  
     SelectExp="Id,Name,Price,Cost,Count,IsReturn,OrderMinCount,Goods.GoodsDetails.Select(Name,Detail)" OrderByExp="Id desc"   />  
     </div>
     </ContentTemplate>
</asp:UpdatePanel>
 </asp:Content>
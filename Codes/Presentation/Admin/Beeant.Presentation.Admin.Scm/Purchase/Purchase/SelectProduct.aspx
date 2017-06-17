<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="SelectProduct.aspx.cs" Inherits="Beeant.Presentation.Admin.Scm.Purchase.Purchase.SelectProduct" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Wms" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
     
      <link href="/Styles/Style.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="/scripts/Winner/Winner.ClassBase.js"></script>
      <script type="text/javascript" src="/scripts/jquery-1.7.1.min.js"></script>
      <script type="text/javascript" src="/Scripts/Plug/Calendar.js"></script>

</head>
<body>
 <form id="form1" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager1" runat="server" onasyncpostbackerror="ScriptManager1_AsyncPostBackError" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
            <div class="main" style="padding: 0;margin: 0;position:relative; top: 0;">
               <div class="body" style="padding: 0;margin: 0;">
        <div   id="divSearch" class="search" runat="server" >
               <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />

     </table>
        </div>
        <div class="list" id="divProduct">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
                  <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall" checked="checked"   />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>'  type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove, Price" checked="checked"  SerializeName="Id"   />
           </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <a href='/Product/Product/Detail.aspx?id=<%#Eval("Id") %>' target="_blank"><%#Eval("Name")%></a> 
                <input  SerializeName="Name" type="hidden" class="input" value='<%#Eval("Name")%>' />
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类目"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Goods.Category.Name")%>
                  <input  SerializeName="CategoryName" type="hidden" class="input" value='<%#Eval("Goods.Category.Name")%>' />
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="面见"  ItemStyle-CssClass="left status">
            <ItemTemplate>
               <%# DataBinder.Eval(Container.DataItem, "PriceRate", "{0:N2}%")%>
                <input  SerializeName="Price" type="hidden" class="input" value= <%# DataBinder.Eval(Container.DataItem, "Price", "{0:N2}%")%>' />
            </ItemTemplate>
        </asp:TemplateField>
     <asp:TemplateField HeaderText="毛利率"  ItemStyle-CssClass="left status">
            <ItemTemplate>
               <%# DataBinder.Eval(Container.DataItem, "PriceRate", "{0:N2}%")%>
                <input  SerializeName="PriceRate" type="hidden" class="input" value= <%# DataBinder.Eval(Container.DataItem, "PriceRate", "{0:N2}%")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="毛利率警戒线"  ItemStyle-CssClass="left status" ItemStyle-Width="100" >
            <ItemTemplate>               
                <%# DataBinder.Eval(Container.DataItem, "Goods.Category.Rate", "{0:N2}%")%>
                 <input  SerializeName="Rate" type="hidden" class="input" value='<%# DataBinder.Eval(Container.DataItem, "Goods.Category.Rate", "{0:N2}%")%>' />
            </ItemTemplate>
        </asp:TemplateField>
       
             <asp:TemplateField HeaderText="销售状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("IsSalesName")%>
                 <input  SerializeName="IsSalesName" type="hidden" class="input" value='<%#Eval("IsSalesName")%>' />
            </ItemTemplate>
        </asp:TemplateField> 
         
      
                <asp:TemplateField HeaderText="进价"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input SerializeName="Price" value='<%#Eval("Cost") %>' id="txtCost" runat="server" type="text" class="input" style="width: 80px;" />  
                          
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="采购数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input SerializeName="Count" value='1' id="txtCount" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
                              
        </Columns>
     </asp:GridView>
     <uc1:Pager ID="Pager1" runat="server" PageSize="20" FromExp="Beeant.Domain.Entities.Product.ProductEntity,Beeant.Domain.Entities"  
     SelectExp="Id,Name,Goods.Category.Name,Price,Goods.Category.Rate,Cost,IsSales" OrderByExp="Id desc"   />  
            </div>
            </div>
            <div style="text-align: center;">
                <input id="btnSure" type="button" value="确定" class="btn" SerializeSelect='sure' containerId="divProduct" />
                <input id="btnClose" type="button" value="返回" class="btn"  SerializeSelect='cancel'/>
            </div>
                 <uc3:Progress ID="Progress2" runat="server" />
                      <script type="text/javascript" src="/Scripts/Serializator.js"></script>
    <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
    <script type="text/javascript">
        function Init() {
            var serializator = new Serializator();
            serializator.Initialize();
        }
    </script>
     </ContentTemplate>
 </asp:UpdatePanel>
            </form>
      
  </body>
  </html>
<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="SelectProduct.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Stock.SelectProduct" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Wms" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>
<%@ Register TagPrefix="uc10" TagName="GeneralDropDownList" Src="~/Controls/GeneralDropDownList.ascx" %>
<%@ Register src="../../Controls/Wms/StorehouseComboBox.ascx" tagname="StorehouseComboBox" tagprefix="uc4" %>
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
    <tr>
         <td class="font">状态</td>
              <td class="text"   >
                <uc10:GeneralDropDownList ID="ddlSearchStatus" runat="server" ObjectName="Beeant.Domain.Entities.Product.ProductStatus" IsEnum="True" SearchPropertyTypeName="Status" SearchWhere="Status==@Status" SearchParamterName="Status"  />
             </td>
              <td class="font">品牌</td>
            <td class="text">
                  <asp:TextBox ID="txtBrand" runat="server"  SearchWhere="Brand==@Brand" SearchParamterName="Brand"  SearchPropertyTypeName="Brand"  CssClass="seinput"></asp:TextBox>
            </td>
               <td class="font">是否直送</td>
            <td class="text">
               <asp:DropDownList ID="ddlIsDirectDelivery" runat="server"  SearchWhere="IsDirectDelivery==@IsDirectDelivery" SearchParamterName="IsDirectDelivery"  SearchPropertyTypeName="IsDirectDelivery">
                     <asp:ListItem  Value="True"  Text="是" ></asp:ListItem>  
                     <asp:ListItem  Value="False" Text="否"></asp:ListItem>
                  </asp:DropDownList>
            </td>
               <td class="font">是否上架</td>
            <td class="text">
                   <asp:DropDownList ID="ddlIsOnlineSales" runat="server"  SearchWhere="IsOnlineSales==@IsOnlineSales" SearchParamterName="IsOnlineSales"  SearchPropertyTypeName="IsOnlineSales">
                     <asp:ListItem  Value="True"  Text="是" ></asp:ListItem>  
                     <asp:ListItem  Value="False" Text="否"></asp:ListItem>
                  </asp:DropDownList>
            </td>
    </tr>
         <tr>
            <td class="font">处理开始时间</td>
            <td class="text"><asp:TextBox ID="txtBeginStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime>==@BeginStatusTime" SearchParamterName="BeginStatusTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBeginStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">处理新结束时间</td>
            <td  class="text" ><asp:TextBox ID="txtEndStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime<==@EndStatusTime" SearchParamterName="EndStatusTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtEndStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">
                            编号 
                        </td>
                        <td class="text" >
                                <asp:TextBox ID="txtId" runat="server" CssClass="seinput" SearchParamterName="Id" SearchWhere="Id==@Id" SearchPropertyName="Id"></asp:TextBox>
                        </td>
                        <td colspan="2">
                             <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
                        </td>
        </tr>
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
     
        <asp:TemplateField HeaderText="品牌"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Brand")%>
            </ItemTemplate>
        </asp:TemplateField> 
            <asp:TemplateField HeaderText="仓库"  ItemStyle-CssClass="left" >
            <ItemTemplate>
                           <uc4:StorehouseComboBox ID="StorehouseComboBox1" runat="server" />
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="采购数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input SerializeName="Count" value='1' id="txtCount" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField> 
              <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input SerializeName="Remark" value='' id="txtRemark" runat="server" type="text" class="input" style="width: 80px;" />                
            </ItemTemplate>
        </asp:TemplateField>                  
        </Columns>
     </asp:GridView>
 <style type="text/css">
     .main .body .list .table td{ overflow:inherit;}
 </style>
     <uc1:Pager ID="Pager1" runat="server" PageSize="20" FromExp="Beeant.Domain.Entities.Product.ProductEntity,Beeant.Domain.Entities"  
     SelectExp="Id,Name,Goods.Category.Name,Brand" OrderByExp="Id desc"   />  
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
            $("#<%=GridView1.ClientID %>").find("input[BindName='Storehouse.Name']").each(function (index, sender) {
                $(sender).attr("SerializeName", "StorehouseName");
            });
            $("#<%=GridView1.ClientID %>").find("input[BindName='Storehouse.Id']").each(function (index, sender) {
                $(sender).attr("SerializeName", "StorehouseId");
            });
        }
    </script>
     </ContentTemplate>
 </asp:UpdatePanel>
            </form>
      
  </body>
  </html>
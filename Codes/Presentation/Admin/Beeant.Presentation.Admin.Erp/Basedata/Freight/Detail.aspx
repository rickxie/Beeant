<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Freight.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>物流模板详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">名称</td>
            <td class="text"  >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
               <td class="font">包邮利润比例</td>
            <td class="text" >
                <asp:Label ID="lblFreeProfit" runat="server" BindName="FreeProfit"></asp:Label>
             </td>
        </tr>
  
       
        <tr>
          <td class="font">包邮区域</td>
            <td class="text" >
                <asp:Label ID="lblFreeRegion" runat="server" BindName="FreeRegion"></asp:Label>
            </td>
            <td class="font">默认数量</td>
            <td class="text" >
                <asp:Label ID="lblDefaultCount" runat="server" BindName="DefaultCount"></asp:Label>
            </td>
        </tr>
           <tr>
                 <td class="font">默认价格</td>
            <td class="text" >
                <asp:Label ID="lblDefaultPrice" runat="server" BindName="DefaultPrice"></asp:Label>
            </td>
            <td class="font">续重数量</td>
            <td class="text" >
                <asp:Label ID="lblContinueCount" runat="server" BindName="ContinueCount"></asp:Label>
            </td>
           
         </tr>
              <tr>
                 <td class="font">默认价格</td>
            <td class="text" colspan="3" >
                <asp:Label ID="lblContinuePrice" runat="server" BindName="ContinuePrice"></asp:Label>
            </td>
            
         </tr>
         <tr>
           <td class="font">描述</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="lblDescription" runat="server" BindName="Description"></asp:Label>
            </td>
            
        </tr>
       
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
     <div class="subtitle" onclick="SetEntityBody('divFreightDistrict')">运价信息(<span class="count"><%=pgFreightDistrict.DataCount%></span>)</div>
       <div id="divFreightDistrict" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
 
   
           <asp:GridView ID="gvFreightDistrict" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>

         <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="地区"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Region")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="默认数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("DefaultCount")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="默认价格"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("DefaultPrice")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="续费数量"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ContinueCount")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="续费价格"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ContinuePrice")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgFreightDistrict" runat="server" PageSize="10"  SelectExp="Id,Name,Region,DefaultCount,DefaultPrice,ContinueCount,ContinuePrice" 
     FromExp="Beeant.Domain.Entities.Basedata.FreightDistrictEntity,Beeant.Domain.Entities" OrderByExp="Id asc" WhereExp="Freight.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
         

     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
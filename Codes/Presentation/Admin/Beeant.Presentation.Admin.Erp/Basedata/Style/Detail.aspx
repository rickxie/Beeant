<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Style.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>店铺模板详情</title>  
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
             
        </tr>
        <tr>
            <td class="font">路径</td>
            <td class="text"  >
                <asp:Label ID="lblPath" runat="server"  BindName="Path"></asp:Label>
             </td>
              <td class="font">排序</td>
            <td class="text"  >
                <asp:Label ID="lblSequence" runat="server"  BindName="Sequence"></asp:Label>
             </td>
        </tr>
         <tr>
          <td class="font">类型</td>
            <td class="text" >
                <asp:Label ID="lblTypeName" runat="server" BindName="TypeName"></asp:Label>
            </td>
            <td class="font">是否显示</td>
            <td class="text" >
                <asp:Label ID="lblIsShowName" runat="server" BindName="IsShowName"></asp:Label>
            </td>
           
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="lblRemark" runat="server" BindName="Remark"></asp:Label>
            </td>
            
        </tr>
         <tr>
           <td class="font">详情</td>
            <td class="mtext" colspan="3" >
                <asp:Label ID="lblDetail" runat="server" BindName="Detail"></asp:Label>
            </td>
            
        </tr>
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
     
     <div class="subtitle" onclick="SetEntityBody('divWebsitePartner')">网站加盟商信息(<span class="count"><%=pgWebsitePartner.DataCount%></span>)</div>
       <div id="divWebsitePartner" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
 
   
           <asp:GridView ID="gvWebsitePartner" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>

        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
               <a href='/Merchant/Partner/Detail.aspx?id=<%#Eval("Id")%>'><%#Eval("Id")%></a> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="联系人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Linkman")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="固定电话"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Telephone")%>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="手机号码"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Mobile")%>   
            </ItemTemplate>
        </asp:TemplateField>
    
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgWebsitePartner" runat="server" PageSize="10"  
     SelectExp="Id,Name,Linkman,Telephone,Mobile" 
     FromExp="Beeant.Domain.Entities.Merchant.PartnerEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="WebsiteStyle.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
      
      <div class="subtitle" onclick="SetEntityBody('divMobilePartner')">手机加盟商信息(<span class="count"><%=pgMobilePartner.DataCount%></span>)</div>
       <div id="divMobilePartner" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
 
   
           <asp:GridView ID="gvMobilePartner" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>

         
        <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
               <a href='/Merchant/Partner/Detail.aspx?id=<%#Eval("Id")%>'><%#Eval("Id")%></a> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="联系人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Linkman")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="固定电话"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Telephone")%>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="手机号码"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Mobile")%>   
            </ItemTemplate>
        </asp:TemplateField>
    
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgMobilePartner" runat="server" PageSize="10"  
     SelectExp="Id,Name,Linkman,Telephone,Mobile" 
     FromExp="Beeant.Domain.Entities.Merchant.PartnerEntity,Beeant.Domain.Entities" OrderByExp="UpdateTime desc" WhereExp="MobileStyle.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>   

     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
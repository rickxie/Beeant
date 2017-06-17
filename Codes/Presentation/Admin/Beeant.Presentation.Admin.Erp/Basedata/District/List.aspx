<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.District.List" MasterPageFile="~/Main.Master" %>
<%@ Register src="../../Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %> 
<%@ Register TagPrefix="uc4" TagName="GeneralTreeView" Src="~/Controls/GeneralTreeView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>区域列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
       <input type="hidden" id="hfClassId" runat="server"/>
  
         <div class="mainten">
             <div>您当前选中的类：<span id="spName" runat="server"></span></div>
             
              <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
                     <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn" OnClientClick="return confirm('您确定删除吗？');" ></asp:Button>
              <%=string.IsNullOrEmpty(hfClassId.Value) ? "" : "<a href='Update.aspx?Id=" + hfClassId.Value + "' class='btn' target='_blank'>编辑</a>"%>
         
            
           
        </div>

        <div class="list">
                   <uc4:GeneralTreeView ID="tvDistrictTree" runat="server" OnSelectedNodeChanged="tvDistrictTree_SelectedNodeChanged" EntityName="DistrictEntity" />
        </div>
 

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="CreateSelectProduct.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.Order.CreateSelectProduct" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
     
      <link href="<%=Page.GetUrl("PresentationAdminHomeUrl")%>/Styles/Style.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="/scripts/Winner/Winner.ClassBase.js"></script>
      <script type="text/javascript" src="/scripts/Plug/jquery-1.7.1.min.js"></script>
      <script type="text/javascript" src="/Scripts/Plug/WdatePicker/WdatePicker.js"></script>

</head>
<body>
 <form id="form1" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="ScriptManager1" runat="server" onasyncpostbackerror="ScriptManager1_AsyncPostBackError" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
            <div class="main" style="padding: 0;margin: 0;position:relative; top: 0;">
               <div class="body" style="padding: 0;margin: 0;">
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
              <tr>
                  <td class="font">类目</td>
                    <td >
                        <asp:DropDownList ID="ddlCategoryOne" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlCategoryOne_SelectedIndexChanged" >
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlCategoryTow" runat="server"  AutoPostBack="True" 
                            onselectedindexchanged="ddlCategoryTow_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlCategoryTree" runat="server"  >
                        </asp:DropDownList>
                </td>
                <td class="font">产品名称</td>
                <td class="text">
                    <asp:TextBox ID="txtName" runat="server"  SearchWhere="Name.Contains(@Name)" SearchParamterName="Name" CssClass="seinput"></asp:TextBox>
                </td>
                <td class="font">产品编号</td>
                <td class="text">
                    <asp:TextBox ID="txtId" runat="server"  SearchWhere="Id==@Id" SearchParamterName="Id"  SearchPropertyTypeName="Id"  CssClass="seinput"></asp:TextBox>
                </td>
                    <td> <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  /></td>
                </tr>
            </table>
        </div>
  
        <div class="list" id="divProduct">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
            <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
            <asp:TemplateField ItemStyle-CssClass="center ckbox">
                <HeaderTemplate>
                 <input id="ckSelectAll" type="checkbox" AllCheckName="selectall" checked="checked"  />
                </HeaderTemplate>
                <ItemTemplate>
                  <input value='<%#Eval("Id") %>' id="hfProductId" type="hidden" runat="server" SerializeName="ProductId" />
                   <input value='<%#Eval("Id") %>' id="ckSelect"  type="checkbox" SerializeName="Id" SubCheckName="selectall" ComfirmValidate="UpdatePrice" <%#Eval("Count").Convert<int>()>=Eval("OrderMinCount").Convert<int>()?"checked='checked'":""%> />
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
                <asp:TemplateField HeaderText="起购量"  ItemStyle-CssClass="left status">
                <ItemTemplate>
                    <%#Eval("OrderMinCount")%>   
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="是否销售"  ItemStyle-CssClass="left status">
                <ItemTemplate>
                    <%#Eval("IsSalesName")%>   
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="商品数量" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                      <input type="text" runat="server" id="txtCount" value='<%#Eval("OrderMinCount") %>' SerializeName="Count" count='count' style="width: 60px;;"/>
                                </ItemTemplate>
                            </asp:TemplateField>   

        </Columns>
     </asp:GridView>
        </div>        
        <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,Price,Cost,Count,OrderMinCount,IsSales" OrderByExp="Id asc"  />


            </div>
            </div>
            <div style="text-align: center;">
                <input id="btnSure" type="button" value="确定" class="btn" SerializeSelect='sure' containerId="divProduct" />
                <input id="btnClose" type="button" value="返回" class="btn" SerializeSelect='cancel'   />
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
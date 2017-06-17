<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Goods.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>


<%@ Register src="../../Controls/Basedata/TagDropDownList.ascx" tagname="TagDropDownList" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>商品列表</title>  
    <style type="text/css">
        .btn
        {
            height: 21px;
        }
    </style>
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
        <tr>
           <td class="font">
                    类目 
                </td>
                <td class="text">
                    <asp:TextBox ID="txtCategory" runat="server"  SearchWhere="Category.Name==@CategoryName" SearchParamterName="CategoryName" CssClass="seinput"></asp:TextBox>
                </td>
                <td class="font">产品名称</td>
                <td class="text">
                    <asp:TextBox ID="txtName" runat="server"  SearchWhere="Name.Contains(@Name)" SearchParamterName="Name" CssClass="seinput"></asp:TextBox>
                </td>
                <td class="font">产品编号</td>
                <td class="text">
                    <asp:TextBox ID="txtId" runat="server"  SearchWhere="Id==@Id" SearchParamterName="Id"  SearchPropertyTypeName="Id"  CssClass="seinput"></asp:TextBox>
                </td>
                 <td class="font">关联编号</td>
                <td class="mtext">
                    <asp:TextBox ID="txtDataId" runat="server"  SearchWhere="DataId==@DataId" SearchParamterName="DataId"  SearchPropertyTypeName="DataId"  CssClass="seinput"></asp:TextBox>
                </td>
        </tr>
       <tr>
            
              <td class="font">
                标签
            </td>
            <td class="text" colspan="5" >
              <uc5:TagDropDownList ID="ddlTag" runat="server" SearchWhere="Tag.Contains(@Tag)"  SearchParamterName="Tag" CssClass="seinput" />
            </td>
          
              <td class="font">是否销售</td>
              <td class="text"   >
                  <asp:DropDownList ID="ddlIsSales" runat="server"  SearchWhere="IsSales==@IsSales" SearchParamterName="IsSales"  SearchPropertyTypeName="IsSales">
                     <asp:ListItem  Value="True"  Text="销售" ></asp:ListItem>  
                     <asp:ListItem  Value="False" Text="下架"></asp:ListItem>
                  </asp:DropDownList>
             </td>
           
      </tr>

         <tr>
            <td class="font">
                显示内容
            </td>
            <td colspan="7" class="mtext"> 
                <asp:CheckBoxList ID="ckSelectList" runat="server" >
                     <asp:ListItem Value="Id" Text="编号" Selected="True" ></asp:ListItem>
                     <asp:ListItem  Value="Name" Text="名称" Selected="True"></asp:ListItem>
                      <asp:ListItem  Value="Category.Name" Text="类目" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="SalesCount" Text="销售数量"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="VisitCount" Text="访问数量"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="AttentionCount" Text="关注数量"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="DepositRate" Text="定金比率"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsCustom" Text="是否定制"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsReturn" Text="是否支持退货"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="UnusedStatus" Text="不占用库存状态"  Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="IsSales" Text="是否销售"  Selected="True"></asp:ListItem>
                      <asp:ListItem  Value="Tag" Text="标签" ></asp:ListItem>
                     <asp:ListItem  Value="DataId" Text="关联编号" ></asp:ListItem>
                     <asp:ListItem  Value="Description" Text="描述" ></asp:ListItem>
                    <asp:ListItem  Value="PublishTime" Text="发布时间"  ></asp:ListItem> 
                    <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间" ></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            
        </tr>
        <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                     <asp:ListItem Value="Id" Text="编号" Selected="True"></asp:ListItem>
                     <asp:ListItem  Value="PublishTime" Text="发布时间"  ></asp:ListItem>
                     <asp:ListItem  Value="InsertTime" Text="录入时间" ></asp:ListItem>
                     <asp:ListItem  Value="UpdateTime" Text="编辑时间"  ></asp:ListItem>
                </asp:DropDownList>
            </td>
          <td class="font">
                排序方式
            </td>
            <td >
                <asp:RadioButtonList ID="rdOrderbyType" runat="server" RepeatDirection="Horizontal">
                     <asp:ListItem  Value="asc" Text="升序" ></asp:ListItem>
                     <asp:ListItem  Value="desc" Text="降序" Selected="True" ></asp:ListItem>
                </asp:RadioButtonList>
               
            </td>
            <td colspan="4">
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
                <td><a href="Add.aspx" name="Add" target="_blank"class="btn"  >添加</a></td>
                <td><asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button></td>
                <td>
                    <asp:Button ID="btnUnSales" runat="server" Text="下架" CssClass="btn" OnClick="btnUnSales_Click"   ConfirmBox="UnSales" ConfirmMessage="您保存本次修改吗？" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button> 
                </td>
                  <td>
                    <asp:Button ID="btnSales" runat="server" Text="上架" CssClass="btn" OnClick="btnSales_Click"   ConfirmBox="Sales" ConfirmMessage="您保存本次修改吗？" ComfirmCheckBoxMessage="你没有选择任何行"></asp:Button> 
                </td>
            </tr>
            </table>
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove,Sales,UnSales"  />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Entity">详情</a>
         

            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <%#Eval("IsSales").Convert<bool>() ?  "" : string.Format("<a href='update.aspx?id={0}' target='_blank' name='Edit'>编辑</a>", Eval("Id"))%> 
           
                 &nbsp;<a href='/Product/Inquery/add.aspx?goodsid=<%#Eval("Id") %>' target="_blank" name="Inquery">问答录入</a>
             </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="平台" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='javascript:void(0);' GoodsId='<%#Eval("Id") %>'  name="Platform">平台</a>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="属性设置" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='javascript:void(0);' GoodsId='<%#Eval("Id") %>'  name="GoodsProperty">属性设置</a>
                <div></div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="详情页" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='javascript:void(0);' GoodsId='<%#Eval("Id") %>'  name="GoodsDetail">详情页</a>
                <div></div>
            </ItemTemplate>
        </asp:TemplateField>
     
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="类目"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Category.Name")%>
            </ItemTemplate>
        </asp:TemplateField>


         <asp:TemplateField HeaderText="销售数量"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("SalesCount")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="访问数量"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("VisitCount")%>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="关注数量"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("AttentionCount")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="定金比率"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("PercentageOfDepositRate")%>
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="是否支持退货"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsReturnName")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="不占用库存状态"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("UnusedStatusName")%> 
            </ItemTemplate>
      </asp:TemplateField>
            <asp:TemplateField HeaderText="是否销售"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsSalesName")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="标签" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("Tag")%>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="关联编号"  ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("DataId")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="发布时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("PublishTime", "{0:yyyy-MM-dd HH:mm}")%>
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

  <script type="text/javascript" src="/Scripts/Platform.js"></script>
  <script type="text/javascript">
      function Init() {
          var platform = new Platform();
          platform.Initialize();
          $(document).find("a[name='GoodsDetail']").click(function () {
              loadProduct(this, "/Product/GoodsDetail/Add.aspx");
          });
          $(document).find("a[name='GoodsProperty']").click(function () {
              loadProduct(this, "/Product/GoodsProperty/list.aspx");
          });
          function loadProduct(sender,url) {
              if ($(sender).next().attr("IsLoad") == "true")
                  return;
              $.ajax({
                  type: "Post",
                  url: "/Ajax/Product/ProductByGoods.aspx?goodsId=" + $(sender).attr("GoodsId"),
                  async: false,
                  dataType: "text",
                  success: function (mess) {
                      var infos = eval(mess);
                      $(infos).each(function (index, obj) {
                          var html = "<a target='_blank' href='" + url + "?productId=" + obj.Value + "&goodsid=" + $(sender).attr("GoodsId") + "'>" + obj.Text + "</a></br/>";
                          $(sender).next().html($(sender).next().html() + html);
                      });
                      $(sender).next().attr("IsLoad", "true");
                      $(sender).hide();
                  },
                  error: function () {
                      alert("操作异常");
                  }
              });
          }
      }
  </script>
     

 </asp:Content>
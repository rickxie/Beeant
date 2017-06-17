<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Stock.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>进出库列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
        <tr>
                <td class="font">
                    状态 
                </td>
                <td class="mtext" colspan="3">
                    <asp:CheckBoxList ID="ckStatusList" runat="server">
                    </asp:CheckBoxList>
                </td>
                <td class="font">
                    级别 
                </td>
                <td class="text"  colspan="3">
                    <asp:CheckBoxList ID="ckLevelList" runat="server" SearchParamterName="Level" 
                        SearchPropertyTypeName="Level" SearchWhere="Level==@Level">
                    </asp:CheckBoxList>
                </td>
          </tr>
          <tr>
              <td class="font">
                    采购单编号 
                </td>
                <td class="mtext" colspan="3">

                <asp:TextBox ID="txtPurchaseId" runat="server"  SearchWhere="Purchase.Id==@PurchaseId" SearchParamterName="PurchaseId" SearchPropertyTypeName="PurchaseId" CssClass="seinput"></asp:TextBox>
           
            </td>
              
              <td class="font">
                    订单编号 
                </td>
                <td class="mtext" colspan="3">

                <asp:TextBox ID="txtOrderId" runat="server"  SearchWhere="Order.Id==@OrderId" SearchParamterName="OrderId" SearchPropertyTypeName="OrderId" CssClass="seinput"></asp:TextBox>
           
            </td>
              

          </tr>
           <tr>
            <td class="font">
                    显示内容 
                </td>
                <td class="mtext" colspan="7">
                    <asp:CheckBoxList ID="ckSelectList" runat="server">
                        <asp:ListItem Selected="True" Text="编号" Value="Id"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="采购单编号" Value="Purchase.Id"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="订单编号" Value="Order.Id"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="类型" Value="Type"></asp:ListItem>
                        <asp:ListItem Text="备注" Value="Remark"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="所属人" Value="User.RealName"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="状态" Value="Status"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="处理时间" Value="StatusTime"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="处理等级" Value="Level"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="提交人" Value="Submit.RealName"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="录入时间" Value="InsertTime"></asp:ListItem>
                        <asp:ListItem Text="编辑时间" Value="UpdateTime"></asp:ListItem>
                    </asp:CheckBoxList>
                </td>
               
       
        </tr>
         <tr>
            <td class="font">
                排序
            </td>
            <td class="mtext"  colspan="3">
                <asp:DropDownList ID="ddlOrderbyList" runat="server">
                    <asp:ListItem Text="编号" Value="Id"></asp:ListItem>
                    <asp:ListItem Text="采购单编号" Value="Purchase.Id"></asp:ListItem>
                    <asp:ListItem Text="订单编号" Value="Order.Id"></asp:ListItem>
                    <asp:ListItem Text="类型" Value="Type"></asp:ListItem>
                    <asp:ListItem Text="备注" Value="Remark"></asp:ListItem>
                    <asp:ListItem Text="所属人" Value="User.RealName"></asp:ListItem>
                    <asp:ListItem Text="状态" Value="Status"></asp:ListItem>
                    <asp:ListItem Text="处理时间" Value="StatusTime"></asp:ListItem>
                    <asp:ListItem Text="处理等级" Value="Level"></asp:ListItem>
                    <asp:ListItem Text="提交人姓名" Value="Submit.RealName"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="录入时间" Value="InsertTime"></asp:ListItem>
                    <asp:ListItem Text="编辑时间" Value="UpdateTime"></asp:ListItem>
                </asp:DropDownList>
            </td>
            
             <td class="font">
                 排序方式 
             </td>
             <td>
                 <asp:RadioButtonList ID="rdOrderbyType" runat="server" 
                     RepeatDirection="Horizontal">
                     <asp:ListItem Text="升序" Value="asc"></asp:ListItem>
                     <asp:ListItem Selected="True" Text="降序" Value="desc"></asp:ListItem>
                 </asp:RadioButtonList>
             </td>
             <td colspan="3">
                 <asp:Button ID="btnSearch" runat="server" CssClass="btn" Text="搜索" />
                 <asp:Button ID="btnSavePersonalization" runat="server" CssClass="btn" 
                     Text="保存" />
                 <asp:Button ID="btnClearPersonalization" runat="server" CssClass="btn" 
                     Text="清除" />
             </td>
            
        </tr>
     </table>
        </div>

        <div class="mainten">
         <a href="Add.aspx" name="Add" target="_blank"class="btn" >添加</a>
         <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn"></asp:Button>
        
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
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove"  />
           </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="详情" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='Detail.aspx?id=<%#Eval("Id") %>' target="_blank" name="Entity">详情</a>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='update.aspx?id=<%#Eval("Id") %>' target="_blank" name="Edit">编辑</a>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="处理" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='handle.aspx?id=<%#Eval("Id") %>' target="_blank" name="Handle">处理</a>
            </ItemTemplate>
        </asp:TemplateField>
              <asp:TemplateField HeaderText="出入库明细" ItemStyle-CssClass="center operate">
            <ItemTemplate>
                <a href='../StockItem/list.aspx?Stockid=<%#Eval("Id") %>' target="_blank" name="StockItem" >出入库明细</a>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center xlstext">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>

  
         <asp:TemplateField HeaderText="采购单编号"  ItemStyle-CssClass="right xlsfloat">
            <ItemTemplate>
        <span style='<%#Eval("Purchase.Id")!=null && Eval("Purchase.Id").ToString()!="0" ? "":"display:none" %>' >
            <a href='/Purchase/Purchase/Detail.aspx?id=<%#Eval("Purchase.Id") %>' target="_blank" ><%#Eval("Purchase.Id")%></a>
         </span>  
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="订单编号"  ItemStyle-CssClass="right xlsfloat">
            <ItemTemplate>
            <%#Eval("Order.Id")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="类型"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="操作人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="所属人"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="处理时间"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("StatusTime")%>
            </ItemTemplate>
        </asp:TemplateField>
 
        <asp:TemplateField HeaderText="提交人"  ItemStyle-CssClass="left Sequence">
            <ItemTemplate>
                <%#Eval("Submit.RealName")%>
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
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id" OrderByExp="Id desc" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

     

     

     

 </asp:Content>
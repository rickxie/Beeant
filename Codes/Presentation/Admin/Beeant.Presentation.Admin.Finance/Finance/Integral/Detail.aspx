<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Integral.Detail" MasterPageFile="~/Main.Master" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>积分申请详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
      <a href='add.aspx?id=<%=RequestId%>'  name="Add">新增</a>
   <a href='update.aspx?id=<%=RequestId%>'  name="Edit">编辑</a>
   <a href='handle.aspx?id=<%=RequestId%>'  name="Handle">处理</a>
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
            <tr>
             <td class="font">积分</td>
            <td class="text" >
                  <asp:Label ID="txtAmount" runat="server" Text=""  BindName="Amount"></asp:Label>
             </td>
              <td class="font">账户</td>
            <td class="text">
                <a href="/Finance/Account/Detail.aspx?Id=" id="hfAccountId" runat="server" BindName="Account.Id"> <asp:Label ID="lblAccountName" runat="server" Text=""  BindName="Account.Name"></asp:Label></a>
            </td>
        </tr>  

      
  
         <tr>
           <td class="font">抬头</td>
            <td class="mtext" colspan="3">
                <asp:Label ID="lblTitle" runat="server" Text=""  BindName="Title"></asp:Label>
            </td>
            </tr>
   
       <tr>
            <td class="font">状态</td>
            <td class="text" >
                 <asp:Label ID="lblStatusName" runat="server" Text=""  BindName="StatusName"></asp:Label>
            </td>
            <td class="font">状态更新时间</td>
            <td class="text" >
                 <asp:Label ID="lblStatusTime" runat="server" Text=""  BindName="StatusTime"></asp:Label>
            </td>
        </tr>
         <tr>
          <td class="font">级别</td>
            <td class="text">
                 <asp:Label ID="lblLevel" runat="server" Text=""  BindName="Level"></asp:Label>
            </td>
             <td class="font">所属人</td>
            <td class="text">
                 <asp:Label ID="lblUserRealName" runat="server" Text=""  BindName="User.RealName"></asp:Label>
            </td>
        </tr>
         <tr>
          <td class="font">提交人</td>
            <td class="text">
                 <asp:Label ID="lblSubmitRealName" runat="server" Text=""  BindName="Submit.RealName"></asp:Label>
            </td>
             <td class="font">提交时间</td>
            <td class="text">
                 <asp:Label ID="lblInsertTime" runat="server" Text=""  BindName="InsertTime"></asp:Label>
            </td>
        </tr>
         
         <tr>
           <td class="font">备注</td>
            <td class="mtext" colspan="3" >
                    <asp:Label ID="lblRemark" runat="server" Text=""  BindName="Remark"></asp:Label>
            </td>
        </tr>
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
   
    <div class="subtitle" onclick="SetEntityBody('divHistory')">流程详情记录(<span class="count"><%=pgHistory.DataCount%></span>)</div>
       <div id="divHistory" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="text"><asp:TextBox ID="txtBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>==@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="text"><asp:TextBox ID="txtEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<==@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
              <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td >
                <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>

      <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" CssClass="table"  >
       <Columns>
        <asp:TemplateField HeaderText="步骤"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                第<%#pgHistory.DataCount-pgHistory.PageIndex*pgHistory.PageSize-Index%>步
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="级别"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("LevelName")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="转发人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("ToUser.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
       <asp:TemplateField HeaderText="当前操作人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("HandleUser.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Remark")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="业务信息"  ItemStyle-CssClass="center">
            <ItemTemplate>
            <a href="javascript:void(0);" onclick="SetEntityBody('divHistory<%#Index%>')">业务信息</a> 
            <div id='divHistory<%#Index++%>' style="display: none;">
                <%#Eval("WebDataEntity")%>
            </div>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>


     <uc1:Pager ID="pgHistory" runat="server" PageSize="10"  SelectExp="Id,StatusName,DataEntity,ToUser.RealName,HandleUser.RealName,LevelName,Remark,InsertTime" FromExp="HistoryEntity" OrderByExp="UpdateTime desc" WhereExp="DataId==@Id && FlowId==@FlowId" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>

       

     <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>
 </asp:Content>
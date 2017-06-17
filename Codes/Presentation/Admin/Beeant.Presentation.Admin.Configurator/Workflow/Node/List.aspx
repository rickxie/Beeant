<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Workflow.Node.List" MasterPageFile="~/Main.Master" %>
 <%@ Register src="/Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>
 <%@ Register src="/Controls/DataSearch.ascx" tagname="DataSearch" tagprefix="uc2" %>
<%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
<%@ Register src="/Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc4" %>  
<%@ Register TagPrefix="uc5" TagName="Message" Src="~/Controls/Message.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>节点列表</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
<div id="Edit" class="edit">
      <input type="button" id="Hide" class="btn" value="隐藏"/>
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="text"><input id="txtName" runat="server" type="text" class="input"  BindName="Name" SaveName="Name"  /></td>
             <td class="font">昵称</td>
            <td class="text"><input id="txtNickname" runat="server" type="text" class="input"  BindName="Nickname" SaveName="Nickname"  /></td>
        </tr>
         <tr>
            <td class="font">审核组</td>
            <td class="text" >
                <uc4:GeneralDropDownList ID="ddlAuditor" runat="server" ObjectName="AuditorEntity"  BindName="Auditor.Id" SaveName="Auditor.Id" />
            </td>
           <td class="font">工作流</td>
            <td class="text">
                <uc4:GeneralDropDownList ID="ddlFlow" runat="server" ObjectName="FlowEntity"  BindName="Flow.Id" SaveName="Flow.Id" />
            </td>
        </tr>
          <tr>
            <td class="font">是否同组审批</td>
            <td class="text">
                <asp:CheckBox ID="ckIsGroup" runat="server"  BindName="IsGroup" SaveName="IsGroup" /></td>
            <td class="font">超时时间</td>
            <td class="text" ><input id="txtTimeout" runat="server" type="text" class="input"  BindName="Timeout" SaveName="Timeout"  /></td>
        </tr>
       <tr>
            <td class="font">分配规则</td>
            <td class="text" >
                 <uc4:GeneralDropDownList ID="ddlAssignType" runat="server" ObjectName="Beeant.Domain.Entities.Workflow.NodeAssignType" IsEnum="True"  BindName="AssignType" SaveName="AssignType" />
            </td>
                 <td class="font">条件类型</td>
            <td class="text" >
                <uc4:GeneralDropDownList ID="ddlConditionType" runat="server" ObjectName="Beeant.Domain.Entities.Workflow.ConditionType" IsEnum="True"  BindName="ConditionType" SaveName="ConditionType" />
            </td>
        </tr>
            <tr>
            <td class="font">节点类型</td>
            <td class="text" >
                 <uc4:GeneralDropDownList ID="ddlNodeType" runat="server" ObjectName="Beeant.Domain.Entities.Workflow.NodeType" IsEnum="True"  BindName="NodeType" SaveName="NodeType" />
            </td>
               <td class="font">排序</td>
            <td class="text" ><input id="txtSequence" runat="server" type="text" class="input"  BindName="Sequence" SaveName="Sequence"  /></td>
                  
          
        </tr>
          <tr>
            <td class="font">通过节点名</td>
            <td class="text" >
                <input id="txtPassName" runat="server" type="text" class="input"  BindName="PassName" SaveName="PassName"  />
            </td>
               <td class="font">拒绝节点名</td>
            <td class="text" ><input id="txtRejectName" runat="server" type="text" class="input"  BindName="RejectName" SaveName="RejectName"  /></td>
                  
          
        </tr>
       <tr>              
          
           <td class="font">消息类型</td>
            <td class="text"  >
                   <asp:CheckBoxList ID="ckMessageType" runat="server" ></asp:CheckBoxList>
            </td>
           <td class="font">消息标题</td>
            <td class="mtext" colspan="3" ><input id="txtMessageTitle" runat="server" class="input long"  type="text"  BindName="MessageTitle" SaveName="MessageTitle"  /> </td>
        </tr>
          <tr>
         <td class="font">默认消息模板</td>
            <td class="mtext" colspan="3" >
                <textarea id="txtDefaultMessage" runat="server"  type="text"  BindName="DefaultMessage" SaveName="DefaultMessage"  /> 
            </td>
        </tr>
          <tr>
         <td class="font">邮件消息模板</td>
            <td class="mtext" colspan="3" >
                <textarea id="txtEmailMessage" runat="server"  type="text"  BindName="EmailMessage" SaveName="EmailMessage"  /> 
            </td>
        </tr>
          <tr>
         <td class="font">手机消息模板</td>
            <td class="mtext" colspan="3" >
                <textarea id="txtMobileMessage" runat="server"  type="text"  BindName="MobileMessage" SaveName="MobileMessage"  /> 
            </td>
        </tr>
         <tr>              
         <td class="font">条件比较方法</td>
            <td class="mtext" colspan="3" ><input id="txtConditionMethod" runat="server" class="input long"  type="text"  BindName="ConditionMethod" SaveName="ConditionMethod"  /> </td>
        </tr>
         <tr>              
         <td class="font">执行前方法</td>
            <td class="mtext" colspan="3" ><input id="txtBeforeMethod" runat="server" class="input long"  type="text"  BindName="BeforeMethod" SaveName="BeforeMethod"  /> </td>
        </tr>
          <tr>              
         <td class="font">执行后方法</td>
            <td class="mtext" colspan="3" ><input id="txtAfterMethod" runat="server" class="input long"  type="text"  BindName="BeforeMethod" SaveName="BeforeMethod"  /> </td>
        </tr>
        <tr>              
         <td class="font">业务状态名称</td>
            <td class="text" ><input id="txtStatusName" runat="server" class="input long"  type="text"  BindName="StatusName" SaveName="StatusName"  /> </td>
            <td class="font">业务状态值</td>
            <td class="text" ><input id="txtStatusValue" runat="server" class="input long"  type="text"  BindName="StatusValue" SaveName="StatusValue"  /> </td>
        </tr>
        <tr> 
          
         <td class="font">备注</td>
            <td class="mtext" colspan="3" ><input id="txtRemark" runat="server" class="input long"  type="text"  BindName="Remark" SaveName="Remark"  /> </td>
        </tr>
      
           
      
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   /></td>
        </tr>
    </table>
     <uc5:Message ID="Message1" runat="server" />
 <input id="IdControl" type="hidden" runat="server" />
</div>
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
        <tr>
                <uc2:DataSearch ID="DataSearch1" runat="server" />
          
            <tr>
                <td class="font">工作流
                </td>
                <td colspan="7"><uc4:GeneralDropDownList ID="ddlSearchFlow" runat="server" ObjectName="FlowEntity" SearchWhere="Flow.Id==@FlowId" SearchParamterName="FlowId" /></td>
            </tr>
        </tr>
     </table>
        </div>

        <div class="mainten">
                <a href='javascript:void(0);' id="Add" class="btn" >添加</a>
        <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn" ></asp:Button>
        <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  />
        </div>

        <div class="list">
              <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
         <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall" ComfirmValidate="Remove"  />
            </HeaderTemplate>
            <ItemTemplate>
               <input value='<%#Eval("Id") %>' id="ckSelect" runat="server" type="checkbox" SubCheckName="selectall" ComfirmValidate="Remove" />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑" ItemStyle-CssClass="center operate">
            <ItemTemplate>
            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Modify" CommandArgument='<%#Eval("Id") %>'>编辑</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="编辑属性" ItemStyle-CssClass="center loperate">
            <ItemTemplate>
                <a href='/Workflow/Property/list.aspx?nodeid=<%#Eval("Id") %>' target="_blank">编辑属性</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="编辑条件" ItemStyle-CssClass="center operate">
            <ItemTemplate>
              <a href='/Workflow/Condition/list.aspx?nodeid=<%#Eval("Id") %>' target="_blank">编辑条件</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="名称"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
               <asp:TemplateField HeaderText="昵称"  ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Nickname")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="工作流" ItemStyle-CssClass="left name">
            <ItemTemplate>
                <%#Eval("Flow.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="审批组" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Auditor.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="是否同组审批" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsGroupName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="分配规则" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("AssignTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="条件类型" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("ConditionTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="通过节点名称" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("PassName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="拒绝节点名称" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("RejectName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="节点类型" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("NodeTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="消息类型" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("MessageTypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
           
           <asp:TemplateField HeaderText="排序" ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Sequence")%>
            </ItemTemplate>
        </asp:TemplateField>
          
         <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
            <ItemTemplate>
                <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
     </asp:GridView>
        </div>
     <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Name,Nickname,Flow.Name,Auditor.Name,AssignType,ConditionType,PassName,RejectName,NodeType,MessageType,Sequence,InsertTime" FromExp="NodeEntity" />

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
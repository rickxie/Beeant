<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Configurator.Authority.Menu.List" MasterPageFile="~/Main.Master" %>

  <%@ Register src="/Controls/Progress.ascx" tagname="Progress" tagprefix="uc3" %>
 <%@ Register src="/Controls/Authority/MenuTreeView.ascx" tagname="Menu" tagprefix="uc4" %>
 <%@ Register src="/Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc1" %> 
 <%@ Register TagPrefix="uc5" TagName="Message" Src="/Controls/Message.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
      <title>菜单列表</title> 
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <div id="Edit" class="edit">
             <input type="button" id="Hide" class="btn" value="隐藏"/>
               <input type="hidden" id="hfMenuId" runat="server"/>
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="text"><input id="txtName" runat="server" type="text" class="input"  BindName="Name" SaveName="Name"  /></td>
            <td class="font">菜单类型</td>
            <td class="text">
                
                <uc1:GeneralDropDownList ID="ddlEditSubsystem" runat="server" ObjectName="SubsystemEntity" BindName="Subsystem.Id" SaveName="Subsystem.Id" AutoPostBack="True" OnSelectedIndexChanged="Subsystem_SelectedIndexChanged" />
                
            </td>
        </tr>
        <tr>
            <td class="font">是否新窗口打开</td>
            <td class="text"><asp:CheckBox ID="ckIsBlank" runat="server" BindName="IsBlank" SaveName="IsBlank" ></asp:CheckBox></td>
         <td class="font">排序</td>
            <td class="text" ><input id="txtSequence" runat="server"  type="text" class="input"  BindName="Sequence" SaveName="Sequence" DefaultValue="1" value="1" /> </td>
        </tr>
       
          <tr>
            <td class="font">是否显示</td>
            <td class="mtext" colspan="3"><asp:CheckBox ID="ckIsShow" runat="server" BindName="IsShow" SaveName="IsShow" Checked="True" DefaultValue="True" ></asp:CheckBox></td>
          </tr>
         <tr>
         <td class="font">连接地址</td>
            <td class="mtext" colspan="3" ><input id="txtUrl" runat="server" class="input long"  type="text"  BindName="Url" SaveName="Url"  /> </td>
        </tr>
       
         <tr>
             <td colspan="4" class="text">
    <table class="intb">
        <tr>
         <td class="infont">父级菜单</td>
            <td class="intext">
                <uc4:Menu ID="tvParentMenu" runat="server" OnSelectedNodeChanged="tvParentMenu_SelectedNodeChanged" IsShowNone="True" />
            </td>
             <td class="infont">已经选择</td>
              <td class="intext">
                  <asp:Label ID="lblParentName" runat="server"  BindName="Parent.Name" Text=""></asp:Label>
                 <input id="hfParentId" type="hidden" runat="server"  BindName="Parent.Id"  SaveName="Parent.Id"/>   
                 
               </td>
               </tr>
    </table>
             </td>
           
        </tr>
  
         <tr>
            <td class="font">备注</td>
            <td class="mtext" colspan="3">
                <input id="txtRemark" runat="server"  type="text" class="input long"  BindName="Remark" SaveName="Remark"  />
            </td>
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
            <td class="font">菜单类型</td>
            <td class="mtext">
                <uc1:GeneralDropDownList ID="ddlSearchSubsystem" runat="server" ObjectName="SubsystemEntity" SearchWhere="Subsystem.Id==@SubsystemId" SearchParamterName="SubsystemId" />
            </td>
  <td> <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="btn"  /></td>
        </tr>
     </table>
        </div>

         <div class="mainten">
              <a href="javascript:void(0);" id="Add"  class="btn" style="margin-top: 0">添加</a>
              <asp:Button ID="btnRemove" runat="server" Text="删除" CssClass="btn" ></asp:Button>
             
               <asp:Button ID="btnModify" runat="server" Text="编辑" CssClass="btn" onclick="btnModify_Click" Visible="False"></asp:Button>
    
          <%=string.IsNullOrEmpty(hfMenuId.Value) ? "" : "<a href='../Ability/list.aspx?menuid=" + hfMenuId.Value + "' class='btn' target='_blank'>功能</a>"%>
           <%=string.IsNullOrEmpty(hfMenuId.Value) ? "" : "<a href='../Resource/list.aspx?menuid=" + hfMenuId.Value + "' class='btn' target='_blank'>资源</a>"%>
                
        </div>

        <div class="list">
                   <uc4:Menu ID="tvMenuList" runat="server" OnSelectedNodeChanged="tvMenuList_SelectedNodeChanged"  />
        </div>
 

     <uc3:Progress ID="Progress1" runat="server" />
     </ContentTemplate>
 </asp:UpdatePanel>

 </asp:Content>
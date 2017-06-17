<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="SelectOrder.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payin.SelectOrder" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="~/Controls/Progress.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>
<%@ Register TagPrefix="uc2" TagName="DataSearch" Src="~/Controls/DataSearch.ascx" %>

<%@ Register TagPrefix="uc5" TagName="LevelCheckBoxList" Src="~/Controls/Workflow/LevelCheckBoxList.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc4" TagName="StatusCheckBoxList" Src="~/Controls/Workflow/StatusCheckBoxList.ascx" %>
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
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">
               <uc2:DataSearch ID="DataSearch1" runat="server" />
  
         <tr>
            <td class="font">处理开始时间</td>
            <td class="text"><asp:TextBox ID="txtBeginStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime>==@BeginStatusTime" SearchParamterName="BeginStatusTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBeginStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
             <td class="font">处理新结束时间</td>
            <td  class="text" ><asp:TextBox ID="txtEndStatusTime" runat="server" CssClass="seinput"  SearchWhere="StatusTime<==@EndStatusTime" SearchParamterName="EndStatusTime"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndStatusTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
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
  
        <div class="list" id="divGridView">
          <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="tblist"  >
              
       <Columns>
           <asp:BoundField  HeaderText="序号" ItemStyle-CssClass="sequence"/>
        <asp:TemplateField ItemStyle-CssClass="center ckbox">
            <HeaderTemplate>
             <input id="ckSelectAll" type="checkbox" AllCheckName="selectall" checked="checked" />
            </HeaderTemplate>
            <ItemTemplate>
                    <input value='<%#Eval("StatusName") %>' id="hfStatusName" type="hidden" SerializeName="StatusName" />
                    <input value='<%#Eval("Level") %>' id="hfLevel" type="hidden" SerializeName="Level" />
                    <input value='<%#Eval("User.RealName") %>' id="hfUserRealName" type="hidden" SerializeName="UserName" />
                    <input value='<%#Eval("Submit.RealName") %>' id="hfSubmitRealName" type="hidden"  SerializeName="SubmitName" />
                    <input value='<%#Eval("Amount") %>' id="hfAmount" type="hidden" runat="server" SerializeName="TotalAmount" />
                    <input value='<%#Eval("ReceivedAmount") %>' id="hfReceivedAmount" type="hidden" SerializeName="ReceivedAmount" />
                    <input value='<%#Eval("InsertTime", "{0:yyyy-MM-dd HH:mm}")%>' id="hfInsertTime" type="hidden"  SerializeName="InsertTime" />
               <input value='<%#Eval("Id") %>' id="ckSelect"  type="checkbox" <%# Convert.ToDouble(Eval("Amount").ToString())-Convert.ToDouble(Eval("ReceivedAmount").ToString())>0?"checked='checked'":"" %>  SubCheckName="selectall" ComfirmValidate="Remove, Price" SerializeName="Id" />
           </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="订单编号"  ItemStyle-CssClass="left ">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField>       
    
  
        <asp:TemplateField HeaderText="状态"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("StatusName")%>
            </ItemTemplate>
        </asp:TemplateField>  

        <asp:TemplateField HeaderText="级别"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Level")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="所属人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("User.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="提交人"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("Submit.RealName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="提交时间"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("InsertTime", "{0:yyyy-MM-dd HH:mm}")%>
            </ItemTemplate>
        </asp:TemplateField>   

        <asp:TemplateField HeaderText="应收金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("TotalAmount")%>
            </ItemTemplate>
        </asp:TemplateField>  
        <asp:TemplateField HeaderText="实收金额"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <%#Eval("PayAmount")%>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="本次付款"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input value='<%# Convert.ToDouble(Eval("TotalAmount").ToString())-Convert.ToDouble(Eval("PayAmount").ToString()) %>' id="txtAmount" runat="server" type="text" class="input"  SerializeName="Amount" />                
            </ItemTemplate>
        </asp:TemplateField> 
          <asp:TemplateField HeaderText="备注"  ItemStyle-CssClass="left">
            <ItemTemplate>
                <input id="txtRemark" runat="server" type="text" class="input" SerializeName="Remark"/>                
            </ItemTemplate>
        </asp:TemplateField>                          
        </Columns>
     </asp:GridView>
        </div>        
        <uc1:Pager ID="Pager1" runat="server" PageSize="10"   SelectExp="Id,Status,Level,User.RealName,Submit.RealName,InsertTime,TotalAmount,PayAmount" OrderByExp="Id asc"  />


            </div>
            </div>
            <div style="text-align: center;">
                <input id="btnSure" type="button" value="确定" class="btn" SerializeSelect='sure' containerId="divGridView" />
                <input id="btnClose" type="button" value="返回" class="btn"  SerializeSelect='cancel'/>
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
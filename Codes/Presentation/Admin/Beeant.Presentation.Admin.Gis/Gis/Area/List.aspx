<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Beeant.Presentation.Admin.Gis.Gis.Area.List" MasterPageFile="~/Main.Master" %>
<%@ Import Namespace="Component.Extension" %>
<%@ Register src="/Controls/GeneralDropDownList.ascx" tagname="GeneralDropDownList" tagprefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>地图区域</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

     
 
        <div id="divSearch" class="search" runat="server" >
           <table class="tb">

                
        <tr>
        
            <td class="font">
                城市
            </td>
            <td class="mtext" >
               <uc10:GeneralDropDownList ID="ddlCity" runat="server" DataValueField="Name" ObjectName="CityEntity" />
                <asp:Button ID="btnImport" runat="server" Text="导入线上地图" CssClass="lmbtn" Width="120" OnClick="btnImport_Click" OnClientClick="return confirm('您确定要导入地图吗，这会清除您真正修改的地图?');" />
                <asp:Button ID="btnPublish" runat="server" Text="发布" CssClass="btn" OnClick="btnPublish_Click" OnClientClick="return confirm('您确定要发布地图吗？');" />
                <span style="color: #ff0000">地图发布后将在5小单内生效</span>
            </td>
            </td>
            <td class="font">
                颜色
            </td>
            <td class="mtext" >
               <input id="txtColor" type="text" value="#FF0000" readonly="readonly" style="width: 60px; background: #FF0000;"/>
            </td>
              
        </tr>
         <tr>
              <td class="font">
                地址检查
            </td>
            <td class="mtext" colspan="3">
               <input id="txtAddress" type="text" value="<%=Address %>" class="input" style="width: 300px;" />
                <input id="btnPantoAddress" type="button" class="btn" value="定位"/>
                <input type="checkbox" id="ckIsStartWith"/>
                <label for="ckIsStartWith">是否前置匹配</label>
            </td>
          
         </tr>
           <tr>
                       <td class="font">
                匹配区域
            </td>
            <td class="mtext" colspan="3">
                <span id="spAreas"></span>
            </td>
               </tr>
            <tr>
              <td class="font">
                行政区
            </td>
            <td class="mtext" colspan="3">
               <input id="txtDistrict" type="text" value="" class="input"  />
                <input id="btnDistrict" type="button" class="btn" value="定位"/>
            </td>
          
         </tr>
     </table>
        </div>
          <script type="text/javascript" src="/Scripts/Plug/iColor/iColor-min.js"></script>
        <link rel="stylesheet" href="/Scripts/Plug/iColor/iColor.css"/>
     	<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&amp;ak=<%=GetKey() %>"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/library/DrawingManager/1.4/src/DrawingManager_min.js"></script>
     <link rel="stylesheet" href="http://api.map.baidu.com/library/DrawingManager/1.4/src/DrawingManager_min.css"/>
     	<script type="text/javascript" src="/Scripts/Area.js"></script>
    <div id="allmap" style="height:700px; width: 100%;">
        <div class="BMapLib_Drawing_panel"><a class="BMapLib_box BMapLib_hander_hover" drawingtype="hander" href="javascript:void(0)" title="拖动地图" onfocus="this.blur()"></a><a class="BMapLib_box BMapLib_marker" drawingtype="marker" href="javascript:void(0)" title="画点" onfocus="this.blur()" style="display: none;"></a><a class="BMapLib_box BMapLib_circle" drawingtype="circle" href="javascript:void(0)" title="画圆" onfocus="this.blur()" style="display: none;"></a><a class="BMapLib_box BMapLib_polyline" drawingtype="polyline" href="javascript:void(0)" title="画折线" onfocus="this.blur()" style="display: none;"></a><a class="BMapLib_box BMapLib_polygon" drawingtype="polygon" href="javascript:void(0)" title="画多边形" onfocus="this.blur()"></a><a class="BMapLib_box BMapLib_rectangle BMapLib_last" drawingtype="rectangle" href="javascript:void(0)" title="画矩形" onfocus="this.blur()" style="display: none;"></a></div>
     
   </div>
   <div id="dviArea" style="height:100px; width: 100%;">
    
   </div>

<script type="text/javascript">
    Area("<%=ddlCity.DropDownList.ClientID%>", "tag", "txtColor", "divSaveContent",<%=(Request.QueryString["isedit"].Convert<string>()!="false").ToString().ToLower()%>,<%=(Request.QueryString["ispublish"].Convert<string>()=="true").ToString().ToLower()%>);


</script>
     <div id="divSaveContent" style="width: 400px;" class="">
           <table class="edittb">
             <tr>
                 <td class="font">名称</td>
                 <td class="text"><input id="txtName" class="input"/></td>
                  <td class="font">
                颜色
            </td>
            <td class="mtext" >
               <input id="txtsaveColor" type="text" value="#FF0000" readonly="readonly" style="width: 60px; background: #FF0000;"/>
            </td>
             </tr>
               <tr>
             <td class="font">标签</td>
                 <td class="mtext" colspan="3">
                        <input type="radio" id="rdDelivery" checked="checked" value="配送站" group="tag" name="tag"/>
                      <label for="rdDelivery">配送站</label>
                     <input type="radio" id="rdFreight" group="tag" name="tag" value="运费"/>
                      <label for="rdDelivery" >运费</label>
                       <input type="radio" id="rdYun" group="tag" name="tag" value="云配送"/>
                      <label for="rdYun" >云配送</label>
                 </td>
               </tr>
                 <tr>
                 <td class="font">标签值</td>
                 <td class="text"><input id="txtValue" class="input"/></td>
                 <td class="font">是否启用</td>
                 <td class="text" ><input id="ckIsUsed" type="checkbox" checked="checked"/></td>
                 
             </tr>
             <tr>
                 <td class="mtext center" colspan="4">
                       
                     <input id="btnSave" class="btn" value="保存" type="button"/>
                       <input id="btnCancel" class="btn" value="取消" type="button"/>
                 </td>
             </tr>
         </table>
     </div>

     

 </asp:Content>
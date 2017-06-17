<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Basedata.Freight.Edit" %>
<%@ Import Namespace="Winner.Persistence" %>
     


     

<div class="edit">
    <table class="tb">
        <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3" >
             <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"  /> 
            </td>
            
        </tr>
        
  
     <tr>
            <td class="font">包邮利润比例</td>
            <td class="text" >
                <asp:CheckBox ID="ckFreeProfit" runat="server" SaveName="FreeProfit" BindName="FreeProfit" />
               
            </td>
               <td class="font">是否配送</td>
            <td class="text" >
                <asp:CheckBox ID="ckIsGis" runat="server" SaveName="IsDelivery" BindName="IsDelivery" />
               
            </td>
        </tr>
        <tr>
           <td class="font">描述</td>
           <td class="text"  colspan="3" >
                 <input id="txtDescription" runat="server"  type="text" class="input long"   BindName="Description" SaveName="Description"   />
                 
            </td>
        </tr>
           <td class="font">免费区域</td>
           <td class="text"  colspan="3" >
                <table class="intb" id="divFreeRegion" >
                    <tr Instance="FreeRegionTemplate">
                        <td class="infont" style="width: 300px;">
                        <input type="hidden" id="hfFreeRegion" name="FreeRegion" runat="server" BindName="FreeRegion" SaveName="FreeRegion" />
                        </td>
                          <td class="intext" style="width: 30px;">设置</td>
                    </tr>
                    </table>
                 
            </td>
             <tr>
     
        </tr>
        <tr>
            <td colspan="4" class="text">
                 <table class="intb" id="divCarry" >
                    <tr Instance="CarryTemplate">
                        <td class="infont" style="width: 300px;">默认运费
                        <input type="hidden" name="CarryId"/>
                        <input type="hidden" name="CarryRegion"/>
                        </td>
                        <td class="intext">
                            配送方式
                              <input type="text" name="CarryName"  class="input shortinput" value="5" /> 
                             开始<input type="text" name="CarryDefaultCount" filtername="int" class="input shortinput" value="1"/> 
                             <span name="UnitType"></span>内，
                             <input type="text" name="CarryDefaultPrice" filtername="decimal" class="input shortinput" value="5" /> 元， 每增加
                              <input type="text" name="CarryContinueCount" filtername="int" class="input shortinput" value="1"/> 
                              <span name="UnitType"></span>， 增加运费
                              <input type="text" name="CarryContinuePrice" filtername="decimal" class="input shortinput" value="1"/> 元
                        </td>
                        <td class="intext" style="width: 30px;"></td>
                    </tr>
                    </table>
                    <a href="javascript:void(0);" id="btnCarryAddId">为指定地区城市设置运费</a>
            </td>
        </tr>
   
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>
<div id="divDistrict" class="freight" style="display: none;" >
    <div class="content">
        <%=GetDistrictHtml() %>
    </div>
    
    <div class="sure"><input type="button" value="确定"/></div>
</div>
<script type="text/javascript" src="/Scripts/Winner/CheckBox/Winner.CheckBox.js"></script>
<script type="text/javascript" src="/Scripts/Freight.js"></script>
 <script type="text/javascript">
     function InitFreight() {
         var freight = new Freight("divCarry", "btnCarryAddId", "dviFreeRegion", "divDistrict");
         freight.Initialize();
         var carries = eval("<%=GetCarryEntities().Replace("\"","'") %>");
    
         if (carries != null && carries.length > 0) {
             for (var i = 0; i < carries.length; i++) {
                 if (<%=(SaveType==SaveType.Add).ToString().ToLower() %>) {
                     carries[i].Id = "";
                 }
                 freight.AddCarry(carries[i]);
             }
         }
     }

 </script>
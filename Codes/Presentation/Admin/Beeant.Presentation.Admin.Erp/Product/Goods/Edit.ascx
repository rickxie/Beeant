<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Goods.Edit" %>
<%@ Import Namespace="Winner.Persistence" %>
<%@ Register src="../../Controls/Editor.ascx" tagname="Editor" tagprefix="uc3" %>
 

  <%@ Register src="../../Controls/Uploader.ascx" tagname="Uploader" tagprefix="uc4" %>


 



<%@ Register src="../../Controls/Basedata/TagRadioButtonList.ascx" tagname="TagRadioButtonList" tagprefix="uc8" %>



<%@ Register src="/Controls/GeneralCheckBoxList.ascx" tagname="GeneralCheckBoxList" tagprefix="uc1" %>
<%@ Register src="/Controls/Basedata/FreightDropDownList.ascx" tagname="FreightDropDownList" tagprefix="uc7" %>



<input id="btnReset" type="button" value="重置" style="display: none;width: 100px;height: 30px;text-align: center;font-size: 18px;line-height: 30px;" />
   <input id="btnPushlish" type="button" value="发布" disabled="disabled" style="width: 100px;height: 30px;text-align: center;font-size: 18px;line-height: 30px;" />
     <div id="divCategoryContainer">
         
         
         
        </div>

<div class="edit" id="divGoodsContainer" style="display: none;">
    
    <table class="tb"> 
        
        <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3" >
              <input id="txtName" runat="server"  type="text" class="input long"  BindName="Name" SaveName="Name"   /> 
            </td>
         
        </tr>
    
       
        <tr>
                <td class="font">数量</td>
            <td class="text"  >
                 <input id="txtCount" runat="server"  type="text" class="input"  BindName="Count"  SaveName="Count" value="1000"    /> 
            </td>
            
             <td class="font">是否上架</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsSales" runat="server" BindName="IsSales" SaveName="IsSales" />
                </td>
       </tr>
   
       
        <tr>
             <td class="font">面价</td>
            <td class="text" >
                 <input id="txtPrice" runat="server"  type="text" class="input" BindName="Price"   SaveName="Price" value="2000"   /> 
            </td>
              <td class="font">底价</td>
            <td class="text" >
                 <input id="txtCost" runat="server"  type="text" class="input" BindName="Cost"  SaveName="Cost" value="1000" /> 
            </td>
            
             
       </tr>
            <tr  style="display: none;">
               <td class="font">最小起订数量</td>
        <td class="text" >
           <input id="txtOrderMinCount" runat="server"  type="text" class="input" BindName="OrderMinCount"  SaveName="OrderMinCount"  value="1" /> 
        </td>
             <td class="font">订购步长数量</td>
        <td class="text" >
           <input id="txtOrderStepCount" runat="server"  type="text" class="input" BindName="OrderStepCount"  SaveName="OrderStepCount"  value="1" /> 
        </td>
      
        </tr>
         <tr  style="display: none;">
               
             <td class="font">定金比率</td>
            <td class="text" >
                 <input id="txtDespoitRate" runat="server"  type="text" value="0" class="input" ValidateName="DepositRate" SaveName="DepositRate"  BindName="DepositRate" />
            </td>
               <td class="font">排序</td>
            <td class="text"  >
                 <input id="txtSequence" runat="server"  type="text" class="input"  BindName="Sequence" SaveName="Sequence" value="1"    /> 
            </td>
       </tr>
             <tr style="display: none;">
            <td class="font">商家编码</td>
        <td class="text" colspan="3" >
           <input id="txtDataId" runat="server"  type="text" class="input" BindName="DataId"  SaveName="DataId"  /> 
        </td>
       </tr>      
          
          <tr  style="display: none;">
             <td class="font">标签</td>
            <td class="text" colspan="3" >
                <uc8:TagRadioButtonList ID="ckTag" runat="server" SaveName="Tag" BindName="Tag" />
            </td> 
       </tr>
       <tr  style="display: none;">
             <td class="font">支付方式限制</td>
            <td class="text ckmul" colspan="3" >
                <uc1:GeneralCheckBoxList ID="cbPayTypes" runat="server" SaveName="PayTypes" BindName="PayTypes" ObjectName="Beeant.Domain.Entities.Basedata.PayTypeEntity,Beeant.Domain.Entities"  />
  
            </td> 
       </tr>
        <tr  style="display: none;">
          
             <td class="font">是否定制</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsCustom" runat="server" BindName="IsCustom" SaveName="IsCustom" />
                </td>
             <td class="font">可以退货</td>
            <td class="text"  >
                <asp:CheckBox ID="ckIsReturn" runat="server" BindName="IsReturn" SaveName="IsReturn" />
                </td>
       </tr>
   <tr  style="display: none;">
           <td class="font">不占用库存状态</td>
            <td class="mtext ckmul" colspan="3" >
               
                <uc1:GeneralCheckBoxList ID="ckUnusedStatus" runat="server" SearchWhere="Status==@Status" SearchParamterName="Status" ObjectName="Beeant.Domain.Entities.Order.OrderStatusType" IsEnum="True" />
                   
                   
            </td>
   </tr>
  
    <tr  style="display: none;">
        <td class="font">连接地址</td>
        <td class="mtext" colspan="3" >
            <input id="txtUrl" runat="server" type="text" class="text long"  BindName="Url" SaveName="Url"/>
        </td>
        
    </tr>

       <tr  style="display: none;">
          <td class="font">物流信息</td>
            <td class="mtext" colspan="3" >
            <uc7:FreightDropDownList ID="ddlFreight" runat="server" BindName="Freight.Id" SaveName="Freight.Id" />
            </td> 
       </tr>
       <tr>
    
       <tr>
           <td class="font">商品属性</td>
           <td colspan="3" class="mtext">
                <div id="divPropertyContainer"></div>
           </td>
       </tr> 
        <tr>
           <td class="font">商品图片(小于300KB)</td>
           <td colspan="3" class="mtext">
                <div id="divImageContainer"></div>
           </td>
       </tr> 
       <tr>
            <td class="font">商品销售</td>
           <td colspan="3" class="mtext">
                <div id="divSelectSku"></div>
                 <div id="divSkuProperty" ></div>
                <div id="divSkuImage" ></div>
                <div id="divSkuProduct" ></div>
               
           </td>
       </tr>
        <tr>
        <td class="font">附件</td>
            <td class="mtext" colspan="3" >
             <uc4:Uploader ID="upAttchment" runat="server" Path="Files/Documents/Goods/" IsShowViewControl="False"   />
             <a href="" id="hfAttchment" runat="server">
            
                  </a>
            </td>
       </tr>

   
       <tr>
           <td class="font">详情</td>
           <td colspan="3" class="mul text">
               

               <uc3:Editor ID="Editor1" runat="server" ImagePath="Files/Images/Goods/" FlashPath="Files/Flashs/Goods/" ValidateName="Detail" />
               

           </td>
       </tr>
         <tr>
            <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
 
</div>


    <input id="hfbranchId" runat="server" type="hidden" value="" />
    <input id="hfPropertyValue" runat="server" type="hidden" value="" />
    <input id="hfImageValue" runat="server" type="hidden" value="" />
    <input id="hfProductValue" runat="server" type="hidden" />
    <input id="hfPricesValue" runat="server" type="hidden" />
    <input id="hfProductId" runat="server" type="hidden" />
    <input id="hfCategoryPropertyId" runat="server" type="hidden" />
 
     
   
 <script type="text/javascript" src="/Scripts/Winner/Publisher/Winner.Publisher.js"></script>
  <script type="text/javascript" src="/Scripts/Goods.js"></script>
  <script type="text/javascript">
      function InitGoods(isPublish) {
          window.Goods = new Goods(
              {
                  BranchId: "<%=hfbranchId.ClientID %>",
                  PropertyValueId: "<%=hfPropertyValue.ClientID %>",
                  CategoryPropertyId: "<%=hfCategoryPropertyId.ClientID %>",
                  ImageValueId: "<%=hfImageValue.ClientID %>",
                  ProductValueId: "<%=hfProductValue.ClientID %>",
                  PricesValueId: "<%=hfPricesValue.ClientID %>",
                  GoodsId: '<%=Request.QueryString["Id"] %>',
                  GoodsImagesValue: "<%=GoodsImages %>",
                  ProductsValue: "<%=Products %>",
                  SaveButtonId: "<%=btnSave.ClientID %>",
                  IsPublish: isPublish
              });
          window.Goods.Initialize();
      }
    $(document).ready(function() {
   
        //隐藏上下架开关
        if (<%=(SaveType==SaveType.Add).ToString().ToLower() %>) {
            $("#divSkuProduct").find("input[name='IsSales']").prop("checked", "");
            $("#divSkuProduct").find("input[name='IsSales']").hide();
     
        }
    });
  </script>



<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Product.Goods.Detail" MasterPageFile="~/Datum.Master" %>
<%@ Register TagPrefix="uc3" TagName="Progress" Src="/Controls/Progress.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>商品详情</title>  
 </asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
 <div class="info">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <table class="tb">
       
           <tr>
            <td class="font">名称</td>
            <td class="mtext" colspan="3"  >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
        </tr>
        <tr>
            
              <td class="font">是否上下架</td>
              <td class="text">
                 <asp:Label ID="lblIsSalesName" runat="server" Text=""  BindName="IsSalesName"></asp:Label>

            </td>
             <td class="font">附件</td>
            <td  class="mtext">
                  <a href="" id="hfAttchment" runat="server" BindName="DownAttachmentUrl">
                      附件
                  </a>
            </td>
        </tr>
    <%--     <tr>
          
             <td class="font">定金比率</td>
            <td class="text" >
                 <asp:Label ID="lblDepositRate" runat="server" BindName="PercentageOfDepositRate"></asp:Label>
            </td>
             <td class="font">是否定制</td>
            <td class="text"  >
                <asp:Label ID="lblCustom" runat="server" Text=""  ></asp:Label>
         </tr>
         <tr>
           
           <td class="font">不占用库存状态</td>
               <td class="mtext" colspan="3"  >
                <asp:Label ID="lblUnUsedStatusName" runat="server" BindName="UnusedStatusName"></asp:Label>
             </td>
       
        </tr>--%>
 
         <tr>
            <td class="font">类目</td>
            <td class="text"  >
                <asp:Label ID="lblCatagoryName" runat="server"  BindName="Category.Name"></asp:Label>
             </td>
              <td class="font">支付方式</td>
            <td class="mtext"   >
                <asp:Label ID="Label12" runat="server" BindName="PayTypes"></asp:Label>
            </td>
          
             
    
         </tr>
         <tr style="display: none;">
              <td class="font">关注数量</td>
               <td class="text" >
                <asp:Label ID="lblAttentionCount" runat="server" BindName="AttentionCount"></asp:Label>
             </td>
             <td class="font">访问数量</td>
               <td class="text"  >
                <asp:Label ID="lblVisitCount" runat="server" BindName="VisitCount"></asp:Label>
             </td>
        </tr>
         <tr style="display: none;">
             <td class="font">销售数量</td>
               <td class="text"  >
                <asp:Label ID="lblSalesCount" runat="server" BindName="SalesCount"></asp:Label>
             </td>
            <td class="font">标签</td>
            <td class="text"  >
                 <asp:Label ID="lblTag" runat="server" ></asp:Label>
            </td>    
        </tr>
         <tr style="display: none;">
              <td class="font">是否支持退货</td>
               <td class="text"  >
                <asp:Label ID="lblIsReturn" runat="server" BindName="IsReturnName"></asp:Label>
             </td>
            <td class="font">物流信息</td>
            <td  class="text">
                  <a href="/Basedata/Freight/Detail.aspx?Id=" id="hfFreight" runat="server" BindName="Freight.Id">
              <asp:Label ID="lblFreight" runat="server" Text=""  BindName="Freight.Name"></asp:Label>
                   </a>
            </td>

        </tr>
        <tr style="display: none;">
            <td class="font">包装说明</td>
            <td colspan="3" class="mtext">
              <asp:Label ID="lblDescription" runat="server" Text=""  BindName="Description"></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td class="font">连接地址</td>
            <td class="mtext">
                  <a href="" id="hfUrl" runat="server" BindName="Url"> <asp:Label ID="Label1" runat="server" Text=""  BindName="Url"></asp:Label></a>
            </td>
             <td class="font">是否定制</td>
            <td class="mtext">
                 <asp:Label ID="lblIsCustom" runat="server" Text=""  BindName="IsCustomName"></asp:Label>
            </td>
        </tr>
      
     <tr style="display: none;">
           <td class="font">关联编号</td>
            <td class="mtext"  >
                    <asp:Label ID="lblDataId" runat="server" Text=""  BindName="DataId"></asp:Label>
            </td>
       <td class="font">排序</td>
               <td class="text"  >
                <asp:Label ID="lblSequence" runat="server" BindName="Sequence"></asp:Label>
             </td>
        </tr>
  
      
          <tr>
            
             <td colspan="4" class="center">
                 <input id="btnClose" type="button" value="关闭" class="btn"   />
             </td>
         </tr>
     </table>
  <div class="subtitle" onclick="SetEntityBody('divGoodsProperty')">商品属性列表(<span class="count"><%=pgGoodsProperty.DataCount%></span>)</div>
       <div id="divGoodsProperty" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
     <div  class="search" >
           <table class="tb">
        <tr>
            <td class="font">开始日期</td>
            <td class="mtext"><asp:TextBox ID="txtBeginInsertTime" runat="server" CssClass="seinput" SearchWhere="InsertTime>=@BeginInsertTime" SearchParamterName="BeginInsertTime"></asp:TextBox>
            <cc1:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtBeginInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td class="font">截止日期</td>
            <td class="mtext"><asp:TextBox ID="txtEndInsertTime" runat="server" CssClass="seinput"  SearchWhere="InsertTime<=@EndInsertTime" SearchParamterName="EndInsertTime"></asp:TextBox>
             <cc1:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtEndInsertTime" Format="yyyy-MM-dd" ></cc1:CalendarExtender>
            </td>
            <td >
                <asp:Button ID="Button1" runat="server" Text="搜索" CssClass="btn"  />
            </td>
        </tr>
 
     </table>
        </div>
   
           <asp:GridView ID="gvGoodsProperty" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
         <asp:TemplateField HeaderText="属性"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Property.Name")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="值"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Value")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="产品编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Product.Id")%>
            </ItemTemplate>
        </asp:TemplateField>

        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgGoodsProperty" runat="server" PageSize="10"  
     SelectExp="Id,Property.Name,Value,Product.Id"
      FromExp="GoodsPropertyEntity"
      OrderByExp="UpdateTime desc" WhereExp="Goods.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
         
         <div class="subtitle" onclick="SetEntityBody('divProduct')">商品SKU列表(<span class="count"><%=pgProduct.DataCount%></span>)</div>
       <div id="divProduct" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
 
   
           <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns>
             <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Id")%>
            </ItemTemplate>
        </asp:TemplateField> 
         <asp:TemplateField HeaderText="属性"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("Name")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="面价"  ItemStyle-CssClass="left status">
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
           <asp:TemplateField HeaderText="最小起订数量"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("OrderMinCount")%>
            </ItemTemplate>
        </asp:TemplateField>
           <asp:TemplateField HeaderText="是否销售"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("IsSalesName")%>
            </ItemTemplate>
        </asp:TemplateField>
       
        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgProduct" runat="server" PageSize="10"  
     SelectExp="Id,Name,Price,Cost,Count,OrderMinCount,IsSales"
      FromExp="ProductEntity"
      OrderByExp="UpdateTime desc" WhereExp="Goods.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>

         

      
         
           <div class="subtitle" onclick="SetEntityBody('divPlatform')">商品同步列表(<span class="count"><%=pgPlatform.DataCount%></span>)</div>
       <div id="divPlatform" style="display: none;" >
     <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
    
   
           <asp:GridView ID="gvPlatform" runat="server" AutoGenerateColumns="False" CssClass="table" >
       <Columns> 
         <asp:TemplateField HeaderText="属性"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("TypeName")%>
            </ItemTemplate>
        </asp:TemplateField>
            <asp:TemplateField HeaderText="外部编号"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("DataId")%>
            </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="同步时间"  ItemStyle-CssClass="left status">
            <ItemTemplate>
                <%#Eval("SynchTime")%>
            </ItemTemplate>
        </asp:TemplateField>

        </Columns>
     </asp:GridView>

     <uc1:Pager ID="pgPlatform" runat="server" PageSize="10"  
     SelectExp="Id,DataId,Type,SynchTime"
      FromExp="PlatformEntity"
      OrderByExp="UpdateTime desc" WhereExp="Goods.Id==@Id" />
     

          </ContentTemplate>
 </asp:UpdatePanel>
         </div>
         

         
  <div class="subtitle" onclick="SetEntityBody('divInquery')"> 商品询问信息(<span class="count"><%=pgInquery.DataCount%></span>)</div>
                <div id="divInquery" style="display: none;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvInquery" runat="server" AutoGenerateColumns="False" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
                                        <ItemTemplate><%#Eval("Id")%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="问题"  ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Question")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否回答"  ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("IsReplyName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="回答"  ItemStyle-CssClass="left Sequence">
                                    <ItemTemplate>
                                        <%#Eval("Answer")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="回答时间"  ItemStyle-CssClass="left Sequence">
                                    <ItemTemplate>
                                        <%#Eval("AnswerTime")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                          <asp:TemplateField HeaderText="账户"  ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Account.Name")%>
                                        </ItemTemplate>
                                      </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="录入时间" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                        <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}")%>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:Pager ID="pgInquery" runat="server" PageSize="10" SelectExp="Id,Question,IsReply,Answer,AnswerTime,Account.Id,Account.Name,InsertTime"
                                FromExp="Beeant.Domain.Entities.Product.InqueryEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Goods.Id==@Id" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                 
         


           <div class="subtitle" onclick="SetEntityBody('divGoodsImage')">商品图片列表(<span class="count"><%=pgGoodsImage.DataCount%></span>)</div>
       <div id="divGoodsImage" style="display: none;" >

       
       <table class="table" id="<%=repGoodsImage.ClientID %>">
          <tr>
            <td>商品图片</td>
            <td>
                <asp:Repeater ID="repGoodsImage" runat="server">
                   <ItemTemplate>
                        <img src='<%#string.IsNullOrEmpty(Eval("FullFileName").ToString())?"/Images/Nopic.jpg":Eval("FullFileName")%>' alt="" class="img"/> 
                      
                   
                   </ItemTemplate>
                 
                </asp:Repeater>
            </td>
         </tr>
        
  </table>
               
               
               

 
     <uc1:Pager ID="pgGoodsImage" runat="server"  Visible="False" PageSize="1000"
     SelectExp="Id,FileName"
      FromExp="GoodsImageEntity"
      OrderByExp="Sequence desc" WhereExp="Goods.Id==@Id" />
     

         </div>
           <uc3:Progress ID="Progress1" runat="server" />
          
          </ContentTemplate>
 </asp:UpdatePanel>
  </div>


 </asp:Content>
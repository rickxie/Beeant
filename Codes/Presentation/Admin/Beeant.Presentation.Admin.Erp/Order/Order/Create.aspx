<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.Order.Create" MasterPageFile="~/Datum.Master" ValidateRequest="false"%>
<%@ Import Namespace="Component.Extension" %>
<%@ Import Namespace="Beeant.Domain.Entities.Member" %>
<%@ Import Namespace="Beeant.Domain.Entities.Product" %>
<%@ Import Namespace="Beeant.Basic.Services.WebForm.Extension" %>
<%@ Register src="/Controls/Message.ascx" tagname="Message" tagprefix="uc2" %>
<%@ Register src="../../Controls/Account/AccountComboBox.ascx" tagname="AccountComboBox" tagprefix="uc8" %>
 


<%@ Register TagPrefix="uc1" TagName="Pager" Src="~/Controls/Pager.ascx" %>

 
 
 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
   <title>代客下单</title>  
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">



     <script type="text/javascript" src="/Scripts/Plug/Calendar.js"></script>




    


  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div class="edit">
      <table class="tb">
        
          <tr>
            <td class="font">账户</td>
            <td class="mul mtext" colspan="3"  >
                <uc8:AccountComboBox ID="cbAccount" runat="server" />
                <asp:Button ID="btnChangeAccount" runat="server" Text="查询"  CssClass="btn"
                    onclick="btnChangeAccount_Click" Visible="True" />
            </td>
               
        </tr>
           <tr>
            <td class="font">代理名称</td>
            <td class="text" >
                <asp:Label ID="lblAgentName" runat="server"  BindName="AgentName"></asp:Label>
             </td>
               <td class="font">账户余额</td>
            <td class="text">
                 <asp:Label ID="lblBalance" runat="server" BindName="Balance"></asp:Label>
            </td>
        </tr>   
          <tr>
            <td class="font">用户名</td>
            <td class="text" >
                <asp:Label ID="lblName" runat="server"  BindName="Name"></asp:Label>
             </td>
             <td class="font">真实姓名</td>
            <td class="text" >
                <asp:Label ID="lblRealName" runat="server" BindName="RealName"></asp:Label>
             </td>
        </tr>
         <tr>
            <td class="font">手机号码</td>
            <td class="text" >
                <asp:Label ID="lblMobile" runat="server" BindName="Mobile"></asp:Label>
             </td>
              <td class="font">电子邮箱</td>
            <td class="text" >
                <asp:Label ID="lblEmail" runat="server" BindName="Email"></asp:Label>
             </td>
        </tr>
      
     
        </tr>
         
   
    </table>

      <div class="subtitle" style="color: #000;">收货地址 <a href="javascript:void" style='<%=string.IsNullOrEmpty(cbAccount.InputHidden.Value)?"display:none;":"" %>'  Note="note" NoteUrl='CreateSelectAddress.aspx?accountId=<%=cbAccount.InputHidden.Value %>&btn=<%=btnChangeAccount.ClientID %>'>新增收货地址</a></div>
                <div id="divAddress" style="max-height: 400px;overflow: auto;">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAddress" runat="server" AutoGenerateColumns="False" CssClass="tblist">
                                <Columns>
                                    <asp:TemplateField HeaderText="编号" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <%#Eval("Id")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                           <asp:TemplateField HeaderText="收货人" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Recipient")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="公司名称" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Company")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="所在省份" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("Province")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                             
                                    <asp:TemplateField HeaderText="所在城市" ItemStyle-CssClass="left Sequence xlstext">
                                        <ItemTemplate>
                                            <%#Eval("City")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="所在区县" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("County")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="详细地址" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Address")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="手机号码" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("Mobile")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="固定电话" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Telephone")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                         <asp:TemplateField HeaderText="邮箱" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Email")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="地址标签" ItemStyle-CssClass="left Sequence">
                                        <ItemTemplate>
                                            <%#Eval("Tag")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否默认" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <%#Eval("IsDefaultName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                    <input type="radio" name="Address" value='<%#Eval("Id") %>' <%#Eval("IsDefault").Convert<bool>()?"checked='checked'":"" %>/>
                                </ItemTemplate>
                            </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                      <%--      <uc1:Pager ID="pgAddress" runat="server" PageSize="10" SelectExp="Id,District.Name,District.Parent.Name,District.Parent.Parent.Name,Company,Recipient,Mobile,Address,Tag,Email,Telephone,IsDefault"
                                FromExp="Beeant.Domain.Entities.Member.AddressEntity,Beeant.Domain.Entities" OrderByExp="Id desc"
                                WhereExp="Account.Id==@AccountId" />--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
 
 <div class="subtitle" style="color: #000;">开票信息 <a href="javascript:void" style='<%=string.IsNullOrEmpty(cbAccount.InputHidden.Value)?"display:none;":"" %>'  Note="note" NoteUrl='CreateSelectInvoice.aspx?accountId=<%=cbAccount.InputHidden.Value %>&btn=<%=btnChangeAccount.ClientID %>'>新增发票信息</a></div>
        <div id="divInvoice"  style="max-height: 400px;overflow: auto;">
            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" CssClass="tblist">
                        <Columns>
                            <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <%#Eval("Id")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" 发票类型" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                    <%#Eval("TypeName")%>
                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="发票类型" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                    <%#Eval("GeneralTypeName")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                               <asp:TemplateField HeaderText="收票人姓名"  ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                     <%#Eval("Recipient")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="收票人手机"  ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                     <%#Eval("Mobile")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="详细地址"  ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                     <%#Eval("Address")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                    <input type="radio" name="Invoice" checked="checked" value='<%#Eval("Id") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                         
                        </Columns>
                    </asp:GridView>
              <%--      <uc1:Pager ID="pgInvoices" runat="server" PageSize="10" SelectExp="Id,Title,Content,Company,Type,GeneralType,Recipient,Mobile,Address"
                        FromExp="Beeant.Domain.Entities.Member.InvoiceEntity,Beeant.Domain.Entities" OrderByExp="Id asc"
                        WhereExp="Account.Id==@AccountId" Visible="False" />--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>



 <div class="subtitle" style="color: #000;">商品信息 <a href="javascript:void" style='<%=string.IsNullOrEmpty(cbAccount.InputHidden.Value)?"display:none;":"" %>'  Note="note" NoteUrl='CreateSelectProduct.aspx?SerializeValueId=<%=hfProducts.ClientID %>&SerializeContainerId=<%=gvProduct.ClientID %>'>新增商品</a></div>
       
        <div id="divProduct" style="max-height: 400px;overflow: auto;" >
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="False" CssClass="tblist">
                        <Columns>
                            <asp:TemplateField HeaderText="编号"  ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <%#Eval("Product.Id")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                 <asp:TemplateField HeaderText="图片" ItemStyle-CssClass="center time">
                                        <ItemTemplate>
                                            <img src='<%#string.IsNullOrEmpty(Eval("Product.Goods.FullFileName").Convert<string>()) ? "/Images/Nopic.jpg" : Eval("Product.Goods.FullFileName").Convert<string>()%>'
                                                alt="" class="img" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="商品名称" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                    <%#Eval("Product.Name")%>
                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="商品单价/元" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                    <%#Eval("Price")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="商品数量" ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                      <input type="text"  id="txtCount" value='<%#Eval("Count") %>' SerializeName="Count" SerializeValueId="<%=hfProducts.ClientID %>" SerializeId="<%#Eval("Product.Id") %>" count='count' style="width: 60px;"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="起订数量" ItemStyle-CssClass="left Sequence">
                                <ItemTemplate>
                                      <%#Eval("OrderMinCount")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                       
                         
                              <asp:TemplateField HeaderText="操作"  ItemStyle-CssClass="left Sequence xlstext">
                                <ItemTemplate>
                                    <a href='/product/Product/Detail.aspx?id=<%#Eval("Product.Id") %>' target="_blank">查看详情</a>
                                     &nbsp; &nbsp;
                                    <a href="javascript:void(0)" SerializeRemove="true" SerializeValueId="<%#hfProducts.ClientID %>" SerializeId="<%#Eval("Product.Id") %>">移除</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <input type="hidden" id="hfProducts" runat="server"/>
        
        
        

        <input type="hidden" id="hfPromotion" runat="server"/>
        <div class="subtitle" style="color: #000;">支付信息 </div>
              <table class="tb">
        
          <tr>
            <td class="font">商品总类</td>
            <td class="mul mtext" colspan="3"  >
                <asp:Label ID="lblKindCount" runat="server" Text=""></asp:Label>
       
            </td>
               
        </tr>
         
          <tr>
            <td class="font">商品数量</td>
            <td class="mul mtext" colspan="3"  >
                <asp:Label ID="lblTotalCount" runat="server" Text=""></asp:Label>
             
            </td>
               
        </tr>
          <tr>
            <td class="font">运费(元)</td>
            <td class="mul mtext" colspan="3"  >
                <asp:Label ID="lblFeightPrice" runat="server" Text=""></asp:Label>

            </td>
               
        </tr>
         <tr>
            <td class="font">商品总金额(元)</td>
            <td class="mul mtext" colspan="3"  >
                 <asp:Label ID="lblProductPrice" runat="server" Text=""></asp:Label>
                
            </td> 
        </tr>
        <tr>
            <td class="font">应付金额(元)</td>
            <td class="mul mtext" colspan="3"  >
                 <asp:Label ID="lblFactPrice" runat="server" Text=""></asp:Label>
              
            </td> 
        </tr>
         
          <tr>
          
        <td class="font">备注</td>
            <td class="mtext" colspan="3">
                 <input id="txtRemark" runat="server" type="text" class="input long"/>          
            </td>
        </tr>

           <tr>
             <td colspan="4" class="center"><asp:Button ID="btnSave" runat="server" Text="保存" 
                     CssClass="btn mbtn" onclick="btnSave_Click"   />
             <input id="btnClose" type="button" value="取消" class="btn mbtn" onclick="window.close();"  /></td>

           
        </tr>
    </table>
        <uc2:Message ID="Message1" runat="server" />
</div>


        
    </ContentTemplate>
</asp:UpdatePanel>
 
 

 

    
 <script type="text/javascript" src="/Scripts/Serializator.js"></script>
    <script type="text/javascript" src="/Scripts/Winner/Note/Winner.Note.js"></script>
  <script type="text/javascript" >
      var Init = function () {
          window.Serial = new Serializator({ Serializes: [{ Id: '<%=gvProduct.ClientID %>', Html: "", ValueId: "<%=hfProducts.ClientID %>" }] });
          window.Serial.Initialize();
          window.Serial.ResetFunction = function () {
              $("#<%=btnChangeAccount.ClientID %>").click();
          };
          $(".img").click(function () {
              $("#<%=btnChangeAccount.ClientID %>").click();
          });
      };

  </script>


 </asp:Content>
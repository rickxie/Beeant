<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Beeant.Presentation.Admin.Erp.Promotion.Promotion.Edit" %>
<%@ Register TagPrefix="uc5" TagName="GeneralCheckBoxList" Src="~/Controls/GeneralCheckBoxList.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.37663, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="uc8" TagName="TagRadioButtonList" Src="~/Controls/Basedata/TagRadioButtonList.ascx" %>
<%@ Register TagPrefix="uc6" TagName="MonthsCheckBoxList" Src="~/Controls/MonthsCheckBoxList.ascx" %>
<%@ Register src="/Controls/GeneralCheckBoxList.ascx" tagname="GeneralCheckBoxList" tagprefix="uc1" %>
<div class="edit" >
    <table class="tb">
       
        <tr>
            <td class="font">
                活动名称
            </td>
            <td class="mtext" colspan="3" >
                <input id="txtName" runat="server" type="text" class="input" BindName="Name" SaveName="Name" />
            </td>
        </tr> 
      
        <tr>
          
             <td class="font">标签</td>
            <td class="mtext" colspan="3" >
                <uc8:TagRadioButtonList ID="ckTag" runat="server" SaveName="Tag" BindName="Tag" />
            </td>
               
       </tr>
        <tr>
            <td class="font">
                开始日期
            </td>
            <td class="text">
                <asp:TextBox ID="txtStartDate" runat="server" Enabled="True" CssClass="input" BindName="StartDate"
                    SaveName="StartDate"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarStartDate"  Enabled="True" runat="server" TargetControlID="txtStartDate"
                    Format="yyyy-MM-dd">
                </cc1:CalendarExtender>
            </td>
            <td class="font">
                截止日期
            </td>
            <td class="text">
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="input" BindName="EndDate" SaveName="EndDate"></asp:TextBox>
                <cc2:CalendarExtender ID="CalendarEndDate" runat="server" TargetControlID="txtEndDate"
                    Format="yyyy-MM-dd">
                </cc2:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="font">
                开始时间
            </td>
            <td class="text">
                 <input id="txtStartTime" runat="server" type="text" class="input"  BindName="StartTime" SaveName="StartTime" DefaultValue="00:00:00" value="00:00:00"  />
                
            </td>
            <td class="font">
                结束时间
            </td>
            <td class="text">
                 <input id="txtEndTime" runat="server" type="text" class="input"  BindName="EndTime" SaveName="EndTime" DefaultValue="00:00:00" value="23:59:59"  />
            </td>
        </tr>
        <tr>
            <td class="font">
                日期
            </td>
            <td class="mtext ckmul" colspan="3">
                  <uc6:MonthsCheckBoxList ID="ckMonths" runat="server" BindName="Months" SaveName="Months"></uc6:MonthsCheckBoxList>
            </td>
           
        </tr>
        
        <tr>
             <td class="font">
                周期
                
            </td>
            <td class="mtext ckmul" colspan="3" >
                <uc5:GeneralCheckBoxList ID="ckWeeks" runat="server" BindName="BindWeeks" SaveName="BindWeeks"
                    IsEnum="True" ObjectName="System.DayOfWeek" />
            </td>
            </tr>
            <tr>
                 <td class="font"  >
                        是否启用
                    </td>
                    <td class="mtext" colspan="3" >
                        <asp:RadioButtonList runat="server" ID="rdbtnIsUsed" RepeatDirection="Horizontal"
                            BindName="IsUsed" SaveName="IsUsed">
                            <asp:ListItem Selected="True" Value="true">启用</asp:ListItem>
                            <asp:ListItem Value="false">停用</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
            </tr>
           <tr>
             <td class="font">支付方式限制</td>
            <td class="text" colspan="3" >
                <uc1:GeneralCheckBoxList ID="cbPayTypes" runat="server" SaveName="PayTypes" BindName="PayTypes" ObjectName="Beeant.Domain.Entities.Basedata.PayTypeEntity,Beeant.Domain.Entities"  />
  
            </td> 
       </tr>
           <tr>
             <td class="font">城市限制</td>
            <td class="text" colspan="3" >
                <uc1:GeneralCheckBoxList ID="GeneralCheckBoxList1" runat="server" SaveName="Cities" BindName="Cities" ObjectName="Beeant.Domain.Entities.Basedata.CityEntity,Beeant.Domain.Entities"  />
  
            </td> 
       </tr>
        <tr>
              <td class="font">
                       活动价
                    </td>
                   <td class="text">
                          <input id="txtPrice" runat="server"  type="text" class="input" BindName="Price"  SaveName="Price"  /> 
                       
                    </td> 
              <td class="font">
                       活动限购
                    </td>
                   <td class="text">
                          <input id="txtOrderLimitCount" runat="server"  type="text" class="input" BindName="OrderLimitCount"  SaveName="OrderLimitCount"  /> 
                       
                    </td> 
        </tr>
            <tr>
            <td class="font">
                备注
            </td>
            <td class="text" colspan="3">
                <input id="txtRemark" runat="server" class="input long" type="text" BindName="Remark"
                    SaveName="Remark" />
            </td>
        </tr>
        
      <tr>
            <td  class="text" style="text-align:center;vertical-align:middle;" colspan="4"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn"   />
             <input id="btnClose" type="button" value="关闭" class="btn"  /></td>
        </tr>
    </table>
</div>
 
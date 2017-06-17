<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="Beeant.Presentation.Admin.Log.Controls.Pager" %>
<div id="divPanel" runat="server" class="pager">
    <asp:LinkButton ID="lnkFirst" runat="server"  Pager="first" Text="首页" CssClass="link" onclick="lnkFirst_Click" ></asp:LinkButton>
    <asp:LinkButton ID="lnkPrevious" runat="server"  Pager="previous" Text="上一页" CssClass="link"   onclick="lnkPrevious_Click" ></asp:LinkButton>
    <asp:LinkButton ID="LinkButton1" runat="server"   Pager="link"  Text="1" CssClass="link"    onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton2" runat="server"  Pager="link" Text="2" CssClass="link"   onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton3" runat="server"  Pager="link" Text="3" CssClass="link"   onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton4" runat="server"  Pager="link" Text="4" CssClass="link"   onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton5" runat="server"  Pager="link" Text="5" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton6" runat="server"  Pager="link" Text="6" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton7" runat="server"  Pager="link" Text="7" CssClass="link" onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton8" runat="server"  Pager="link" Text="8" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton9" runat="server"  Pager="link" Text="9" CssClass="link" onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton10" runat="server"  Pager="link" Text="10" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton11" runat="server"  Pager="link" Text="11" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton12" runat="server"  Pager="link" Text="12" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton13" runat="server"  Pager="link" Text="13" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton14" runat="server"  Pager="link" Text="14" CssClass="link"  onclick="LinkButton_Click"></asp:LinkButton>
   <asp:LinkButton ID="LinkButton15" runat="server"  Pager="link" Text="15"  CssClass="link" onclick="LinkButton_Click"></asp:LinkButton>
    <asp:LinkButton ID="lnkNext" runat="server"  Pager="next"  Text="下一页" CssClass="link" onclick="lnkNext_Click"></asp:LinkButton>
    <asp:LinkButton ID="lnkLast" runat="server"   Pager="last"  Text="末页"  CssClass="link" onclick="lnkLast_Click"></asp:LinkButton>
        
    <asp:TextBox ID="txtPage" runat="server"  Pager="pagebox" CssClass="box" ></asp:TextBox><span class="text">/<%=PageCount %></span>
    <asp:Button ID="btnPage" runat="server"   Pager="pagebutton" CssClass="btngo"  Text="" onclick="btnPage_Click" /><span  class="text">总共记录有：<big><%=DataCount %></big></span>
        <asp:DropDownList ID="ddlPageSize" runat="server" onselectedindexchanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="True">
          <asp:ListItem Value="10" Text="10"></asp:ListItem>
         <asp:ListItem  Value="15" Text="15"></asp:ListItem>
         <asp:ListItem  Value="20" Text="20"></asp:ListItem>
         <asp:ListItem  Value="25" Text="25" ></asp:ListItem>
         <asp:ListItem  Value="30" Text="30"></asp:ListItem>
    </asp:DropDownList>
   
</div>
<asp:HiddenField ID="hfPageIndex" runat="server" Value="0"></asp:HiddenField>
<asp:HiddenField ID="hfDataCount" runat="server" Value="0"></asp:HiddenField>



<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mainten.aspx.cs" Inherits="Beeant.Presentation.Admin.Erp.Order.OrderNote.Mainten"  %>
<%@ Register TagPrefix="uc2" TagName="Message" Src="~/Controls/Message.ascx" %>

   <%@ Register src="../../Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>

   <html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
       <title>订单维护记录</title>
      <link href="/Styles/popup.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="/Scripts/Winner/Winner.ClassBase.js"></script>
</head>
<body>
    <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" onasyncpostbackerror="ScriptManager1_AsyncPostBackError" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

 

    <div class="p-main">
      
        <p class="p-input">
<span><a href="javascript:void(0);" onclick="Show(this);">显示》</a></span>
            
<div id="save" style="display: none;">
     <textarea id="txtContent" runat="server" class="textarea" cols="50"   rows="2"></textarea><p>
     </p>
     <p class="p-inputbut">
         <asp:Button ID="btnSave" runat="server" CssClass="p-but" 
             onclick="btnSave_Click" Text="保存" />
     </p>
       <uc2:Message ID="Message1" runat="server" />
   
        <p>
     </p>
      
        <p>
     </p>
      
        <p>
     </p>
      
        <p>
     </p>
        </div>
            <asp:Repeater ID="Repeater1" runat="server" 
                onitemcommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
                <ItemTemplate>
                    <p class="cf">
                        <%#Eval("Account.RealName") %>： <%#Eval("InsertTime","{0:yyyy-MM-dd HH:mm}") %>
                        <span class="p-bftxt"><%#Eval("Content") %></span>
                        <asp:LinkButton ID="lkbtnRemove" runat="server" 
                            CommandArgument='<%#Eval("Id") %>' CommandName="Remove" CssClass="p-del">删除</asp:LinkButton>
                    </p>
                </ItemTemplate>
            </asp:Repeater>
            <uc1:Pager ID="Pager1" runat="server" FromExp="NoteEntity" 
                OnPagerChanged="Pager1_PagerChanged" OrderByExp="InsertTime desc" PageSize="10" 
                SelectExp="Id,Account.Id,Account.RealName,InsertTime,Content" />
      
            <p>
            </p>
      
        </p>
     
      </div>

    </ContentTemplate>
    </asp:UpdatePanel>
        </form>
        <script type="text/javascript">
            function Show(obj) {
                var save = document.getElementById("save");
                if (save.style.display == "none") {
                    save.style.display = "";
                    obj.innerHTML = "隐藏》";
                } else {
                    save.style.display = "none";
                    obj.innerHTML = "显示》";
                }
            }
        </script>
</body>
</html>
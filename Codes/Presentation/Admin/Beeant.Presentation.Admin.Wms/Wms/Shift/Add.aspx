<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Beeant.Presentation.Admin.Wms.Wms.Shift.Add"  %>
<%@ Register TagPrefix="uc2" TagName="Message" Src="~/Controls/Message.ascx" %>

   <%@ Register src="../../Controls/Pager.ascx" tagname="Pager" tagprefix="uc1" %>

   <html xmlns="http://www.w3.org/1999/xhtml">
       <script src="../../Scripts/jquery-1.7.1.min.js"></script>
<head id="Head1" runat="server">
       <title>供应链商品审核</title>
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
              <div class="checkline">
       数量
        <input type="text" id="txtCount" runat="server" ></input>
    </div>
    <div class="checkline">
       备注
        <textarea id="txtContent" runat="server" class="textarea" cols="50"   rows="2"></textarea>
    </div>
     <p>
     </p>
     <p class="p-inputbut">
         <asp:Button ID="btnSave" runat="server" CssClass="p-but" 
             onclick="btnSave_Click" Text="保存" />
     </p>
       <uc2:Message ID="Message1" runat="server" />
   
        <p>
     </p>
        </div>
      
        </p>
     
      </div>

    </ContentTemplate>
    </asp:UpdatePanel>
        </form>
  
</body>
</html>
<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="PrintDetail.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payin.PrintEntity" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>打印收款单</title>
 <link href="../../Styles/PrintR.css" rel="stylesheet" type="text/css" />
</head>
<script type="text/javascript">


    function preview() {

        var bdhtml = window.document.body.innerHTML;
        var sprnstr = "<!--startprint-->";
        var eprnstr = "<!--endprint-->";
        var prnhtml = bdhtml.substring(bdhtml.indexOf(sprnstr) + 17);
        prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
        window.document.body.innerHTML = prnhtml;
        window.print();
        window.document.body.innerHTML = bdhtml;
    }
</script>  

<body>
    <form runat="server">
    <input type="button" id="print" value="打印" onclick="preview();" />
    <!--startprint-->
	<div class="box">
    	<h3>收款单</h3>
        <h4><span>到账日期：</span><em>货币种类：&nbsp;人民币</em></h4>
        <ul>
        	<li class="line"><strong>客户名称</strong><span><asp:Label ID="lblName" runat="server" Text=""  BindName="Name"></asp:Label></span></li>
            <li class="line2"><strong>金额（大写）</strong><asp:Label ID="lblAmountinwords" runat="server" Text=""  BindName="Amountinwords"></asp:Label><em class="em1">￥</em><em class="em2"><asp:Label ID="lblAmount" runat="server" Text=""  BindName="Amount"></asp:Label></em></li>
            <li class="line"><strong>收款流水单号</strong></li>
            <li class="line4"><strong>采购单编号</strong><span><em><asp:Label ID="lblOrdersId" runat="server"></asp:Label></em></span></li>
        </ul>
        <p><strong>审核人：</strong><span class="p_span">审批人：</span><em>制单人:<asp:Label ID="lblSubmitRealName" runat="server" Text="" CssClass="em_span"  BindName="Submit.RealName"></asp:Label></em></p>
    </div>
   <!--endprint-->
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDetail.aspx.cs" Inherits="Beeant.Presentation.Admin.Finance.Finance.Payout.PrintEntity"  %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>打印付款单</title>
    <link href="../../Styles/PrintP.css" rel="stylesheet" type="text/css" />
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
</head>

<body>
    <form runat="server">
	<input type="button" id="print" value="打印" onclick="preview();" />
    <!--startprint-->
	<div class="box">
    	<h3>付款单</h3>
        <h4><span>付款日期：</span><em>货币种类：&nbsp;人民币</em></h4>
        <ul>
        	<li class="line"><strong>客户名称</strong><span> <asp:Label ID="lblName" runat="server" Text=""  BindName="Name"></asp:Label></span></li>
            <li class="line"><strong>开户银行</strong><span><asp:Label ID="lblBankName" runat="server" Text=""  BindName="BankName"></asp:Label></span></li>
            <li class="line"><strong>账号</strong><span><asp:Label ID="lblBankNumber" runat="server" Text=""  BindName="BankNumber"></asp:Label></span></li>
            <li class="line2"><strong>金额（大写）</strong><asp:Label ID="lblAmountinwords" runat="server" Text=""  BindName="Amountinwords"></asp:Label>
            <em class="em1">￥</em><em class="em2"><asp:Label ID="lblAmount" runat="server" Text=""  BindName="Amount"></asp:Label></em></li>
            <li class="line"><strong>付款流水单号</strong><asp:Label ID="lblOriginalNumber" runat="server" Text=""  BindName="OriginalNumber"></asp:Label></li>
            <li class="line4"><strong>订单编号</strong><asp:Label ID="lblPurchasheId" runat="server"></asp:Label></li>
        </ul>
        <p><strong>审核人：</strong><span class="p_span">审批人：</span><em>制单人：<asp:Label ID="lblSubmitRealName" runat="server" Text="" CssClass="em_span"  BindName="Submit.RealName"></asp:Label></em></p>
    </div>
    <!--endprint-->
    </form>
</body>
</html>
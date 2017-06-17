function seccess(data) {
    alert("支付成功");
    window.location.href = window.PayUrl;
}
function fail(data) {
    if (data == undefined || data == "") {
        data = "支付失败";
    }
    alert(data);
    $("#btnPay").show();
}

function create(data) {
    if ($("#selPayTypes").find("option:selected").text() == "支付宝") {
        $("#frPayType").show();
        $("#frPayType").css("width", $(window).width() + "px");
        $("#frPayType").css("height", $(window).height() + "px");
    } else {
        $("#frPayType").hide();
    }
}
$(document).ready(function () {
    function setPayForm(parameters) {
        if ($("#selPayTypes").find("option:selected").text() == "支付宝") {
            $("#fmPayType").attr("target", "");
            $("#hfseccesshandle").val("");
            $("#hffailhandle").val("");
            $("#hfcreatehandle").val("");
        } else {
            $("#fmPayType").attr("target", "fmPayType");
            $("#hfseccesshandle").val("parent.seccess");
            $("#hffailhandle").val("parent.fail");
            $("#hfcreatehandle").val("parent.create");
        }
    }
    var buyer = new Winner.Buyer();
    buyer.Initialize();
    $.ajax({
        type: "Get",
        url: "/Coupon/Index" ,
        async: true,
        dataType: "text",
        success: function (result) {
            $("#coupon").html(result);
        },
        error: function (ex) {

        }
    });
    $("#selPayTypes").bind("change", function () {
        $("#fmPayType").attr("action", $(this).val());
        $("#btnPay").show();
        $("#frPayType").hide();
        setPayForm();
    });
    $("#divClose").bind("click", function () {
        $("#divPayTypes").hide();
        window.location.href = $("#selPayTypes").attr("NopayUrl");
    });
    $("#btnPay").click(function() {
        $(this).hide();
    });
    $("#btnSubmit").click(function () {
        $(this).hide();
        $(".loading").show();
        $(".buymask").show();
        var url = "/Home/Buy?AddressId=" + $("input[name='AddressId']").val();
        var productids = $("input[name='ProductId']");
        var counts = $("input[name='Count']");
        for (var i = 0; i < productids.length; i++) {
            url += "&Products[" + i + "].ProductId=" + $(productids[i]).val() + "&Products[" + i + "].Count=" + $(counts[i]).val();
        }
        var orderPay = function (result) {
            $(".loading").hide();
            if ($("#selPayTypes").find("option").length == 0) {
                window.location.href = $("#selPayTypes").attr("NopayUrl");
                return;
            }
            $("#fmPayType").attr("action", $("#selPayTypes").find("option")[0].value);
            $("#divPayTypes").show();
            $(".order").hide();
            $(".buymask").show();
            $("#fmPayType").find("input[name='OrderIds']").val(result.Message);
            setPayForm();
        }
        $.ajax({
            type: "get",
            url: url,
            async: true,
            dataType: "json",
            data:{},
            success: function (result) {
                $(".loading").hide();
                if (result.Status == "true") {
                    orderPay(result);
                } else {
                    alert(result.Message);
                    $("#btnSubmit").show();
                    $(".buymask").hide();
                }
            },
            error: function (ex) {
                $("#btnSubmit").show();
                $(".loading").hide();
                $(".buymask").hide();
                alert("系统忙，请稍等再试");
            }
        });
    });
    $(".addrress").click(function () {
        var url =$(this).attr("Url")+"?couponid=0";
        var productids = $("input[name='ProductId']");
        var counts = $("input[name='Count']");
        for (var i = 0; i < productids.length; i++) {
            url += "&Products[" + i + "].ProductId=" + $(productids[i]).val() + "&Products[" + i + "].Count=" + $(counts[i]).val();
        }
        window.location.href = url;
    });
    function isWeiXin() {
        var ua = window.navigator.userAgent.toLowerCase();
        if (ua.match(/MicroMessenger/i) == 'micromessenger') {
            return true;
        } else {
            return false;
        }
    }
    if (isWeiXin()) {
        $("#fmPayType").find("input[name='trade_type']").val("JSAPI");
    }


});
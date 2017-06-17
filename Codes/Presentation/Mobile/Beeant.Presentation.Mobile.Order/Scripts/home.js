function seccess(data) {
    alert("支付成功");
    window.location.reload();
}
function fail(data) {
    if (data == undefined || data == "") {
        data = "支付失败";
    }
    alert(data);
    $("#btnPay").show();
}
$(document).ready(function () {
    var config = [
        {
            Triggers: [
                {
                    Sender: window,
                    Event: "scroll",
                    Function: function() {
                        return $(document).scrollTop() >= $(document).height() - $(window).height() - 40;
                    }
                }
            ],
            Loading: { Content: "Content", Type: "Append" },
            Url: $(".list").attr("url"),
            Paramters: { page: 0 },
            Content: $(".list")[0],
            ShowType: "Append",
            DataType: "text",
            RequestType: "Repeat",
            IsExecute: true,
            IsLoadHideContent: false,
            BeginLoadFunction: function() {

            },
            BeginShowFunction: function(sender, info, data) {
                info.Paramters.page = info.Paramters.page + 1;

            },
            EndShowFunction: function (sender, info, data) {
            
                window.ImageRecover.LoadImages($(info.Content)[0]);
            }
        }
    ];
    var dataloader = new Winner.DataLoader(config);
    dataloader.Initialize();

    $("#divClose").bind("click", function () {
        $("#divPayTypes").hide();
        $(".buymask").hide();
    });
    $("#btnPay").click(function () {
        $(this).hide();
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
    $("#selPayTypes").bind("change", function () {
        $("#fmPayType").attr("action", $(this).val());
        $("#btnPay").show();
    });
});

function sure(id,sender) {
    var rev = confirm("您确认收货吗？");
    if (!rev)
        return false;
    $.ajax({
        type: "Post",
        url: "/Home/Sure",
        data: { id: id },
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.Status) {
                alert("确认收货成功");
                $(sender).parent().parent().find("span[name='status']").html(data.Message);
                $(sender).parent().find("a[name='lkcancel']").remove();
                $(sender).parent().find("a[name='lksure']").remove();
                $(sender).parent().find("a[name='lkpayment']").remove();
                $(sender).remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("系统忙，请稍候再试");
        }
    });
    return false;
}
function cancel(id, sender) {
    var rev = confirm("您确认取消订单吗？");
    if (!rev)
        return false;
    $.ajax({
        type: "Post",
        url: "/Home/Cancel",
        data: { id: id },
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.Status) {
                alert("取消成功");
                $(sender).parent().parent().find("span[name='status']").html(data.Message);
                $(sender).parent().find("a[name='lkpayment']").remove();
                $(sender).parent().find("a[name='lksure']").remove();
                $(sender).parent().find("a[name='lkwait']").remove();
                $(sender).remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("系统忙，请稍候再试");
        }
    });
    return false;
}
function wait(id, sender) {
    var rev = confirm("您确认申请退款吗？");
    if (!rev)
        return false;
    $.ajax({
        type: "Post",
        url: "/Home/Wait",
        data: { id: id },
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.Status) {
                alert("申请退款成功");
                $(sender).parent().parent().find("span[name='status']").html(data.Message);
                $(sender).parent().find("a[name='lkcancel']").remove();
                $(sender).parent().find("a[name='lksure']").remove();
                $(sender).parent().find("a[name='lkpayment']").remove();
                $(sender).remove();
            } else {
                alert(data.Message);
            }
        },
        error: function () {
            alert("系统忙，请稍候再试");
        }
    });
    return false;
}

function payment(id,price, paytype) {
    $(".buymask").show();
    var pays = getPayTypes(paytype);
    $("#selPayTypes").html("");
    $(pays).each(function (index, val) {
        $("#selPayTypes").append("<option value='" + val.Url + "'>" + val.Name + "</option>");
    });
    if ($("#selPayTypes").find("option").length > 0) {
        $("#fmPayType").attr("action", $("#selPayTypes").find("option")[0].value);
    }
    $("#divPayTypes").find("input[name='OrderIds']").val(id);
    $("#divPayTypes").find("span[Buyer='Price']").html(price);
    $("#divPayTypes").show();
    return false;
}
function getPayTypes(paytype) {
    var paytypes = eval($("#hfPaytype").val());
    if (paytype == null || paytype == "")
        return paytypes;
    var pays = paytype.split(',');
    return  $.grep(paytypes, function(val, key) { 
        if($.inArray(val, pays)) 
            return true; 
    }, false); 
}
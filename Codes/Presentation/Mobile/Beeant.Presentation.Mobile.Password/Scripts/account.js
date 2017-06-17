$(document).ready(function() {
    var defaultValueFunc = function (input) {
        function focusValue(input) {
            var ctrl = $(input);
            ctrl.addClass("nor");
            if (ctrl.attr("DefaultValue") == undefined) {
                ctrl.attr("DefaultValue", ctrl.value);
            }
            if (ctrl.val() == ctrl.attr("DefaultValue")) {
                ctrl.val("");
            }
        }

        function blurValue(input) {
            var ctrl = $(input);
            if (ctrl.val() == "") {
                ctrl.val($(ctrl).attr("DefaultValue"));
                ctrl.attr("class", ctrl[0].className.replace(" validctrlerror", "").replace(" validctrlsucess", "").replace(" select", ""));
                ctrl.removeClass("nor");
            } else {
                ctrl.addClass("nor");
            }
        }
        $(input).blur(function () {
            blurValue(input);
        });
        $(input).focus(function () {
            focusValue(input);
        });
    }
    $(document).find("input[DefaultValue]").each(function (index, sender) {
        defaultValueFunc(this);
    });
    $("#hfCode").click(function () {
        var date = new Date();
        $("#imgCode").attr("src", $(this).attr("url") + "?vesion=" + date);
    });
    $("#btnCode").click(function () {
        var sender = $("#btnCode");
        if (sender.attr("disabled") == "disabled")
            return;
        if ($("#txtClientCode").val() == "") {
            alert("请输入验证码");
            return;
        }
        var times = 0;
        var func = function () {
            if (times <= 0) {
                sender.removeAttr("disabled");
                sender.val("重新发送");
                return;
            }
            sender.val(times + "秒后重新发送");
            times = times - 1;
            setTimeout(func, 1000);
        };
        $.ajax({
            type: "Post",
            url: sender.attr("url"),
            data: { value: $("#txtValue").val(), code: $("#txtClientCode").val(), action: $("#hfAction").val() },
            async: false,
            dataType: "json",
            success: function (data) {
                if (data.Status) {
                    sender.attr("disabled", "disabled");
                    times = parseInt(data.Message);
                    func();
                } else {
                    alert(data.Message);
                    var date = new Date();
                    $("#imgCode").attr("src", $("#hfCode").attr("url") + "?vesion=" + date);
                }
            },
            error: function () {
                alert("发送失败，请重新发送");
            }
        });
    });
});
 
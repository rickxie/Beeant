$(document).ready(function () {
    function focusValue(ctrl) {
        if ($(ctrl).attr("DefaultValue") == undefined) {
            $(ctrl).attr("DefaultValue", ctrl.value);
        }
        if (ctrl.value == $(ctrl).attr("DefaultValue")) {
            ctrl.value = "";
        }
    }
    function blurValue(ctrl) {
        if (ctrl.value == "") {
            ctrl.value = $(ctrl).attr("DefaultValue");
        }
    }
    function submit() {
        $("span[class='failure']").hide();
        $.getJSON(window.LoginUrl + "/Home/DialogLogin?callback=?", { Name: $("#loginName").val(), Password: $("#loginPass").val(), Url: $("#hfUrl").val(), Code: $("#loginCode").val() },function (data) {
            if (data.Status == true) {
                eval(data.Message);
            } else {
                $("span[class='failure']").html(data.Message);
                $("span[class='failure']").show();
                if (data.IsShowCode) {
                    $("loginCode").attr("IsShowCode", "true");
                    $("loginCode").parent().show();
                }
            }
        });
      
    }

    $("#loginName").blur(function () {
        blurValue(this);
        this.className = "text iname";
        $(this).parent().parent().attr("class", "in bor");
    });
    $("#loginName").focus(function () {
        focusValue(this);
        this.className = "text select iname";
        $(this).parent().parent().attr("class", "in pass");
    });

    $("#loginPass").blur(function () {
        if (this.value == "")
            this.type = "text";
        blurValue(this);
        this.className = "text ipass";
        $(this).parent().parent().attr("class", "in bor");
    });
    $("#loginPass").focus(function () {
        focusValue(this);
        this.type = "password";
        this.className = "text select ipass";
        $(this).parent().parent().attr("class", "in pass");
    });
    $("#loginCode").blur(function () {
        blurValue(this);
        this.className = "code";
    });
    $("#loginCode").focus(function () {
        focusValue(this);
        this.className = "code select";
    });
    $("#aCode").click(function () {
        var url = window.LoginUrl == undefined ? "/Home/Code?vesion=" : window.LoginUrl + "/Home/Code?vesion=";
        var date = new Date();
        $("#imgCode").attr("src", url + date);
    });
    $("#loginSubimt").click(function () {
        var rev = true;
        if ($("#loginName").val() == "" || $("#loginName").attr("DefaultValue") == undefined || $("#loginName").val() == $("#loginName").attr("DefaultValue")) {
            $("#loginName").parent().parent().attr("class", "in error");
            rev = false;
        }
        if ($("#loginPass").val() == "" || $("#loginPass").attr("DefaultValue") == undefined || $("#loginPass").val() == $("#loginPass").attr("DefaultValue")) {
            $("#loginPass").parent().parent().attr("class", "in error");
            rev = false;
        }
        if ($("#loginCode").attr("IsShowCode") == "true" && ($("#loginCode").val() == "" || $("#loginCode").attr("DefaultValue") == undefined || $("#loginCode").val() == $("#loginCode").attr("DefaultValue"))) {
            $("#loginCode").attr("class", "code error");
            rev = false;
        }
        if (rev && $("#loginSubimt").attr("IsSubmit") == "true") {
            submit();
        }
        return rev;
    });
    $(".login").find("input[type='text']").focus(function() {
        $("#loginSubimt").hide();
    }).blur(function() {
        $("#loginSubimt").show();
    });
});